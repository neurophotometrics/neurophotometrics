#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"

#include "ad5204.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_RAW_POT_L410,
	&app_read_REG_RAW_POT_L470,
	&app_read_REG_RAW_POT_L560,
	&app_read_REG_RAW_POT_LEXTRA,
	&app_read_REG_RAW_IOS_SET,
	&app_read_REG_RAW_IOS_CLEAR,
	&app_read_REG_RAW_IOS_TOGGLE,
	&app_read_REG_RAW_IOS_WRITE,
	&app_read_REG_RAW_IOS_READ,
	&app_read_REG_START,
	&app_read_REG_FRAME_EVENT,
	&app_read_REG_FRAME_EVENT_CFG,
	&app_read_REG_TRIGGER_STATE,
	&app_read_REG_TRIGGER_STATE_LENGTH,	
	&app_read_REG_TRIGGER_PERIOD,
	&app_read_REG_TRIGGER_T_ON,
	&app_read_REG_TRIGGER_PRE_LEDS,
	&app_read_REG_DUMMY0,
	&app_read_REG_DUMMY1,
	&app_read_REG_OUT0_CONF,
	&app_read_REG_OUT1_CONF,
	&app_read_REG_IN0_CONF,		
	&app_read_REG_OUT_SET,
	&app_read_REG_OUT_CLEAR,
	&app_read_REG_OUT_TOGGLE,
	&app_read_REG_OUT_WRITE,
	&app_read_REG_IN_READ,		
	&app_read_REG_ADCIN,
	&app_read_REG_ADCIN_CONF,
	&app_read_REG_STIM_START,
	&app_read_REG_STIM_PERIOD,
	&app_read_REG_STIM_ON,
	&app_read_REG_STIM_REPS,
	&app_read_REG_EXT_CAMERA_START,
	&app_read_REG_EXT_CAMERA_PERIOD
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_RAW_POT_L410,
	&app_write_REG_RAW_POT_L470,
	&app_write_REG_RAW_POT_L560,
	&app_write_REG_RAW_POT_LEXTRA,
	&app_write_REG_RAW_IOS_SET,
	&app_write_REG_RAW_IOS_CLEAR,
	&app_write_REG_RAW_IOS_TOGGLE,
	&app_write_REG_RAW_IOS_WRITE,
	&app_write_REG_RAW_IOS_READ,
	&app_write_REG_START,
	&app_write_REG_FRAME_EVENT,
	&app_write_REG_FRAME_EVENT_CFG,
	&app_write_REG_TRIGGER_STATE,
	&app_write_REG_TRIGGER_STATE_LENGTH,	
	&app_write_REG_TRIGGER_PERIOD,
	&app_write_REG_TRIGGER_T_ON,
	&app_write_REG_TRIGGER_PRE_LEDS,	
	&app_write_REG_DUMMY0,
	&app_write_REG_DUMMY1,
	&app_write_REG_OUT0_CONF,
	&app_write_REG_OUT1_CONF,
	&app_write_REG_IN0_CONF,	
	&app_write_REG_OUT_SET,
	&app_write_REG_OUT_CLEAR,
	&app_write_REG_OUT_TOGGLE,
	&app_write_REG_OUT_WRITE,
	&app_write_REG_IN_READ,		
	&app_write_REG_ADCIN,
	&app_write_REG_ADCIN_CONF,
	&app_write_REG_STIM_START,
	&app_write_REG_STIM_PERIOD,
	&app_write_REG_STIM_ON,
	&app_write_REG_STIM_REPS,
	&app_write_REG_EXT_CAMERA_START,
	&app_write_REG_EXT_CAMERA_PERIOD
};


/************************************************************************/
/* REG_RAW_POT_L410                                                     */
/************************************************************************/
void app_read_REG_RAW_POT_L410(void) {}
bool app_write_REG_RAW_POT_L410(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	as5204_write_channel(0, reg);
	app_regs.REG_RAW_POT_L410 = reg;
	return true;
}


/************************************************************************/
/* REG_RAW_POT_L470                                                     */
/************************************************************************/
void app_read_REG_RAW_POT_L470(void) {}
bool app_write_REG_RAW_POT_L470(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	as5204_write_channel(2, reg);
	app_regs.REG_RAW_POT_L470 = reg;
	return true;
}


