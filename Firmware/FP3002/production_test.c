#include "production_test.h"
#include "app_ios_and_regs.h"
#define F_CPU 32000000
#include <util/delay.h>

#include "dac.h"

uint8_t production_test_state = 0;
#define PRODUCTION_TEST_SPEED_MS 500
uint16_t production_test_speed = 0;

uint8_t first_time = true;

/* EXT_IO_1 */
#define set_TEST_EXT_IO_1 set_io(PORTD, 4)
#define clr_TEST_EXT_IO_1 clear_io(PORTD, 4)

/* EXT_IO_2 */
#define set_TEST_EXT_IO_2 set_io(PORTD, 6)
#define clr_TEST_EXT_IO_2 clear_io(PORTD, 6)

/* EXT_IO_3 */
#define set_TEST_EXT_IO_3 set_io(PORTF, 6)
#define clr_TEST_EXT_IO_3 clear_io(PORTF, 6)

/* EXT_IO_4 */
#define set_TEST_EXT_IO_4 set_io(PORTF, 7)
#define clr_TEST_EXT_IO_4 clear_io(PORTF, 7)

/* EXT_IO_5 */
#define set_TEST_EXT_IO_5 set_io(PORTA, 1)
#define clr_TEST_EXT_IO_5 clear_io(PORTA, 1)

/* EXT_IO_6 */
#define set_TEST_EXT_IO_6 set_io(PORTA, 2)
#define clr_TEST_EXT_IO_6 clear_io(PORTA, 2)

/* EXT_IO_RX */
#define set_TEST_EXT_IO_RX set_io(PORTC, 2)
#define clr_TEST_EXT_IO_RX clear_io(PORTC, 2)

/* EXT_IO_TX */
#define set_TEST_EXT_IO_TX set_io(PORTC, 3)
#define clr_TEST_EXT_IO_TX clear_io(PORTC, 3)

