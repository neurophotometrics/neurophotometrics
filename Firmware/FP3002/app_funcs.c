#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"

#include "dac.h"
#include "screen.h"
#include "ad5204.h"
#include "screen_state_control.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_CONFIG,
	&app_read_REG_RESERVED0,
	&app_read_REG_DAC_L410,
	&app_read_REG_DAC_L470,
	&app_read_REG_DAC_L560,
	&app_read_REG_DAC_ALL_LEDS,
	&app_read_REG_DAC_LASER,
	&app_read_REG_SCREEN_BRIGHT,
	&app_read_REG_SCREEN_IMG_INDEX,
	&app_read_REG_GAIN_PD_L410,
	&app_read_REG_GAIN_PD_L470,
	&app_read_REG_GAIN_PD_L560,
	&app_read_REG_STIM_KEY_SWITCH_STATE,
	&app_read_REG_STIM_START,
	&app_read_REG_STIM_WAVELENGTH,
	&app_read_REG_STIM_PERIOD,
	&app_read_REG_STIM_ON,
	&app_read_REG_STIM_REPS,
	&app_read_REG_EXT_CAMERA_START,
	&app_read_REG_EXT_CAMERA_PERIOD,
	&app_read_REG_OUT0_CONF,
	&app_read_REG_OUT1_CONF,
	&app_read_REG_IN0_CONF,
	&app_read_REG_IN1_CONF,
	&app_read_REG_OUT_SET,
	&app_read_REG_OUT_CLEAR,
	&app_read_REG_OUT_TOGGLE,
	&app_read_REG_OUT_WRITE,
	&app_read_REG_IN_READ,
	&app_read_REG_START,
	&app_read_REG_FRAME_EVENT,
	&app_read_REG_TRIGGER_STATE,
	&app_read_REG_TRIGGER_STATE_LENGTH,
	&app_read_REG_TRIGGER_PERIOD,
	&app_read_REG_TRIGGER_T_ON,
	&app_read_REG_TRIGGER_T_UPDATE_OUTPUTS,
	&app_read_REG_TRIGGER_STIM_BEHAVIOR,
	&app_read_REG_PHOTODIODES_START,
	&app_read_REG_PHOTODIODES,
	&app_read_REG_TEMPERATURE,
	&app_read_REG_SCREEN_HW_VERSION_H,
	&app_read_REG_SCREEN_HW_VERSION_L,
	&app_read_REG_SCREEN_ASSEMBLY_VERSION,
	&app_read_REG_SCREEN_FW_VERSION_H,
	&app_read_REG_SCREEN_FW_VERSION_L,
	&app_read_REG_CAMERA_SN,
	&app_read_REG_CAL_L410,
	&app_read_REG_CAL_L470,
	&app_read_REG_CAL_L560,
	&app_read_REG_CAL_LASER,
	&app_read_REG_CAL_PH410,
	&app_read_REG_CAL_PH470,
	&app_read_REG_CAL_PH560
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_CONFIG,
	&app_write_REG_RESERVED0,
	&app_write_REG_DAC_L410,
	&app_write_REG_DAC_L470,
	&app_write_REG_DAC_L560,
	&app_write_REG_DAC_ALL_LEDS,
	&app_write_REG_DAC_LASER,
	&app_write_REG_SCREEN_BRIGHT,
	&app_write_REG_SCREEN_IMG_INDEX,
	&app_write_REG_GAIN_PD_L410,
	&app_write_REG_GAIN_PD_L470,
	&app_write_REG_GAIN_PD_L560,
	&app_write_REG_STIM_KEY_SWITCH_STATE,
	&app_write_REG_STIM_START,
	&app_write_REG_STIM_WAVELENGTH,
	&app_write_REG_STIM_PERIOD,
	&app_write_REG_STIM_ON,
	&app_write_REG_STIM_REPS,
	&app_write_REG_EXT_CAMERA_START,
	&app_write_REG_EXT_CAMERA_PERIOD,
	&app_write_REG_OUT0_CONF,
	&app_write_REG_OUT1_CONF,
	&app_write_REG_IN0_CONF,
	&app_write_REG_IN1_CONF,
	&app_write_REG_OUT_SET,
	&app_write_REG_OUT_CLEAR,
	&app_write_REG_OUT_TOGGLE,
	&app_write_REG_OUT_WRITE,
	&app_write_REG_IN_READ,
	&app_write_REG_START,
	&app_write_REG_FRAME_EVENT,
	&app_write_REG_TRIGGER_STATE,
	&app_write_REG_TRIGGER_STATE_LENGTH,
	&app_write_REG_TRIGGER_PERIOD,
	&app_write_REG_TRIGGER_T_ON,
	&app_write_REG_TRIGGER_T_UPDATE_OUTPUTS,
	&app_write_REG_TRIGGER_STIM_BEHAVIOR,
	&app_write_REG_PHOTODIODES_START,
	&app_write_REG_PHOTODIODES,
	&app_write_REG_TEMPERATURE,
	&app_write_REG_SCREEN_HW_VERSION_H,
	&app_write_REG_SCREEN_HW_VERSION_L,
	&app_write_REG_SCREEN_ASSEMBLY_VERSION,
	&app_write_REG_SCREEN_FW_VERSION_H,
	&app_write_REG_SCREEN_FW_VERSION_L,
	&app_write_REG_CAMERA_SN,
	&app_write_REG_CAL_L410,
	&app_write_REG_CAL_L470,
	&app_write_REG_CAL_L560,
	&app_write_REG_CAL_LASER,
	&app_write_REG_CAL_PH410,
	&app_write_REG_CAL_PH470,
	&app_write_REG_CAL_PH560
};


