#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

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
/* CAM_STROB_IN                                                         */
/************************************************************************/
ISR(PORTH_INT0_vect, ISR_NAKED)
{
	if (read_CAM_STROB_IN)
	{
		if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_STROBE)
			set_OUT0;
		if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_STROBE)
			set_OUT1;
		
		if (app_regs.REG_FRAME_EVENT_CFG == MSK_FRAME_TRIG_STROBE_IN)
		{
			if (read_CAM_STROB_IN)
			{
				app_regs.REG_FRAME_EVENT  = read_L410 ? B_ON_L410 : 0;
				app_regs.REG_FRAME_EVENT |= read_L470 ? B_ON_L470 : 0;
				app_regs.REG_FRAME_EVENT |= read_L560 ? B_ON_L560 : 0;
				app_regs.REG_FRAME_EVENT |= read_LEXTRA ? B_ON_LEXTRA : 0;
				app_regs.REG_FRAME_EVENT |= read_OUT0 ? B_ON_OUT0 : 0;
				app_regs.REG_FRAME_EVENT |= read_OUT1 ? B_ON_OUT1 : 0;
				app_regs.REG_FRAME_EVENT |= app_regs.REG_STIM_START ? B_ON_OPTOGEN_BEHAVIOR : 0;
				app_regs.REG_FRAME_EVENT |= read_IN0 ? B_ON_IN : 0;
				
				core_func_send_event(ADD_REG_FRAME_EVENT, true);
			}
		}
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
/* BUTTON_START_STOP                                                    */
/************************************************************************/
ISR(PORTK_INT0_vect, ISR_NAKED)
{
	reti();
}

/************************************************************************/ 
/* BUTTON_TEST_1                                                        */
/************************************************************************/
ISR(PORTK_INT1_vect, ISR_NAKED)
{
	reti();
}

/************************************************************************/ 
/* IN0                                                                  */
/************************************************************************/
bool in0_previous_state = false;

ISR(PORTE_INT0_vect, ISR_NAKED)
{	
	if (read_IN0)
	{
		if (in0_previous_state == false)
		{
			in0_previous_state = true;
		
			if (app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_R || app_regs.REG_IN0_CONF == MSK_IN_C_SOFTWARE_R_AND_F)
			{
				app_regs.REG_IN_READ = B_DIN0;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_TRIG || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_START_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_CAM || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM)
			{
				app_regs.REG_START = B_START_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_TRIG_AND_CAM || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM)
			{
				app_regs.REG_START = B_START_TRIGGER | B_START_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_OPTO_BEHAVIOR || app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_OPTO_BEHAVIOR)
			{
				app_regs.REG_IN_READ = B_DIN0;
				core_func_send_event(ADD_REG_IN_READ, true);
				
				app_regs.REG_STIM_START = 1;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
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
				app_regs.REG_IN_READ = 0;
				core_func_send_event(ADD_REG_IN_READ, true);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG)
			{
				app_regs.REG_START = B_STOP_TRIGGER;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_CAM)
			{
				app_regs.REG_START = B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_TRIG_AND_CAM)
			{
				app_regs.REG_START = B_STOP_TRIGGER | B_STOP_EXT_CAMERA;
				app_write_REG_START(&app_regs.REG_START);
			}
			
			if (app_regs.REG_IN0_CONF == MSK_IN_C_START_STOP_OPTO_BEHAVIOR)
			{
				app_regs.REG_IN_READ = 0;
				core_func_send_event(ADD_REG_IN_READ, true);
				
				app_regs.REG_STIM_START = 0;
				app_write_REG_STIM_START(&app_regs.REG_STIM_START);
			}
		}
	}
	
	reti();
}

/************************************************************************/
/* Camera                                                               */
/************************************************************************/
uint8_t trigger_state_index;
bool trigger_stop;
bool first_cam_trig;

/************************************************************************/
/* Light up LEDs                                                        */
/************************************************************************/
ISR(TCE0_OVF_vect, ISR_NAKED)
{
	uint8_t trigger_state = app_regs.REG_TRIGGER_STATE[trigger_state_index];
	PORTC_OUTSET = (trigger_state & 7) << 1;
	reti();
}

