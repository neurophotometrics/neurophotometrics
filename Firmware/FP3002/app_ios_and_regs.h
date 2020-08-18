#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// KEY_SWITCH             Description: Key switch state
// IN0                    Description: Digital input 0
// IN1                    Description: Digital input 1
// ADC_MISO_410           Description: Data from ADC connected to L410
// ADC_MISO_470           Description: Data from ADC connected to L470
// ADC_MISO_560           Description: Data from ADC connected to L560
// SCREEN_IS_USING_USB    Description: Logic high ->  Screen is using the comunication
// CAM_STROBE             Description: Camera strobe
// EXT_IO_1               Description: External IO 1
// EXT_IO_2               Description: External IO 2
// EXT_IO_3               Description: External IO 3
// EXT_IO_4               Description: External IO 4
// EXT_IO_5               Description: External IO 5
// EXT_IO_6               Description: External IO 6
// EXT_IO_RX              Description: External IO RX
// EXT_IO_TX              Description: External IO TX

#define read_KEY_SWITCH read_io(PORTA, 5)       // KEY_SWITCH
#define read_SW1 read_io(PORTA, 7)              // SW1
#define read_IN0 read_io(PORTH, 1)              // IN0
#define read_IN1 read_io(PORTK, 6)              // IN1
#define read_ADC_MISO_410 read_io(PORTE, 3)     // ADC_MISO_410
#define read_ADC_MISO_470 read_io(PORTE, 5)     // ADC_MISO_470
#define read_ADC_MISO_560 read_io(PORTE, 7)     // ADC_MISO_560
#define read_SCREEN_IS_USING_USB read_io(PORTJ, 2)// SCREEN_IS_USING_USB
#define read_CAM_STROBE read_io(PORTH, 2)       // CAM_STROBE
#define read_EXT_IO_1 read_io(PORTD, 4)         // EXT_IO_1
#define read_EXT_IO_2 read_io(PORTD, 6)         // EXT_IO_2
#define read_EXT_IO_3 read_io(PORTF, 6)         // EXT_IO_3
#define read_EXT_IO_4 read_io(PORTF, 7)         // EXT_IO_4
#define read_EXT_IO_5 read_io(PORTA, 1)         // EXT_IO_5
#define read_EXT_IO_6 read_io(PORTA, 2)         // EXT_IO_6
#define read_EXT_IO_RX read_io(PORTC, 2)        // EXT_IO_RX
#define read_EXT_IO_TX read_io(PORTC, 3)        // EXT_IO_TX

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// EN_INT_LASER           Description: Enable trigger for internal laser
// EN_OUT0                Description: Enable the digital output 0 (enabled by default)
// EN_OUT1                Description: Enable the digital output 1 (enabled by default)
// OUT0                   Description: Digital output 0
// OUT1                   Description: Digital output 1
// EN_IN0                 Description: Enable buffer of digital input 0 (enabled by default)
// EN_IN1                 Description: Enable buffer of digital input 1 (enabled by default)
// EN_CLKIN               Description: Enable input of sync clock  (enabled by default)
// EN_CLKOUT              Description: Enable output of sync clock
// EN_SERIAL_MAIN         Description: Switch serial multiplexer to main microcontroller (enabled by default)
// EN_SERIAL_SCREEN       Description: Switch serial multiplexer to screen
// DAC_CS_410             Description: Chip select of the DAC that controls the L410
// DAC_CS_470             Description: Chip select of the DAC that controls the L470
// DAC_CS_560             Description: Chip select of the DAC that controls the L560
// DAC_CLR_410            Description: Clear of the DAC that controls the L410
// DAC_CLR_470            Description: Clear of the DAC that controls the L470
// DAC_CLR_560            Description: Clear of the DAC that controls the L560
// DAC_SCLK               Description: Clock for the DACs
// DAC_MOSI               Description: Data for the DACs
// ADC_SCLK_410           Description: Clock for the ADC connected to L410
// ADC_SCLK_470           Description: Clock for the ADC connected to L470
// ADC_SCLK_560           Description: Clock for the ADC connected to L560
// ADC_CNV_410            Description: Start conversion of DAC L410
// ADC_CNV_470            Description: Start conversion of DAC L470
// ADC_CNV_560            Description: Start conversion of DAC L560
// SCREEN_CAN_USE_USB     Description: Logic low->  Screen has comunication with USB
// POT_CLK                Description: Clock for the photodiode potentiometers
// POT_SDI                Description: Data for the photodiode potentiometers
// POT_CS_410             Description: Chip select for L410's potentiometer
// POT_CS_470             Description: Chip select for L470's potentiometer
// POT_CS_560             Description: Chip select for L560's potentiometer
// DAC_CS_LASER           Description: Chip select of the DAC that controls the laser
// DAC_SCLK_LASER         Description: Clock for the laser's DAC
// DAC_MOSI_LASER         Description: Data for the laser's DAC
// CAM_TRIGGER            Description: Camera trigger
// CAM_GPIO2              Description: Camera GPIO2
// CAM_GPIO3              Description: Camera PGIO3