/************************************************************************/
/* REG_CONFIG0                                                          */
/************************************************************************/
int8_t ms_countdown_to_switch_to_screen = -1;
bool over_current_protection = true;

void app_read_REG_CONFIG(void)
{
	app_regs.REG_CONFIG = 0;
	
	if (read_EN_CLKOUT)  app_regs.REG_CONFIG |= B_SYNC_TO_MASTER;
	if (read_EN_CLKIN)   app_regs.REG_CONFIG |= B_SYNC_TO_SLAVE;
	
	if (!read_EN_INT_LASER &&  read_EN_OUT0) app_regs.REG_CONFIG |= B_OUT0_TO_BNC;
	if ( read_EN_INT_LASER && !read_EN_OUT0) app_regs.REG_CONFIG |= B_OUT0_TO_INT_LASER;
	if ( read_EN_INT_LASER &&  read_EN_OUT0) app_regs.REG_CONFIG |= B_OUT0_TO_BOTH;
	
	if (!read_EN_SERIAL_MAIN)   app_regs.REG_CONFIG |= B_COM_TO_MAIN;
	if ( read_EN_SERIAL_SCREEN) app_regs.REG_CONFIG |= B_COM_TO_SCREEN;
	if ( ms_countdown_to_switch_to_screen != -1) app_regs.REG_CONFIG |= B_COM_TO_SCREEN;
	
	if ( over_current_protection) app_regs.REG_CONFIG |= B_ENABLE_LED_CURRENT_PROTECTION;
	if (!over_current_protection) app_regs.REG_CONFIG |= B_DISABLE_LED_CURRENT_PROTECTION;
	
}

