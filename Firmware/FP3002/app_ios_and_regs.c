#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTA, 5, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // KEY_SWITCH
	io_pin2in(&PORTA, 7, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // SW1
	io_pin2in(&PORTH, 1, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // IN0
	io_pin2in(&PORTK, 6, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // IN1
	io_pin2in(&PORTE, 3, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ADC_MISO_410
	io_pin2in(&PORTE, 5, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ADC_MISO_470
	io_pin2in(&PORTE, 7, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ADC_MISO_560
	io_pin2in(&PORTJ, 2, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // SCREEN_IS_USING_USB
	io_pin2in(&PORTH, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // CAM_STROBE
	io_pin2in(&PORTD, 4, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_1
	io_pin2in(&PORTD, 6, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_2
	io_pin2in(&PORTF, 6, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_3
	io_pin2in(&PORTF, 7, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_4
	io_pin2in(&PORTA, 1, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_5
	io_pin2in(&PORTA, 2, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_6
	io_pin2in(&PORTC, 2, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_RX
	io_pin2in(&PORTC, 3, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);            // EXT_IO_TX

	/* Configure input interrupts */
	io_set_int(&PORTA, INT_LEVEL_LOW, 0, (1<<5), false);                 // KEY_SWITCH
	io_set_int(&PORTH, INT_LEVEL_LOW, 0, (1<<1), false);                 // IN0
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<6), false);                 // IN1
	io_set_int(&PORTJ, INT_LEVEL_LOW, 1, (1<<2), false);                 // SCREEN_IS_USING_USB
	io_set_int(&PORTH, INT_LEVEL_LOW, 1, (1<<2), false);                 // CAM_STROBE

	/* Configure output pins */
	io_pin2out(&PORTH, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_INT_LASER
	io_pin2out(&PORTK, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_OUT0
	io_pin2out(&PORTK, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_OUT1
	io_pin2out(&PORTE, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT0
	io_pin2out(&PORTE, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT1
	io_pin2out(&PORTK, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_IN0
	io_pin2out(&PORTK, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_IN1
	io_pin2out(&PORTB, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_CLKIN
	io_pin2out(&PORTB, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_CLKOUT
	io_pin2out(&PORTJ, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_SERIAL_MAIN
	io_pin2out(&PORTH, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_SERIAL_SCREEN
	io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CS_410
	io_pin2out(&PORTB, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CS_470
	io_pin2out(&PORTB, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CS_560
	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CLR_410
	io_pin2out(&PORTB, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CLR_470
	io_pin2out(&PORTB, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CLR_560
	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_SCLK
	io_pin2out(&PORTD, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_MOSI
	io_pin2out(&PORTE, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_SCLK_410
	io_pin2out(&PORTE, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_SCLK_470
	io_pin2out(&PORTE, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_SCLK_560
	io_pin2out(&PORTH, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_CNV_410
	io_pin2out(&PORTH, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_CNV_470
	io_pin2out(&PORTH, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // ADC_CNV_560
	io_pin2out(&PORTJ, 1, OUT_IO_WIREDAND, IN_EN_IO_EN);                 // SCREEN_CAN_USE_USB
	io_pin2out(&PORTK, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CLK
	io_pin2out(&PORTK, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_SDI
	io_pin2out(&PORTJ, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CS_410
	io_pin2out(&PORTJ, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CS_470
	io_pin2out(&PORTJ, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CS_560
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CS_LASER
	io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_SCLK_LASER
	io_pin2out(&PORTC, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_MOSI_LASER
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_TRIGGER
	io_pin2out(&PORTF, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_GPIO2
	io_pin2out(&PORTF, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_GPIO3

	/* Initialize output pins */
	clr_EN_INT_LASER;
	set_EN_OUT0;
	set_EN_OUT1;
	clr_OUT0;
	clr_OUT1;
	set_EN_IN0;
	set_EN_IN1;
	set_EN_CLKIN;
	clr_EN_CLKOUT;
	set_EN_SERIAL_MAIN;
	clr_EN_SERIAL_SCREEN;
	clr_DAC_CS_410;
	clr_DAC_CS_470;
	clr_DAC_CS_560;
	clr_DAC_CLR_410;
	clr_DAC_CLR_470;
	clr_DAC_CLR_560;
	clr_DAC_SCLK;
	clr_DAC_MOSI;
	clr_ADC_SCLK_410;
	clr_ADC_SCLK_470;
	clr_ADC_SCLK_560;
	clr_ADC_CNV_410;
	clr_ADC_CNV_470;
	clr_ADC_CNV_560;
	clr_SCREEN_CAN_USE_USB;
	clr_POT_CLK;
	clr_POT_SDI;
	clr_POT_CS_410;
	clr_POT_CS_470;
	clr_POT_CS_560;
	clr_DAC_CS_LASER;
	clr_DAC_SCLK_LASER;
	clr_DAC_MOSI_LASER;
	clr_CAM_TRIGGER;
	clr_CAM_GPIO2;
	clr_CAM_GPIO3;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	1,
	1,
	1,
	3,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	32,
	1,
	1,
	1,
	1,
	1,
	1,
	12,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	8,
	8,
	8,
	8,
	8,
	8,
	8
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_CONFIG),
	(uint8_t*)(&app_regs.REG_RESERVED0),
	(uint8_t*)(&app_regs.REG_DAC_L410),
	(uint8_t*)(&app_regs.REG_DAC_L470),
	(uint8_t*)(&app_regs.REG_DAC_L560),
	(uint8_t*)(app_regs.REG_DAC_ALL_LEDS),
	(uint8_t*)(&app_regs.REG_DAC_LASER),
	(uint8_t*)(&app_regs.REG_SCREEN_BRIGHT),
	(uint8_t*)(&app_regs.REG_SCREEN_IMG_INDEX),
	(uint8_t*)(&app_regs.REG_GAIN_PD_L410),
	(uint8_t*)(&app_regs.REG_GAIN_PD_L470),
	(uint8_t*)(&app_regs.REG_GAIN_PD_L560),
	(uint8_t*)(&app_regs.REG_STIM_KEY_SWITCH_STATE),
	(uint8_t*)(&app_regs.REG_STIM_START),
	(uint8_t*)(&app_regs.REG_STIM_WAVELENGTH),
	(uint8_t*)(&app_regs.REG_STIM_PERIOD),
	(uint8_t*)(&app_regs.REG_STIM_ON),
	(uint8_t*)(&app_regs.REG_STIM_REPS),
	(uint8_t*)(&app_regs.REG_EXT_CAMERA_START),
	(uint8_t*)(&app_regs.REG_EXT_CAMERA_PERIOD),
	(uint8_t*)(&app_regs.REG_OUT0_CONF),
	(uint8_t*)(&app_regs.REG_OUT1_CONF),
	(uint8_t*)(&app_regs.REG_IN0_CONF),
	(uint8_t*)(&app_regs.REG_IN1_CONF),
	(uint8_t*)(&app_regs.REG_OUT_SET),
	(uint8_t*)(&app_regs.REG_OUT_CLEAR),
	(uint8_t*)(&app_regs.REG_OUT_TOGGLE),
	(uint8_t*)(&app_regs.REG_OUT_WRITE),
	(uint8_t*)(&app_regs.REG_IN_READ),
	(uint8_t*)(&app_regs.REG_START),
	(uint8_t*)(&app_regs.REG_FRAME_EVENT),
	(uint8_t*)(app_regs.REG_TRIGGER_STATE),
	(uint8_t*)(&app_regs.REG_TRIGGER_STATE_LENGTH),
	(uint8_t*)(&app_regs.REG_TRIGGER_PERIOD),
	(uint8_t*)(&app_regs.REG_TRIGGER_T_ON),
	(uint8_t*)(&app_regs.REG_TRIGGER_T_UPDATE_OUTPUTS),
	(uint8_t*)(&app_regs.REG_TRIGGER_STIM_BEHAVIOR),
	(uint8_t*)(&app_regs.REG_PHOTODIODES_START),
	(uint8_t*)(app_regs.REG_PHOTODIODES),
	(uint8_t*)(&app_regs.REG_TEMPERATURE),
	(uint8_t*)(&app_regs.REG_SCREEN_HW_VERSION_H),
	(uint8_t*)(&app_regs.REG_SCREEN_HW_VERSION_L),
	(uint8_t*)(&app_regs.REG_SCREEN_ASSEMBLY_VERSION),
	(uint8_t*)(&app_regs.REG_SCREEN_FW_VERSION_H),
	(uint8_t*)(&app_regs.REG_SCREEN_FW_VERSION_L),
	(uint8_t*)(&app_regs.REG_RESERVED1),
	(uint8_t*)(app_regs.REG_CAL_L410),
	(uint8_t*)(app_regs.REG_CAL_L470),
	(uint8_t*)(app_regs.REG_CAL_L560),
	(uint8_t*)(app_regs.REG_CAL_LASER),
	(uint8_t*)(app_regs.REG_CAL_PH410),
	(uint8_t*)(app_regs.REG_CAL_PH470),
	(uint8_t*)(app_regs.REG_CAL_PH560)
};