/************************************************************************/
/* REG_RAW_POT_L560                                                     */
/************************************************************************/
void app_read_REG_RAW_POT_L560(void) {}
bool app_write_REG_RAW_POT_L560(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	as5204_write_channel(1, reg);
	app_regs.REG_RAW_POT_L560 = reg;
	return true;
}


/************************************************************************/
/* REG_RAW_POT_LEXTRA                                                   */
/************************************************************************/
void app_read_REG_RAW_POT_LEXTRA(void) {}
bool app_write_REG_RAW_POT_LEXTRA(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	as5204_write_channel(3, reg);
	app_regs.REG_RAW_POT_LEXTRA = reg;
	return true;
}


/************************************************************************/
/* REG_RAW_IOS_SET                                                      */
/************************************************************************/
void app_read_REG_RAW_IOS_SET(void) {}
bool app_write_REG_RAW_IOS_SET(void *a)
{
	uint32_t reg = *((uint32_t*)a);
	
	if (reg & B_CAM_TRIGGER) set_CAM_TRIGGER;
	if (reg & B_CAM_IO_0) set_CAM_IO_0;
	if (reg & B_CAM_IO_1) set_CAM_IO_1;
	// B_CAM_STROB_IN is input only
	// B_BUTTON_START_STOP is input only
	// B_BUTTON_TEST_1 is input only
	if (reg & B_EN_OUT_1) set_EN_OUT_1;
	if (reg & B_EN_OUT_0) set_EN_OUT_0;
	if (reg & B_EN_IN_0) clr_EN_IN_0;
	if (reg & B_EN_L410) set_EN_L410;
	if (reg & B_EN_L470) set_EN_L470;
	if (reg & B_EN_L560) set_EN_L560;
	if (reg & B_EN_LEXTRA) set_EN_LEXTRA;
	if (reg & B_L410) set_L410;
	if (reg & B_L470) set_L470;
	if (reg & B_L560) set_L560;
	if (reg & B_LPHOTODIODE) set_LPHOTODIODE;
	if (reg & B_LEXTRA) set_LEXTRA;
	if (reg & B_OUT1) set_OUT1;
	if (reg & B_OUT0) set_OUT0;
	//B_IN0is input only
	if (reg & B_POT_CS) clr_POT_CS;
	if (reg & B_POT_SDI) set_POT_SDI;
	if (reg & B_POT_CLK) set_POT_CLK;
	if (reg & B_POT_MIDSCALE) clr_POT_MIDSCALE;
	if (reg & B_POT_SHDN) clr_POT_SHDN;
	if (reg & B_CLK_EN_IN) set_CLK_EN_IN;
	if (reg & B_CLK_EN_BYPASS) set_CLK_EN_BYPASS;
	if (reg & B_CLK_EN_OUT) set_CLK_EN_OUT;

	return true;
}


/************************************************************************/
/* REG_RAW_IOS_CLEAR                                                    */
/************************************************************************/
void app_read_REG_RAW_IOS_CLEAR(void) {}
bool app_write_REG_RAW_IOS_CLEAR(void *a)
{
	uint32_t reg = *((uint32_t*)a);
	
	if (reg & B_CAM_TRIGGER) clr_CAM_TRIGGER;
	if (reg & B_CAM_IO_0) clr_CAM_IO_0;
	if (reg & B_CAM_IO_1) clr_CAM_IO_1;
	// B_CAM_STROB_IN is input only
	// B_BUTTON_START_STOP is input only
	// B_BUTTON_TEST_1 is input only
	if (reg & B_EN_OUT_1) clr_EN_OUT_1;
	if (reg & B_EN_OUT_0) clr_EN_OUT_0;
	if (reg & B_EN_IN_0) set_EN_IN_0;
	if (reg & B_EN_L410) clr_EN_L410;
	if (reg & B_EN_L470) clr_EN_L470;
	if (reg & B_EN_L560) clr_EN_L560;
	if (reg & B_EN_LEXTRA) clr_EN_LEXTRA;
	if (reg & B_L410) clr_L410;
	if (reg & B_L470) clr_L470;
	if (reg & B_L560) clr_L560;
	if (reg & B_LPHOTODIODE) clr_LPHOTODIODE;
	if (reg & B_LEXTRA) clr_LEXTRA;
	if (reg & B_OUT1) clr_OUT1;
	if (reg & B_OUT0) clr_OUT0;
	//B_IN0is input only
	if (reg & B_POT_CS) set_POT_CS;
	if (reg & B_POT_SDI) clr_POT_SDI;
	if (reg & B_POT_CLK) clr_POT_CLK;
	if (reg & B_POT_MIDSCALE) set_POT_MIDSCALE;
	if (reg & B_POT_SHDN) set_POT_SHDN;
	if (reg & B_CLK_EN_IN) clr_CLK_EN_IN;
	if (reg & B_CLK_EN_BYPASS) clr_CLK_EN_BYPASS;
	if (reg & B_CLK_EN_OUT) clr_CLK_EN_OUT;

	return true;
}


