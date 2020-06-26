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

void set_dac_L560(uint16_t content)
{
	dac_L560_state = true;
	
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