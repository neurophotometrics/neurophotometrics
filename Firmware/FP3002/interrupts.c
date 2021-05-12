#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

#include "dac.h"
#include "screen_state_control.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
// 
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
// 
// ISR(TCD1_OVF_vect, ISR_NAKED)
// 
// ISR(TCD1_CCA_vect, ISR_NAKED)


/************************************************************************/
/* CAM_STROBE                                                           */
/************************************************************************/
uint8_t trigger_state_index;
bool trigger_stop;

extern bool dac_L410_state;
extern bool dac_L470_state;
extern bool dac_L560_state;

ISR(PORTH_INT1_vect, ISR_NAKED)
{
	if (read_CAM_STROBE)
	{
		if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_STROBE)
			set_controlled_OUT0;
		if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_STROBE)
			set_OUT1;

		app_regs.REG_FRAME_EVENT  = dac_L410_state ? B_ON_L410 : 0;
		app_regs.REG_FRAME_EVENT |= dac_L470_state ? B_ON_L470 : 0;
		app_regs.REG_FRAME_EVENT |= dac_L560_state ? B_ON_L560 : 0;		
		app_regs.REG_FRAME_EVENT |= read_OUT0 ? B_ON_OUT0 : 0;
		app_regs.REG_FRAME_EVENT |= read_OUT1 ? B_ON_OUT1 : 0;		
		app_regs.REG_FRAME_EVENT |= (TCE0_CTRLA != 0) ? B_START_STIM : 0;
		app_regs.REG_FRAME_EVENT |= (TCC0_CTRLB & TC0_CCCEN_bm) ? B_START_STIM : 0;	// Camera's channel C is enabled, meaning interleave leaser is ON
		app_regs.REG_FRAME_EVENT |= read_CAM_GPIO2 ? B_ON_INTERNAL_CAM_GPIO2 : 0;		
		app_regs.REG_FRAME_EVENT |= read_CAM_GPIO3 ? B_ON_INTERNAL_CAM_GPIO3 : 0;		
		app_regs.REG_FRAME_EVENT |= read_IN0 ? B_ON_IN0 : 0;
		app_regs.REG_FRAME_EVENT |= read_IN1 ? B_ON_IN1 : 0;
		
		core_func_send_event(ADD_REG_FRAME_EVENT, true);
	}
	else
	{
		if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_STROBE)
			clr_OUT0;
		if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_STROBE)
			clr_OUT1;
	}
	
	reti();
}

/************************************************************************/
/* Camera's trigger to high                                             */
/************************************************************************/
ISR(TCC0_OVF_vect, ISR_NAKED)
{
	reti();
}
			
		
/************************************************************************/
/* Camera's trigger to low                                              */
/************************************************************************/
ISR(TCC0_CCA_vect, ISR_NAKED)
{	
	if (trigger_stop)
	{
		trigger_stop = false;
			
		timer_type0_stop(&TCC0);
		clr_DAC_L410;
		clr_DAC_L470;
		clr_DAC_L560;
		
		/* Stop photodiode acquisition if wasn't working already */
		if (app_regs.REG_PHOTODIODES_START == 0)
			timer_type0_stop(&TCD0);	// Stop photodiode frame rate timer
		
		/* Don't need to make code to stop when in interleave mode because TCC0_CCC_vect
		*  was not executed at this point
		*/
		
		app_regs.REG_STIM_START = MSK_STIM_STOP;
		app_write_REG_STIM_START(&app_regs.REG_STIM_START);
			
		if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_STATE_CTRL)
		{
			clr_OUT0;
		}
			
		if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_STATE_CTRL)
		{
			clr_OUT1;
		}
	}
	reti();
}

