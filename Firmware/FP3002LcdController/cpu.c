// Author: Filipe Carvalho
#include <avr/io.h>
#include <avr/interrupt.h>
#include "cpu.h"

/************************************************************************/
/* CPU - Clock                                                          */
/************************************************************************/
static int8_t clock_prescaler (uint32_t clock_freq);

bool cpu_config_clock(uint32_t cpu_freq, bool lock_clock, bool external_clock)
{
	/* Configure and enable oscillators */
	if (external_clock)
	{
		OSC_XOSCCTRL = OSC_X32KLPM_bm | OSC_XOSCSEL_EXTCLK_gc;	// Enable low-power mode for a 32.768 kHz oscillator
		// Select External Clock
		OSC_CTRL = OSC_XOSCEN_bm;											// Selected external clock
		loop_until_bit_is_set(OSC_STATUS, OSC_XOSCRDY_bp);			// Wait until external clock source is ready and stable
	}
	else
	{
		OSC_XOSCCTRL = OSC_X32KLPM_bm | OSC_XOSCSEL_32KHz_gc;		// Enable low-power mode for a 32.768 kHz oscillator
		// Select crystal type and the start-up time
		OSC_CTRL = OSC_RC32MEN_bm | OSC_XOSCEN_bm;					// Enable internal 32MHz oscillator and selected external clock
		loop_until_bit_is_set(OSC_STATUS, OSC_RC32MRDY_bp);		// Wait until 32MHz oscillator is ready and stable
		loop_until_bit_is_set(OSC_STATUS, OSC_XOSCRDY_bp);			// Wait until external clock source is ready and stable
	}
	
	
	/* Set up prescaler */
	if (clock_prescaler(cpu_freq) == -1)
	return false;
	if (external_clock)
	{
		CPU_CCP	= CCP_IOREG_gc;									// Disable 'Protected IO Register'
		CLK_CTRL = CLK_SCLKSEL_XOSC_gc;							// Select external oscillator or clock for the system clock source *
	}
	else
	{
		CPU_CCP	= CCP_IOREG_gc;									// Disable 'Protected IO Register'
		CLK_CTRL = CLK_SCLKSEL_RC32M_gc;							// Select 32MHz for the system clock source *
	}
	uint8_t prescaler = (clock_prescaler(cpu_freq) << 2) | CLK_PSBCDIV_1_1_gc;
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	CLK_PSCTRL = prescaler;											// Division ratio of the prescaler A

	/* Enable the DFLL for the 32MHz internal clock*/
	if (!external_clock)
	{
		CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
		OSC_DFLLCTRL = OSC_RC32MCREF_XOSC32K_gc;					// Select the 32.768 crystal oscillator as the calibration source
		DFLLRC32M_CTRL = DFLL_ENABLE_bm;								// Enable calibration of the internal oscillator
	}

	/* Lock clock prescaler */
	if (lock_clock) {
		CPU_CCP	= CCP_IOREG_gc;									// Disable 'Protected IO Register'
		CLK_LOCK = CLK_LOCK_bm;										// Further prescaler change is not allowed
	}
	
	/* Disable watchdog */
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	WDT_CTRL =  WDT_CEN_bm;											// Disable watchdog
	
	return true;
}

void cpu_config_clock_32Khz(uint8_t prescaler, bool lock_clock)
{
	/* Configure and enable oscillators */
	OSC_XOSCCTRL = OSC_X32KLPM_bm | OSC_XOSCSEL_32KHz_gc;	// Enable low-power mode for a 32.768 kHz oscillator
																			// Select crystal type and the start-up time
	OSC_CTRL = OSC_XOSCEN_bm;										// Enable external clock
	loop_until_bit_is_set(OSC_STATUS, OSC_XOSCRDY_bp);		// Wait until external clock source is ready and stable
	
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	CLK_CTRL = CLK_SCLKSEL_XOSC_gc;								// Select external clock for the system clock source
	
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	CLK_PSCTRL = prescaler;								// Division ratio of the prescaler A
	
	/* Lock clock prescaler */
	if (lock_clock) {
		CPU_CCP	= CCP_IOREG_gc;									// Disable 'Protected IO Register'
		CLK_LOCK = CLK_LOCK_bm;										// Further prescaler change is not allowed
	}
	
	/* Disable watchdog */
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	WDT_CTRL =  WDT_CEN_bm;											// Disable watchdog
}