void exec_production_test(void)
{	
	bool ios_ok;
	
	if (first_time == true)
	{
		first_time = false;
		
		/* Set all external IO lines to output */
		io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_1
		io_pin2out(&PORTD, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_2
		io_pin2out(&PORTF, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_3
		io_pin2out(&PORTF, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_4
		io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_5
		io_pin2out(&PORTA, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_6
		io_pin2out(&PORTC, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_RX
		io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EXT_IO_TX
		
		/* Point OUT0 to laser TTL to test the line */
		set_EN_INT_LASER;
	}
	
	if (++production_test_speed >= PRODUCTION_TEST_SPEED_MS)
	{
		production_test_speed = 0;		
		
		switch(production_test_state)
		{
			case 0: set_POT_CLK; clr_POT_SDI; set_POT_CS_560; set_POT_CS_470; set_POT_CS_410; break;	
			case 1: clr_POT_CLK; set_POT_SDI; set_POT_CS_560; set_POT_CS_470; set_POT_CS_410; break;
			case 2: clr_POT_CLK; clr_POT_SDI; set_POT_CS_560; set_POT_CS_470; set_POT_CS_410; break;
			case 3: clr_POT_CLK; clr_POT_SDI; clr_POT_CS_560; set_POT_CS_470; set_POT_CS_410; break;
			case 4: clr_POT_CLK; clr_POT_SDI; set_POT_CS_560; clr_POT_CS_470; set_POT_CS_410; break;
			case 5: clr_POT_CLK; clr_POT_SDI; set_POT_CS_560; set_POT_CS_470; clr_POT_CS_410; break;
			case 6: clr_POT_CLK; clr_POT_SDI; set_POT_CS_560; set_POT_CS_470; set_POT_CS_410; break;
			
			case 7:  set_CAM_GPIO2; clr_CAM_GPIO3; break;
			case 8:  clr_CAM_GPIO2; set_CAM_GPIO3; break;
			case 9:  clr_CAM_GPIO2; clr_CAM_GPIO3; break;
			
			case 10: set_DAC_SCLK_LASER; set_DAC_CS_LASER; clr_OUT0; clr_DAC_MOSI_LASER; break;
			case 11: clr_DAC_SCLK_LASER; clr_DAC_CS_LASER; clr_OUT0; clr_DAC_MOSI_LASER; break;
			case 12: clr_DAC_SCLK_LASER; set_DAC_CS_LASER; set_OUT0; clr_DAC_MOSI_LASER; break;
			case 13: clr_DAC_SCLK_LASER; set_DAC_CS_LASER; clr_OUT0; set_DAC_MOSI_LASER; break;
			case 14: clr_DAC_SCLK_LASER; set_DAC_CS_LASER; clr_OUT0; clr_DAC_MOSI_LASER; break;			

			case 15: set_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 16: clr_TEST_EXT_IO_1; set_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 17: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; set_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 18: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; set_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 19: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; set_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 20: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; set_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 21: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; set_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			case 22: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; set_TEST_EXT_IO_TX; break;
			case 23: clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX; break;
			
			case 24:
					ios_ok = true;
				
					set_OUT0; set_EN_OUT0; set_OUT1; set_EN_OUT1; set_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (read_IN0 && read_IN1); else ios_ok = false;
	
					set_OUT0; set_EN_OUT0; set_OUT1; set_EN_OUT1; clr_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (!read_IN0 && read_IN1); else ios_ok = false;
	
					set_OUT0; set_EN_OUT0; set_OUT1; set_EN_OUT1; set_EN_IN0; clr_EN_IN1;
					_delay_us(25);
					if (read_IN0 && !read_IN1); else ios_ok = false;
	
					set_OUT0; clr_EN_OUT0; set_OUT1; set_EN_OUT1; set_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (!read_IN0 && read_IN1); else ios_ok = false;
	
					set_OUT0; set_EN_OUT0; set_OUT1; clr_EN_OUT1; set_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (read_IN0 && !read_IN1); else ios_ok = false;
	
					clr_OUT0; set_EN_OUT0; set_OUT1; set_EN_OUT1; set_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (!read_IN0 && read_IN1); else ios_ok = false;
	
					set_OUT0; set_EN_OUT0; clr_OUT1; set_EN_OUT1; set_EN_IN0; set_EN_IN1;
					_delay_us(25);
					if (read_IN0 && !read_IN1); else ios_ok = false;
				
					if (ios_ok == true)
					{		
						set_POT_CLK; set_POT_SDI; clr_POT_CS_560; clr_POT_CS_470; clr_POT_CS_410;
						set_CAM_GPIO2; set_CAM_GPIO3;
						set_DAC_SCLK_LASER; clr_DAC_CS_LASER; set_OUT0; set_DAC_MOSI_LASER;
						set_TEST_EXT_IO_1; set_TEST_EXT_IO_2; set_TEST_EXT_IO_3; set_TEST_EXT_IO_4; set_TEST_EXT_IO_5; set_TEST_EXT_IO_6; set_TEST_EXT_IO_RX; set_TEST_EXT_IO_TX;	
					}
				
					break;
		
			case 25:
				clr_POT_CLK; clr_POT_SDI; set_POT_CS_560; set_POT_CS_470; set_POT_CS_410;
				clr_CAM_GPIO2; clr_CAM_GPIO3;
				clr_DAC_SCLK_LASER; set_DAC_CS_LASER; clr_OUT0; clr_DAC_MOSI_LASER;
				clr_TEST_EXT_IO_1; clr_TEST_EXT_IO_2; clr_TEST_EXT_IO_3; clr_TEST_EXT_IO_4; clr_TEST_EXT_IO_5; clr_TEST_EXT_IO_6; clr_TEST_EXT_IO_RX; clr_TEST_EXT_IO_TX;	
				break;
		}
		
		if (read_CAM_STROBE)
		{
			switch(production_test_state)
			{
				case 0:
				set_dac_L470(9920);		// ~10 mA
				set_dac_L560(0);
				set_dac_L410(0);
				break;					
			
				case 2:
				set_dac_L470(0);
				set_dac_L560(9920);		// ~10 mA
				set_dac_L410(0);
				break;			
			
				case 4:
				set_dac_L470(0);
				set_dac_L560(0);
				set_dac_L410(9920);		// ~10 mA
				break;			
			
				case 6:
				set_dac_L470(0);
				set_dac_L560(0);
				set_dac_L410(0);
				break;			
			
				case 8:
				set_dac_L470(19200);	// ~300 mA
				set_dac_L560(0);
				set_dac_L410(0);
				break;
			
				case 10:
				set_dac_L470(0);
				set_dac_L560(19200);	// ~300 mA
				set_dac_L410(0);
				break;
			
				case 12:
				set_dac_L470(0);
				set_dac_L560(0);
				set_dac_L410(19200);	// ~300 mA
				break;
						
				case 14:
				set_dac_L470(0);
				set_dac_L560(0);
				set_dac_L410(0);
				break;
			
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
				set_dac_L470(0);
				set_dac_L560(0);
				set_dac_L410(0);		
			}
		}
		else
		{			
			set_dac_L470(25600);	// ~500 mA
			set_dac_L560(25600);	// ~500 mA
			set_dac_L410(25600);	// ~500 mA
		}
		
		if (++production_test_state > 25)
			production_test_state = 0;
	}
	
	if (!read_CAM_STROBE)
		set_CAM_TRIGGER;
	else
		clr_CAM_TRIGGER;
}