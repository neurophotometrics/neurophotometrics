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

/* Check version */
#define CMD_MASK_VERSION_MASK 0x50
#define CMD_FUNC_FW_H_REQUEST 0x01
#define CMD_FUNC_FW_L_REQUEST 0x02
#define CMD_FUNC_HW_H_REQUEST 0x03
#define CMD_FUNC_HW_L_REQUEST 0x04
#define CMD_FUNC_ASS_REQUEST 0x05
#define CMD_FUNC_ALL_REQUEST 0x06
#define CMD_REPLY_FW_H_MASK 0x00
#define CMD_REPLY_FW_L_MASK 0x20
#define CMD_REPLY_HW_H_MASK 0x40
#define CMD_REPLY_HW_L_MASK 0x60
#define CMD_REPLY_ASS_MASK 0x80

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

void screen_get_versions(void);

void display_image(uint8_t image_index);
void display_image_now_byte(uint8_t image_index);

#endif /* _SCREEN_H_ */