uint16_t asdasd;
bool app_write_REG_CONFIG(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	asdasd = reg;
	
	if (reg & B_SYNC_TO_MASTER)	{clr_EN_CLKIN;  set_EN_CLKOUT;}
	if (reg & B_SYNC_TO_SLAVE)	{clr_EN_CLKOUT; set_EN_CLKIN;}
	
	if (reg & B_OUT0_TO_BNC)        {clr_EN_INT_LASER; set_EN_OUT0;}
	if (reg & B_OUT0_TO_INT_LASER)	{if (!read_EN_INT_LASER) clr_OUT0; set_EN_INT_LASER; clr_EN_OUT0;}
	if (reg & B_OUT0_TO_BOTH)       {if (!read_EN_INT_LASER) clr_OUT0; set_EN_INT_LASER; set_EN_OUT0;}
	
	update_screen_indication();
	
	if (reg & B_COM_TO_MAIN)
	{
		ms_countdown_to_switch_to_screen = -1;

		clr_SCREEN_CAN_USE_USB;
		
		clr_EN_SERIAL_MAIN;
		clr_EN_SERIAL_SCREEN;
		set_EN_SERIAL_MAIN;
	}
	
	if (reg & B_COM_TO_SCREEN)
	{
		ms_countdown_to_switch_to_screen = 10;
	}
	
	if (reg & B_SCREEN_TO_BOOTLOADER)
	{		
		screen_send_to_bootloader();
	}
	
	if (reg & B_ENABLE_LED_CURRENT_PROTECTION)
	{
		over_current_protection = true;
	}
	
	if (reg & B_DISABLE_LED_CURRENT_PROTECTION)
	{
		over_current_protection = false;
	}
	
	app_read_REG_CONFIG();			// Needed to keep configuration when boot
	//app_regs.REG_CONFIG0 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED0                                                        */
/************************************************************************/
void app_read_REG_RESERVED0(void) {}
bool app_write_REG_RESERVED0(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_RESERVED0 = reg;
	return true;
}


/************************************************************************/
/* REG_DAC_L410                                                         */
/************************************************************************/
void app_read_REG_DAC_L410(void) {}
bool app_write_REG_DAC_L410(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (over_current_protection)
		if (reg > 36800)	// ~850 mA
			return false;

	app_regs.REG_DAC_L410 = reg;
	return true;
}


/************************************************************************/
/* REG_DAC_L470                                                         */
/************************************************************************/
void app_read_REG_DAC_L470(void) {}
bool app_write_REG_DAC_L470(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (over_current_protection)
		if (reg > 36800)	// ~850 mA
			return false;

	app_regs.REG_DAC_L470 = reg;
	return true;
}


/************************************************************************/
/* REG_DAC_L560                                                         */
/************************************************************************/
void app_read_REG_DAC_L560(void) {}
bool app_write_REG_DAC_L560(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (over_current_protection)
		if (reg > 36800)	// ~850 mA
			return false;

	app_regs.REG_DAC_L560 = reg;
	return true;
}


/************************************************************************/
/* REG_DAC_ALL_LEDS                                                     */
/************************************************************************/
// This register is an array with 3 positions
void app_read_REG_DAC_ALL_LEDS(void) {}
bool app_write_REG_DAC_ALL_LEDS(void *a)
{
	uint16_t *reg = ((uint16_t*)a);
	
	if (over_current_protection) if (reg[0] > 36800) return false;	// ~850 mA
	if (over_current_protection) if (reg[1] > 36800) return false;	// ~850 mA
	if (over_current_protection) if (reg[1] > 36800) return false;	// ~850 mA
	
	app_regs.REG_DAC_L410 = reg[0];
	app_regs.REG_DAC_L470 = reg[1];
	app_regs.REG_DAC_L560 = reg[2];

	app_regs.REG_DAC_ALL_LEDS[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DAC_LASER                                                        */
/************************************************************************/
void app_read_REG_DAC_LASER(void) {}
bool app_write_REG_DAC_LASER(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	set_dac_LASER(reg>>1);	// Laser has a gain of 4.882813
	// 32768 -> 1.024 V

	app_regs.REG_DAC_LASER = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_BRIGHT                                                    */
/************************************************************************/
void app_read_REG_SCREEN_BRIGHT(void) {}
bool app_write_REG_SCREEN_BRIGHT(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg > 15) reg = 15;
		screen_set_bright(reg);

	app_regs.REG_SCREEN_BRIGHT = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_IMG_INDEX                                                 */
/************************************************************************/
void app_read_REG_SCREEN_IMG_INDEX(void) {}
bool app_write_REG_SCREEN_IMG_INDEX(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg > 127) reg = 127;
	display_image(reg);

	app_regs.REG_SCREEN_IMG_INDEX = reg;
	return true;
}


/************************************************************************/
/* REG_GAIN_PD_L410                                                     */
/************************************************************************/
void app_read_REG_GAIN_PD_L410(void) {}
bool app_write_REG_GAIN_PD_L410(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg < 1) reg = 1;
	if (reg > 32) reg = 32;
	
	set_gain_L410(reg);

	app_regs.REG_GAIN_PD_L410 = reg;
	return true;
}


/************************************************************************/
/* REG_GAIN_PD_L470                                                     */
/************************************************************************/
void app_read_REG_GAIN_PD_L470(void) {}
bool app_write_REG_GAIN_PD_L470(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg < 1) reg = 1;
	if (reg > 32) reg = 32;
	
	set_gain_L470(reg);

	app_regs.REG_GAIN_PD_L470 = reg;
	return true;
}


/************************************************************************/
/* REG_GAIN_PD_L560                                                     */
/************************************************************************/
void app_read_REG_GAIN_PD_L560(void) {}
bool app_write_REG_GAIN_PD_L560(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg < 1) reg = 1;
	if (reg > 32) reg = 32;
	
	set_gain_L560(reg);

	app_regs.REG_GAIN_PD_L560 = reg;
	return true;
}

/************************************************************************/
/* REG_STIM_KEY_SWITCH_STATE                                        */
/************************************************************************/
void app_read_REG_STIM_KEY_SWITCH_STATE(void)
{
	app_regs.REG_STIM_KEY_SWITCH_STATE = read_KEY_SWITCH ? B_KEY_SWITCH_IS_ON : 0;
}

