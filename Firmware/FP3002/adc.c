#include "cpu.h"
#include "adc.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

#define ADC_CONV_PULSE	set_io_mask(PORTH, 0x38); \
						set_io_mask(PORTH, 0x38); \
						 \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); /* 312 ns */ \
						 \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); /* 624 ns */ \
						 \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); \
						clear_io_mask(PORTH, 0x38); /* 812.5 ns */
	
#define ADC_ACCUMULATOR_TARGET 128			// 128 is ok in terms of data
											// This number needs to be a 2^n
#define ACCUMULATOR_INTERVAL_uS_FLOAT 1
#define PHOTODIODES_FRAME_RATE_uS_INT 500

uint8_t adc_accumulator_counter;
uint8_t photodiodes_interleave_counter;

#define START_ACCUMULATOR_TIMER	do { \
			adc_accumulator_counter = 1; \
			set_io_mask(PORTH, 0x38); \
			set_io_mask(PORTH, 0x38); \
			clear_io_mask(PORTH, 0x38); \
			timer_type1_enable(&TCD1, TIMER_PRESCALER_DIV1,  ACCUMULATOR_INTERVAL_uS_FLOAT * 32,   INT_LEVEL_LOW); \
			} while (0)

#define START_FRAME_RATE_TIMER do { \
			timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV64, PHOTODIODES_FRAME_RATE_uS_INT >> 1, INT_LEVEL_LOW); \
			photodiodes_interleave_counter = 0; \
			} while(0)
			
ISR(TCE1_OVF_vect, ISR_NAKED)
{
	timer_type1_stop(&TCE1);
	START_FRAME_RATE_TIMER;
	START_ACCUMULATOR_TIMER;
	reti();
}

ISR(TCD0_OVF_vect, ISR_NAKED)
{
	START_ACCUMULATOR_TIMER;
	reti();
}

