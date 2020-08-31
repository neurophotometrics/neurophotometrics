#ifndef _DAC_H_
#define _DAC_H_
#include <avr/io.h>

// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif


#define clr_DAC_L410 do {set_DAC_CLR_410; set_DAC_CLR_410; dac_L410_state = false; clr_DAC_CLR_410;} while(0)
#define clr_DAC_L470 do {set_DAC_CLR_470; set_DAC_CLR_470; dac_L470_state = false; clr_DAC_CLR_470;} while(0)
#define clr_DAC_L560 do {set_DAC_CLR_560; set_DAC_CLR_560; dac_L560_state = false; clr_DAC_CLR_560;} while(0)

void set_dac_L410(uint16_t content);
void set_dac_L470(uint16_t content);
void set_dac_L560(uint16_t content);
//void _set_dac_L560(uint16_t content);

void set_dac_LASER(uint16_t content);

//void clr_dac_L410(void);
//void clr_dac_L470(void);
//void clr_dac_L560(void);

void tgl_dac_L410(uint16_t content);
void tgl_dac_L470(uint16_t content);
void tgl_dac_L560(uint16_t content);

bool read_dac_L410(void);
bool read_dac_L470(void);
bool read_dac_L560(void);

#endif /* _DAC_H_ */