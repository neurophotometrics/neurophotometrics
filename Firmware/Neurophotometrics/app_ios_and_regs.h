#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// CAM_STROB_IN           Description: (u_CAM_STROB_IN) Switch to select and stop
// BUTTON_START_STOP      Description: (u_test_switch_2) Start and stop the selected behavior
// BUTTON_TEST_1          Description: (u_test_switch_1) For debug or future proof
// IN0                    Description: (u_In6) Digital input

#define read_CAM_STROB_IN read_io(PORTH, 2)     // CAM_STROB_IN
#define read_BUTTON_START_STOP read_io(PORTK, 1)// BUTTON_START_STOP
#define read_BUTTON_TEST_1 read_io(PORTK, 2)    // BUTTON_TEST_1
#define read_IN0 read_io(PORTE, 2)              // IN0

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// CAM_TRIGGER            Description: (u_CAM_TRIG_OUT) Triggers a camera frame
// CAM_IO_0               Description: (u_CAM_IO_0) Connects to camera's input
// CAM_IO_1               Description: (u_CAM_IO_1) Connects to camera's input
// EN_OUT_1               Description: (u_Out4_Enbl) Enables the digital output 1
// EN_OUT_0               Description: (u_Out5_Enbl) Enables the digital output 0
// EN_IN_0                Description: (u_In6_Enbl) Enables the digital input 0
// EN_L410                Description: (L410_uPWM_ENBL) Enables the digital buffer for the signal PWN_L410
// EN_L470                Description: (L470_uPWM_ENBL) Enables the digital buffer for the signal PWN_L470
// EN_L560                Description: (L560_uPWM_ENBL) Enables the digital buffer for the signal PWN_L560
// EN_LEXTRA              Description: (extra_uPWM_ENBL) Enables the digital buffer for the signal PWN_LEXTRA
// L410                   Description: (L410_uPWM) Set LED 410 output
// L470                   Description: (L470_uPWM) Set LED 470 output
// L560                   Description: (L560_uPWM) Set LED 560 output
// LPHOTODIODE            Description: (u_LED2Photodiode) Set the LED line of the photodiode port
// LEXTRA                 Description: (extra_uPWM) Set LED EXTRA
// OUT1                   Description: (u_Out4) Set digital output 0
// OUT0                   Description: (u_Out5) Set digital output 1
// POT_CS                 Description: (!POT_CS) Digital pot CS
// POT_SDI                Description: (POT_SDI) Digital pot SDI
// POT_CLK                Description: (POT_CLK) Digital pot CLK
// POT_MIDSCALE           Description: (!POT_PR) Set digital pot power to mid scale
// POT_SHDN               Description: (!POT_SHDN) Digital pot shutdown
// CLK_EN_IN              Description: (u_Clck_In\ENBL)
// CLK_EN_BYPASS          Description: (u_Clck_Bypass_ENBL)
// CLK_EN_OUT             Description: (u_Clck_Out_ENBL)

/* CAM_TRIGGER */
#define set_CAM_TRIGGER set_io(PORTC, 0)
#define clr_CAM_TRIGGER clear_io(PORTC, 0)
#define tgl_CAM_TRIGGER toggle_io(PORTC, 0)
#define read_CAM_TRIGGER read_io(PORTC, 0)

/* CAM_IO_0 */
#define set_CAM_IO_0 set_io(PORTH, 0)
#define clr_CAM_IO_0 clear_io(PORTH, 0)
#define tgl_CAM_IO_0 toggle_io(PORTH, 0)
#define read_CAM_IO_0 read_io(PORTH, 0)

/* CAM_IO_1 */
#define set_CAM_IO_1 set_io(PORTH, 1)
#define clr_CAM_IO_1 clear_io(PORTH, 1)
#define tgl_CAM_IO_1 toggle_io(PORTH, 1)
#define read_CAM_IO_1 read_io(PORTH, 1)

/* EN_OUT_1 */
#define set_EN_OUT_1 set_io(PORTK, 3)
#define clr_EN_OUT_1 clear_io(PORTK, 3)
#define tgl_EN_OUT_1 toggle_io(PORTK, 3)
#define read_EN_OUT_1 read_io(PORTK, 3)

