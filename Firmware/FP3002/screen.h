#ifndef _SCREEN_H_
#define _SCREEN_H_
#include <avr/io.h>

#define CMD_HEADER_MASK 0xF0
#define CMD_PAYLOAD_MASK 0x0F

/* General functions */
#define CMD_MASK_FUNC 0x70
#define CMD_FUNC_SCREEN_WAKEUP 0x0E
#define CMD_FUNC_SCREEN_CLEAN  0x0D
#define CMD_FUNC_JUMP_TO_BOOTLOADER 0x01

/* Set the brightness */
#define CMD_MASK_SCREEN_BRIGHT_MASK 0x60

/* Display image */
#define CMD_BIT_MASK_DISPLAY_INDEX (1<<7)
#define CMD_MASK_IMAGE_INDEX 0x7F


void init_screen_serial(void);

void screen_wakeup_now(void);
void screen_clean(void);
void screen_clean_now(void);

void screen_send_to_bootloader(void);

void screen_set_bright(uint8_t bright);
void screen_set_bright_now(uint8_t bright);

void display_image(uint8_t image_index);
void display_image_now_byte(uint8_t image_index);

#endif /* _SCREEN_H_ */