/************************************************************************/
/* Update OUTPUTS                                                       */
/************************************************************************/
ISR(TCC0_CCB_vect, ISR_NAKED)
{
	app_regs.REG_FRAME_EVENT[1]++;
	
	uint8_t trigger_state = app_regs.REG_TRIGGER_STATE[trigger_state_index];
	
	trigger_state_index++;
	if (trigger_state_index == app_regs.REG_TRIGGER_STATE_LENGTH)
		trigger_state_index = 0;	
	
	if (trigger_state & B_ON_L410) set_dac_L410(app_regs.REG_DAC_L410); else clr_DAC_L410;
	if (trigger_state & B_ON_L470) set_dac_L470(app_regs.REG_DAC_L470); else clr_DAC_L470;
	if (trigger_state & B_ON_L560) set_dac_L560(app_regs.REG_DAC_L560); else clr_DAC_L560;
	
	if (trigger_state & B_ON_OUT0) set_controlled_OUT0; else clr_OUT0;
	if (trigger_state & B_ON_OUT1) set_OUT1; else clr_OUT1;
	
	if (trigger_state & B_ON_INTERNAL_CAM_GPIO2) set_CAM_GPIO2; else clr_CAM_GPIO2;
	if (trigger_state & B_ON_INTERNAL_CAM_GPIO3) set_CAM_GPIO3; else clr_CAM_GPIO3;
	
	if (trigger_state & B_START_STIM)
	{
		if (app_regs.REG_TRIGGER_STIM_BEHAVIOR == MSK_TRIGGER_STIM_CONF_START_REPS || app_regs.REG_TRIGGER_STIM_BEHAVIOR == MSK_TRIGGER_STIM_CONF_START_STOP_INFINITE)
		{
			app_regs.REG_STIM_START = MSK_STIM_START_REPS;
			app_write_REG_STIM_START(&app_regs.REG_STIM_START);
		}
		
		if (app_regs.REG_TRIGGER_STIM_BEHAVIOR == MSK_TRIGGER_STIM_CONF_START_INFINITE)
		{
			app_regs.REG_STIM_START = MSK_STIM_START_INFINITE;
			app_write_REG_STIM_START(&app_regs.REG_STIM_START);
		}
	}
	else
	{
		if (app_regs.REG_TRIGGER_STIM_BEHAVIOR == MSK_TRIGGER_STIM_CONF_START_STOP_INFINITE)
		{
			app_regs.REG_STIM_START = MSK_STIM_STOP;
			app_write_REG_STIM_START(&app_regs.REG_STIM_START);
		}
	}
	
	reti();
}

/************************************************************************/
/* Laser ON in case interleave stim is ON                               */
/************************************************************************/
ISR(TCC0_CCC_vect, ISR_NAKED)
{
	set_controlled_OUT0;
	reti();
}
	
/************************************************************************/
/* Laser OFF in case interleave stim is ON                              */
/************************************************************************/
ISR(TCC0_CCD_vect, ISR_NAKED)
{
	clr_OUT0;
	reti();
}

/************************************************************************/ 
/* IN0                                                                  */
/************************************************************************/
bool in0_previous_state = false;

ISR(PORTH_INT0_vect, ISR_NAKED)
{
	if (read_IN0)
	{
		if (in0_previous_state == false)
		{
			in0_previous_state = true;
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_R || app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_R_AND_F)
			{
				app_regs.REG_IN_READ &= ~(B_DIN1<<4);		// Remove IN1 mask
				app_regs.REG_IN_READ |=  (B_DIN0<<4);		// Add IN0 mask
				
				app_regs.REG_IN_READ |= B_DIN0;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_START_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM_WITHOUT_EVTS)
			{
				app_regs.REG_START = B_START_EXT_CAMERA_WITHOUT_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_START_EXT_CAMERA_WITH_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITHOUT_EVTS)
			{
				app_regs.REG_START = B_START_TRIGGER | B_START_EXT_CAMERA_WITHOUT_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_START_TRIGGER | B_START_EXT_CAMERA_WITH_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STIM_REPS)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_REPS;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
		
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STIM_INFINITE)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_INFINITE;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STIM_INTERLEAVE)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_INTERLEAVE;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
		}
	}
	else
	{
		if (in0_previous_state == true)
		{
			in0_previous_state = false;
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_F || app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_R_AND_F)
			{
				app_regs.REG_IN_READ &= ~(B_DIN1<<4);		// Remove IN1 mask
				app_regs.REG_IN_READ |=  (B_DIN0<<4);		// Add IN0 mask
				
				app_regs.REG_IN_READ &= ~B_DIN0;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_STOP_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM_WITHOUT_EVTS || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITHOUT_EVTS || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_TRIGGER | B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STIM_INFINITE)
			{
				app_regs.REG_STIM_START = MSK_STIM_STOP;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
			
			else if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STIM_INTERLEAVE)
			{
				app_regs.REG_STIM_START = MSK_STIM_STOP;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
		}
	}
	
	reti();
}

