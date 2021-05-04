#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_CONFIG(void);
void app_read_REG_RESERVED0(void);
void app_read_REG_DAC_L410(void);
void app_read_REG_DAC_L470(void);
void app_read_REG_DAC_L560(void);
void app_read_REG_DAC_ALL_LEDS(void);
void app_read_REG_DAC_LASER(void);
void app_read_REG_SCREEN_BRIGHT(void);
void app_read_REG_SCREEN_IMG_INDEX(void);
void app_read_REG_GAIN_PD_L410(void);
void app_read_REG_GAIN_PD_L470(void);
void app_read_REG_GAIN_PD_L560(void);
void app_read_REG_STIM_KEY_SWITCH_STATE(void);
void app_read_REG_STIM_START(void);
void app_read_REG_STIM_WAVELENGTH(void);
void app_read_REG_STIM_PERIOD(void);
void app_read_REG_STIM_ON(void);
void app_read_REG_STIM_REPS(void);
void app_read_REG_EXT_CAMERA_START(void);
void app_read_REG_EXT_CAMERA_PERIOD(void);
void app_read_REG_OUT0_CONF(void);
void app_read_REG_OUT1_CONF(void);
void app_read_REG_IN0_CONF(void);
void app_read_REG_IN1_CONF(void);
void app_read_REG_OUT_SET(void);
void app_read_REG_OUT_CLEAR(void);
void app_read_REG_OUT_TOGGLE(void);
void app_read_REG_OUT_WRITE(void);
void app_read_REG_IN_READ(void);
void app_read_REG_START(void);
void app_read_REG_FRAME_EVENT(void);
void app_read_REG_TRIGGER_STATE(void);
void app_read_REG_TRIGGER_STATE_LENGTH(void);
void app_read_REG_TRIGGER_PERIOD(void);
void app_read_REG_TRIGGER_T_ON(void);
void app_read_REG_TRIGGER_T_UPDATE_OUTPUTS(void);
void app_read_REG_TRIGGER_STIM_BEHAVIOR(void);
void app_read_REG_PHOTODIODES_START(void);
void app_read_REG_PHOTODIODES(void);
void app_read_REG_TEMPERATURE(void);
void app_read_REG_SCREEN_HW_VERSION_H(void);
void app_read_REG_SCREEN_HW_VERSION_L(void);
void app_read_REG_SCREEN_ASSEMBLY_VERSION(void);
void app_read_REG_SCREEN_FW_VERSION_H(void);
void app_read_REG_SCREEN_FW_VERSION_L(void);
void app_read_REG_CAMERA_SN(void);
void app_read_REG_TRIGGER_LASER_ON(void);
void app_read_REG_TRIGGER_LASER_OFF(void);
void app_read_REG_CAL_L410(void);
void app_read_REG_CAL_L470(void);
void app_read_REG_CAL_L560(void);
void app_read_REG_CAL_LASER(void);
void app_read_REG_CAL_PH410(void);
void app_read_REG_CAL_PH470(void);
void app_read_REG_CAL_PH560(void);

bool app_write_REG_CONFIG(void *a);
bool app_write_REG_RESERVED0(void *a);
bool app_write_REG_DAC_L410(void *a);
bool app_write_REG_DAC_L470(void *a);
bool app_write_REG_DAC_L560(void *a);
bool app_write_REG_DAC_ALL_LEDS(void *a);
bool app_write_REG_DAC_LASER(void *a);
bool app_write_REG_SCREEN_BRIGHT(void *a);
bool app_write_REG_SCREEN_IMG_INDEX(void *a);
bool app_write_REG_GAIN_PD_L410(void *a);
bool app_write_REG_GAIN_PD_L470(void *a);
bool app_write_REG_GAIN_PD_L560(void *a);
bool app_write_REG_STIM_KEY_SWITCH_STATE(void *a);
bool app_write_REG_STIM_START(void *a);
bool app_write_REG_STIM_WAVELENGTH(void *a);
bool app_write_REG_STIM_PERIOD(void *a);
bool app_write_REG_STIM_ON(void *a);
bool app_write_REG_STIM_REPS(void *a);
bool app_write_REG_EXT_CAMERA_START(void *a);
bool app_write_REG_EXT_CAMERA_PERIOD(void *a);
bool app_write_REG_OUT0_CONF(void *a);
bool app_write_REG_OUT1_CONF(void *a);
bool app_write_REG_IN0_CONF(void *a);
bool app_write_REG_IN1_CONF(void *a);
bool app_write_REG_OUT_SET(void *a);
bool app_write_REG_OUT_CLEAR(void *a);
bool app_write_REG_OUT_TOGGLE(void *a);
bool app_write_REG_OUT_WRITE(void *a);
bool app_write_REG_IN_READ(void *a);
bool app_write_REG_START(void *a);
bool app_write_REG_FRAME_EVENT(void *a);
bool app_write_REG_TRIGGER_STATE(void *a);
bool app_write_REG_TRIGGER_STATE_LENGTH(void *a);
bool app_write_REG_TRIGGER_PERIOD(void *a);
bool app_write_REG_TRIGGER_T_ON(void *a);
bool app_write_REG_TRIGGER_T_UPDATE_OUTPUTS(void *a);
bool app_write_REG_TRIGGER_STIM_BEHAVIOR(void *a);
bool app_write_REG_PHOTODIODES_START(void *a);
bool app_write_REG_PHOTODIODES(void *a);
bool app_write_REG_TEMPERATURE(void *a);
bool app_write_REG_SCREEN_HW_VERSION_H(void *a);
bool app_write_REG_SCREEN_HW_VERSION_L(void *a);
bool app_write_REG_SCREEN_ASSEMBLY_VERSION(void *a);
bool app_write_REG_SCREEN_FW_VERSION_H(void *a);
bool app_write_REG_SCREEN_FW_VERSION_L(void *a);
bool app_write_REG_CAMERA_SN(void *a);
bool app_write_REG_TRIGGER_LASER_ON(void *a);
bool app_write_REG_TRIGGER_LASER_OFF(void *a);
bool app_write_REG_CAL_L410(void *a);
bool app_write_REG_CAL_L470(void *a);
bool app_write_REG_CAL_L560(void *a);
bool app_write_REG_CAL_LASER(void *a);
bool app_write_REG_CAL_PH410(void *a);
bool app_write_REG_CAL_PH470(void *a);
bool app_write_REG_CAL_PH560(void *a);


#endif /* _APP_FUNCTIONS_H_ */