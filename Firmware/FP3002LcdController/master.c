#include "master.h"
#include "uart0.h"
#include "uart1.h"
#include "wake.h"
#include "ILI9341.h"
#include "images.h"

void init_master_serial(void)
{
	uart1_init(0, 2, false);
	uart1_enable();
}

void xmit_to_master(const uint8_t * content, uint8_t len)
{
	uart1_xmit(content, len);
}

void uart1_rcv_byte_callback(uint8_t byte)
{
	uint8_t cmd_header  = byte & CMD_HEADER_MASK;
	uint8_t cmd_payload = byte & CMD_PAYLOAD_MASK;
	
	if (cmd_header == CMD_MASK_FUNC)
	{		
		switch(cmd_payload)
		{
			case CMD_FUNC_SCREEN_WAKEUP:			
				disable_uart1_rx;
				disable_uart0_rx;
				wakeup();
				enable_uart0_rx;
				enable_uart1_rx;				
				return;
			
			case CMD_FUNC_SCREEN_CLEAN:
				disable_uart1_rx;
				disable_uart0_rx;
				disable_all_pixels();
				enable_uart0_rx;
				enable_uart1_rx;
				return;
			
			case CMD_FUNC_JUMP_TO_BOOTLOADER:
				wdt_reset_device();
		}		
	}	
	
	if (cmd_header == CMD_MASK_SCREEN_BRIGHT_MASK)
	{
		//set_backlight_intensity((byte & 0x0F) << 3);
		uint16_t intensity = (cmd_payload << 3) * 10;
		TCD0_CCA = (intensity > 125*10) ? 125*10 : intensity;		
		return;
	}
	
	if (cmd_header == CMD_MASK_VERSION_MASK)
	{
		disable_uart1_rx;
		disable_uart0_rx;
		uint8_t b;
		switch(cmd_payload)
		{
			case CMD_FUNC_FW_H_REQUEST: b = VERSION_FW_H | CMD_REPLY_FW_H_MASK; uart1_xmit(&b, 1);
			case CMD_FUNC_FW_L_REQUEST: b = VERSION_FW_L | CMD_REPLY_FW_L_MASK; uart1_xmit(&b, 1);			
			case CMD_FUNC_HW_H_REQUEST: b = VERSION_HW_H | CMD_REPLY_HW_H_MASK; uart1_xmit(&b, 1);
			case CMD_FUNC_HW_L_REQUEST: b = VERSION_HW_L | CMD_REPLY_HW_L_MASK; uart1_xmit(&b, 1);
			case CMD_FUNC_ASS_REQUEST:  b = VERSION_ASS  | CMD_REPLY_ASS_MASK;  uart1_xmit(&b, 1);
		}
		enable_uart0_rx;
		enable_uart1_rx;
		return;
	}
	
	if ((byte & CMD_BIT_MASK_DISPLAY_INDEX) == CMD_BIT_MASK_DISPLAY_INDEX)
	{
		disable_uart1_rx;
		disable_uart0_rx;
		display_image_index(byte & CMD_MASK_IMAGE_INDEX);		
		enable_uart0_rx;
		enable_uart1_rx;
		return;
	}
}