/************************************************************************/ 
/* IN1                                                                  */
/************************************************************************/
bool in1_previous_state = false;

ISR(PORTK_INT0_vect, ISR_NAKED)
{
	if (read_IN1)
	{
		if (in1_previous_state == false)
		{
			in1_previous_state = true;
			
			if (app_regs.REG_IN1_CONF == MSK_IN_C_SOFTWARE_R || app_regs.REG_IN1_CONF == MSK_IN_C_SOFTWARE_R_AND_F)
			{
				app_regs.REG_IN_READ &= ~(B_DIN0<<4);		// Remove IN0 mask
				app_regs.REG_IN_READ |=  (B_DIN1<<4);		// Add IN1 mask
				
				app_regs.REG_IN_READ |=  B_DIN1;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_START_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_CAM_WITHOUT_EVTS)
			{
				app_regs.REG_START = B_START_EXT_CAMERA_WITHOUT_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_START_EXT_CAMERA_WITH_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITHOUT_EVTS)
			{
				app_regs.REG_START = B_START_TRIGGER | B_START_EXT_CAMERA_WITHOUT_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_START_TRIGGER | B_START_EXT_CAMERA_WITH_EVENTS;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STIM_REPS)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_REPS;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STIM_INFINITE)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_INFINITE;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STIM_INTERLEAVE)
			{
				app_regs.REG_STIM_START = MSK_STIM_START_INTERLEAVE;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
		}
	}
	else
	{
		if (in1_previous_state == true)
		{
			in1_previous_state = false;
			
			if (app_regs.REG_IN1_CONF == MSK_IN_C_SOFTWARE_F || app_regs.REG_IN1_CONF == MSK_IN_C_SOFTWARE_R_AND_F)
			{
				app_regs.REG_IN_READ &= ~(B_DIN0<<4);		// Remove IN0 mask
				app_regs.REG_IN_READ |=  (B_DIN1<<4);		// Add IN1 mask
				
				app_regs.REG_IN_READ &= ~B_DIN1;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_STOP_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_CAM_WITHOUT_EVTS || app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITHOUT_EVTS || app_regs.REG_IN1_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM_WITH_EVTS)
			{
				app_regs.REG_START = B_STOP_TRIGGER | B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STIM_INFINITE)
			{
				app_regs.REG_STIM_START = MSK_STIM_STOP;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
			else if (app_regs.REG_IN1_CONF == MSK_IN_C_START_STIM_INTERLEAVE)
			{
				app_regs.REG_STIM_START = MSK_STIM_STOP;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
				core_func_send_event(ADD_REG_STIM_START, true);
			}
		}
	}
	
	reti();
}

/************************************************************************/
/* SCREEN_IS_USING_USB                                                  */
/************************************************************************/
extern int8_t ms_countdown_to_switch_to_screen;

ISR(PORTJ_INT1_vect, ISR_NAKED)
{
	/* Screen doesn't need the communication */
	/* Move switch to main                   */
	if (!read_SCREEN_IS_USING_USB)
	{
		ms_countdown_to_switch_to_screen = -1;

		clr_SCREEN_CAN_USE_USB;
		
		clr_EN_SERIAL_MAIN;
		clr_EN_SERIAL_SCREEN;
		set_EN_SERIAL_MAIN;
	}
	
	reti();
}