/************************************************************************/
/* REG_RAW_IOS_TOGGLE                                                   */
/************************************************************************/
void app_read_REG_RAW_IOS_TOGGLE(void) {}
bool app_write_REG_RAW_IOS_TOGGLE(void *a)
{
	uint32_t reg = *((uint32_t*)a);
	
	if (reg & B_CAM_TRIGGER) tgl_CAM_TRIGGER;
	if (reg & B_CAM_IO_0) tgl_CAM_IO_0;
	if (reg & B_CAM_IO_1) tgl_CAM_IO_1;
	// B_CAM_STROB_IN is input only
	// B_BUTTON_START_STOP is input only
	// B_BUTTON_TEST_1 is input only
	if (reg & B_EN_OUT_1) tgl_EN_OUT_1;
	if (reg & B_EN_OUT_0) tgl_EN_OUT_0;
	if (reg & B_EN_IN_0) tgl_EN_IN_0;
	if (reg & B_EN_L410) tgl_EN_L410;
	if (reg & B_EN_L470) tgl_EN_L470;
	if (reg & B_EN_L560) tgl_EN_L560;
	if (reg & B_EN_LEXTRA) tgl_EN_LEXTRA;
	if (reg & B_L410) tgl_L410;
	if (reg & B_L470) tgl_L470;
	if (reg & B_L560) tgl_L560;
	if (reg & B_LPHOTODIODE) tgl_LPHOTODIODE;
	if (reg & B_LEXTRA) tgl_LEXTRA;
	if (reg & B_OUT1) tgl_OUT1;
	if (reg & B_OUT0) tgl_OUT0;
	//B_IN0is input only
	if (reg & B_POT_CS) tgl_POT_CS;
	if (reg & B_POT_SDI) tgl_POT_SDI;
	if (reg & B_POT_CLK) tgl_POT_CLK;
	if (reg & B_POT_MIDSCALE) tgl_POT_MIDSCALE;
	if (reg & B_POT_SHDN) tgl_POT_SHDN;
	if (reg & B_CLK_EN_IN) tgl_CLK_EN_IN;
	if (reg & B_CLK_EN_BYPASS) tgl_CLK_EN_BYPASS;
	if (reg & B_CLK_EN_OUT) tgl_CLK_EN_OUT;

	return true;
}


