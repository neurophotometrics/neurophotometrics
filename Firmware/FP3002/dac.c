#include "dac.h"
#include "app_ios_and_regs.h"

bool dac_L410_state = false;
bool dac_L470_state = false;
bool dac_L560_state = false;

void set_dac_L410(uint16_t content)
{
	dac_L410_state = true;
	
	set_DAC_CS_410;
	
	if (content & (1<<15)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<14)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<13)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<12)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<11)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<10)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<9))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<8))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	if (content & (1<<7))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<6))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<5))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<4))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<3))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<1))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	clr_DAC_CS_410;
}

void set_dac_L470(uint16_t content)
{
	dac_L470_state = true;
	
	set_DAC_CS_470;
	
	if (content & (1<<15)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<14)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<13)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<12)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<11)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<10)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<9))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<8))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	if (content & (1<<7))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<6))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<5))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<4))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<3))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<1))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	clr_DAC_CS_470;
}


/*
#define L560_OFFSET 9280
uint16_t l560_target;
uint16_t l560_current;

uint8_t l560_rump_up_state;

//#define L560_TIMER_TARGET 84 // 168 us
#define L560_TIMER_TARGET 500 // 1 ms
#define L560_TIMER_PRESCALER TIMER_PRESCALER_DIV64

void _set_dac_L560(uint16_t content)
{	
	if(content > L560_OFFSET) // 0.29V
	{
		l560_target = content - L560_OFFSET;
		
		l560_rump_up_state = 0;
		
		//l560_current = L560_OFFSET + l560_target/2; // 1/2
		//l560_current = L560_OFFSET + l560_target/8; // 1/8
		l560_current = L560_OFFSET + l560_target/10; // 1/10
		
		set_dac_L560(l560_current);
		
		TCF1_CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
		TCF1_CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
		TCF1_PER = L560_TIMER_TARGET-1;		// Set up target // 168 us
		TCF1_INTCTRLA = INT_LEVEL_LOW;		// Enable timer1 overflow interrupt
		TCF1_CTRLA = L560_TIMER_PRESCALER;	// Start timer
	}
	else
	{
		set_dac_L560(content);
	}
}

ISR(TCF1_OVF_vect, ISR_NAKED)
{
	l560_rump_up_state++;
	
	// 1.29%
	switch (l560_rump_up_state)
	{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		l560_current += l560_target/10; // + 1/8 = 2/8
		set_dac_L560(l560_current);
		reti();
		
		case 9:
			set_dac_L560(l560_target + L560_OFFSET); // = 1
			TCF1_CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
			TCF1_CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
			reti();
		
	}
	
	// 1.56%
	switch(l560_rump_up_state)
	{
		case 1:
			l560_current += l560_target/8; // + 1/8 = 2/8
			set_dac_L560(l560_current);
			reti();
		
		case 2:
			l560_current += l560_target/4; // + 2/8 = 4/8
			set_dac_L560(l560_current);
			reti();
			
		case 3:
			l560_current += l560_target/4; // + 2/8 = 6/8
			set_dac_L560(l560_current);
			reti();
		
		case 4:
			set_dac_L560(l560_target + L560_OFFSET); // = 1
			TCF1_CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
			TCF1_CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
			reti();		
	}
	
	reti();
	
	switch (l560_rump_up_state)
	{
		case 1:
			l560_current += l560_target/4; // + 1/4 = 3/4
			set_dac_L560(l560_current);
			reti();
		
		case 2:
			l560_current += l560_target/8; // + 1/8 = 7/8
			set_dac_L560(l560_current);
			reti();
		
		case 3:
			set_dac_L560(l560_target + L560_OFFSET); // = 1
			TCF1_CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
			TCF1_CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
			reti();		
	}
	
	reti();
}
*/

void set_dac_L560(uint16_t content)
{
	dac_L560_state = true;
	//l560_current = content;
	
	set_DAC_CS_560;
	
	if (content & (1<<15)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<14)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<13)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<12)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<11)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<10)) set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<9))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<8))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	if (content & (1<<7))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<6))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<5))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<4))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<3))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<2))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	if (content & (1<<1))  set_DAC_MOSI; else clr_DAC_MOSI; set_DAC_SCLK;  clr_DAC_SCLK;
	
	clr_DAC_CS_560;
}



void set_dac_LASER(uint16_t content)
{
	/* Frequency reduced because it's used on a cable              */
	/* Nevertheless, its' working fine without frequency reduction */
	
	set_DAC_CS_LASER;
	set_DAC_CS_LASER;
	set_DAC_CS_LASER;
	
	if (content & (1<<15)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<14)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<13)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<12)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<11)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<10)) {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<9))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<8))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	
	if (content & (1<<7))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<6))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<5))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<4))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<3))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<2))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<2))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	if (content & (1<<1))  {set_DAC_MOSI_LASER; set_DAC_MOSI_LASER;} else {clr_DAC_MOSI_LASER; clr_DAC_MOSI_LASER;} {set_DAC_SCLK_LASER ; set_DAC_SCLK_LASER;}  {clr_DAC_SCLK_LASER ; clr_DAC_SCLK_LASER;}
	
	clr_DAC_CS_LASER;
}

/*
void clr_dac_L410(void)
{
	if (dac_L410_state) {
		dac_L410_state = false;
		set_dac_L410(0);
	}
}

void clr_dac_L470(void)
{
	if (dac_L470_state) {
		dac_L470_state = false;
		set_dac_L470(0);
	}
}

void clr_dac_L560(void)
{
	if (dac_L560_state) {
		dac_L560_state = false;
		set_dac_L560(0);
	}
}
*/


void tgl_dac_L410(uint16_t content)
{
	if (dac_L410_state)
		clr_DAC_L410;
	else
		set_dac_L410(content);
}

void tgl_dac_L470(uint16_t content)
{
	if (dac_L470_state)
		clr_DAC_L470;
	else
		set_dac_L470(content);
}

void tgl_dac_L560(uint16_t content)
{
	if (dac_L560_state)
		clr_DAC_L560;
	else
		set_dac_L560(content);
}


bool read_dac_L410(void) { return dac_L410_state; }
bool read_dac_L470(void) { return dac_L470_state; }
bool read_dac_L560(void) { return dac_L560_state; }