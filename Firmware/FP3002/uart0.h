#ifndef _UART0_H_
#define _UART0_H_

#include "cpu.h"


/************************************************************************/
/* UART definitions                                                     */
/************************************************************************/
#define UART0_RX_INT_LEVEL    INT_LEVEL_LOW
#define UART0_TX_INT_LEVEL    INT_LEVEL_LOW
#define UART0_CTS_INT_LEVEL   INT_LEVEL_LOW

#define UART0_RXBUFSIZ			32
#define UART0_TXBUFSIZ			32

#define UART0_UART				USARTD0
#define UART0_PORT				PORTD
#define UART0_DATA				USARTD0_DATA
#define UART0_RX_pin			2
#define UART0_TX_pin			3

#define UART0_RX_ROUTINE_		ISR(USARTD0_RXC_vect, ISR_NAKED)
#define UART0_TX_ROUTINE_		ISR(USARTD0_DRE_vect, ISR_NAKED)


#define UART0_USE_FLOW_CONTROL	// comment this line if don't use
#define UART0_RTS_PORT			PORTD
#define UART0_RTS_pin			0
#define UART0_CTS_PORT			PORTD
#define UART0_CTS_pin			1

#define UART0_CTS_ROUTINE_		ISR(PORTD_INT0_vect, ISR_NAKED)
#define UART0_CTS_INT_N			0

#define uart0_leave_interrupt /*return*/reti()

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
#define disable_uart0_rx set_io(UART0_RTS_PORT, UART0_RTS_pin)
#define enable_uart0_rx	 clear_io(UART0_RTS_PORT, UART0_RTS_pin)

void uart0_init(uint16_t BSEL, int8_t BSCALE, bool use_clk2x);
void uart0_enable(void);
void uart0_disable();

void uart0_xmit_now(const uint8_t *dataIn0, uint8_t siz);
void uart0_xmit_now_byte(const uint8_t byte);
void uart0_xmit(const uint8_t *dataIn0, uint8_t siz);

void uart0_rcv_byte_callback(uint8_t byte);
bool uart0_rcv_now(uint8_t * byte);

#endif /* _UART0_H_ */