/* EN_OUT_0 */
#define set_EN_OUT_0 set_io(PORTK, 4)
#define clr_EN_OUT_0 clear_io(PORTK, 4)
#define tgl_EN_OUT_0 toggle_io(PORTK, 4)
#define read_EN_OUT_0 read_io(PORTK, 4)

/* EN_IN_0 */
#define set_EN_IN_0 clear_io(PORTK, 5)
#define clr_EN_IN_0 set_io(PORTK, 5)
#define tgl_EN_IN_0 toggle_io(PORTK, 5)
#define read_EN_IN_0 read_io(PORTK, 5)

/* EN_L410 */
#define set_EN_L410 set_io(PORTB, 3)
#define clr_EN_L410 clear_io(PORTB, 3)
#define tgl_EN_L410 toggle_io(PORTB, 3)
#define read_EN_L410 read_io(PORTB, 3)

/* EN_L470 */
#define set_EN_L470 set_io(PORTB, 4)
#define clr_EN_L470 clear_io(PORTB, 4)
#define tgl_EN_L470 toggle_io(PORTB, 4)
#define read_EN_L470 read_io(PORTB, 4)

/* EN_L560 */
#define set_EN_L560 set_io(PORTB, 5)
#define clr_EN_L560 clear_io(PORTB, 5)
#define tgl_EN_L560 toggle_io(PORTB, 5)
#define read_EN_L560 read_io(PORTB, 5)

/* EN_LEXTRA */
#define set_EN_LEXTRA set_io(PORTB, 6)
#define clr_EN_LEXTRA clear_io(PORTB, 6)
#define tgl_EN_LEXTRA toggle_io(PORTB, 6)
#define read_EN_LEXTRA read_io(PORTB, 6)

/* L410 */
#define set_L410 set_io(PORTC, 1)
#define clr_L410 clear_io(PORTC, 1)
#define tgl_L410 toggle_io(PORTC, 1)
#define read_L410 read_io(PORTC, 1)

/* L470 */
#define set_L470 set_io(PORTC, 2)
#define clr_L470 clear_io(PORTC, 2)
#define tgl_L470 toggle_io(PORTC, 2)
#define read_L470 read_io(PORTC, 2)

/* L560 */
#define set_L560 set_io(PORTC, 3)
#define clr_L560 clear_io(PORTC, 3)
#define tgl_L560 toggle_io(PORTC, 3)
#define read_L560 read_io(PORTC, 3)

/* LPHOTODIODE */
#define set_LPHOTODIODE set_io(PORTC, 4)
#define clr_LPHOTODIODE clear_io(PORTC, 4)
#define tgl_LPHOTODIODE toggle_io(PORTC, 4)
#define read_LPHOTODIODE read_io(PORTC, 4)

/* LEXTRA */
#define set_LEXTRA set_io(PORTD, 0)
#define clr_LEXTRA clear_io(PORTD, 0)
#define tgl_LEXTRA toggle_io(PORTD, 0)
#define read_LEXTRA read_io(PORTD, 0)

/* OUT1 */
#define set_OUT1 set_io(PORTE, 0)
#define clr_OUT1 clear_io(PORTE, 0)
#define tgl_OUT1 toggle_io(PORTE, 0)
#define read_OUT1 read_io(PORTE, 0)

/* OUT0 */
#define set_OUT0 set_io(PORTE, 1)
#define clr_OUT0 clear_io(PORTE, 1)
#define tgl_OUT0 toggle_io(PORTE, 1)
#define read_OUT0 read_io(PORTE, 1)

/* POT_CS */
#define set_POT_CS clear_io(PORTD, 4)
#define clr_POT_CS set_io(PORTD, 4)
#define tgl_POT_CS toggle_io(PORTD, 4)
#define read_POT_CS read_io(PORTD, 4)

/* POT_SDI */
#define set_POT_SDI set_io(PORTD, 5)
#define clr_POT_SDI clear_io(PORTD, 5)
#define tgl_POT_SDI toggle_io(PORTD, 5)
#define read_POT_SDI read_io(PORTD, 5)

/* POT_CLK */
#define set_POT_CLK set_io(PORTD, 7)
#define clr_POT_CLK clear_io(PORTD, 7)
#define tgl_POT_CLK toggle_io(PORTD, 7)
#define read_POT_CLK read_io(PORTD, 7)

