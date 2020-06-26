#define F_CPU 32000000
#include "cpu.h"
#include <util/delay.h>

#include "ILI9341.h"
#include "memory.h"
#include "usb.h"
#include "master.h"
#include "images.h"

/************************************************************************/
/* Serial connections using FTDI cable                                  */
/************************************************************************/
/*
   X       X
   X       X
   X       X
   X       X
   BROWN   GREEN
   YELLOW  ORANGE
(2) BLACK  RED (1)
   
*/



/************************************************************************/
/* Initialize IOs                                                       */
/************************************************************************/
void init_ios (void)
{
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);			// STATE
}


void fatal_error(void)
{
	while(1)
	{
		for (uint32_t i = 0; i < F_CPU/100; i++)
			;

		toggle_io(PORTA, 6);
	}
}


/************************************************************************/
/* main()                                                               */
/************************************************************************/
int main(void)
{	
	/* Set CPU clock */
	if (cpu_config_clock(F_CPU, true, true) == false)
		while(1);
	
	/* Initialize communication with computer */
	init_usb_serial();

	/* Initialize communication with master */
	init_master_serial();
	
	/* Initialize ios */
	init_ios();
	init_lcd_ios();
	init_memory_ios();
	init_usb_ios();
	
	/* Prepare external hardware */
	reset_lcd();
			
	if (check_lcd_ID())
	{		
		set_backlight_intensity(0);
		lcd_begin();
		//disable_all_pixels();
		
		set_backlight_intensity(125*10);	// Max for the "BOOTING THE SYSTEM ..." initial lingo
	}
	else
	{
		fatal_error();
	}
	
	if (read_memory_size() != 2)
		fatal_error();
	
	/* Enable interrupts */
	PMIC_CTRL = PMIC_CTRL | PMIC_RREN_bm | PMIC_LOLVLEN_bm;
	__asm volatile("sei");
	
	while(1);
}