/************************************************************************/
/* REG_RAW_IOS_WRITE                                                    */
/************************************************************************/
void app_read_REG_RAW_IOS_WRITE(void) {}
bool app_write_REG_RAW_IOS_WRITE(void *a)
{
	uint32_t reg = *((uint32_t*)a);
	
	if (reg & B_CAM_TRIGGER) set_CAM_TRIGGER; else clr_CAM_TRIGGER;
	if (reg & B_CAM_IO_0) set_CAM_IO_0; else clr_CAM_TRIGGER;
	if (reg & B_CAM_IO_1) set_CAM_IO_1; else clr_CAM_TRIGGER;
	// B_CAM_STROB_IN is input only
	// B_BUTTON_START_STOP is input only
	// B_BUTTON_TEST_1 is input only
	if (reg & B_EN_OUT_1) set_EN_OUT_1; else clr_CAM_TRIGGER;
	if (reg & B_EN_OUT_0) set_EN_OUT_0; else clr_CAM_TRIGGER;
	if (reg & B_EN_IN_0) clr_EN_IN_0; else clr_CAM_TRIGGER;
	if (reg & B_EN_L410) set_EN_L410; else clr_CAM_TRIGGER;
	if (reg & B_EN_L470) set_EN_L470; else clr_CAM_TRIGGER;
	if (reg & B_EN_L560) set_EN_L560; else clr_CAM_TRIGGER;
	if (reg & B_EN_LEXTRA) set_EN_LEXTRA; else clr_CAM_TRIGGER;
	if (reg & B_L410) set_L410; else clr_CAM_TRIGGER;
	if (reg & B_L470) set_L470; else clr_CAM_TRIGGER;
	if (reg & B_L560) set_L560; else clr_CAM_TRIGGER;
	if (reg & B_LPHOTODIODE) set_LPHOTODIODE; else clr_CAM_TRIGGER;
	if (reg & B_LEXTRA) set_LEXTRA; else clr_CAM_TRIGGER;
	if (reg & B_OUT1) set_OUT1; else clr_CAM_TRIGGER;
	if (reg & B_OUT0) set_OUT0; else clr_CAM_TRIGGER;
	//B_IN0is input only
	if (reg & B_POT_CS) clr_POT_CS; else set_CAM_TRIGGER;
	if (reg & B_POT_SDI) set_POT_SDI; else clr_CAM_TRIGGER;
	if (reg & B_POT_CLK) set_POT_CLK; else clr_CAM_TRIGGER;
	if (reg & B_POT_MIDSCALE) clr_POT_MIDSCALE; else set_CAM_TRIGGER;
	if (reg & B_POT_SHDN) clr_POT_SHDN; else set_CAM_TRIGGER;
	if (reg & B_CLK_EN_IN) set_CLK_EN_IN; else clr_CAM_TRIGGER;
	if (reg & B_CLK_EN_BYPASS) set_CLK_EN_BYPASS; else clr_CAM_TRIGGER;
	if (reg & B_CLK_EN_OUT) set_CLK_EN_OUT; else clr_CAM_TRIGGER;

	return true;
}


/************************************************************************/
/* REG_RAW_IOS_READ                                                     */
/************************************************************************/
void app_read_REG_RAW_IOS_READ(void)
{
	uint32_t reg = 0;
	
	if (read_CAM_TRIGGER) reg |= B_CAM_TRIGGER;
	if (read_CAM_IO_0) reg |= B_CAM_IO_0;
	if (read_CAM_IO_1) reg |= B_CAM_IO_1;
	if (read_CAM_STROB_IN) reg |= B_CAM_STROB_IN;
	if (read_BUTTON_START_STOP) reg |= B_BUTTON_START_STOP;
	if (read_BUTTON_TEST_1) reg |= B_BUTTON_TEST_1;
	if (read_EN_OUT_1) reg |= B_EN_OUT_1;
	if (read_EN_OUT_0) reg |= B_EN_OUT_0;
	if (read_EN_IN_0) reg |= clr_EN_IN_0;
	if (read_EN_L410) reg |= B_EN_L410;
	if (read_EN_L470) reg |= B_EN_L470;
	if (read_EN_L560) reg |= B_EN_L560;
	if (read_EN_LEXTRA) reg |= B_EN_LEXTRA;
	if (read_L410) reg |= B_L410;
	if (read_L470) reg |= B_L470;
	if (read_L560) reg |= B_L560;
	if (read_LPHOTODIODE) reg |= B_LPHOTODIODE;
	if (read_LEXTRA) reg |= B_LEXTRA;
	if (read_OUT1) reg |= B_OUT1;
	if (read_OUT0) reg |= B_OUT0;
	if (read_IN0) reg |= B_IN0;
	if (read_POT_CS) reg |= clr_POT_CS;
	if (read_POT_SDI) reg |= B_POT_SDI;
	if (read_POT_CLK) reg |= B_POT_CLK;
	if (read_POT_MIDSCALE) reg |= clr_POT_MIDSCALE;
	if (read_POT_SHDN) reg |= clr_POT_SHDN;
	if (read_CLK_EN_IN) reg |= B_CLK_EN_IN;
	if (read_CLK_EN_BYPASS) reg |= B_CLK_EN_BYPASS;
	if (read_CLK_EN_OUT) reg |= B_CLK_EN_OUT;
	
	app_regs.REG_RAW_IOS_READ = reg;
}