/* POT_MIDSCALE */
#define set_POT_MIDSCALE clear_io(PORTE, 6)
#define clr_POT_MIDSCALE set_io(PORTE, 6)
#define tgl_POT_MIDSCALE toggle_io(PORTE, 6)
#define read_POT_MIDSCALE read_io(PORTE, 6)

/* POT_SHDN */
#define set_POT_SHDN clear_io(PORTE, 7)
#define clr_POT_SHDN set_io(PORTE, 7)
#define tgl_POT_SHDN toggle_io(PORTE, 7)
#define read_POT_SHDN read_io(PORTE, 7)

/* CLK_EN_IN */
#define set_CLK_EN_IN set_io(PORTB, 0)
#define clr_CLK_EN_IN clear_io(PORTB, 0)
#define tgl_CLK_EN_IN toggle_io(PORTB, 0)
#define read_CLK_EN_IN read_io(PORTB, 0)

/* CLK_EN_BYPASS */
#define set_CLK_EN_BYPASS set_io(PORTB, 1)
#define clr_CLK_EN_BYPASS clear_io(PORTB, 1)
#define tgl_CLK_EN_BYPASS toggle_io(PORTB, 1)
#define read_CLK_EN_BYPASS read_io(PORTB, 1)

/* CLK_EN_OUT */
#define set_CLK_EN_OUT set_io(PORTB, 2)
#define clr_CLK_EN_OUT clear_io(PORTB, 2)
#define tgl_CLK_EN_OUT toggle_io(PORTB, 2)
#define read_CLK_EN_OUT read_io(PORTB, 2)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_RAW_POT_L410;
	uint8_t REG_RAW_POT_L470;
	uint8_t REG_RAW_POT_L560;
	uint8_t REG_RAW_POT_LEXTRA;
	uint32_t REG_RAW_IOS_SET;
	uint32_t REG_RAW_IOS_CLEAR;
	uint32_t REG_RAW_IOS_TOGGLE;
	uint32_t REG_RAW_IOS_WRITE;
	uint32_t REG_RAW_IOS_READ;
	uint8_t REG_START;
	uint8_t REG_FRAME_EVENT;
	uint8_t REG_FRAME_EVENT_CFG;
	uint8_t REG_TRIGGER_STATE[240];
	uint8_t REG_TRIGGER_STATE_LENGTH;
	uint16_t REG_TRIGGER_PERIOD;
	uint16_t REG_TRIGGER_T_ON;
	uint16_t REG_TRIGGER_PRE_LEDS;
	uint8_t REG_DUMMY0;
	uint8_t REG_DUMMY1;
	uint8_t REG_OUT0_CONF;
	uint8_t REG_OUT1_CONF;
	uint8_t REG_IN0_CONF;
	uint8_t REG_OUT_SET;
	uint8_t REG_OUT_CLEAR;
	uint8_t REG_OUT_TOGGLE;
	uint8_t REG_OUT_WRITE;
	uint8_t REG_IN_READ;
	uint16_t REG_ADCIN;
	uint8_t REG_ADCIN_CONF;
	uint8_t REG_STIM_START;
	uint16_t REG_STIM_PERIOD;
	uint16_t REG_STIM_ON;
	uint16_t REG_STIM_REPS;
	uint8_t REG_EXT_CAMERA_START;
	uint16_t REG_EXT_CAMERA_PERIOD;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_RAW_POT_L410                32 // U8     Writes to the digital pot controlling LED 410
