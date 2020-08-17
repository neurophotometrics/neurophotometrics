#include "ad5204.h"
#include "app_ios_and_regs.h"


#define XMIT_CHANNEL 	for (uint8_t i = 0; i < 3; i++) \
						{ \
							if ((channel<<i) & 0x04) \
							{ \
								set_POT_SDI; \
							} \
							else \
							{ \
								clr_POT_SDI; \
							} \
							clr_POT_CLK; 	/* 93.75 ns */ \
							clr_POT_CLK; \
							clr_POT_CLK; \
							set_POT_CLK; 	/* 93.75 ns */ \
							set_POT_CLK; \
							set_POT_CLK; \
							clr_POT_CLK; 	/* 21.25 ns */ \
						}

#define XMIT_DATA		for (uint8_t i = 0; i < 8; i++) \
						{ \
							if ((data<<i) & 0x80) \
							{ \
								set_POT_SDI; \
							} \
							else \
							{ \
								clr_POT_SDI; \
							} \
							clr_POT_CLK;	/* 93.75 ns */ \
							clr_POT_CLK; \
							clr_POT_CLK; \
							set_POT_CLK;	/* 93.75 ns */ \
							set_POT_CLK; \
							set_POT_CLK; \
							clr_POT_CLK;	/* 31.25 ns */ \
						}

void as5204_write_channel_L410(uint8_t channel, uint8_t data)
{	
	set_POT_CS_410;	/* 93.75 ns */
	set_POT_CS_410;
	set_POT_CS_410;	
	
	XMIT_CHANNEL;
	XMIT_DATA;
		
	clr_POT_CS_410;
}

void as5204_write_channel_L470(uint8_t channel, uint8_t data)
{
	set_POT_CS_470;	/* 93.75 ns */
	set_POT_CS_470;
	set_POT_CS_470;
	
	XMIT_CHANNEL;
	XMIT_DATA;
	
	clr_POT_CS_470;
}

void as5204_write_channel_L560(uint8_t channel, uint8_t data)
{
	set_POT_CS_560;	/* 93.75 ns */
	set_POT_CS_560;
	set_POT_CS_560;
	
	XMIT_CHANNEL;
	XMIT_DATA;
	
	clr_POT_CS_560;
}

/*
void get_resitor_values(uint8_t gain, uint8_t * rf, uint8_t * rt)
{
	// Gain = 1 + RF/RT
	//
	// R = byte x 10000 / 256 + 45 (Rw)
	
	switch(gain)
	{
		case 1:	*rf = 0;	// Gain = 1 + 45/10005.9375 = 1.00045
				*rt = 255;
				break;
		case 2:	*rf = 96;	// Gain = 1 + 3795/3795 = 2.00000
				*rt = 96;
				break;
		case 3:	*rf = 128;	// Gain = 3.01322
				*rt = 63;
				break;
		case 4:	*rf = 128;	// Gain = 3.99296
				*rt = 42;
				break;
		case 5:	*rf = 191;	// Gain = 4.99053
				*rt = 47;
				break;
		case 6:	*rf = 190;	// Gain = 6.01027
				*rt = 37;
				break;
		case 7:	*rf = 192;	// Gain = 7.00746
				*rt = 31;
				break;
		case 8:	*rf = 189;	// Gain = 8.00324
				*rt = 26;
				break;
	}
}
*/

uint8_t rt_values[32] = {255, 96, 63, 42, 47, 50, 41, 35, 30, 27, 24, 22, 20, 18, 17, 15, 14, 13, 13, 12, 11, 11, 10, 9, 9, 9, 8, 8, 7, 7, 7, 7};
uint8_t rf_values[32] = {0, 96, 127, 128, 191, 255, 252, 252, 248, 252, 250, 254, 253, 248, 253, 241, 241, 239, 254, 249, 242, 254, 244, 232, 242, 253, 237, 246, 227, 235, 243, 252};

void set_gain_L410(uint8_t gain)
{
	//uint8_t rf, rt;
	//get_resitor_values(gain, &rf, &rt);
	as5204_write_channel_L410(1, rf_values[gain-1]);
	as5204_write_channel_L410(0, rt_values[gain-1]);
}

void set_gain_L470(uint8_t gain)
{
	//uint8_t rf, rt;
	//get_resitor_values(gain, &rf, &rt);
	as5204_write_channel_L470(1, rf_values[gain-1]);
	as5204_write_channel_L470(0, rt_values[gain-1]);
}

void set_gain_L560(uint8_t gain)
{
	//uint8_t rf, rt;
	//get_resitor_values(gain, &rf, &rt);
	as5204_write_channel_L560(1, rf_values[gain-1]);
	as5204_write_channel_L560(0, rt_values[gain-1]);
}