bool app_write_REG_RAW_IOS_READ(void *a) { return false; }


/************************************************************************/
/* REG_START                                                            */
/************************************************************************/
uint8_t trigger_prescaler;
uint16_t trigger_target_count;

extern uint8_t trigger_state_index;
extern bool trigger_stop;
extern bool first_cam_trig;

void app_read_REG_START(void)
{
	app_regs.REG_START = 0;
	
	if (TCC0_CTRLA != 0)
		app_regs.REG_START |= B_START_TRIGGER;
	else
		app_regs.REG_START |= B_STOP_TRIGGER;
		
}

bool app_write_REG_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_START_TRIGGER && TCC0_CTRLA == 0)
	{
		trigger_stop = false;
		trigger_state_index = 0;
		first_cam_trig = true;
		app_regs.REG_ADCIN_CONF = MSK_ADCIN_DISABLED;
		
		timer_type0_pwm(&TCC0, TIMER_PRESCALER_DIV64, (app_regs.REG_TRIGGER_PERIOD >> 1), (app_regs.REG_TRIGGER_T_ON >> 1), INT_LEVEL_LOW, INT_LEVEL_LOW);
		timer_type0_enable(&TCE0, TIMER_PRESCALER_DIV64, ((app_regs.REG_TRIGGER_PERIOD - app_regs.REG_TRIGGER_PRE_LEDS) - 2) >> 1, INT_LEVEL_LOW);
		
		set_L470;	// Always light the 470 on the first frame
		clr_CAM_TRIGGER;
		
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
	}
		
	if (reg & B_START_EXT_CAMERA)
	{
		uint8_t aux = B_START_EXT_CAM;
		app_write_REG_EXT_CAMERA_START(&aux);
	}
	
	if (reg & B_STOP_TRIGGER && TCC0_CTRLA != 0)
		trigger_stop = true;
		
	
	if (reg & B_STOP_EXT_CAMERA)
	{
		uint8_t aux = 0;
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
	app_regs.REG_FRAME_EVENT  = read_L410 ? B_ON_L410 : 0;
	app_regs.REG_FRAME_EVENT |= read_L470 ? B_ON_L470 : 0;
	app_regs.REG_FRAME_EVENT |= read_L560 ? B_ON_L560 : 0;
	app_regs.REG_FRAME_EVENT |= read_LEXTRA ? B_ON_LEXTRA : 0;
	app_regs.REG_FRAME_EVENT |= read_OUT0 ? B_ON_OUT0 : 0;
	app_regs.REG_FRAME_EVENT |= read_OUT1 ? B_ON_OUT1 : 0;
	app_regs.REG_FRAME_EVENT |= app_regs.REG_STIM_START ? B_ON_OPTOGEN_BEHAVIOR : 0;
	app_regs.REG_FRAME_EVENT |= read_IN0 ? B_ON_IN : 0;
}

bool app_write_REG_FRAME_EVENT(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_FRAME_EVENT = reg;
	return true;
}