/************************************************************************/
/* Stimulation															*/
/************************************************************************/
uint16_t opto_stim_reps_counter;
uint16_t opto_stim_period_counter;
uint16_t opto_stim_on_counter;

ISR(TCE0_OVF_vect, ISR_NAKED)
{
	if (read_OUT0)
	{
		if (++opto_stim_on_counter == app_regs.REG_STIM_ON)
		{
			opto_stim_on_counter = 0;
			
			/* If t_ON and t_PERIOD are equal, it means that STIM will be
			 * always on. So, turn STIM OFF only if they are different.
			 */
			if (app_regs.REG_STIM_ON != app_regs.REG_STIM_PERIOD)
				clr_OUT0;
		}
	}
	
	if (++opto_stim_period_counter == app_regs.REG_STIM_PERIOD)
	{
		if (++opto_stim_reps_counter == app_regs.REG_STIM_REPS && app_regs.REG_STIM_START == MSK_STIM_START_REPS)
		{
			clr_OUT0;
			timer_type0_stop(&TCE0);
			
			update_screen_indication();
				
			reti();
		}
		else
		{		
			opto_stim_period_counter = 0;
			set_OUT0;
		}
	}
	
	reti();
}

/************************************************************************/
/* KEY_SWITCH                                                           */
/************************************************************************/
int8_t ms_countdown_to_enable_internal_laser = -1;
void manage_key_switch(void);

ISR(PORTA_INT0_vect, ISR_NAKED)
{
	manage_key_switch();
	reti();
}

void manage_key_switch(void)
{
	/* Key switch is ON */
	if (read_KEY_SWITCH)
	{
		ms_countdown_to_enable_internal_laser = 100;
	}
	
	/* Key switch is OFF */
	else
	{
		/* If internal laser is selected, stop everything */
		if (read_EN_INT_LASER)
		{
			clr_OUT0;
			timer_type0_stop(&TCE0);
		}		
		
		ms_countdown_to_enable_internal_laser = -1; // Disable any attempt to turn laser ON
			
		/* Send event if the state changed */
		if (app_regs.REG_STIM_KEY_SWITCH_STATE == B_KEY_SWITCH_IS_ON)
		{
			app_regs.REG_STIM_KEY_SWITCH_STATE = 0;
			core_func_send_event(ADD_REG_STIM_KEY_SWITCH_STATE, true);
			
			update_screen_indication();
		}
	}
	
	reti();
}

void enable_internal_laser(void)
{
	if (read_KEY_SWITCH && read_EN_INT_LASER)
	{
		clr_OUT0;	// Disable laser even if OUT0 is already high
	}
	
	app_write_REG_DAC_LASER(&app_regs.REG_DAC_LASER);	// Restore laser analog voltage
		
	
	/* Send event if the state changed */
	if (app_regs.REG_STIM_KEY_SWITCH_STATE == 0)
	{
		app_regs.REG_STIM_KEY_SWITCH_STATE = B_KEY_SWITCH_IS_ON;
		core_func_send_event(ADD_REG_STIM_KEY_SWITCH_STATE, true);
	}
	
	update_screen_indication();
}

/************************************************************************/
/* External camera                                                      */
/************************************************************************/
bool ext_camera_stop = false;

ISR(TCF0_OVF_vect, ISR_NAKED)
{
	if(read_OUT1)
	{
		clr_OUT1;
		
		if (ext_camera_stop)
		{
			ext_camera_stop = false;
			app_regs.REG_EXT_CAMERA_START = MSK_EXT_CAM_STOP;
			timer_type0_stop(&TCF0);
		}
	}
	else
	{
		set_OUT1;
		
		if (app_regs.REG_EXT_CAMERA_START == MSK_EXT_CAM_START_WITH_EVENTS)
		{
			core_func_send_event(ADD_REG_EXT_CAMERA_START, true);
		}
	}
	
	reti();
}