ISR(TCD1_OVF_vect, ISR_NAKED)
{
	if (adc_accumulator_counter < ADC_ACCUMULATOR_TARGET)
	{
		set_io_mask(PORTH, 0x38);
		set_io_mask(PORTH, 0x38);
		clear_io_mask(PORTH, 0x38);
				
		adc_accumulator_counter++;
	}
	else if(adc_accumulator_counter == ADC_ACCUMULATOR_TARGET)	
	{
		timer_type1_stop(&TCD1);		
		
		uint16_t adc[3] = {0, 0, 0};
		
		/*
		//set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<13); if (read_ADC_MISO_470) adc[1] |= (1<<13); if (read_ADC_MISO_560) adc[2] |= (1<<13); clear_io_mask(PORTE, 0x54);
		//set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<12); if (read_ADC_MISO_470) adc[1] |= (1<<12); if (read_ADC_MISO_560) adc[2] |= (1<<12); clear_io_mask(PORTE, 0x54);	
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<11); if (read_ADC_MISO_470) adc[1] |= (1<<11); if (read_ADC_MISO_560) adc[2] |= (1<<11); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<10); if (read_ADC_MISO_470) adc[1] |= (1<<10); if (read_ADC_MISO_560) adc[2] |= (1<<10); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<9);  if (read_ADC_MISO_470) adc[1] |= (1<<9);  if (read_ADC_MISO_560) adc[2] |= (1<<9);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<8);  if (read_ADC_MISO_470) adc[1] |= (1<<8);  if (read_ADC_MISO_560) adc[2] |= (1<<8);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<7);  if (read_ADC_MISO_470) adc[1] |= (1<<7);  if (read_ADC_MISO_560) adc[2] |= (1<<7);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<6);  if (read_ADC_MISO_470) adc[1] |= (1<<6);  if (read_ADC_MISO_560) adc[2] |= (1<<6);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<5);  if (read_ADC_MISO_470) adc[1] |= (1<<5);  if (read_ADC_MISO_560) adc[2] |= (1<<5);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<4);  if (read_ADC_MISO_470) adc[1] |= (1<<4);  if (read_ADC_MISO_560) adc[2] |= (1<<4);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<3);  if (read_ADC_MISO_470) adc[1] |= (1<<3);  if (read_ADC_MISO_560) adc[2] |= (1<<3);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<2);  if (read_ADC_MISO_470) adc[1] |= (1<<2);  if (read_ADC_MISO_560) adc[2] |= (1<<2);  clear_io_mask(PORTE, 0x54);
	
		//#ifdef USE_ADC_14BITS
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<1);  if (read_ADC_MISO_470) adc[1] |= (1<<1);  if (read_ADC_MISO_560) adc[2] |= (1<<1);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<0);  if (read_ADC_MISO_470) adc[1] |= (1<<0);  if (read_ADC_MISO_560) adc[2] |= (1<<0);  clear_io_mask(PORTE, 0x54);	
		//#endif
		*/
	
		set_io_mask(PORTE, 0x54); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<13); if (read_ADC_MISO_470) adc[1] |= (1<<13); if (read_ADC_MISO_560) adc[2] |= (1<<13); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<12); if (read_ADC_MISO_470) adc[1] |= (1<<12); if (read_ADC_MISO_560) adc[2] |= (1<<12); clear_io_mask(PORTE, 0x54);
	
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<11); if (read_ADC_MISO_470) adc[1] |= (1<<11); if (read_ADC_MISO_560) adc[2] |= (1<<11); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<10); if (read_ADC_MISO_470) adc[1] |= (1<<10); if (read_ADC_MISO_560) adc[2] |= (1<<10); clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<9);  if (read_ADC_MISO_470) adc[1] |= (1<<9);  if (read_ADC_MISO_560) adc[2] |= (1<<9);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<8);  if (read_ADC_MISO_470) adc[1] |= (1<<8);  if (read_ADC_MISO_560) adc[2] |= (1<<8);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<7);  if (read_ADC_MISO_470) adc[1] |= (1<<7);  if (read_ADC_MISO_560) adc[2] |= (1<<7);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<6);  if (read_ADC_MISO_470) adc[1] |= (1<<6);  if (read_ADC_MISO_560) adc[2] |= (1<<6);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<5);  if (read_ADC_MISO_470) adc[1] |= (1<<5);  if (read_ADC_MISO_560) adc[2] |= (1<<5);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<4);  if (read_ADC_MISO_470) adc[1] |= (1<<4);  if (read_ADC_MISO_560) adc[2] |= (1<<4);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<3);  if (read_ADC_MISO_470) adc[1] |= (1<<3);  if (read_ADC_MISO_560) adc[2] |= (1<<3);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<2);  if (read_ADC_MISO_470) adc[1] |= (1<<2);  if (read_ADC_MISO_560) adc[2] |= (1<<2);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<1);  if (read_ADC_MISO_470) adc[1] |= (1<<1);  if (read_ADC_MISO_560) adc[2] |= (1<<1);  clear_io_mask(PORTE, 0x54);
		set_io_mask(PORTE, 0x54); if (read_ADC_MISO_410) adc[0] |= (1<<0);  if (read_ADC_MISO_470) adc[1] |= (1<<0);  if (read_ADC_MISO_560) adc[2] |= (1<<0);  clear_io_mask(PORTE, 0x54);
		
		app_regs.REG_PHOTODIODES[photodiodes_interleave_counter*3 + 0] = adc[0];
		app_regs.REG_PHOTODIODES[photodiodes_interleave_counter*3 + 1] = adc[1];
		app_regs.REG_PHOTODIODES[photodiodes_interleave_counter*3 + 2] = adc[2];
		
		photodiodes_interleave_counter++;
		
		if (photodiodes_interleave_counter == 4)
		{
			photodiodes_interleave_counter = 0;
			core_func_send_event(ADD_REG_PHOTODIODES, true);
		}
	}
	
	reti();
}