bool app_write_REG_STIM_KEY_SWITCH_STATE(void *a) {	return false; }


/************************************************************************/
/* REG_STIM_START                                                       */
/************************************************************************/
extern uint16_t opto_stim_reps_counter;
extern uint16_t opto_stim_period_counter;
extern uint16_t opto_stim_on_counter;
extern bool opto_behavior_stop;

void app_read_REG_STIM_START(void)
{
	/* Return current selected configuration or ZERO if stopped */
	app_regs.REG_STIM_START = (TCE0_CTRLA != 0) ? app_regs.REG_STIM_START : MSK_STIM_STOP;
}

bool app_write_REG_STIM_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & ~GM_STIM_START_CONF)
		return false;
	
	/* Return error if the intervals are not consistent. */
	if (app_regs.REG_STIM_ON > app_regs.REG_STIM_PERIOD)
		return false;
	
	/* Start the stimulation.
	*  Note: Stimulation will restart if a new start command is received while
	*  still stimulating
	*/
	if (reg == MSK_STIM_START_REPS || reg == MSK_STIM_START_INFINITE)
	{
		if (app_regs.REG_STIM_WAVELENGTH == 450 || app_regs.REG_STIM_WAVELENGTH == 635)
		{
			/* Start if:
			*  - Internal laser is not selected, or
			*  - Internal laser is selected and enabled by the key switch.
			*/
			if (!read_EN_INT_LASER || (read_EN_INT_LASER && read_KEY_SWITCH))
			{
				timer_type0_enable(&TCE0, TIMER_PRESCALER_DIV256,125, INT_LEVEL_LOW);
				set_OUT0;
				opto_stim_reps_counter = 0;
				opto_stim_period_counter = 0;
				opto_stim_on_counter = 0;
			
				update_screen_indication();
			}
		}
	}
	
	/* Stop the stimulation. */
	else if (reg == MSK_STIM_STOP)
	{
		if (TCE0_CTRLA != 0 || (read_KEY_SWITCH && read_EN_INT_LASER))
		{
			clr_OUT0;
			timer_type0_stop(&TCE0);
			
			update_screen_indication();
		}
	}
		
	app_regs.REG_STIM_START = reg;
	return true;
}


/************************************************************************/
/* REG_STIM_WAVELENGTH                                                  */
/************************************************************************/
void app_read_REG_STIM_WAVELENGTH(void) {}
bool app_write_REG_STIM_WAVELENGTH(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_STIM_WAVELENGTH = reg;
	
	update_screen_indication();
	
	return true;
}


/************************************************************************/
/* REG_STIM_PERIOD                                                      */
/************************************************************************/
void app_read_REG_STIM_PERIOD(void) {}
bool app_write_REG_STIM_PERIOD(void *a)
{
	if (*((uint16_t*)a) < 2 || *((uint16_t*)a) > 60000)
		return false;

	app_regs.REG_STIM_PERIOD = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_STIM_ON                                                          */
/************************************************************************/
void app_read_REG_STIM_ON(void) {}
bool app_write_REG_STIM_ON(void *a)
{
	if (*((uint16_t*)a) < 1 || *((uint16_t*)a) > 60000)
		return false;

	app_regs.REG_STIM_ON = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_STIM_REPS                                                        */
/************************************************************************/
void app_read_REG_STIM_REPS(void) {}
bool app_write_REG_STIM_REPS(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_STIM_REPS = reg;
	return true;
}


/************************************************************************/
/* REG_EXT_CAMERA_START                                                 */
/************************************************************************/
extern bool ext_camera_stop;

void app_read_REG_EXT_CAMERA_START(void) {}
bool app_write_REG_EXT_CAMERA_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & ~GM_EXT_CAM_CONFIG)
		return false;
	
	if (reg == MSK_EXT_CAM_START_WITH_EVENTS || reg == MSK_EXT_CAM_START_WITHOUT_EVENTS)
	{
		if (TCF0_CTRLA == 0)
		{
			timer_type0_enable(&TCF0, TIMER_PRESCALER_DIV64, app_regs.REG_EXT_CAMERA_PERIOD >> 2,  INT_LEVEL_LOW);
			set_OUT0;
			
			if (reg == MSK_EXT_CAM_START_WITH_EVENTS)
			{
				core_func_send_event(ADD_REG_EXT_CAMERA_START, true);				
			}
			
			ext_camera_stop = false;
		}
	}
	else if (reg == MSK_EXT_CAM_STOP)
	{
		if (TCF0_CTRLA != 0)
		{
			ext_camera_stop = true;
		}
	}	

	app_regs.REG_EXT_CAMERA_START = reg;
	return true;
}


