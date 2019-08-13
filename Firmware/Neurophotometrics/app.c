#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "ad5204.h"

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
    uint8_t hwH = 1;
    uint8_t hwL = 0;
    uint8_t fwH = 0;
    uint8_t fwL = 1;
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
	clr_L410;
	clr_L470;
	clr_L560;
	clr_LEXTRA;
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */
static void delay_1ms (TC0_t* timer, int ms)
{
	while(ms--)
		timer_type0_wait(timer, TIMER_PRESCALER_DIV256, 125); // 1ms
}

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_1st_config_hw_after_boot(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	/* Initialize hardware */
	adc_A_initialize_single_ended(ADC_REFSEL_INTVCC_gc);  // VCC/1.6 = 3.3/1.6 = 2.0625 V
	
	/* Reset the pot             */
	/* low pulse @ SHDN for 1 ms */
	//clr_POT_SHDN; delay_1ms(&TCC0, 1);
	//set_POT_SHDN; delay_1ms(&TCC0, 1);
	//clr_POT_SHDN; delay_1ms(&TCC0, 1);	
	
	/* Center the pot outputs to mid scale */
	/* Low pulse @ !PR for 1 ms            */
	//clr_POT_MIDSCALE; delay_1ms(&TCC0, 1);
	//set_POT_MIDSCALE; delay_1ms(&TCC0, 1);
	//clr_POT_MIDSCALE; delay_1ms(&TCC0, 1);
	
	/* Remove reset from the digital pot */
	clr_POT_SHDN;
	
	set_EN_L410;
	set_EN_L470;
	set_EN_L560;
	set_EN_LEXTRA;
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_RAW_POT_L410 = 125;
	app_regs.REG_RAW_POT_L470 = 125;
	app_regs.REG_RAW_POT_L560 = 125;
	app_regs.REG_RAW_POT_LEXTRA = 125;
	
	app_regs.REG_FRAME_EVENT_CFG = MSK_FRAME_TRIG_TRIGGER_OUT;
	
	app_regs.REG_TRIGGER_STATE[0] = B_ON_L410;
	app_regs.REG_TRIGGER_STATE[1] = B_ON_L470;
	app_regs.REG_TRIGGER_STATE[2] = B_ON_L560;
	app_regs.REG_TRIGGER_STATE[3] = B_ON_LEXTRA;
	app_regs.REG_TRIGGER_STATE_LENGTH = 4;
	
	app_regs.REG_TRIGGER_PERIOD = 50000; // 20 Hz
	app_regs.REG_TRIGGER_T_ON = 25000;   // 
	app_regs.REG_TRIGGER_PRE_LEDS = 100; // 100 us before
	
	app_regs.REG_OUT0_CONF = MSK_OUT_CONF_SOFTWARE;
	app_regs.REG_OUT1_CONF = MSK_OUT_CONF_CAMERA;
	app_regs.REG_IN0_CONF = MSK_IN_C_SOFTWARE_R;
	
	app_regs.REG_ADCIN_CONF = MSK_ADCIN_DISABLED;
	
	app_regs.REG_STIM_PERIOD = 100;
	app_regs.REG_STIM_ON = 50;
	app_regs.REG_STIM_REPS = 10;
	
	app_regs.REG_STIM_PERIOD = 100;
	app_regs.REG_STIM_ON = 50;
	app_regs.REG_STIM_REPS = 10;
	
	app_regs.REG_EXT_CAMERA_PERIOD = 25000; // 40 Hz
}

void core_callback_registers_were_reinitialized(void)
{
	/* Set pots to configured values */
	as5204_write_channel(0, app_regs.REG_RAW_POT_L410);
	as5204_write_channel(2, app_regs.REG_RAW_POT_L470);
	as5204_write_channel(1, app_regs.REG_RAW_POT_L560);
	as5204_write_channel(3, app_regs.REG_RAW_POT_LEXTRA);
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void) {}
void core_callback_t_500us(void) {}

uint8_t adc_read_counter = 0;

void core_callback_t_1ms(void)
{
	if (++adc_read_counter == 5)
	{
		if (app_regs.REG_ADCIN_CONF == MSK_ADCIN_200Hz)
		{
			/* Read ADC */
			int16_t adc = adc_A_read_channel(5);
			
			if (adc < 0 || adc > 4096)
				app_regs.REG_ADCIN = 0;
			else
				app_regs.REG_ADCIN = adc;
		
			core_func_send_event(ADD_REG_ADCIN, true);
		}
		
		adc_read_counter = 0;
	}
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