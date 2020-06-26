#ifndef _USB_H_
#define _USB_H_
#include <avr/io.h>

#define read_SCREEN_CAN_USE_USB read_io(PORTK, 3)// SCREEN_CAN_USE_USB
#define set_SCREEN_IS_USING_USB set_io(PORTK, 2)
#define clr_SCREEN_IS_USING_USB clear_io(PORTK, 2)

#define HARP_HEADER_LENGHT 7
#define HARP_MESSAGE_LENGHT 4+488	// 492
#define HARP_MESSAGE_LENGHT_DIV2 246
#define HARP_PAYLOAD_LENGHT 240		// pixels/column

#define MAX_IMAGE_INDEX 128-1
#define MAX_COLUMN_INDEX 320-1

typedef struct
{
	uint16_t image_index;
	uint16_t column_index;
	uint16_t content[HARP_PAYLOAD_LENGHT];
} Payload_t;

typedef struct
{
	uint8_t MessageType;
	uint8_t Lenght;
	uint16_t ExtendedLenght;
	uint8_t Address;
	uint8_t Port;
	uint8_t PayloadType;
	Payload_t Payload;
	uint8_t Checksum;
} HarpMessage_t;

void init_usb_serial(void);

void init_usb_ios(void);

#endif /* _USB_H_ */