/************************************************************************/
/* REG_EXT_CAMERA_PERIOD                                                */
/************************************************************************/
void app_read_REG_EXT_CAMERA_PERIOD(void) {}
bool app_write_REG_EXT_CAMERA_PERIOD(void *a)
{
	if (*((uint16_t*)a) < 5000) // 5 ms -> 200Hz
		return false;
	
	if (*((uint16_t*)a) > 65000) // 65 ms -> ~15.259Hz
		return false;

	app_regs.REG_EXT_CAMERA_PERIOD = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT0_CONF                                                        */
/************************************************************************/
void app_read_REG_OUT0_CONF(void) {}
bool app_write_REG_OUT0_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_OUT_CONF) return false;
	app_regs.REG_OUT0_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT1_CONF                                                        */
/************************************************************************/
void app_read_REG_OUT1_CONF(void) {}
bool app_write_REG_OUT1_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_OUT_CONF) return false;
	app_regs.REG_OUT1_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_IN0_CONF                                                         */
/************************************************************************/
void app_read_REG_IN0_CONF(void) {}
bool app_write_REG_IN0_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_IN_CONF) return false;
	app_regs.REG_IN0_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_IN1_CONF                                                         */
/************************************************************************/
void app_read_REG_IN1_CONF(void) {}
bool app_write_REG_IN1_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_IN_CONF) return false;
	app_regs.REG_IN1_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT_SET                                                          */
/************************************************************************/
extern bool dac_L410_state;
extern bool dac_L470_state;
extern bool dac_L560_state;

void app_read_REG_OUT_SET(void) {}
bool app_write_REG_OUT_SET(void *a)
{
	if (*((uint8_t*)a) & B_L410) set_dac_L410(app_regs.REG_DAC_L410);
	if (*((uint8_t*)a) & B_L470) set_dac_L470(app_regs.REG_DAC_L470);
	if (*((uint8_t*)a) & B_L560) set_dac_L560(app_regs.REG_DAC_L560);
	
	if (*((uint8_t*)a) & B_DOUT0) {set_controlled_OUT0; update_screen_indication();}
	if (*((uint8_t*)a) & B_DOUT1) set_OUT1;
	
	if (*((uint8_t*)a) & B_INTERNAL_CAM_TRIGGER) set_CAM_TRIGGER;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO2) set_CAM_GPIO2;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO3) set_CAM_GPIO3;
	
	app_regs.REG_OUT_SET = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT_CLEAR                                                        */
/************************************************************************/
void app_read_REG_OUT_CLEAR(void) {}
bool app_write_REG_OUT_CLEAR(void *a)
{
	if (*((uint8_t*)a) & B_L410) clr_DAC_L410;
	if (*((uint8_t*)a) & B_L470) clr_DAC_L470;
	if (*((uint8_t*)a) & B_L560) clr_DAC_L560;
		
	if (*((uint8_t*)a) & B_DOUT0) {clr_OUT0; update_screen_indication();}
	if (*((uint8_t*)a) & B_DOUT1) clr_OUT1;

	if (*((uint8_t*)a) & B_INTERNAL_CAM_TRIGGER) clr_CAM_TRIGGER;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO2) clr_CAM_GPIO2;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO3) clr_CAM_GPIO3;

	app_regs.REG_OUT_CLEAR = *((uint8_t*)a);;
	return true;
}


/************************************************************************/
/* REG_OUT_TOGGLE                                                       */
/************************************************************************/
void app_read_REG_OUT_TOGGLE(void) {}
bool app_write_REG_OUT_TOGGLE(void *a)
{	
	if (*((uint8_t*)a) & B_L410) tgl_dac_L410(app_regs.REG_DAC_L410);
	if (*((uint8_t*)a) & B_L470) tgl_dac_L470(app_regs.REG_DAC_L470);
	if (*((uint8_t*)a) & B_L560) tgl_dac_L560(app_regs.REG_DAC_L560);
	
	if (*((uint8_t*)a) & B_DOUT0)
	{
		if (read_OUT0)
		{
			clr_OUT0;
		}
		else
		{
			set_controlled_OUT0;
		}
		
		update_screen_indication();
	}	
	
	if (*((uint8_t*)a) & B_DOUT1) tgl_OUT1;
	
	if (*((uint8_t*)a) & B_INTERNAL_CAM_TRIGGER) tgl_CAM_TRIGGER;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO2) tgl_CAM_GPIO2;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO3) tgl_CAM_GPIO3;

	app_regs.REG_OUT_TOGGLE = *((uint8_t*)a);;
	return true;
}


