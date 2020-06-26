#include "uart0.h"
#include <string.h>


/************************************************************************/
/* Buffers and pointers                                                 */
/************************************************************************/
uint8_t txbuff_uart0[UART0_TXBUFSIZ];
uint8_t rxbuff_uart0[UART0_RXBUFSIZ];

#if UART0_TXBUFSIZ > 256
	uint16_t uart0_tail = 0;
	uint16_t uart0_head = 0;
#else
	uint8_t uart0_tail = 0;
	uint8_t uart0_head = 0;
#endif

#if UART0_RXBUFSIZ > 256
	uint16_t uart0_rx_pointer = 0;
#else
	uint8_t uart0_rx_pointer = 0;
#endif
	

	
/************************************************************************/
/* Initialization and ON/OFF                                            */
/************************************************************************/
void uart0_init(uint16_t BSEL, int8_t BSCALE, bool use_clk2x)
{
	#ifdef UART0_USE_FLOW_CONTROL
		io_pin2out(&UART0_RTS_PORT, UART0_RTS_pin, OUT_IO_DIGITAL, IN_EN_IO_EN);
		io_pin2in(&UART0_CTS_PORT, UART0_CTS_pin, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
		io_set_int(&UART0_CTS_PORT, UART0_CTS_INT_LEVEL, UART0_CTS_INT_N, (1<<UART0_CTS_pin), true);
		enable_uart0_rx;
	#endif
	
	UART0_UART.CTRLC = USART_CMODE_ASYNCHRONOUS_gc | USART_PMODE_DISABLED_gc | USART_CHSIZE_8BIT_gc;
	UART0_UART.BAUDCTRLA = *((uint8_t*)&BSEL);
	UART0_UART.BAUDCTRLB = (*(1+(uint8_t*)&BSEL) & 0x0F) | ((BSCALE<<4) & 0xF0);

	if (use_clk2x)
		UART0_UART.CTRLB |= USART_CLK2X_bm;
	
	set_io(UART0_PORT, UART0_TX_pin);
	io_pin2out(&UART0_PORT, UART0_TX_pin, OUT_IO_DIGITAL, IN_EN_IO_DIS);
	io_pin2in(&UART0_PORT, UART0_RX_pin, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);
	
	#ifdef UART0_USE_FLOW_CONTROL
		if (UART0_CTS_PORT.IN & (1 << UART0_CTS_pin) )
			/* Disable uart interrupt until RTS is logic low */
			UART0_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);
	#endif
}

void uart0_enable(void)
{
	UART0_UART.CTRLB |= (USART_RXEN_bm | USART_TXEN_bm);
	UART0_UART.STATUS = USART_RXCIF_bm | USART_TXCIF_bm | USART_DREIF_bm;
	UART0_UART.CTRLA |= (UART0_RX_INT_LEVEL<< 4);
}

void uart0_disable(void)
{
	UART0_UART.CTRLB &= (USART_RXEN_bm | USART_TXEN_bm);
}

/************************************************************************/
/* Interrupt TX                                                         */
/************************************************************************/
UART0_TX_ROUTINE_
{
	disable_uart0_rx;

	UART0_UART.DATA = txbuff_uart0[uart0_tail++];
	if (uart0_tail == UART0_TXBUFSIZ)
		uart0_tail = 0;
	
	/* disable this interrupt until new data arrive to buffer */
	if (uart0_head == uart0_tail)
		UART0_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);

	enable_uart0_rx;

	uart0_leave_interrupt;
}

/************************************************************************/
/* Interrupt CTS                                                        */
/************************************************************************/
UART0_CTS_ROUTINE_
{
	if (UART0_CTS_PORT.IN & (1 << UART0_CTS_pin) )
		/* Disable uart interrupt until RTS is logic low */
		UART0_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);
	else
		if (uart0_tail != uart0_head)
			/* If the buffer is not empty, enable Tx interrupt */
			UART0_UART.CTRLA |= UART0_TX_INT_LEVEL;

	uart0_leave_interrupt;
}

/************************************************************************/
/* Send data                                                            */
/************************************************************************/
void uart0_xmit_now(const uint8_t *dataIn0, uint8_t siz)
{
	for (uint8_t i = 0; i < siz; i++) {
		loop_until_bit_is_set(UART0_UART.STATUS, USART_DREIF_bp);
		UART0_UART.DATA = dataIn0[i];
	}
}

void uart0_xmit_now_byte(const uint8_t byte)
{
	loop_until_bit_is_set(UART0_UART.STATUS, USART_DREIF_bp);
	UART0_UART.DATA = byte;
}

void uart0_xmit(const uint8_t *dataIn0, uint8_t siz)
{
	#ifdef UART0_USE_FLOW_CONTROL
		if (!(UART0_CTS_PORT.IN & (1 << UART0_CTS_pin)))
	#endif
			UART0_UART.CTRLA |= UART0_TX_INT_LEVEL;	// Re-enable TX interrupt
	
	
	uint16_t space = UART0_TXBUFSIZ - uart0_head;
	if (space >= siz)
	{
		memcpy(txbuff_uart0+uart0_head, dataIn0, siz);
		
		bool tail_ahead = (uart0_tail > uart0_head);
		uart0_head += siz;
		if (uart0_head == UART0_TXBUFSIZ)
		{
			uart0_head = 0;
			if (uart0_tail == 0)    uart0_tail = 1;			// lose oldest byte in buffer
		}
		else if (tail_ahead && uart0_tail <= uart0_head)	// if buffer overflow
		{
			uart0_tail = uart0_head+1;								// lose oldest bytes in buffer
			if (uart0_tail == UART0_TXBUFSIZ)    uart0_tail = 0;
			
		}
	}
	else
	{
		memcpy(txbuff_uart0+uart0_head, dataIn0, space);
		siz -= space;
		memcpy(txbuff_uart0, dataIn0+space, siz);
		bool tail_ahead = (uart0_tail > uart0_head);
		uart0_head = siz;
		if (tail_ahead || uart0_tail <= uart0_head)			// if buffer overflow
		{
			uart0_tail = uart0_head+1;								// lose oldest bytes in buffer
			if (uart0_tail == UART0_TXBUFSIZ)    uart0_tail = 0;
		}
	}
}

/************************************************************************/
/* Receive data                                                         */
/************************************************************************/
bool uart0_rcv_now(uint8_t * byte)
{
	enable_uart0_rx;
	
	uint8_t uart_state = read_io(UART0_RTS_PORT, UART0_RTS_pin) ? true : false;

	if (UART0_UART.STATUS & USART_RXCIF_bm)
	{
		*byte = UART0_UART.DATA;
		return true;
	}

	if (!uart_state)
		disable_uart0_rx;
	
	return false;
}

extern uint8_t rx[];

UART0_RX_ROUTINE_
{
	//disable_uart0_rx;
	uart0_rcv_byte_callback(UART0_DATA);
	//enable_uart0_rx;
	uart0_leave_interrupt;
}