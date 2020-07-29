#ifndef _AD5204_H_
#define _AD5204_H_
#include <avr/io.h>

void as5204_write_channel_L410(uint8_t channel, uint8_t data);
void as5204_write_channel_L470(uint8_t channel, uint8_t data);
void as5204_write_channel_L560(uint8_t channel, uint8_t data);

//void get_resitor_values(uint8_t gain, uint8_t * rf, uint8_t * rt);

void set_gain_L410(uint8_t gain);
void set_gain_L470(uint8_t gain);
void set_gain_L560(uint8_t gain);

#endif /* _AD5204_H_ */