bool cpu_change_clock(uint32_t cpu_freq, bool lock_clock)
{
	/* Set up prescaler */
	if (clock_prescaler(cpu_freq) == -1)
		return false;
	
	uint8_t prescaler = (clock_prescaler(cpu_freq) << 2) | CLK_PSBCDIV_1_1_gc;
	CPU_CCP	= CCP_IOREG_gc;										// Disable 'Protected IO Register'
	CLK_PSCTRL = prescaler;											// Division ratio of the prescaler A
	
	/* Lock clock prescaler */
	if (lock_clock) {
		CPU_CCP	= CCP_IOREG_gc;									// Disable 'Protected IO Register'
		CLK_LOCK = CLK_LOCK_bm;										// Further prescaler change is not allowed
	}
	
	/* Check */
	if (CLK_PSCTRL == ((clock_prescaler(cpu_freq) << 2) | CLK_PSBCDIV_1_1_gc))
		return true;
	else
		return false;
}

static int8_t clock_prescaler (uint32_t cpu_freq)
{
	if (cpu_freq == 32000000)	return 0;
	if (cpu_freq == 16000000)	return 1;
	if (cpu_freq ==  8000000)	return 3;
	if (cpu_freq ==  4000000)	return 5;
	if (cpu_freq ==  2000000)	return 7;
	if (cpu_freq ==  1000000)	return 9;
	if (cpu_freq ==   500000)	return 11;
	if (cpu_freq ==   250000)	return 13;
	if (cpu_freq ==   125000)	return 15;
	if (cpu_freq ==    62500)	return 17;
	return -1;
}

void cpu_clk_output(bool stay_on_loop, bool pin4_instead_of_7, uint8_t out_port)
{
	if (out_port == OUT_PORT_CLK_PORTC)
	{
		if(!pin4_instead_of_7)
		{
			PORTC.DIR |= 1<<7;
			PORTC.PIN7CTRL = PORT_OPC_TOTEM_gc | PORT_ISC_INPUT_DISABLE_gc;
		}
		else
		{
			PORTC.DIR |= 1<<4;
			PORTC.PIN4CTRL = PORT_OPC_TOTEM_gc | PORT_ISC_INPUT_DISABLE_gc;
		}
	}
	else
	{
		if(!pin4_instead_of_7)
		{
			PORTD.DIR |= 1<<7;
			PORTD.PIN7CTRL = PORT_OPC_TOTEM_gc | PORT_ISC_INPUT_DISABLE_gc;
		}
		else
		{
			PORTD.DIR |= 1<<4;
			PORTD.PIN4CTRL = PORT_OPC_TOTEM_gc | PORT_ISC_INPUT_DISABLE_gc;
		}
	}
	
	uint8_t reg;
	if (out_port == OUT_PORT_CLK_PORTC)
		reg = PORTCFG_CLKOUT_PC7_gc | PORTCFG_CLKOUTSEL_CLK4X_gc;
	else
		reg = PORTCFG_CLKOUT_PD7_gc | PORTCFG_CLKOUTSEL_CLK4X_gc;
	
	if (pin4_instead_of_7)
		reg |= PORTCFG_CLKEVPIN_bm;
		
	PORTCFG_CLKEVOUT = reg;
	SLEEP_CTRL = SLEEP_SMODE_IDLE_gc | SLEEP_SEN_bm;
	while(stay_on_loop)
	{
		__asm volatile("sleep");
	}
}

/************************************************************************/
/* CPU - High level interrupt enable                                    */
/************************************************************************/
void cpu_enable_int_level(uint8_t int_level)
{
	PMIC_CTRL = PMIC_CTRL | PMIC_RREN_bm | (1 << (int_level-1));
	sei();
}

void cpu_disable_int_level(uint8_t int_level)
{
	PMIC_CTRL &= ~(1 << (int_level-1));
	if (!(PMIC_CTRL & 0x07))
		cli();
}

/************************************************************************/
/* Watchdog                                                             */
/************************************************************************/
void wdt_enable(uint8_t per)
{
	CPU_CCP	= CCP_IOREG_gc;
	WDT_CTRL	= per | WDT_WEN_bm | WDT_WCEN_bm;
}

void wdt_disable(void)
{
	CPU_CCP	= CCP_IOREG_gc;
	WDT_CTRL	= WDT_WCEN_bm;
}

void wdt_reset_device(void)
{
	wdt_enable(PER_WDT_8ms);
	while(1);
}

/************************************************************************/
/* IO                                                                   */
/************************************************************************/
void io_pin2in(PORT_t* port, uint8_t pin, uint8_t pull, uint8_t sense)
{
	port->DIR &= ~(1<<pin);
	*((&port->PIN0CTRL)+pin) =	(*((&port->PIN0CTRL)+pin) & 0xC0) | pull | sense;
}