/************************************************************************/
/* REG_FRAME_EVENT_CFG                                                  */
/************************************************************************/
void app_read_REG_FRAME_EVENT_CFG(void){}
bool app_write_REG_FRAME_EVENT_CFG(void *a)
{
	if (*((uint8_t*)a) & ~GM_FRAME_TRIG) return false;
	app_regs.REG_FRAME_EVENT_CFG = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_TRIGGER_STATE                                                    */
/************************************************************************/
// This register is an array with 240 positions
void app_read_REG_TRIGGER_STATE(void){}
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
void app_read_REG_TRIGGER_STATE_LENGTH(void){}
bool app_write_REG_TRIGGER_STATE_LENGTH(void *a)
{
	if (*((uint8_t*)a) == 0) return false;
	if (*((uint8_t*)a) > 32) return false;

	app_regs.REG_TRIGGER_STATE_LENGTH = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_TRIGGER_FREQUENCY                                                */
/************************************************************************/
void app_read_REG_TRIGGER_PERIOD(void) {}
bool app_write_REG_TRIGGER_PERIOD(void *a)
{
	if (*((uint16_t*)a) < 5000) // 5 ms -> 200Hz
		return false;
		
	app_regs.REG_TRIGGER_PERIOD = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_TRIGGER_FREQUENCY                                                */
/************************************************************************/
void app_read_REG_TRIGGER_T_ON(void) {}
bool app_write_REG_TRIGGER_T_ON(void *a)
{
	app_regs.REG_TRIGGER_T_ON = *((uint16_t*)a);
	return true;
}

/************************************************************************/
/* REG_TRIGGER_FREQUENCY                                                */
/************************************************************************/
void app_read_REG_TRIGGER_PRE_LEDS(void) {}
bool app_write_REG_TRIGGER_PRE_LEDS(void *a)
{
	if (*((uint16_t*)a) < 10) // 10 us
		return false;
	
	app_regs.REG_TRIGGER_PRE_LEDS = *((uint16_t*)a);
	return true;
}


/************************************************************************/
/* REG_DUMMY0                                                           */
/************************************************************************/
void app_read_REG_DUMMY0(void){}
bool app_write_REG_DUMMY0(void *a)
{
	app_regs.REG_DUMMY0 = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_DUMMY1                                                           */
/************************************************************************/
void app_read_REG_DUMMY1(void){}
bool app_write_REG_DUMMY1(void *a)
{
	app_regs.REG_DUMMY1 = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT0_CONF                                                        */
/************************************************************************/
void app_read_REG_OUT0_CONF(void){}
bool app_write_REG_OUT0_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_OUT_CONF) return false;
	app_regs.REG_OUT0_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_OUT1_CONF                                                        */
/************************************************************************/
void app_read_REG_OUT1_CONF(void){}
bool app_write_REG_OUT1_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_OUT_CONF) return false;
	app_regs.REG_OUT1_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_IN0_CONF                                                         */
/************************************************************************/
void app_read_REG_IN0_CONF(void){}
bool app_write_REG_IN0_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_IN_CONF) return false;
	app_regs.REG_IN0_CONF = *((uint8_t*)a);
	return true;
}

	
/************************************************************************/
/* REG_OUT_X                                                            */
/************************************************************************/
void app_read_REG_OUT_SET(void) {}
void app_read_REG_OUT_CLEAR(void) {}
void app_read_REG_OUT_TOGGLE(void) {}
void app_read_REG_OUT_WRITE(void){
	app_regs.REG_OUT_WRITE = (read_OUT0 ? B_DOUT0 : 0) | (read_OUT1 ? B_DOUT1 : 0);
}
	
bool app_write_REG_OUT_SET(void *a)
{	
	if (*((uint8_t*)a) & B_DOUT0) set_OUT0;
	if (*((uint8_t*)a) & B_DOUT1) set_OUT1;
	
	app_regs.REG_OUT_SET = *((uint8_t*)a);
	return true;
}

bool app_write_REG_OUT_CLEAR(void *a)
{
	if (*((uint8_t*)a) & B_DOUT0) clr_OUT0;
	if (*((uint8_t*)a) & B_DOUT1) clr_OUT1;
	
	app_regs.REG_OUT_CLEAR = *((uint8_t*)a);
	return true;
}

bool app_write_REG_OUT_TOGGLE(void *a)
{
	if (*((uint8_t*)a) & B_DOUT0) tgl_OUT0;
	if (*((uint8_t*)a) & B_DOUT1) tgl_OUT1;
	
	app_regs.REG_OUT_TOGGLE = *((uint8_t*)a);
	return true;
}

bool app_write_REG_OUT_WRITE(void *a)
{
	if (*((uint8_t*)a) & B_DOUT0) set_OUT0; else clr_OUT0;
	if (*((uint8_t*)a) & B_DOUT1) tgl_OUT1; else clr_OUT1;
	
	app_regs.REG_OUT_WRITE = *((uint8_t*)a);
	return true;
}

/************************************************************************/
/* REG_IN_READ                                                          */
/************************************************************************/
void app_read_REG_IN_READ(void){
	app_regs.REG_IN_READ = (read_IN0 ? B_DIN0 : 0);
}
bool app_write_REG_IN_READ(void *a){
	return false;
}

/************************************************************************/
/* REG_ADCIN                                                            */
/************************************************************************/
void app_read_REG_ADCIN(void)
{
	app_regs.REG_ADCIN = adc_A_read_channel(5);
}

bool app_write_REG_ADCIN(void *a)
{	
	return false;
}


/************************************************************************/
/* REG_ADCIN_CONF                                                       */
/************************************************************************/
void app_read_REG_ADCIN_CONF(void){}
bool app_write_REG_ADCIN_CONF(void *a)
{
	if(*((uint8_t*)a) & ~GM_ADCIN_CONF) return false;
	
	if (app_regs.REG_START & B_START_TRIGGER)
		return true;

	app_regs.REG_ADCIN_CONF = *((uint8_t*)a);
	return true;
}


/************************************************************************/
/* REG_STIM_START                                                       */
/************************************************************************/
extern uint16_t opto_stim_reps_counter;
extern uint16_t opto_stim_period_counter;
extern uint16_t opto_stim_on_counter;
extern bool opto_behavior_stop;

void app_read_REG_STIM_START(void)
{
	app_regs.REG_STIM_START = (TCD0_CTRLA != 0) ? B_START_OPTO : 0;
}

bool app_write_REG_STIM_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & ~B_START_OPTO)
		return false;
	
	if (app_regs.REG_STIM_ON >= app_regs.REG_STIM_PERIOD)
		return false;

	if (reg)
	{
		if (app_regs.REG_STIM_REPS != 0 /*&& TCD0_CTRLA == 0*/)
		{
			timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV256,125, INT_LEVEL_LOW);
			set_LEXTRA;
			opto_stim_reps_counter = 0;
			opto_stim_period_counter = 0;
			opto_stim_on_counter = 0;
			opto_behavior_stop = false;
		}
	}
	else
	{
		if (TCD0_CTRLA != 0)
		{
			// Stop in the end of the pulse
			if (0)
			{
				opto_behavior_stop = true;
			}
			
			// Stop immediately
			if(1)
			{		
				clr_LEXTRA;
				timer_type0_stop(&TCD0);
			}
		}
	}
		
	app_regs.REG_STIM_START = reg;
	return true;
}


