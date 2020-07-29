#ifndef _I2C_TIMING_H_
#define _I2C_TIMING_H_
#include <util/delay.h>


//*****************************************************************************
// NOPs
//*****************************************************************************
#define NOPS_1 asm("nop");
#define NOPS_2 NOPS_1 NOPS_1
#define NOPS_3 NOPS_1 NOPS_1 NOPS_1
#define NOPS_4 NOPS_1 NOPS_1 NOPS_1 NOPS_1
#define NOPS_5 NOPS_1 NOPS_1 NOPS_1 NOPS_1 NOPS_1
#define NOPS_10 NOPS_5 NOPS_5
#define NOPS_50 NOPS_10 NOPS_10 NOPS_10 NOPS_10 NOPS_10
#define NOPS_100 NOPS_50 NOPS_50


//*****************************************************************************
// Define available SCL clock pulses
//*****************************************************************************
#define CLK_32MHZ_200KHz	_delay_us(2); NOPS_10; NOPS_5; NOPS_1 	// 2us + 16CLK
#define CLK_32MHZ_300KHz	_delay_us(1); NOPS_10; NOPS_10; NOPS_2	// 1us + 21.3CLK
#define CLK_32MHZ_400KHz	_delay_us(1); NOPS_5;						// 1us + 8CLK
#define CLK_32MHZ_750KHz	NOPS_10 NOPS_10 NOPS_1 NOPS_1				// 22
#define CLK_32MHZ_1000KHz	NOPS_10 NOPS_5 NOPS_1						// 16
#define CLK_32MHZ_1500KHz	NOPS_10 NOPS_1									// 11
#define CLK_32MHZ_2000KHz	NOPS_5 NOPS_1 NOPS_1 NOPS_1				// 8
#define CLK_32MHZ_2500KHz	NOPS_5 NOPS_1 NOPS_1							// 7
#define CLK_32MHZ_3000KHz	NOPS_5 NOPS_1									// 6
#define CLK_32MHZ_3500KHz	NOPS_5											// 5
#define CLK_32MHZ_4000KHz	NOPS_1 NOPS_1 NOPS_1 NOPS_1				// 4

#define CLK_16MHZ_200KHz	_delay_us(2); NOPS_5; NOPS_3				// 2us + 8CLK
#define CLK_16MHZ_300KHz	_delay_us(1); NOPS_10; NOPS_1				// 1us + 10.7CLK
#define CLK_16MHZ_400KHz	_delay_us(1); NOPS_4 						// 1us + 4CLK
#define CLK_16MHZ_750KHz	NOPS_10 NOPS_1									// 11
#define CLK_16MHZ_1000KHz	NOPS_5 NOPS_1 NOPS_1 NOPS_1				// 8
#define CLK_16MHZ_1500KHz	NOPS_5 NOPS_1									// 6
#define CLK_16MHZ_2000KHz	NOPS_1 NOPS_1 NOPS_1 NOPS_1				// 4
#define CLK_16MHZ_2500KHz	NOPS_1 NOPS_1 NOPS_1 NOPS_1				// 4
#define CLK_16MHZ_3000KHz	NOPS_1 NOPS_1 NOPS_1							// 3
#define CLK_16MHZ_3500KHz	NOPS_1 NOPS_1 NOPS_1							// 3
#define CLK_16MHZ_4000KHz	NOPS_1 NOPS_1									// 2