/* EN_INT_LASER */
#define set_EN_INT_LASER set_io(PORTH, 0)
#define clr_EN_INT_LASER clear_io(PORTH, 0)
#define tgl_EN_INT_LASER toggle_io(PORTH, 0)
#define read_EN_INT_LASER read_io(PORTH, 0)

/* EN_OUT0 */
#define set_EN_OUT0 set_io(PORTK, 3)
#define clr_EN_OUT0 clear_io(PORTK, 3)
#define tgl_EN_OUT0 toggle_io(PORTK, 3)
#define read_EN_OUT0 read_io(PORTK, 3)

/* EN_OUT1 */
#define set_EN_OUT1 set_io(PORTK, 4)
#define clr_EN_OUT1 clear_io(PORTK, 4)
#define tgl_EN_OUT1 toggle_io(PORTK, 4)
#define read_EN_OUT1 read_io(PORTK, 4)

/* OUT0 */
#define set_OUT0 set_io(PORTE, 0)
#define clr_OUT0 clear_io(PORTE, 0)
#define tgl_OUT0 toggle_io(PORTE, 0)
#define read_OUT0 read_io(PORTE, 0)
#define set_controlled_OUT0 do { if (!read_EN_INT_LASER || ((read_EN_INT_LASER && read_KEY_SWITCH) && (app_regs.REG_STIM_WAVELENGTH == 450 || app_regs.REG_STIM_WAVELENGTH == 635))) set_io(PORTE, 0); } while(0)

/* OUT1 */
#define set_OUT1 set_io(PORTE, 1)
#define clr_OUT1 clear_io(PORTE, 1)
#define tgl_OUT1 toggle_io(PORTE, 1)
#define read_OUT1 read_io(PORTE, 1)

/* EN_IN0 */
#define set_EN_IN0 clear_io(PORTK, 5)
#define clr_EN_IN0 set_io(PORTK, 5)
#define tgl_EN_IN0 toggle_io(PORTK, 5)
#define read_EN_IN0 read_io(PORTK, 5)

/* EN_IN1 */
#define set_EN_IN1 clear_io(PORTK, 7)
#define clr_EN_IN1 set_io(PORTK, 7)
#define tgl_EN_IN1 toggle_io(PORTK, 7)
#define read_EN_IN1 read_io(PORTK, 7)

/* EN_CLKIN */
#define set_EN_CLKIN set_io(PORTB, 0)
#define clr_EN_CLKIN clear_io(PORTB, 0)
#define tgl_EN_CLKIN toggle_io(PORTB, 0)
#define read_EN_CLKIN read_io(PORTB, 0)

/* EN_CLKOUT */
#define set_EN_CLKOUT set_io(PORTB, 1)
#define clr_EN_CLKOUT clear_io(PORTB, 1)
#define tgl_EN_CLKOUT toggle_io(PORTB, 1)
#define read_EN_CLKOUT read_io(PORTB, 1)

/* EN_SERIAL_MAIN */
#define set_EN_SERIAL_MAIN clear_io(PORTJ, 0)
#define clr_EN_SERIAL_MAIN set_io(PORTJ, 0)
#define tgl_EN_SERIAL_MAIN toggle_io(PORTJ, 0)
#define read_EN_SERIAL_MAIN read_io(PORTJ, 0)

/* EN_SERIAL_SCREEN */
#define set_EN_SERIAL_SCREEN set_io(PORTH, 7)
#define clr_EN_SERIAL_SCREEN clear_io(PORTH, 7)
#define tgl_EN_SERIAL_SCREEN toggle_io(PORTH, 7)
#define read_EN_SERIAL_SCREEN read_io(PORTH, 7)