/************************************************************************/
/* REG_OUT_WRITE                                                        */
/************************************************************************/
void app_read_REG_OUT_WRITE(void)
{
	app_regs.REG_OUT_WRITE = (read_OUT0 ? B_DOUT0 : 0) | (read_OUT1 ? B_DOUT1 : 0);
}

bool app_write_REG_OUT_WRITE(void *a)
{
	if (*((uint8_t*)a) & B_L410) set_dac_L410(app_regs.REG_DAC_L410); else clr_DAC_L410;
	if (*((uint8_t*)a) & B_L470) set_dac_L470(app_regs.REG_DAC_L470); else clr_DAC_L470;
	if (*((uint8_t*)a) & B_L560) set_dac_L560(app_regs.REG_DAC_L560); else clr_DAC_L560;
	
	if (*((uint8_t*)a) & B_DOUT0) set_controlled_OUT0; else clr_OUT0;
	if (*((uint8_t*)a) & B_DOUT1) set_OUT1; else clr_OUT1;
	
	if (*((uint8_t*)a) & B_INTERNAL_CAM_TRIGGER) set_CAM_TRIGGER; else clr_CAM_TRIGGER;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO2) set_CAM_GPIO2; else clr_CAM_GPIO2;
	if (*((uint8_t*)a) & B_INTERNAL_CAM_GPIO3) set_CAM_GPIO3; else clr_CAM_GPIO3;
	
	update_screen_indication();

	app_regs.REG_OUT_WRITE = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_IN_READ                                                          */
/************************************************************************/
void app_read_REG_IN_READ(void)
{
	app_regs.REG_IN_READ  = (read_IN0 ? B_DIN0 : 0);
	app_regs.REG_IN_READ |= (read_IN1 ? B_DIN1 : 0);
}

bool app_write_REG_IN_READ(void *a)
{
	return false;
}


/************************************************************************/
/* REG_START                                                            */
/************************************************************************/
uint8_t trigger_prescaler;
uint16_t trigger_target_count;

extern uint8_t trigger_state_index;
extern bool trigger_stop;

void app_read_REG_START(void)
{
	app_regs.REG_START = 0;
	
	if (TCC0_CTRLA != 0)
	{
		app_regs.REG_START |= B_START_TRIGGER;
	}
	else
	{
		app_regs.REG_START |= B_STOP_TRIGGER;
	}
		
	if (app_regs.REG_EXT_CAMERA_START == MSK_EXT_CAM_START_WITHOUT_EVENTS)
	{
		app_regs.REG_START |= B_START_EXT_CAMERA_WITHOUT_EVENTS;
	}
	else if (app_regs.REG_EXT_CAMERA_START == MSK_EXT_CAM_START_WITH_EVENTS)
	{
		app_regs.REG_START |= B_START_EXT_CAMERA_WITH_EVENTS;		
	}
	else
	{
		app_regs.REG_START |= B_STOP_EXT_CAMERA;
	}
}

