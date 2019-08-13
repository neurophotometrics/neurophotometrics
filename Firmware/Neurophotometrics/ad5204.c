#include "ad5204.h"
#include "app_ios_and_regs.h"

void as5204_write_channel(uint8_t channel, uint8_t data)
{	
	set_POT_CS;	// 93.75 ns
	set_POT_CS;
	set_POT_CS;	
	
	for (uint8_t i = 0; i < 3; i++)
	{
		if ((channel<<i) & 0x04)
			set_POT_SDI;
		else
			clr_POT_SDI;
		
		clr_POT_CLK;	// 93.75 ns
		clr_POT_CLK;
		clr_POT_CLK;
		set_POT_CLK;	// 93.75 ns
		set_POT_CLK;
		set_POT_CLK;
		clr_POT_CLK;	// 21.25 ns
	}
	
	for (uint8_t i = 0; i < 8; i++)
	{
		if ((data<<i) & 0x80)
			set_POT_SDI;
		else
			clr_POT_SDI;
			
		clr_POT_CLK;	// 93.75 ns
		clr_POT_CLK;
		clr_POT_CLK;
		set_POT_CLK;	// 93.75 ns
		set_POT_CLK;
		set_POT_CLK;
		clr_POT_CLK;	// 31.25 ns
	}
	
	clr_POT_CS;
}