/* DAC_CS_410 */
#define set_DAC_CS_410 clear_io(PORTB, 2)
#define clr_DAC_CS_410 set_io(PORTB, 2)
#define tgl_DAC_CS_410 toggle_io(PORTB, 2)
#define read_DAC_CS_410 read_io(PORTB, 2)

/* DAC_CS_470 */
#define set_DAC_CS_470 clear_io(PORTB, 6)
#define clr_DAC_CS_470 set_io(PORTB, 6)
#define tgl_DAC_CS_470 toggle_io(PORTB, 6)
#define read_DAC_CS_470 read_io(PORTB, 6)

/* DAC_CS_560 */
#define set_DAC_CS_560 clear_io(PORTB, 4)
#define clr_DAC_CS_560 set_io(PORTB, 4)
#define tgl_DAC_CS_560 toggle_io(PORTB, 4)
#define read_DAC_CS_560 read_io(PORTB, 4)

/* DAC_CLR_410 */
#define set_DAC_CLR_410 clear_io(PORTB, 3)
#define clr_DAC_CLR_410 set_io(PORTB, 3)
#define tgl_DAC_CLR_410 toggle_io(PORTB, 3)
#define read_DAC_CLR_410 read_io(PORTB, 3)

/* DAC_CLR_470 */
#define set_DAC_CLR_470 clear_io(PORTB, 7)
#define clr_DAC_CLR_470 set_io(PORTB, 7)
#define tgl_DAC_CLR_470 toggle_io(PORTB, 7)
#define read_DAC_CLR_470 read_io(PORTB, 7)

/* DAC_CLR_560 */
#define set_DAC_CLR_560 clear_io(PORTB, 5)
#define clr_DAC_CLR_560 set_io(PORTB, 5)
#define tgl_DAC_CLR_560 toggle_io(PORTB, 5)
#define read_DAC_CLR_560 read_io(PORTB, 5)

/* DAC_SCLK */
#define set_DAC_SCLK set_io(PORTD, 7)
#define clr_DAC_SCLK clear_io(PORTD, 7)
#define tgl_DAC_SCLK toggle_io(PORTD, 7)
#define read_DAC_SCLK read_io(PORTD, 7)

/* DAC_MOSI */
#define set_DAC_MOSI set_io(PORTD, 5)
#define clr_DAC_MOSI clear_io(PORTD, 5)
#define tgl_DAC_MOSI toggle_io(PORTD, 5)
#define read_DAC_MOSI read_io(PORTD, 5)

/* ADC_SCLK_410 */
#define set_ADC_SCLK_410 set_io(PORTE, 2)
#define clr_ADC_SCLK_410 clear_io(PORTE, 2)
#define tgl_ADC_SCLK_410 toggle_io(PORTE, 2)
#define read_ADC_SCLK_410 read_io(PORTE, 2)

/* ADC_SCLK_470 */
#define set_ADC_SCLK_470 set_io(PORTE, 4)
#define clr_ADC_SCLK_470 clear_io(PORTE, 4)
#define tgl_ADC_SCLK_470 toggle_io(PORTE, 4)
#define read_ADC_SCLK_470 read_io(PORTE, 4)

/* ADC_SCLK_560 */
#define set_ADC_SCLK_560 set_io(PORTE, 6)
#define clr_ADC_SCLK_560 clear_io(PORTE, 6)
#define tgl_ADC_SCLK_560 toggle_io(PORTE, 6)
#define read_ADC_SCLK_560 read_io(PORTE, 6)

/* ADC_CNV_410 */
#define set_ADC_CNV_410 set_io(PORTH, 3)
#define clr_ADC_CNV_410 clear_io(PORTH, 3)
#define tgl_ADC_CNV_410 toggle_io(PORTH, 3)
#define read_ADC_CNV_410 read_io(PORTH, 3)

/* ADC_CNV_470 */
#define set_ADC_CNV_470 set_io(PORTH, 4)
#define clr_ADC_CNV_470 clear_io(PORTH, 4)
#define tgl_ADC_CNV_470 toggle_io(PORTH, 4)
#define read_ADC_CNV_470 read_io(PORTH, 4)

/* ADC_CNV_560 */
#define set_ADC_CNV_560 set_io(PORTH, 5)
#define clr_ADC_CNV_560 clear_io(PORTH, 5)
#define tgl_ADC_CNV_560 toggle_io(PORTH, 5)
#define read_ADC_CNV_560 read_io(PORTH, 5)