bool app_write_REG_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_START_TRIGGER && TCC0_CTRLA == 0)
	{
		trigger_stop = false;
		trigger_state_index = 0;
		
		if (app_regs.REG_TRIGGER_T_ON >= app_regs.REG_TRIGGER_PERIOD) return false;
		if (app_regs.REG_TRIGGER_T_UPDATE_OUTPUTS >= app_regs.REG_TRIGGER_PERIOD) return false;
		
		/* Make sure trigger is low. */
		clr_CAM_TRIGGER;
		
		/* Set up timer. */
		TCC0.CTRLA = TC_CLKSEL_OFF_gc;									// Make sure timer is stopped to make reset
		TCC0.CTRLFSET = TC_CMD_RESET_gc;								// Timer reset (registers to initial value)
		TCC0.PER = (app_regs.REG_TRIGGER_PERIOD >> 1) -1;				// Set up target
		TCC0.CCA = (app_regs.REG_TRIGGER_T_ON >> 1) - 1;				// Set duty cycle A
		TCC0.CCB = (app_regs.REG_TRIGGER_T_UPDATE_OUTPUTS >> 1) - 1;	// Set duty cycle B
		TCC0.INTCTRLA = INT_LEVEL_LOW;									// Enable overflow interrupt
		TCC0.INTCTRLB = INT_LEVEL_LOW;									// Enable compare interrupt on channel A
		TCC0.INTCTRLB |= INT_LEVEL_LOW << 2;							// Enable compare interrupt on channel B
		TCC0.CTRLB = TC0_CCAEN_bm | TC_WGMODE_SINGLESLOPE_gc;			// Enable channel A and single slope mode
		TCC0.CTRLA = TIMER_PRESCALER_DIV64;								// Start timer
		
		/* Start photodiode acquisition if not working yet */
		if (TCE1_CTRLA == 0)
		{
			timer_type1_enable(&TCE1, TIMER_PRESCALER_DIV1024, 7, INT_LEVEL_LOW);	// 224 us to lunch the photodiode acquisition
		}
	}
		
	if (reg & B_START_EXT_CAMERA_WITHOUT_EVENTS)
	{
		uint8_t aux = MSK_EXT_CAM_START_WITHOUT_EVENTS;
		app_write_REG_EXT_CAMERA_START(&aux);
	}
	
	if (reg & B_START_EXT_CAMERA_WITH_EVENTS)
	{
		uint8_t aux = MSK_EXT_CAM_START_WITH_EVENTS;
		app_write_REG_EXT_CAMERA_START(&aux);
	}
	
	if (reg & B_STOP_TRIGGER && TCC0_CTRLA != 0)
		trigger_stop = true;
	
	if (reg & B_STOP_EXT_CAMERA)
	{
		uint8_t aux = MSK_EXT_CAM_STOP;
		app_write_REG_EXT_CAMERA_START(&aux);
	}

	app_regs.REG_START = reg;
	return true;
}


/************************************************************************/
/* REG_FRAME_EVENT                                                      */
/************************************************************************/
void app_read_REG_FRAME_EVENT(void)
{
	//app_regs.REG_FRAME_EVENT = 0;

}

bool app_write_REG_FRAME_EVENT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_FRAME_EVENT = reg;
	return true;
}


/************************************************************************/
/* REG_TRIGGER_STATE                                                    */
/************************************************************************/
// This register is an array with 32 positions
void app_read_REG_TRIGGER_STATE(void) {}
bool app_write_REG_TRIGGER_STATE(void *a)
{
	uint8_t *reg = ((uint8_t*)a);
	
	for (uint8_t i = 32; i != 0; i--)
		app_regs.REG_TRIGGER_STATE[i-1] = reg[i-1];
	
	return true;
}


