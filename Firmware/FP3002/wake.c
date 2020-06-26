#include "wake.h"
#include "cpu.h"

#define F_CPU 32000000
#include <util/delay.h>

#define BOOTING_TEXT_LEN 20
#define DONE_TEXT_LEN 5
#define DOTS_INTERVAL_LEN 40
const uint8_t dot_intervals[DOTS_INTERVAL_LEN] = {	250, 250, 94, 50, 75,
													50, 100, 50, 75, 50,
													25, 25, 50, 75, 250,
													25, 25, 50, 75, 250,
													250, 250, 94, 75, 232,
													25, 25, 50, 75, 250,
													250, 244, 94, 250, 75,
													250, 50, 50, 50, 50};
													
void wakeup(void)
{
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_EN); // State LED
	
	
	for (uint8_t i = 0; i < BOOTING_TEXT_LEN; i ++)
		toggle_io(PORTA, 6);
	
	_delay_ms(1000);
	
	for (uint8_t i = 0; i < DOTS_INTERVAL_LEN; i ++)
	{
		for (uint16_t j = 0; j < dot_intervals[i]; j++)
			_delay_ms(1);
		
		toggle_io(PORTA, 6);
	}
	
	_delay_ms(1000);
	
	for (uint8_t i = 0; i < DONE_TEXT_LEN; i ++)
		toggle_io(PORTA, 6);
}