/************************************************************************/
/* REG_STIM_PERIOD                                                      */
/************************************************************************/
void app_read_REG_STIM_PERIOD(void){}
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
void app_read_REG_STIM_ON(void){}
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
void app_read_REG_STIM_REPS(void){}
bool app_write_REG_STIM_REPS(void *a)
{
	app_regs.REG_STIM_REPS = *((uint16_t*)a);
	return true;
}

/************************************************************************/
/* REG_EXT_CAMERA_START                                                 */
/************************************************************************/
extern bool ext_camera_stop;

void app_read_REG_EXT_CAMERA_START(void)
{
	app_regs.REG_EXT_CAMERA_START = (TCF0_CTRLA != 0 ) ? B_START_EXT_CAM : 0;
}

bool app_write_REG_EXT_CAMERA_START(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & ~B_START_EXT_CAM)
		return false;
	
	app_regs.REG_EXT_CAMERA_START = reg;
	
	if (reg)
	{
		if (TCF0_CTRLA == 0)
		{
			timer_type0_enable(&TCF0, TIMER_PRESCALER_DIV64, app_regs.REG_EXT_CAMERA_PERIOD >> 2 /* double */,  INT_LEVEL_LOW);
			set_OUT0;
			core_func_send_event(ADD_REG_EXT_CAMERA_START, true);
			ext_camera_stop = false;
		}
	}
	else
	{
		if (TCF0_CTRLA != 0)
		{
			ext_camera_stop = true;
		}
	}
	
	return true;
}


/************************************************************************/
/* REG_EXT_CAMERA_FREQUENCY                                             */
/************************************************************************/
void app_read_REG_EXT_CAMERA_PERIOD(void){}
bool app_write_REG_EXT_CAMERA_PERIOD(void *a)
{
	if (*((uint16_t*)a) < 5000) // 5 ms -> 200Hz
		return false;

	app_regs.REG_EXT_CAMERA_PERIOD = *((uint16_t*)a);
	return true;
}