/* SCREEN_CAN_USE_USB */
#define set_SCREEN_CAN_USE_USB clear_io(PORTJ, 1)
#define clr_SCREEN_CAN_USE_USB set_io(PORTJ, 1)
#define tgl_SCREEN_CAN_USE_USB toggle_io(PORTJ, 1)
#define read_SCREEN_CAN_USE_USB read_io(PORTJ, 1)

/* POT_CLK */
#define set_POT_CLK set_io(PORTK, 1)
#define clr_POT_CLK clear_io(PORTK, 1)
#define tgl_POT_CLK toggle_io(PORTK, 1)
#define read_POT_CLK read_io(PORTK, 1)

/* POT_SDI */
#define set_POT_SDI set_io(PORTK, 2)
#define clr_POT_SDI clear_io(PORTK, 2)
#define tgl_POT_SDI toggle_io(PORTK, 2)
#define read_POT_SDI read_io(PORTK, 2)

/* POT_CS_410 */
#define set_POT_CS_410 clear_io(PORTJ, 3)
#define clr_POT_CS_410 set_io(PORTJ, 3)
#define tgl_POT_CS_410 toggle_io(PORTJ, 3)
#define read_POT_CS_410 read_io(PORTJ, 3)

/* POT_CS_470 */
#define set_POT_CS_470 clear_io(PORTJ, 5)
#define clr_POT_CS_470 set_io(PORTJ, 5)
#define tgl_POT_CS_470 toggle_io(PORTJ, 5)
#define read_POT_CS_470 read_io(PORTJ, 5)

/* POT_CS_560 */
#define set_POT_CS_560 clear_io(PORTJ, 7)
#define clr_POT_CS_560 set_io(PORTJ, 7)
#define tgl_POT_CS_560 toggle_io(PORTJ, 7)
#define read_POT_CS_560 read_io(PORTJ, 7)

/* DAC_CS_LASER */
#define set_DAC_CS_LASER clear_io(PORTC, 1)
#define clr_DAC_CS_LASER set_io(PORTC, 1)
#define tgl_DAC_CS_LASER toggle_io(PORTC, 1)
#define read_DAC_CS_LASER read_io(PORTC, 1)

/* DAC_SCLK_LASER */
#define set_DAC_SCLK_LASER set_io(PORTC, 4)
#define clr_DAC_SCLK_LASER clear_io(PORTC, 4)
#define tgl_DAC_SCLK_LASER toggle_io(PORTC, 4)
#define read_DAC_SCLK_LASER read_io(PORTC, 4)

/* DAC_MOSI_LASER */
#define set_DAC_MOSI_LASER set_io(PORTC, 5)
#define clr_DAC_MOSI_LASER clear_io(PORTC, 5)
#define tgl_DAC_MOSI_LASER toggle_io(PORTC, 5)
#define read_DAC_MOSI_LASER read_io(PORTC, 5)

/* CAM_TRIGGER */
#define set_CAM_TRIGGER set_io(PORTC, 0)
#define clr_CAM_TRIGGER clear_io(PORTC, 0)
#define tgl_CAM_TRIGGER toggle_io(PORTC, 0)
#define read_CAM_TRIGGER read_io(PORTC, 0)

/* CAM_GPIO2 */
#define set_CAM_GPIO2 set_io(PORTF, 5)
#define clr_CAM_GPIO2 clear_io(PORTF, 5)
#define tgl_CAM_GPIO2 toggle_io(PORTF, 5)
#define read_CAM_GPIO2 read_io(PORTF, 5)

/* CAM_GPIO3 */
#define set_CAM_GPIO3 set_io(PORTF, 4)
#define clr_CAM_GPIO3 clear_io(PORTF, 4)
#define tgl_CAM_GPIO3 toggle_io(PORTF, 4)
#define read_CAM_GPIO3 read_io(PORTF, 4)

