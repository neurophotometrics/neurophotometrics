#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "wake.h"
#include "dac.h"
#include "adc.h"
#include "screen.h"
#include "ad5204.h"
#include "production_test.h"
#include "screen_state_control.h"
#include "i2c.h"

#define F_CPU 32000000
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "FP3002";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 2;
    uint8_t hwL = 0;
    uint8_t fwH = 1;
    uint8_t fwL = 0;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        2064,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name
    );
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	clr_OUT0;		// Will also disable the laser
	clr_OUT1;
	
	timer_type0_stop(&TCC0);
	set_dac_L410(0);
	set_dac_L470(0);
	set_dac_L560(0);
	
	app_regs.REG_STIM_START = MSK_STIM_STOP;
	app_write_REG_STIM_START(&app_regs.REG_STIM_START);
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/


/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
extern bool screen_is_connected;

extern void manage_key_switch(void);

uint8_t a, b;
float temp;

i2c_dev_t temp_sensor;

void core_callback_1st_config_hw_after_boot(void)
{	
	//op3
	io_pin2out(&PORTH, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_INT_LASER
	clr_EN_INT_LASER;
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_CS_LASER
	io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_SCLK_LASER
	io_pin2out(&PORTC, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DAC_MOSI_LASER
	set_dac_LASER(0);	// Laser has a gain of 4.882813
	
	/* Initialize communication with LCD and wakeup screen */
	init_screen_serial();		// Initialize serial
	_delay_ms(2000);			// Wait for screen to boot
	screen_clean_now();			// Clean screen
	_delay_ms(1000);			// Wait for screen clean
	screen_set_bright_now(7);	// Set brightness to half-scale
	display_image_now_byte(33); // Show laser warning
	_delay_ms(500);				// Wait for image to be displayed
	screen_wakeup_now();		// Start wakeup routine on LCD
		
	/* Wake up indication */
	wakeup();
	_delay_ms(500);
	//display_image_now_byte(0);
	
	/* Initialize IOs */
	init_ios();
	
	/* Initialize hardware */
	i2c0_init();
	temp_sensor.add = 0x32;
	
	/* Check if screen is connected          */
	/* Needs to be done after the init_ios() */
	if (!read_SCREEN_IS_USING_USB)
		screen_is_connected = true;
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_CONFIG = B_SYNC_TO_MASTER | B_OUT0_TO_BOTH | B_COM_TO_MAIN | B_ENABLE_LED_CURRENT_PROTECTION;
	
	app_regs.REG_DAC_L410 = 0;
	app_regs.REG_DAC_L470 = 0;
	app_regs.REG_DAC_L560 = 0;
	app_regs.REG_DAC_LASER = 32760;
	
	app_regs.REG_SCREEN_BRIGHT = 7;
	app_regs.REG_SCREEN_IMG_INDEX = 0;
	
	app_regs.REG_GAIN_PD_L410 = 2;
	app_regs.REG_GAIN_PD_L470 = 2;
	app_regs.REG_GAIN_PD_L560 = 6;
	
	app_regs.REG_STIM_START = MSK_STIM_STOP;
	app_regs.REG_STIM_WAVELENGTH = 0;
	app_regs.REG_STIM_PERIOD = 100;	// 100 ms
	app_regs.REG_STIM_ON = 50;		// 50 ms
	app_regs.REG_STIM_REPS = 10;	// 1 second
	
	app_regs.REG_EXT_CAMERA_START = MSK_EXT_CAM_STOP;
	app_regs.REG_EXT_CAMERA_PERIOD = 33333;	// ~30Hz
	
	app_regs.REG_OUT0_CONF = MSK_OUT_CONF_SOFTWARE;
	app_regs.REG_OUT1_CONF = MSK_OUT_CONF_STROBE;
	
	app_regs.REG_IN0_CONF = MSK_IN_C_SOFTWARE_R;
	app_regs.REG_IN1_CONF = MSK_IN_C_SOFTWARE_R;
	
	app_regs.REG_START = 0;
	
	app_regs.REG_TRIGGER_STATE[0] = B_ON_L470;
	app_regs.REG_TRIGGER_STATE[1] = B_ON_L560;
	app_regs.REG_TRIGGER_STATE[2] = B_ON_L410;
	app_regs.REG_TRIGGER_STATE_LENGTH = 3;
	
	app_regs.REG_TRIGGER_PERIOD = 62500;									// 16 Hz
	app_regs.REG_TRIGGER_T_ON = 1000;										// 1 ms
	app_regs.REG_TRIGGER_T_UPDATE_OUTPUTS = app_regs.REG_TRIGGER_PERIOD/2;	// Half-period
	
	app_regs.REG_TRIGGER_STIM_BEHAVIOR = MSK_TRIGGER_STIM_CONF_START_REPS;
	
	app_regs.REG_PHOTODIODES_START = 0;	
}

void core_callback_registers_were_reinitialized(void)
{
	uint8_t aux8_t = 0;
	uint16_t aux16_t = 0;
	
	/* Update registers if needed */
	app_write_REG_CONFIG(&app_regs.REG_CONFIG);
	
	//app_write_REG_DAC_LASER(&app_regs.REG_DAC_LASER);
	manage_key_switch();	
	
	app_write_REG_SCREEN_BRIGHT(&app_regs.REG_SCREEN_BRIGHT);
	app_regs.REG_SCREEN_IMG_INDEX = 0;
	
	app_write_REG_GAIN_PD_L410(&app_regs.REG_GAIN_PD_L410);
	app_write_REG_GAIN_PD_L470(&app_regs.REG_GAIN_PD_L470);
	app_write_REG_GAIN_PD_L560(&app_regs.REG_GAIN_PD_L560);
	
	app_write_REG_OUT_WRITE(&app_regs.REG_OUT_WRITE);
	
	update_screen_indication();
	screen_get_versions();
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	screen_set_bright(app_regs.REG_SCREEN_BRIGHT);
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	screen_set_bright(0);	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
extern bool bonsai_is_on;
extern bool trigger_stop;
void core_callback_device_to_standby(void)
{
	bonsai_is_on = false;	
	trigger_stop = true;
	
	app_regs.REG_STIM_START = MSK_STIM_STOP;
	app_write_REG_STIM_START(&app_regs.REG_STIM_START);
	
	//timer_type0_stop(&TCC0);
	//set_dac_L410(0);
	//set_dac_L470(0);
	//set_dac_L560(0);
	
	update_screen_indication();	
}
void core_callback_device_to_active(void)
{
	bonsai_is_on = true;
	
	update_screen_indication();
}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
uint16_t axc;

#define TEMP_NONE 0
#define TEMP_CONVERT 1
#define TEMP_READ_H 2
#define TEMP_READ_L 3
uint8_t temperature_state = TEMP_NONE;

uint8_t temperature_counter = 1;
bool temp_cmd_is_sent = true;

void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void)
{
	switch (temperature_counter)
	{
		case 8:
			temp_sensor.reg = 0xC4;
			temp_sensor.reg_val = 0x04;
			
			temperature_state = TEMP_CONVERT;
			temp_cmd_is_sent = false;
			break;
	
		case 9:
			temp_sensor.reg = 0xC1;
			
			temperature_state = TEMP_READ_H;
			temp_cmd_is_sent = false;
			break;
			
		case 10:
			temp_sensor.reg = 0xC2;
			
			temperature_state = TEMP_READ_L;
			temp_cmd_is_sent = false;
			temperature_counter = 0;
			break;
	}
	
	temperature_counter++;
}

uint16_t counter_ = 0;

//bool device_in_test_mode = false;

void core_callback_t_500us(void)
{	
	/*
	if(!read_SW1)
	{
		device_in_test_mode = true;
	}
	
	if (device_in_test_mode)
	{
		exec_production_test();
	}
	*/
	
	if (temp_cmd_is_sent == false)
	{
		switch (temperature_state)
		{
			case TEMP_CONVERT:
				temp_cmd_is_sent = i2c0_wReg_slowly(&temp_sensor);
				break;
				
			case TEMP_READ_H:
				temp_cmd_is_sent = i2c0_rReg_slowly(&temp_sensor, 1);
				if (temp_cmd_is_sent)
				{
					app_regs.REG_TEMPERATURE = temp_sensor.reg_val & 0x7F;	// Remove "fresh" bit
					app_regs.REG_TEMPERATURE = app_regs.REG_TEMPERATURE << 8;
				}
				break;
				
			case TEMP_READ_L:
				temp_cmd_is_sent = i2c0_rReg_slowly(&temp_sensor, 1);
				if (temp_cmd_is_sent)
				{
					app_regs.REG_TEMPERATURE |= temp_sensor.reg_val;					
					core_func_send_event(ADD_REG_TEMPERATURE, true);
				}
				break;
		}
	}
}

uint8_t gain_ = 1;
uint16_t laser = 32768;

extern int8_t ms_countdown_to_switch_to_screen;
extern int8_t ms_countdown_to_enable_internal_laser;
extern void enable_internal_laser(void);

void core_callback_t_1ms(void)
{	
	/* Millisecond countdown to switch communication to screen */
	if (ms_countdown_to_switch_to_screen >= 0)
	{	
		ms_countdown_to_switch_to_screen -= 1;
		
		if (ms_countdown_to_switch_to_screen == -1)
		{
			clr_EN_SERIAL_MAIN;
			clr_EN_SERIAL_SCREEN;
			set_EN_SERIAL_SCREEN;
			
			set_SCREEN_CAN_USE_USB;
		}
	}
	
	/* Key switch was turned ON */
	if (ms_countdown_to_enable_internal_laser >= 0)
	{
		ms_countdown_to_enable_internal_laser -= 1;
		
		if (ms_countdown_to_enable_internal_laser == -1)
		{
			enable_internal_laser();
		}
	}
	
	//app_write_REG_GAIN_PD_L470(&gain_);
}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}