#define CLK_8MHZ_200KHz		_delay_us(2); NOPS_4							// 2us + 4CLK
#define CLK_8MHZ_300KHz		_delay_us(1); NOPS_5; NOPS_1	 			// 1us + 5.3CLK
#define CLK_8MHZ_400KHz		_delay_us(1);	NOPS_2 						// 1us + 2CLK
#define CLK_8MHZ_750KHz		NOPS_5 NOPS_1									// 6
#define CLK_8MHZ_1000KHz	NOPS_1 NOPS_1 NOPS_1 NOPS_1				// 4
#define CLK_8MHZ_1500KHz	NOPS_5 NOPS_1 NOPS_1							// 7
#define CLK_8MHZ_2000KHz	NOPS_1 NOPS_1									// 2
#define CLK_8MHZ_2500KHz	NOPS_1 NOPS_1									// 2
#define CLK_8MHZ_3000KHz	NOPS_1 NOPS_1									// 2
#define CLK_8MHZ_3500KHz	NOPS_1 NOPS_1									// 2
#define CLK_8MHZ_4000KHz	NOPS_1											// 1

#define CLK_4MHZ_200KHz		_delay_us(2); NOPS_2							// 2us + 2CLK
#define CLK_4MHZ_300KHz		_delay_us(1); NOPS_3							// 1us + 2.7CLK
#define CLK_4MHZ_400KHz		_delay_us(1); NOPS_1 						// 1us + 1CLK
#define CLK_4MHZ_750KHz		NOPS_1 NOPS_1 NOPS_1							// 3
#define CLK_4MHZ_1000KHz	NOPS_1 NOPS_1									// 2
#define CLK_4MHZ_1500KHz	NOPS_1 NOPS_1									// 2
#define CLK_4MHZ_2000KHz	NOPS_1											// 1
#define CLK_4MHZ_2500KHz	NOPS_1											// 1
#define CLK_4MHZ_3000KHz	NOPS_1											// 1
#define CLK_4MHZ_3500KHz	NOPS_1											// 1
#define CLK_4MHZ_4000KHz	NOPS_1											// 1

#define CLK_2MHZ_200KHz		_delay_us(2); NOPS_1 						// 2us + 1CLK
#define CLK_2MHZ_300KHz		_delay_us(1); NOPS_2							// 1us + 1.3CLK
#define CLK_2MHZ_400KHz		_delay_us(1); NOPS_1 						// 1us + 0.5CLK
#define CLK_2MHZ_750KHz		NOPS_1 NOPS_1									// 2
#define CLK_2MHZ_1000KHz	NOPS_1											// 1
#define CLK_2MHZ_1500KHz	NOPS_1											// 1
#define CLK_2MHZ_2000KHz	NOPS_1											// 1
#define CLK_2MHZ_2500KHz	NOPS_1											// 1
#define CLK_2MHZ_3000KHz	NOPS_1											// 1
#define CLK_2MHZ_3500KHz	NOPS_1											// 1
#define CLK_2MHZ_4000KHz	NOPS_1											// 1

#define CLK_1MHZ_200KHz		_delay_us(2); NOPS_1 						// 2us + 0.5CLK
#define CLK_1MHZ_300KHz		_delay_us(1); NOPS_1 						// 1us + 0.67CLK
#define CLK_1MHZ_400KHz		_delay_us(1); NOPS_1 						// 1us + 0.25CLK
#define CLK_1MHZ_750KHz		NOPS_1											// 1
#define CLK_1MHZ_1000KHz	NOPS_1											// 1
#define CLK_1MHZ_1500KHz	NOPS_1											// 1
#define CLK_1MHZ_2000KHz	NOPS_1											// 1
#define CLK_1MHZ_2500KHz	NOPS_1											// 1
#define CLK_1MHZ_3000KHz	NOPS_1											// 1
#define CLK_1MHZ_3500KHz	NOPS_1											// 1
#define CLK_1MHZ_4000KHz	NOPS_1											// 1


//*****************************************************************************
// Define bus standard delays
//*****************************************************************************
#define tHDSTA	_delay_us(2);	// value is 0.6us for 400KHz I2C
#define tSUSTO	_delay_us(2);	// value is 0.6us for 400KHz I2C
#define tSUSTA	_delay_us(2);	// value is 0.6us for 400KHz I2C
#define tBUF	_delay_us(3);	// value is 1.3us for 400KHz I2C


#endif