typedef struct
{
	uint16_t REG_CONFIG;
	uint16_t REG_RESERVED0;
	uint16_t REG_DAC_L410;
	uint16_t REG_DAC_L470;
	uint16_t REG_DAC_L560;
	uint16_t REG_DAC_ALL_LEDS[3];
	uint16_t REG_DAC_LASER;
	uint8_t REG_SCREEN_BRIGHT;
	uint8_t REG_SCREEN_IMG_INDEX;
	uint8_t REG_GAIN_PD_L410;
	uint8_t REG_GAIN_PD_L470;
	uint8_t REG_GAIN_PD_L560;
	uint8_t REG_STIM_KEY_SWITCH_STATE;
	uint8_t REG_STIM_START;
	uint16_t REG_STIM_WAVELENGTH;
	uint16_t REG_STIM_PERIOD;
	uint16_t REG_STIM_ON;
	uint16_t REG_STIM_REPS;
	uint8_t REG_EXT_CAMERA_START;
	uint16_t REG_EXT_CAMERA_PERIOD;
	uint8_t REG_OUT0_CONF;
	uint8_t REG_OUT1_CONF;
	uint8_t REG_IN0_CONF;
	uint8_t REG_IN1_CONF;
	uint8_t REG_OUT_SET;
	uint8_t REG_OUT_CLEAR;
	uint8_t REG_OUT_TOGGLE;
	uint8_t REG_OUT_WRITE;
	uint8_t REG_IN_READ;
	uint8_t REG_START;
	uint16_t REG_FRAME_EVENT;
	uint8_t REG_TRIGGER_STATE[32];
	uint8_t REG_TRIGGER_STATE_LENGTH;
	uint16_t REG_TRIGGER_PERIOD;
	uint16_t REG_TRIGGER_T_ON;
	uint16_t REG_TRIGGER_T_UPDATE_OUTPUTS;
	uint8_t REG_TRIGGER_STIM_BEHAVIOR;
	uint8_t REG_PHOTODIODES_START;
	uint16_t REG_PHOTODIODES[12];
	uint16_t REG_TEMPERATURE;
	uint8_t REG_SCREEN_HW_VERSION_H;
	uint8_t REG_SCREEN_HW_VERSION_L;
	uint8_t REG_SCREEN_ASSEMBLY_VERSION;
	uint8_t REG_SCREEN_FW_VERSION_H;
	uint8_t REG_SCREEN_FW_VERSION_L;
	uint16_t REG_SERIAL_NUMBER;
	uint16_t REG_CAL_L410[8];
	uint16_t REG_CAL_L470[8];
	uint16_t REG_CAL_L560[8];
	uint16_t REG_CAL_LASER[8];
	uint16_t REG_CAL_PH410[8];
	uint16_t REG_CAL_PH470[8];
	uint16_t REG_CAL_PH560[8];
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_CONFIG                      32 // U16    Configuration of the device
#define ADD_REG_RESERVED0                   33 // U16    Reserved
#define ADD_REG_DAC_L410                    34 // U16    Writes to the DAC that controls the LED 410
#define ADD_REG_DAC_L470                    35 // U16    Writes to the DAC that controls the LED 470
#define ADD_REG_DAC_L560                    36 // U16    Writes to the DAC that controls the LED 460
#define ADD_REG_DAC_ALL_LEDS                37 // U16    Writes to all the LED DACs in a row
#define ADD_REG_DAC_LASER                   38 // U16    Writes to the DAC that controls the LASER
#define ADD_REG_SCREEN_BRIGHT               39 // U8     Set the brightness of the screen [0:15]
#define ADD_REG_SCREEN_IMG_INDEX            40 // U8     Display the image [0:127]
#define ADD_REG_GAIN_PD_L410                41 // U8     Configures the gain of L410's photodiode [1:32]
#define ADD_REG_GAIN_PD_L470                42 // U8     Configures the gain of L470's photodiode [1:32]
#define ADD_REG_GAIN_PD_L560                43 // U8     Configures the gain of L560's photodiode [1:32]
#define ADD_REG_STIM_KEY_SWITCH_STATE       44 // U8     Read only. Check if the laser is enabled by the Switch Key.
#define ADD_REG_STIM_START                  45 // U8     Starts and stops the stimulation
#define ADD_REG_STIM_WAVELENGTH             46 // U16    Wavelenght of the selected laser
#define ADD_REG_STIM_PERIOD                 47 // U16    Period of the pulse  (in miliseconds) [2;60000]
#define ADD_REG_STIM_ON                     48 // U16    Time that the pulse will be high (in miliseconds) [1;60000]
#define ADD_REG_STIM_REPS                   49 // U16    Number of repetitions.
#define ADD_REG_EXT_CAMERA_START            50 // U8     Start camera triggers on digital output 1 if equal to 1 and stop if equal to 0
#define ADD_REG_EXT_CAMERA_PERIOD           51 // U16    In microseconds [5000:65000]
#define ADD_REG_OUT0_CONF                   52 // U8     Configuration of digital output 0
#define ADD_REG_OUT1_CONF                   53 // U8     Configuration of digital output 1
#define ADD_REG_IN0_CONF                    54 // U8     Configuration of digital input 0
#define ADD_REG_IN1_CONF                    55 // U8     Configuration of digital input 1
#define ADD_REG_OUT_SET                     56 // U8     Sets the digital outputs
#define ADD_REG_OUT_CLEAR                   57 // U8     Clear the digital outputs
#define ADD_REG_OUT_TOGGLE                  58 // U8     Toggle the digital outputs
#define ADD_REG_OUT_WRITE                   59 // U8     Write to the digital outputs
#define ADD_REG_IN_READ                     60 // U8     Contains the state of the digital inputs
#define ADD_REG_START                       61 // U8     Start running trough the TRIGGER_STATE vector
#define ADD_REG_FRAME_EVENT                 62 // U16    Event of the frame executed
#define ADD_REG_TRIGGER_STATE               63 // U8
#define ADD_REG_TRIGGER_STATE_LENGTH        64 // U8
#define ADD_REG_TRIGGER_PERIOD              65 // U16    Period of each trigger in microseconds [5000:65000]
#define ADD_REG_TRIGGER_T_ON                66 // U16    Interval that the trigger line is high in microseconds
#define ADD_REG_TRIGGER_T_UPDATE_OUTPUTS    67 // U16    Timeout to update LED s (410, 470 and 560 only)  in microseconds
#define ADD_REG_TRIGGER_STIM_BEHAVIOR       68 // U8     Configures what the START_STIM bit does
#define ADD_REG_PHOTODIODES_START           69 // U8     Start streaming the photodiodes ' values if equal to 1. Write 0 to stop.
#define ADD_REG_PHOTODIODES                 70 // U16    Value read from the 3 photodiodes in the sequence PH410, PH470, PH560
#define ADD_REG_TEMPERATURE                 71 // U16    Raw temperature. Apply the formula T = 55 + (TEMPERATURE - 16384) / 160
#define ADD_REG_SCREEN_HW_VERSION_H         72 // U8     Version of screen's hardware (Major)
#define ADD_REG_SCREEN_HW_VERSION_L         73 // U8     Version of screen's hardware (Minor)
#define ADD_REG_SCREEN_ASSEMBLY_VERSION     74 // U8     Version of screen's assembly
#define ADD_REG_SCREEN_FW_VERSION_H         75 // U8     Version of screen's firmware (Major)
#define ADD_REG_SCREEN_FW_VERSION_L         76 // U8     Version of screen's firmware (Minor)
#define ADD_REG_SERIAL_NUMBER               77 // U16    Serial number
#define ADD_REG_CAL_L410                    78 // U16    Calibration data
#define ADD_REG_CAL_L470                    79 // U16    Calibration data
#define ADD_REG_CAL_L560                    80 // U16    Calibration data
#define ADD_REG_CAL_LASER                   81 // U16    Calibration data
#define ADD_REG_CAL_PH410                   82 // U16    Calibration data
#define ADD_REG_CAL_PH470                   83 // U16    Calibration data
#define ADD_REG_CAL_PH560                   84 // U16    Calibration data

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x54
#define APP_NBYTES_OF_REG_BANK              234

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_SYNC_TO_MASTER                   (1<<0)       // The device will output its internal clock sync and become a clock sync master
#define B_SYNC_TO_SLAVE                    (1<<1)       // The device receives a clock sync trough the clock sync input and copy it to the clock output
#define B_OUT0_TO_BNC                      (1<<2)       // Output 0 is routed to BNC
#define B_OUT0_TO_INT_LASER                (1<<3)       // Output 0 is routed to the internal laser trigger control
#define B_OUT0_TO_BOTH                     (1<<4)       // Output 0 is routed to both
#define B_COM_TO_MAIN                      (1<<5)       // USB interface is connected to the main microcontroller
#define B_COM_TO_SCREEN                    (1<<6)       // USB interface is connected to the screen
#define B_SCREEN_TO_BOOTLOADER             (1<<7)       // Put the screen on bootloader mode
#define B_ENABLE_LED_CURRENT_PROTECTION    (1<<14)      // Enables the maximum limit applied to LEDs (~850 mA)
#define B_DISABLE_LED_CURRENT_PROTECTION   (1<<15)      // Disables the maximum limit applied to LEDs (~850 mA)
#define B_KEY_SWITCH_IS_ON                 1            //
#define GM_STIM_START_CONF                 3            //
#define MSK_STIM_STOP                      0            //
#define MSK_STIM_START_REPS                1            //
#define MSK_STIM_START_INFINITE            2            //
#define GM_EXT_CAM_CONFIG                  3            //
#define MSK_EXT_CAM_STOP                   0            //
#define MSK_EXT_CAM_START_WITHOUT_EVENTS   1            //
#define MSK_EXT_CAM_START_WITH_EVENTS      2            //
#define GM_OUT_CONF                        3            //
#define MSK_OUT_CONF_SOFTWARE              0            // Controlled by software only
#define MSK_OUT_CONF_STROBE                1            // Outputs the strobe of the internal camera
#define MSK_OUT_CONF_STATE_CTRL            2            // Controlled by the TRIGGER_STATE
#define GM_IN_CONF                         15           //
#define MSK_IN_C_NONE                      0            //
#define MSK_IN_C_SOFTWARE_R                1            // Sends an event when a rising edge is detected
#define MSK_IN_C_SOFTWARE_F                2            // Sends an event when a falling edge is detected
#define MSK_IN_C_SOFTWARE_R_AND_F          3            // Sends an event when both edges are detected
#define MSK_IN_C_START_STOP_TRIG           4            // Starts the triggering when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_CAM_WITHOUT_EVTS 5            // Starts the external camera (without events) when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_CAM_WITH_EVTS  6            // Starts the external camera (with events) when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_TRIG_AND_CAM_WITHOUT_EVTS 7            // Starts both the triggering and the external camera (without events) when a rising edge and stops when falling
#define MSK_IN_C_START_STOP_TRIG_AND_CAM_WITH_EVTS 8            // Starts both the triggering and the external camera (with events) when a rising edge and stops when falling
#define MSK_IN_C_START_STIM_REPS           9            // Starts the optogenetics pulses when a rising edge is detected
#define MSK_IN_C_START_STIM_INFINITE       10           // Starts the optogenetics pulses when a rising edge and stops when falling
#define B_L410                             (1<<0)       //
#define B_L470                             (1<<1)       //
#define B_L560                             (1<<2)       //
#define B_DOUT0                            (1<<3)       //
#define B_DOUT1                            (1<<4)       //
#define B_INTERNAL_CAM_TRIGGER             (1<<5)       //
#define B_INTERNAL_CAM_GPIO2               (1<<6)       //
#define B_INTERNAL_CAM_GPIO3               (1<<7)       //
#define B_DIN0                             (1<<0)       //
#define B_DIN1                             (1<<1)       //
#define B_START_TRIGGER                    (1<<0)       // Start the triggering of the TRIGGER_STATE bitmasks
#define B_START_EXT_CAMERA_WITHOUT_EVENTS  (1<<1)       // Start the external camera triggering (without events)
#define B_START_EXT_CAMERA_WITH_EVENTS     (1<<2)       // Start the external camera triggering (with events)
#define B_STOP_TRIGGER                     (1<<3)       // Stop the triggering of the TRIGGER_STATE
#define B_STOP_EXT_CAMERA                  (1<<4)       // Stop the external camera triggering
#define B_ON_L410                          (1<<0)       // LED 410 was ON when the camera was triggered
#define B_ON_L470                          (1<<1)       // LED 470 was ON when the camera was triggered
#define B_ON_L560                          (1<<2)       // LED 560 was ON when the camera was triggered
#define B_ON_OUT0                          (1<<3)       // State of digital output 0 when the camera was triggered
#define B_ON_OUT1                          (1<<4)       // State of digital output 1 when the camera was triggered
#define B_START_STIM                       (1<<5)       //
#define B_ON_INTERNAL_CAM_GPIO2            (1<<6)       //
#define B_ON_INTERNAL_CAM_GPIO3            (1<<7)       //
#define B_ON_IN0                           (1<<8)       // State of digital input 0 when the camera was triggered
#define B_ON_IN1                           (1<<9)       // State of digital input 1 when the camera was triggered
#define GM_TRIGGER_STIM_CONF               3            //
#define MSK_TRIGGER_STIM_CONF_START_REPS   0            //
#define MSK_TRIGGER_STIM_CONF_START_INFINITE 1            //
#define MSK_TRIGGER_STIM_CONF_START_STOP_INFINITE 2            //

#endif /* _APP_REGS_H_ */