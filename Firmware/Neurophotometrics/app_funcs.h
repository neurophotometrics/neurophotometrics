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
void app_read_REG_RAW_POT_L410(void);
void app_read_REG_RAW_POT_L470(void);
void app_read_REG_RAW_POT_L560(void);
void app_read_REG_RAW_POT_LEXTRA(void);
void app_read_REG_RAW_IOS_SET(void);
void app_read_REG_RAW_IOS_CLEAR(void);
void app_read_REG_RAW_IOS_TOGGLE(void);
void app_read_REG_RAW_IOS_WRITE(void);
void app_read_REG_RAW_IOS_READ(void);
void app_read_REG_START(void);
void app_read_REG_FRAME_EVENT(void);
void app_read_REG_FRAME_EVENT_CFG(void);
void app_read_REG_TRIGGER_STATE(void);
void app_read_REG_TRIGGER_STATE_LENGTH(void);
void app_read_REG_TRIGGER_PERIOD(void);
void app_read_REG_TRIGGER_T_ON(void);
void app_read_REG_TRIGGER_PRE_LEDS(void);
void app_read_REG_DUMMY0(void);
void app_read_REG_DUMMY1(void);
void app_read_REG_OUT0_CONF(void);
void app_read_REG_OUT1_CONF(void);
void app_read_REG_IN0_CONF(void);
void app_read_REG_OUT_SET(void);
void app_read_REG_OUT_CLEAR(void);
void app_read_REG_OUT_TOGGLE(void);
void app_read_REG_OUT_WRITE(void);
void app_read_REG_IN_READ(void);
void app_read_REG_ADCIN(void);
void app_read_REG_ADCIN_CONF(void);
void app_read_REG_STIM_START(void);
void app_read_REG_STIM_PERIOD(void);
void app_read_REG_STIM_ON(void);
void app_read_REG_STIM_REPS(void);
void app_read_REG_EXT_CAMERA_START(void);
void app_read_REG_EXT_CAMERA_PERIOD(void);

bool app_write_REG_RAW_POT_L410(void *a);
bool app_write_REG_RAW_POT_L470(void *a);
bool app_write_REG_RAW_POT_L560(void *a);
bool app_write_REG_RAW_POT_LEXTRA(void *a);
bool app_write_REG_RAW_IOS_SET(void *a);
bool app_write_REG_RAW_IOS_CLEAR(void *a);
bool app_write_REG_RAW_IOS_TOGGLE(void *a);
bool app_write_REG_RAW_IOS_WRITE(void *a);
bool app_write_REG_RAW_IOS_READ(void *a);
bool app_write_REG_START(void *a);
bool app_write_REG_FRAME_EVENT(void *a);
bool app_write_REG_FRAME_EVENT_CFG(void *a);
bool app_write_REG_TRIGGER_STATE(void *a);
bool app_write_REG_TRIGGER_STATE_LENGTH(void *a);
bool app_write_REG_TRIGGER_PERIOD(void *a);
bool app_write_REG_TRIGGER_T_ON(void *a);
bool app_write_REG_TRIGGER_PRE_LEDS(void *a);
bool app_write_REG_DUMMY0(void *a);
bool app_write_REG_DUMMY1(void *a);
bool app_write_REG_OUT0_CONF(void *a);
bool app_write_REG_OUT1_CONF(void *a);
bool app_write_REG_IN0_CONF(void *a);
bool app_write_REG_OUT_SET(void *a);
bool app_write_REG_OUT_CLEAR(void *a);
bool app_write_REG_OUT_TOGGLE(void *a);
bool app_write_REG_OUT_WRITE(void *a);
bool app_write_REG_IN_READ(void *a);
bool app_write_REG_ADCIN(void *a);
bool app_write_REG_ADCIN_CONF(void *a);
bool app_write_REG_STIM_START(void *a);
bool app_write_REG_STIM_PERIOD(void *a);
bool app_write_REG_STIM_ON(void *a);
bool app_write_REG_STIM_REPS(void *a);
bool app_write_REG_EXT_CAMERA_START(void *a);
bool app_write_REG_EXT_CAMERA_PERIOD(void *a);


#endif /* _APP_FUNCTIONS_H_ */