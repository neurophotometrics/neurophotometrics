#include "wake.h"
#include "ILI9341.h"

#define F_CPU 32000000
#include <util/delay.h>

#define BOOTING_TEXT_LEN 20
const char booting_text[BOOTING_TEXT_LEN] = "BOOTING THE SYSTEM";

#define DONE_TEXT_LEN 5
const char done_text[DONE_TEXT_LEN] = " DONE";

#define DOTS_INTERVAL_LEN 40
const uint8_t dot_intervals[DOTS_INTERVAL_LEN] = {	250, 250, 94, 50, 75,
													50, 100, 50, 75, 50,
													25, 25, 50, 75, 250,
													25, 25, 50, 75, 250,
													250, 250, 94, 75, 232,
													25, 25, 50, 75, 250,
													250, 244, 94, 250, 75,
													250, 50, 50, 50, 50};

#define Y_INDEX 15

void wakeup(void)
{
	uint16_t length = 0;
	
	for (uint16_t x = 20 - 2; x < 300; x++)
		for (uint16_t y = Y_INDEX - 2; y < Y_INDEX + 7 + 2; y++ )
			draw_a_pixel_rgb(x, y, 0, 0, 0);
		
	for (uint8_t i = 0; i < BOOTING_TEXT_LEN; i ++)
	{
		length = length + 2 + draw_letter(booting_text[i], 20+length, Y_INDEX, 255, 255, 255, 0, 0, 0);
		toggle_io(PORTA, 6);
	}
		
	_delay_ms(1000);
		
	for (uint8_t i = 0; i < DOTS_INTERVAL_LEN; i ++)
	{
		for (uint16_t j = 0; j < dot_intervals[i]; j++)
		_delay_ms(1);
			
		length = length + 2 + draw_letter('.', 20+length, Y_INDEX, 255, 255, 255, 0, 0, 0);
		
		toggle_io(PORTA, 6);
	}
		
	_delay_ms(1000);
		
	for (uint8_t i = 0; i < DONE_TEXT_LEN; i ++)
	{
		length = length + 2 + draw_letter(done_text[i], 20+length, Y_INDEX, 255, 255, 255, 0, 0, 0);	
		toggle_io(PORTA, 6);
	}
	
	clear_io(PORTA, 6);
}