void io_pin2out(PORT_t* port, uint8_t pin, uint8_t out, bool input_en)
{
	port->DIR |= 1<<pin;
	*((&port->PIN0CTRL)+pin) = (*((&port->PIN0CTRL)+pin) & 0xC0) | out;
	if (!input_en)
		*((&port->PIN0CTRL)+pin) |= 0x7;
}

void io_pin2out_with_interrupt(PORT_t* port, uint8_t pin, uint8_t out, uint8_t sense)
{
	port->DIR |= 1<<pin;
	*((&port->PIN0CTRL)+pin) =	(*((&port->PIN0CTRL)+pin) & 0xC0) | out | sense;
}

void io_set_int(PORT_t* port, uint8_t int_level, uint8_t int_n, uint8_t mask, bool reset_mask)
{
		
	/* Enable interrupt */
	port->INTCTRL = (port->INTCTRL & ~(0x03 << (int_n*2))) | (int_level << (int_n*2));
		
	/* Set io mask */
	if (int_n == 0)
	{
		if (reset_mask)
			port->INT0MASK = mask;
		else
			port->INT0MASK |= mask;
	}		
	if (int_n == 1)
	{
		if (reset_mask)
			port->INT1MASK = mask;
		else
			port->INT1MASK |= mask;
	}	
		
	/* Reset flag */
	port->INTFLAGS |= 1 << int_n;
}

/************************************************************************/
/* Timer - Type 0                                                       */
/************************************************************************/
void timer_type0_enable(TC0_t* timer, uint8_t prescaler, uint16_t target_count, uint8_t int_level) {
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
	timer->CTRLFSET = TC_CMD_RESET_gc;		// Timer reset (registers to initial value)
	timer->PER = target_count-1;			// Set up target
	timer->INTCTRLA = int_level;			// Enable timer1 overflow interrupt
	timer->CTRLA = prescaler;				// Start timer
}

void timer_type0_pwm(TC0_t* timer, uint8_t prescaler, uint16_t target_count, uint16_t duty_cycle_count, uint8_t int_level_ovf, uint8_t int_level_cca) {
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
	timer->CTRLFSET = TC_CMD_RESET_gc;		// Timer reset (registers to initial value)
	timer->PER = target_count-1;			// Set up target
	timer->CCA = duty_cycle_count;		    // Set duty cycle
	timer->INTCTRLA = int_level_ovf;			// Enable overflow interrupt
	timer->INTCTRLB = int_level_cca;			// Enable compare interrupt on channel A
	timer->CTRLB = TC0_CCAEN_bm | TC_WGMODE_SINGLESLOPE_gc;
											// Enable channel A and single slope mode
	timer->CTRLA = prescaler;				// Start timer
}

void timer_type0_set_target(TC0_t* timer, uint16_t target_count) {
	timer->PER = target_count-1;				// Set up target
}

bool timer_type0_get_flag(TC0_t* timer)
{
	return (timer->INTFLAGS & TC0_OVFIF_bm) ? true : false;
}

void timer_type0_reset_flag(TC0_t* timer)
{
	timer->INTFLAGS |=  TC0_OVFIF_bm;
}

void timer_type0_stop(TC0_t* timer)
{
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
}

void timer_type0_set_counter(TC0_t* timer, uint16_t counter) {
	uint16_t timer_prescaler = timer->CTRLA;
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped
	timer->CNT = counter-1;					// Set up counter
	timer->CTRLA = timer_prescaler;
}

void timer_type0_wait(TC0_t* timer, uint8_t prescaler, uint16_t target_count) {
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
	timer->PER = target_count-1;			// Set up target
	timer->CTRLA = prescaler;				// Start timer
	while(!(timer->INTFLAGS & TC0_OVFIF_bm));
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
}