/************************************************************************/
/* REG_TRIGGER_STATE_LENGTH                                             */
/************************************************************************/
void app_read_REG_TRIGGER_STATE_LENGTH(void) {}
bool app_write_REG_TRIGGER_STATE_LENGTH(void *a)
{
	if (*((uint8_t*)a) >= 32) return false;
	app_regs.REG_TRIGGER_STATE_LENGTH = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_TRIGGER_PERIOD                                                   */
/************************************************************************/
void app_read_REG_TRIGGER_PERIOD(void) {}
bool app_write_REG_TRIGGER_PERIOD(void *a)
{
	if (*((uint16_t*)a) < 5000) // 5 ms -> 200Hz
		return false;
	
	if (*((uint16_t*)a) > 65000) // 65 ms -> ~15.259Hz
		return false;

	app_regs.REG_TRIGGER_PERIOD = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_TRIGGER_T_ON                                                     */
/************************************************************************/
void app_read_REG_TRIGGER_T_ON(void) {}
bool app_write_REG_TRIGGER_T_ON(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (*((uint16_t*)a) > 65000-1) // 65 ms
		return false;

	app_regs.REG_TRIGGER_T_ON = reg;
	return true;
}


/************************************************************************/
/* REG_TRIGGER_T_UPDATE_OUTPUTS                                         */
/************************************************************************/
void app_read_REG_TRIGGER_T_UPDATE_OUTPUTS(void) {}
bool app_write_REG_TRIGGER_T_UPDATE_OUTPUTS(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (*((uint16_t*)a) > 65000-1) // 65 ms
	return false;

	app_regs.REG_TRIGGER_T_UPDATE_OUTPUTS = reg;
	return true;
}


/************************************************************************/
/* REG_TRIGGER_STIM_BEHAVIOR                                            */
/************************************************************************/
void app_read_REG_TRIGGER_STIM_BEHAVIOR(void) {}
bool app_write_REG_TRIGGER_STIM_BEHAVIOR(void *a)
{
	if(*((uint8_t*)a) & ~GM_TRIGGER_STIM_CONF) return false;
	app_regs.REG_TRIGGER_STIM_BEHAVIOR = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_PHOTODIODES_START                                                */
/************************************************************************/
void app_read_REG_PHOTODIODES_START(void) {}
bool app_write_REG_PHOTODIODES_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg)
	{
		if (TCE1_CTRLA == 0)
		{
			timer_type1_enable(&TCE1, TIMER_PRESCALER_DIV1024, 8, INT_LEVEL_LOW);	// 256 us to lunch the photodiode acquisition
		}
	}
	else
	{
		timer_type0_stop(&TCD0);	// Stop photodiode frame rate timer
	}

	app_regs.REG_PHOTODIODES_START = reg;
	return true;
}


/************************************************************************/
/* REG_PHOTODIODES                                                      */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_PHOTODIODES(void)
{
	//app_regs.REG_PHOTODIODES[0] = 0;

}

bool app_write_REG_PHOTODIODES(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_PHOTODIODES[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_TEMPERATURE                                                      */
/************************************************************************/
void app_read_REG_TEMPERATURE(void)
{
	//app_regs.REG_TEMPERATURE = 0;

}

bool app_write_REG_TEMPERATURE(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_TEMPERATURE = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_HW_VERSION_H                                              */
/************************************************************************/
void app_read_REG_SCREEN_HW_VERSION_H(void)
{
	//app_regs.REG_SCREEN_HW_VERSION_H = 0;

}

bool app_write_REG_SCREEN_HW_VERSION_H(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_SCREEN_HW_VERSION_H = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_HW_VERSION_L                                              */
/************************************************************************/
void app_read_REG_SCREEN_HW_VERSION_L(void)
{
	//app_regs.REG_SCREEN_HW_VERSION_L = 0;

}

bool app_write_REG_SCREEN_HW_VERSION_L(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_SCREEN_HW_VERSION_L = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_ASSEMBLY_VERSION                                          */
/************************************************************************/
void app_read_REG_SCREEN_ASSEMBLY_VERSION(void)
{
	//app_regs.REG_SCREEN_ASSEMBLY_VERSION = 0;

}

bool app_write_REG_SCREEN_ASSEMBLY_VERSION(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_SCREEN_ASSEMBLY_VERSION = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_FW_VERSION_H                                              */
/************************************************************************/
void app_read_REG_SCREEN_FW_VERSION_H(void)
{
	//app_regs.REG_SCREEN_FW_VERSION_H = 0;

}

bool app_write_REG_SCREEN_FW_VERSION_H(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_SCREEN_FW_VERSION_H = reg;
	return true;
}


/************************************************************************/
/* REG_SCREEN_FW_VERSION_L                                              */
/************************************************************************/
void app_read_REG_SCREEN_FW_VERSION_L(void)
{
	//app_regs.REG_SCREEN_FW_VERSION_L = 0;

}

bool app_write_REG_SCREEN_FW_VERSION_L(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_SCREEN_FW_VERSION_L = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED1                                                    */
/************************************************************************/
void app_read_REG_CAMERA_SN(void) {}
bool app_write_REG_CAMERA_SN(void *a)
{
	uint64_t reg = *((uint64_t*)a);

	app_regs.REG_CAMERA_SN = reg;
	return true;
}


/************************************************************************/
/* REG_CAL_L410                                                         */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_L410(void)
{
	//app_regs.REG_CAL_L410[0] = 0;

}

bool app_write_REG_CAL_L410(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_L410[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_L470                                                         */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_L470(void)
{
	//app_regs.REG_CAL_L470[0] = 0;

}

bool app_write_REG_CAL_L470(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_L470[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_L560                                                         */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_L560(void)
{
	//app_regs.REG_CAL_L560[0] = 0;

}

bool app_write_REG_CAL_L560(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_L560[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_LASER                                                        */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_LASER(void)
{
	//app_regs.REG_CAL_LASER[0] = 0;

}

bool app_write_REG_CAL_LASER(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_LASER[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_PH410                                                        */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_PH410(void)
{
	//app_regs.REG_CAL_PH410[0] = 0;

}

bool app_write_REG_CAL_PH410(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_PH410[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_PH470                                                        */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_PH470(void)
{
	//app_regs.REG_CAL_PH470[0] = 0;

}

bool app_write_REG_CAL_PH470(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_PH470[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_CAL_PH560                                                        */
/************************************************************************/
// This register is an array with 8 positions
void app_read_REG_CAL_PH560(void)
{
	//app_regs.REG_CAL_PH560[0] = 0;

}

bool app_write_REG_CAL_PH560(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_CAL_PH560[0] = reg[0];
	return true;
}