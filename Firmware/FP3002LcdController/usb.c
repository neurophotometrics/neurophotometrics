#include "usb.h"
#include "uart0.h"
#include "uart1.h"
#include "images.h"

extern uint8_t rxbuff_uart0[];
extern uint8_t uart0_rx_pointer;

uint8_t expected_header [HARP_HEADER_LENGHT] = {2, 255, 0xE8, 0x01, 200, 255, 2};
uint16_t harp_message_pointer = 0;

void init_usb_serial(void)
{
	uart0_init(1, 0, false);   // 1 Mb/s
	uart0_enable();
}

void init_usb_ios(void)
{
	io_pin2in(&PORTK, 3, PULL_IO_UP, SENSE_IO_EDGES_BOTH);      // SCREEN_CAN_USE_USB when low value
	io_pin2out(&PORTK, 2, OUT_IO_WIREDAND, IN_EN_IO_EN);        // SCREEN_IS_USING_USB when high level
	
	io_set_int(&PORTK, INT_LEVEL_LOW, 1, (1<<3), false);        // SCREEN_CAN_USE_USB
	
	clr_SCREEN_IS_USING_USB;
}

void usb_release_hold_on(void)
{
	clr_SCREEN_IS_USING_USB;
	uart0_disable();
}
		
ISR(TCC0_OVF_vect, ISR_NAKED)
{
	usb_release_hold_on();
	
	harp_message_pointer = 0;
	timer_type0_stop(&TCC0);
	reti();
}

ISR(PORTK_INT1_vect, ISR_NAKED)
{
	/* Communication not available */
	if (read_SCREEN_CAN_USE_USB)
	{
		clr_SCREEN_IS_USING_USB;
		uart0_disable();
	}
	
	/* Communication available */
	else
	{
		set_SCREEN_IS_USING_USB;
		uart0_enable();
		
		/* If there's no received bytes during 1 second the communication will close */
		timer_type0_enable(&TCC0, TIMER_PRESCALER_DIV1024, 62500/2, INT_LEVEL_LOW);
	}
	
	reti();
}

void uart0_rcv_byte_callback(uint8_t byte)
{
	if (harp_message_pointer < HARP_HEADER_LENGHT)
	{
		if (harp_message_pointer == 0)
		{
			TCC0_CNT = 0;	// win another second
		}
		
		if (expected_header[harp_message_pointer] == byte)
		{
			rxbuff_uart0[harp_message_pointer] = byte;
			harp_message_pointer++;
		}
		else
		{
			harp_message_pointer = 0;
		}
	}
	else
	{
		rxbuff_uart0[harp_message_pointer] = byte;
		harp_message_pointer++;
		
		if(harp_message_pointer == HARP_MESSAGE_LENGHT)
		{
			disable_uart0_rx;
			disable_uart1_rx;
			TCC0_CNT = 0;	// win another second
			
			harp_message_pointer = 0;
			uint8_t checksum = 0;
			
			for (uint16_t i = 0; i<HARP_MESSAGE_LENGHT-1; i++)
			checksum += rxbuff_uart0[harp_message_pointer++];
			
			if (checksum == byte)
			{
				bool error = false;
				
				HarpMessage_t * HarpMessage = ((HarpMessage_t *)(rxbuff_uart0));
				
				if (HarpMessage->Payload.image_index  > MAX_IMAGE_INDEX) error = true;
				if (HarpMessage->Payload.column_index > MAX_COLUMN_INDEX) error = true;
				
				if (error)
				{
					rxbuff_uart0[0] |= 0x80;					// Error flag
					rxbuff_uart0[HARP_MESSAGE_LENGHT] += 0x80;	// Update checksum
				}
				else
				{
					alloc_image(
						HarpMessage->Payload.image_index,
						HarpMessage->Payload.column_index,
						HarpMessage->Payload.content
						);
				}
				
				uart0_xmit(rxbuff_uart0, HARP_MESSAGE_LENGHT_DIV2);
				uart0_xmit(rxbuff_uart0+HARP_MESSAGE_LENGHT_DIV2, HARP_MESSAGE_LENGHT_DIV2);
			}
			
			harp_message_pointer = 0;
			enable_uart1_rx;
			enable_uart0_rx;
		}
	}
}