/************************************************************************/
/* Camera's trigger to high                                             */
/************************************************************************/
ISR(TCC0_OVF_vect, ISR_NAKED)
{	
	if (first_cam_trig)
	{
		first_cam_trig = false;
		reti();
	}
	
	TCE0_CNT = 0;
	
	uint8_t trigger_state = app_regs.REG_TRIGGER_STATE[trigger_state_index++];

	if (app_regs.REG_STIM_REPS == 0)
	{
		if (trigger_state & B_ON_LEXTRA) set_LEXTRA; else clr_LEXTRA;
	}
		
	if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_STATE_CTRL)
	{
		if (trigger_state & B_ON_OUT0) set_OUT0; else clr_OUT0;
	}
		
	if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_STATE_CTRL)
	{
		if (trigger_state & B_ON_OUT1) set_OUT1; else clr_OUT1;
	}
		
	if (trigger_state & B_ON_OPTOGEN_BEHAVIOR)
	{
		app_regs.REG_STIM_START = 1;
		app_write_REG_STIM_START(&app_regs.REG_STIM_START);
	}
		
	//set_CAM_TRIGGER;
		
	if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_CAMERA)
	{
		set_OUT0;
	}
		
	if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_CAMERA)
	{
		set_OUT1;
	}
		
	if (app_regs.REG_FRAME_EVENT_CFG == MSK_FRAME_TRIG_TRIGGER_OUT)
	{
		app_regs.REG_FRAME_EVENT  = read_L410 ? B_ON_L410 : 0;
		app_regs.REG_FRAME_EVENT |= read_L470 ? B_ON_L470 : 0;
		app_regs.REG_FRAME_EVENT |= read_L560 ? B_ON_L560 : 0;
		app_regs.REG_FRAME_EVENT |= read_LEXTRA ? B_ON_LEXTRA : 0;
		app_regs.REG_FRAME_EVENT |= read_OUT0 ? B_ON_OUT0 : 0;
		app_regs.REG_FRAME_EVENT |= read_OUT1 ? B_ON_OUT1 : 0;
		app_regs.REG_FRAME_EVENT |= app_regs.REG_STIM_START ? B_ON_OPTOGEN_BEHAVIOR : 0;
		app_regs.REG_FRAME_EVENT |= read_IN0 ? B_ON_IN : 0;
			
		core_func_send_event(ADD_REG_FRAME_EVENT, true);
	}		
		
	if (trigger_state_index == app_regs.REG_TRIGGER_STATE_LENGTH)
		trigger_state_index = 0;
	
	reti();
}
			
		
/************************************************************************/
/* Camera's trigger to low                                              */
/************************************************************************/
ISR(TCC0_CCA_vect, ISR_NAKED)
{
	//clr_CAM_TRIGGER;
	PORTC_OUTCLR = 0x7 << 1;
		
	if (app_regs.REG_STIM_REPS == 0)
	{
		clr_LEXTRA;
	}
		
	if (app_regs.REG_OUT0_CONF == MSK_OUT_CONF_CAMERA)
	{
		clr_OUT0;
	}
				
	if (app_regs.REG_OUT1_CONF == MSK_OUT_CONF_CAMERA)
	{
		clr_OUT1;
	}
			
	if (trigger_stop)
	{
		trigger_stop = false;
			
		timer_type0_stop(&TCC0);
		timer_type0_stop(&TCE0);
			
		/*
		PORTC_OUTCLR = 0x7 << 1;
		
		if (app_regs.REG_STIM_REPS == 0)
		{
			clr_LEXTRA;
		}
		*/
			
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
/* External camera                                                      */
/************************************************************************/
bool ext_camera_stop = false;

ISR(TCF0_OVF_vect, ISR_NAKED)
{
	if(read_OUT0)
	{
		clr_OUT0;
		
		if (ext_camera_stop)
		{
			ext_camera_stop = false;			
			app_regs.REG_EXT_CAMERA_START = 0;
			timer_type0_stop(&TCF0);
		}
	}
	else
	{
		set_OUT0;
		
		app_regs.REG_EXT_CAMERA_START = B_START_EXT_CAM;
		core_func_send_event(ADD_REG_EXT_CAMERA_START, true);
	}
	
	reti(); 
}


/************************************************************************/
/* Opto stimulation                                                     */
/************************************************************************/
uint16_t opto_stim_reps_counter;
uint16_t opto_stim_period_counter;
uint16_t opto_stim_on_counter;
bool opto_behavior_stop;

ISR(TCD0_OVF_vect, ISR_NAKED)
{
	if (read_LEXTRA)
	{
		if (++opto_stim_on_counter == app_regs.REG_STIM_ON)
		{
			opto_stim_on_counter = 0;		
			clr_LEXTRA;
		}
	}
	
	if (++opto_stim_period_counter == app_regs.REG_STIM_PERIOD)
	{
		if (++opto_stim_reps_counter == app_regs.REG_STIM_REPS || opto_behavior_stop)
		{
			opto_behavior_stop = false;
			app_regs.REG_STIM_START = 0;
			timer_type0_stop(&TCD0);
		}
		else
		{
			opto_stim_period_counter = 0;
			set_LEXTRA;
		}
	}	
	
	reti();
}