bool calculate_timer_16bits(uint32_t f_cpu, float freq, uint8_t * timer_prescaler, uint16_t * timer_target_count)
{
	if (freq == 0)
		return false;	
	
    uint32_t  ratio = (f_cpu/freq) + 0.5;		// The integer division always rounds to floor
	// but if we add 0.5 to the result, the error is minimized
	// Ex: 9/10 = 0.9 -> 0 --> err = 0.9
	//     9/10 + 0.5 = 1.4 --> 1 --> err = 0.4
	
	if (ratio < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV1;
	else if ((ratio /= 2) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV2;
	else if ((ratio /= 2) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV4;
	else if ((ratio /= 2) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV8;
	else if ((ratio /= 8) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV64;
	else if ((ratio /= 4) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV256;
	else if ((ratio /= 4) < 65536)
		*timer_prescaler = TIMER_PRESCALER_DIV1024;
	else
		return false; // The frequency value is too low since the ratio is too big

	*timer_target_count = ratio;
	
	return true;
}

/************************************************************************/
/* Timer - Type 1                                                       */
/************************************************************************/
void timer_type1_enable(TC1_t* timer, uint8_t prescaler, uint16_t target_count, uint8_t int_level) {
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
	timer->PER = target_count-1;			// Set up target
	timer->INTCTRLA = int_level;			// Enable timer1 overflow interrupt
	timer->CTRLA = prescaler;				// Start timer
}

void timer_type1_set_target(TC1_t* timer, uint16_t target_count) {
	timer->PER = target_count-1;				// Set up target
}

bool timer_type1_get_flag(TC1_t* timer)
{
	return (timer->INTFLAGS & TC1_OVFIF_bm) ? true : false;
}

void timer_type1_reset_flag(TC1_t* timer)
{
	timer->INTFLAGS |=  TC1_OVFIF_bm;
}

void timer_type1_stop(TC1_t* timer)
{
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
}

void timer_type1_set_counter(TC1_t* timer, uint16_t counter) {
	uint16_t timer_prescaler = timer->CTRLA;
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped
	timer->CNT = counter-1;					// Set up counter
	timer->CTRLA = timer_prescaler;
}

void timer_type1_wait(TC1_t* timer, uint8_t prescaler, uint16_t target_count) {
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Make sure timer is stopped to make reset
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
	timer->PER = target_count-1;			// Set up target
	timer->CTRLA = prescaler;				// Start timer
	while(!(timer->INTFLAGS & TC1_OVFIF_bm));
	timer->CTRLA = TC_CLKSEL_OFF_gc;		// Stop timer
	timer->CTRLFSET = TC_CMD_RESET_gc;	// Timer reset (registers to initial value)
}
/************************************************************************/
/* EEPROM                                                               */
/************************************************************************/
uint8_t eeprom_rd_byte(uint16_t addr)
{
	while(NVM_STATUS & NVM_NVMBUSY_bm);
	
	NVM_ADDR0 = (uint8_t)addr;
	NVM_ADDR1 = (uint8_t)(addr >> 8);
	NVM_ADDR2 = 0;
	
	NVM_CMD = NVM_CMD_READ_EEPROM_gc;
	
	CPU_CCP	= CCP_IOREG_gc;
	NVM_CTRLA = NVM_CMDEX_bm;
	
	return NVM_DATA0;
}

void eeprom_wr_byte(uint16_t addr, uint8_t byte)
{
	while(NVM_STATUS & NVM_NVMBUSY_bm);
	
	/* flush buffer */
	if ((NVM_STATUS & NVM_EELOAD_bm) != 0) {
		NVM_CMD = NVM_CMD_ERASE_EEPROM_BUFFER_gc;
		CPU_CCP	= CCP_IOREG_gc;
		NVM_CTRLA = NVM_CMDEX_bm;
	}
	
	NVM_CMD = NVM_CMD_LOAD_EEPROM_BUFFER_gc;
	
	NVM_ADDR0 = (uint8_t)addr;
	NVM_ADDR1 = (uint8_t)(addr >> 8);
	NVM_ADDR2 = 0;
	
	NVM_DATA0 = byte;
	
	NVM_CMD = NVM_CMD_ERASE_WRITE_EEPROM_PAGE_gc;
	CPU_CCP	= CCP_IOREG_gc;
	NVM_CTRLA = NVM_CMDEX_bm;
}

void eeprom_wr_i16(uint16_t addr, int16_t _16b)
{
	eeprom_wr_byte(addr, *((uint8_t*)(&_16b)));
	eeprom_wr_byte(addr+1, *(((uint8_t*)(&_16b))+1));
}

void eeprom_wr_i32(uint16_t addr, int32_t _32b)
{
	eeprom_wr_byte(addr, *((uint8_t*)(&_32b)));
	eeprom_wr_byte(addr+1, *(((uint8_t*)(&_32b))+1));
	eeprom_wr_byte(addr+2, *(((uint8_t*)(&_32b))+2));
	eeprom_wr_byte(addr+3, *(((uint8_t*)(&_32b))+3));
}

int16_t eeprom_rd_i16(uint16_t addr)
{
	int16_t _16b;
	*((uint8_t*)(&_16b)) = eeprom_rd_byte(addr);
	*(((uint8_t*)(&_16b))+1) = eeprom_rd_byte(addr+1);
	return _16b;
}

int32_t eeprom_rd_i32(uint16_t addr)
{
	int16_t _32b;
	*((uint8_t*)(&_32b)) = eeprom_rd_byte(addr);
	*(((uint8_t*)(&_32b))+1) = eeprom_rd_byte(addr+1);
	*(((uint8_t*)(&_32b))+2) = eeprom_rd_byte(addr+2);
	*(((uint8_t*)(&_32b))+3) = eeprom_rd_byte(addr+3);
	return _32b;
}



/************************************************************************/
/* ADC                                                                  */
/************************************************************************/
static int16_t adc_A_offset = 180;

void adc_A_initialize_single_ended(uint8_t analog_reference)
{
   /* Initialize ADC */
   PR_PRPA &= ~(PR_ADC_bm);									// Remove power reduction
   ADCA_CTRLA = ADC_ENABLE_bm;								// Enable ADCA
   ADCA_CTRLB = ADC_CURRLIMIT_HIGH_gc;						// High current limit, max. sampling rate 0.5MSPS
   ADCA_CTRLB  |= ADC_RESOLUTION_12BIT_gc;				// 12-bit result, right adjusted
   ADCA_REFCTRL = analog_reference;
   ADCA_PRESCALER = ADC_PRESCALER_DIV128_gc;				// 250 ksps
   // Propagation Delay = (1 + 12[bits]/2 + 1[gain]) / fADC[125k] = 32 us
   // Note: For single measurements, Propagation Delay is equal to Conversion Time
   
   ADCA_CH0_CTRL = ADC_CH_INPUTMODE_SINGLEENDED_gc;	// Single-ended positive input signal
   ADCA_CH0_INTCTRL = ADC_CH_INTMODE_COMPLETE_gc;		// Rise interrupt flag when conversion is complete
   ADCA_CH0_INTCTRL |= ADC_CH_INTLVL_OFF_gc;				// Interrupt is not used
}

int16_t adc_A_read_channel(uint8_t index)
{
   /* Read ADC */
   ADCA_CH0_MUXCTRL = index << 3;							// Select pin
   ADCA_CH0_CTRL |= ADC_CH_START_bm;						// Force the first conversion
   while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));		// Wait for conversion to finish
   ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;						// Clear interrupt bit

   return ((int16_t)(ADCA_CH0_RES & 0x0FFF)) - adc_A_offset;
}

void adc_A_calibrate_offset(uint8_t index)
{
   for (uint8_t i = 0; i < 3; i++)
   {
      /* Measure and safe adc offset */
      ADCA_CH0_MUXCTRL = index << 3;						// Select pin
      ADCA_CH0_CTRL |= ADC_CH_START_bm;					// Start conversion
      while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));	// Wait for conversion to finish
      ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;					// Clear interrupt bit
   }
   
   adc_A_offset = ADCA_CH0_RES;								// Read offset
}
uint16_t adcA_unsigned_single_ended(ADC_t* adc, uint8_t res, uint8_t ref , uint8_t prescaler, uint8_t adc_pin, TC0_t* timer)
{
	/* Remove power reduction */
	PR_PRPA &= ~(PR_ADC_bm);
	
	/* Configure ADC */
	adc->CTRLA = ADC_ENABLE_bm;
	adc->CTRLB = ADC_CURRLIMIT_HIGH_gc | res;
	adc->REFCTRL = ref;
	adc->PRESCALER = prescaler;
	
	/* Configure channel - using channel 0 */
	adc->CH0.CTRL = ADC_CH_INPUTMODE_SINGLEENDED_gc;
	adc->CH0.MUXCTRL = adc_pin << 3;
	adc->CH0.INTCTRL = ADC_CH_INTMODE_COMPLETE_gc | ADC_CH_INTLVL_LO_gc;
	
	/* Wait 10 us */
	timer_type0_enable(timer, TIMER_PRESCALER_DIV2, 80, INT_LEVEL_OFF);
	while(!timer_type0_get_flag(timer));
	timer_type0_stop(timer);
	
	/* Start conversion */
	adc->CH0.CTRL |= ADC_CH_START_bm;
	while(!(adc->CH0.INTFLAGS & ADC_CH_CHIF_bm));
	adc->CH0.INTFLAGS = ADC_CH_CHIF_bm;

	/* Read conversion */
	uint16_t bat = adc->CH0.RES;
	
	/* Disable ADC */
	adc->CTRLA &= ~(ADC_ENABLE_bm);
	
	/* Enable power reduction */
	PR_PRPA |= PR_ADC_bm;
	
	/* Return conversion */
	return bat;
}