#define ADD_REG_RAW_POT_L470                33 // U8     Writes to the digital pot controlling LED 470
#define ADD_REG_RAW_POT_L560                34 // U8     Writes to the digital pot controlling LED 560
#define ADD_REG_RAW_POT_LEXTRA              35 // U8     Writes to the digital pot controlling LED EXTRA
#define ADD_REG_RAW_IOS_SET                 36 // U32    Set the IOs to logic 1
#define ADD_REG_RAW_IOS_CLEAR               37 // U32    Clear the IOs to logic 0
#define ADD_REG_RAW_IOS_TOGGLE              38 // U32    Toggle the Ios
#define ADD_REG_RAW_IOS_WRITE               39 // U32    Write the bit mask to the IOS
#define ADD_REG_RAW_IOS_READ                40 // U32    Read the IOs state
#define ADD_REG_START                       41 // U8     Start running trough the TRIGGER_STATE vector
#define ADD_REG_FRAME_EVENT                 42 // U8     Event of the frame executed
#define ADD_REG_FRAME_EVENT_CFG             43 // U8     Configuration of frame sync
#define ADD_REG_TRIGGER_STATE               44 // U8     
#define ADD_REG_TRIGGER_STATE_LENGTH        45 // U8     
#define ADD_REG_TRIGGER_PERIOD              46 // U16    Period of each trigger in microseconds
#define ADD_REG_TRIGGER_T_ON                47 // U16    Period that the trigger line is high in microseconds
#define ADD_REG_TRIGGER_PRE_LEDS            48 // U16    Time to set LED (410, 470 and 560 only) before trigger the camera in microseconds
#define ADD_REG_DUMMY0                      49 // U8     
#define ADD_REG_DUMMY1                      50 // U8     
#define ADD_REG_OUT0_CONF                   51 // U8     Configuration of digital output 0
#define ADD_REG_OUT1_CONF                   52 // U8     Configuration of digital output 1
#define ADD_REG_IN0_CONF                    53 // U8     Configuration of digital input 0
#define ADD_REG_OUT_SET                     54 // U8     Sets the digital outputs
#define ADD_REG_OUT_CLEAR                   55 // U8     Clear the digital outputs
#define ADD_REG_OUT_TOGGLE                  56 // U8     Toggle the digital outputs
#define ADD_REG_OUT_WRITE                   57 // U8     Write to the digital outputs
#define ADD_REG_IN_READ                     58 // U8     Constains the state of the digital input
#define ADD_REG_ADCIN                       59 // U16    Value of the ADC input
#define ADD_REG_ADCIN_CONF                  60 // U8     Configuration of ADC acquisition
#define ADD_REG_STIM_START                  61 // U8     Starts the stimulation when writen to 1 and stop when writen to 0
#define ADD_REG_STIM_PERIOD                 62 // U16    Period of the pulse  (in miliseconds) [2;60000]
#define ADD_REG_STIM_ON                     63 // U16    Time that the pulse will be high (in miliseconds) [1;60000]
#define ADD_REG_STIM_REPS                   64 // U16    Number of repetitions. If equal to 0, it means the stim will not be used.
#define ADD_REG_EXT_CAMERA_START            65 // U8     Start camera triggers on digital output 0 if equal to 1 and stop if equal to 0
#define ADD_REG_EXT_CAMERA_PERIOD           66 // U16    In microseconds

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x42
#define APP_NBYTES_OF_REG_BANK              297

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_CAM_TRIGGER                      ((uint32_t)1<<0) // 
#define B_CAM_IO_0                         ((uint32_t)1<<1) // 
#define B_CAM_IO_1                         ((uint32_t)1<<2) // 
#define B_CAM_STROB_IN                     ((uint32_t)1<<3) // 
#define B_BUTTON_START_STOP                ((uint32_t)1<<4) // 
#define B_BUTTON_TEST_1                    ((uint32_t)1<<5) // 
#define B_EN_OUT_1                         ((uint32_t)1<<6) // 
#define B_EN_OUT_0                         ((uint32_t)1<<7) // 
#define B_EN_IN_0                          ((uint32_t)1<<8) // 
#define B_EN_L410                          ((uint32_t)1<<9) // 
#define B_EN_L470                          ((uint32_t)1<<10) // 
#define B_EN_L560                          ((uint32_t)1<<11) // 
#define B_EN_LEXTRA                        ((uint32_t)1<<12) // 
#define B_L410                             ((uint32_t)1<<13) // 
#define B_L470                             ((uint32_t)1<<14) // 
#define B_L560                             ((uint32_t)1<<15) // 
#define B_LPHOTODIODE                      ((uint32_t)1<<16) // 
#define B_LEXTRA                           ((uint32_t)1<<17) // 
#define B_OUT1                             ((uint32_t)1<<18) // 
#define B_OUT0                             ((uint32_t)1<<19) // 
#define B_IN0                              ((uint32_t)1<<20) // 
#define B_POT_CS                           ((uint32_t)1<<21) // 
#define B_POT_SDI                          ((uint32_t)1<<22) // 
#define B_POT_CLK                          ((uint32_t)1<<23) // 
#define B_POT_MIDSCALE                     ((uint32_t)1<<24) // 
#define B_POT_SHDN                         ((uint32_t)1<<25) // 
#define B_CLK_EN_IN                        ((uint32_t)1<<26) // 
#define B_CLK_EN_BYPASS                    ((uint32_t)1<<27) // 
#define B_CLK_EN_OUT                       ((uint32_t)1<<28) // 
#define B_START_TRIGGER                    (1<<0)       // Start the triggering of the TRIGGER_STATE bitmasks
#define B_START_EXT_CAMERA                 (1<<1)       // Start the external camera triggering
#define B_STOP_TRIGGER                     (1<<2)       // Stop the triggering of the TRIGGER_STATE
#define B_STOP_EXT_CAMERA                  (1<<3)       // Stop the external camera triggering
#define B_ON_L410                          (1<<0)       // LED 410 was ON when the camera was triggered
#define B_ON_L470                          (1<<1)       // LED 470 was ON when the camera was triggered
#define B_ON_L560                          (1<<2)       // LED 560 was ON when the camera was triggered
#define B_ON_LEXTRA                        (1<<3)       // LED EXTRA was ON when the camera was triggered
#define B_ON_OUT0                          (1<<4)       // Digital output 0 was high when the camera was triggered
#define B_ON_OUT1                          (1<<5)       // Digital output 1 was high when the camera was triggered
#define B_ON_OPTOGEN_BEHAVIOR              (1<<6)       // Optogentics behavior was operating when camera was triggered
#define B_ON_IN                            (1<<7)       // Digital input 0 was high when the camera was triggered
#define GM_FRAME_TRIG                      1            // 
#define MSK_FRAME_TRIG_TRIGGER_OUT         0            // FRAME_EVENT is issued when the trigger of the TRIGGER_STATE is issued
#define MSK_FRAME_TRIG_STROBE_IN           1            // FRAME_EVENT is issued when the internal strobe is detected
#define GM_OUT_CONF                        3            // 
#define MSK_OUT_CONF_SOFTWARE              0            // Controlled by software only
#define MSK_OUT_CONF_CAMERA                1            // Outputs the internal camera trigger
#define MSK_OUT_CONF_STROBE                2            // Outputs the strobe of the internal camera
#define MSK_OUT_CONF_STATE_CTRL            3            // Controlled by the TRIGGER_STATE
#define GM_IN_CONF                         15           // 
#define MSK_IN_C_NONE                      0            // 
#define MSK_IN_C_SOFTWARE_R                1            // Sends an event when a rising edge is detected
#define MSK_IN_C_SOFTWARE_F                2            // Sends an event when a falling edge is detected
#define MSK_IN_C_SOFTWARE_R_AND_F          3            // Sends an event when both edges are detected
#define MSK_IN_C_START_TRIG                4            // 
#define MSK_IN_C_START_CAM                 5            // Starts the external camera when a rising edge is detected
#define MSK_IN_C_START_TRIG_AND_CAM        6            // Starts both the triggering the external camera when a rising edge is detected
#define MSK_IN_C_START_STOP_TRIG           7            // Starts the triggering when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_CAM            8            // Starts the external camera when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_TRIG_AND_CAM   9            // Starts both the triggering the external camera when a rising edge and stops when falling
#define MSK_IN_C_START_OPTO_BEHAVIOR       10           // Starts the optogenetics pulses when a rising edge is detected
#define MSK_IN_C_START_STOP_OPTO_BEHAVIOR  11           // Starts the optogenetics pulses when a rising edge and stops when falling
#define B_DOUT0                            (1<<0)       // 
#define B_DOUT1                            (1<<1)       // 
#define B_DIN0                             (1<<0)       // 
#define GM_ADCIN_CONF                      1            // 
#define MSK_ADCIN_DISABLED                 0            // ADC is not streaming data
#define MSK_ADCIN_200Hz                    1            // ADC is streaming data at 200 Hz
#define B_START_OPTO                       (1<<0)       // 
#define B_START_EXT_CAM                    (1<<0)       // 

#endif /* _APP_REGS_H_ */