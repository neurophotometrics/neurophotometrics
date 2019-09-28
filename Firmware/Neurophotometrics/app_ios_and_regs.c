#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTH, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // CAM_STROB_IN
	io_pin2in(&PORTK, 1, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // BUTTON_START_STOP
	io_pin2in(&PORTK, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // BUTTON_TEST_1
	io_pin2in(&PORTE, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN0

	/* Configure input interrupts */
	io_set_int(&PORTH, INT_LEVEL_LOW, 0, (1<<2), false);                 // CAM_STROB_IN
	io_set_int(&PORTK, INT_LEVEL_LOW, 0, (1<<1), false);                 // BUTTON_START_STOP
	io_set_int(&PORTK, INT_LEVEL_LOW, 1, (1<<2), false);                 // BUTTON_TEST_1
	io_set_int(&PORTE, INT_LEVEL_LOW, 0, (1<<2), false);                 // IN0

	/* Configure output pins */
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_TRIGGER
	io_pin2out(&PORTH, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_IO_0
	io_pin2out(&PORTH, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CAM_IO_1
	io_pin2out(&PORTK, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_OUT_1
	io_pin2out(&PORTK, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_OUT_0
	io_pin2out(&PORTK, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_IN_0
	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_L410
	io_pin2out(&PORTB, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_L470
	io_pin2out(&PORTB, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_L560
	io_pin2out(&PORTB, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_LEXTRA
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // L410
	io_pin2out(&PORTC, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // L470
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // L560
	io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LPHOTODIODE
	io_pin2out(&PORTD, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LEXTRA
	io_pin2out(&PORTE, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT1
	io_pin2out(&PORTE, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT0
	io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CS
	io_pin2out(&PORTD, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_SDI
	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_CLK
	io_pin2out(&PORTE, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_MIDSCALE
	io_pin2out(&PORTE, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // POT_SHDN
	io_pin2out(&PORTB, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CLK_EN_IN
	io_pin2out(&PORTB, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CLK_EN_BYPASS
	io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // CLK_EN_OUT

	/* Initialize output pins */
	clr_CAM_TRIGGER;
	clr_CAM_IO_0;
	clr_CAM_IO_1;
	set_EN_OUT_1;
	set_EN_OUT_0;
	set_EN_IN_0;
	clr_EN_L410;
	clr_EN_L470;
	clr_EN_L560;
	clr_EN_LEXTRA;
	clr_L410;
	clr_L470;
	clr_L560;
	clr_LPHOTODIODE;
	clr_LEXTRA;
	clr_OUT1;
	clr_OUT0;
	clr_POT_CS;
	clr_POT_SDI;
	clr_POT_CLK;
	clr_POT_MIDSCALE;
	set_POT_SHDN;
	set_CLK_EN_IN;
	set_CLK_EN_BYPASS;
	clr_CLK_EN_OUT;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U32,
	TYPE_U32,
	TYPE_U32,
	TYPE_U32,
	TYPE_U32,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
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
	TYPE_U16
};

uint16_t app_regs_n_elements[] = {
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
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_RAW_POT_L410),
	(uint8_t*)(&app_regs.REG_RAW_POT_L470),
	(uint8_t*)(&app_regs.REG_RAW_POT_L560),
	(uint8_t*)(&app_regs.REG_RAW_POT_LEXTRA),
	(uint8_t*)(&app_regs.REG_RAW_IOS_SET),
	(uint8_t*)(&app_regs.REG_RAW_IOS_CLEAR),
	(uint8_t*)(&app_regs.REG_RAW_IOS_TOGGLE),
	(uint8_t*)(&app_regs.REG_RAW_IOS_WRITE),
	(uint8_t*)(&app_regs.REG_RAW_IOS_READ),
	(uint8_t*)(&app_regs.REG_START),
	(uint8_t*)(&app_regs.REG_FRAME_EVENT),
	(uint8_t*)(&app_regs.REG_FRAME_EVENT_CFG),
	(uint8_t*)(app_regs.REG_TRIGGER_STATE),
	(uint8_t*)(&app_regs.REG_TRIGGER_STATE_LENGTH),
	(uint8_t*)(&app_regs.REG_TRIGGER_PERIOD),
	(uint8_t*)(&app_regs.REG_TRIGGER_T_ON),
	(uint8_t*)(&app_regs.REG_TRIGGER_PRE_LEDS),
	(uint8_t*)(&app_regs.REG_DUMMY0),
	(uint8_t*)(&app_regs.REG_DUMMY1),
	(uint8_t*)(&app_regs.REG_OUT0_CONF),
	(uint8_t*)(&app_regs.REG_OUT1_CONF),
	(uint8_t*)(&app_regs.REG_IN0_CONF),
	(uint8_t*)(&app_regs.REG_OUT_SET),
	(uint8_t*)(&app_regs.REG_OUT_CLEAR),
	(uint8_t*)(&app_regs.REG_OUT_TOGGLE),
	(uint8_t*)(&app_regs.REG_OUT_WRITE),
	(uint8_t*)(&app_regs.REG_IN_READ),
	(uint8_t*)(&app_regs.REG_ADCIN),
	(uint8_t*)(&app_regs.REG_ADCIN_CONF),
	(uint8_t*)(&app_regs.REG_STIM_START),
	(uint8_t*)(&app_regs.REG_STIM_PERIOD),
	(uint8_t*)(&app_regs.REG_STIM_ON),
	(uint8_t*)(&app_regs.REG_STIM_REPS),
	(uint8_t*)(&app_regs.REG_EXT_CAMERA_START),
	(uint8_t*)(&app_regs.REG_EXT_CAMERA_PERIOD)
};