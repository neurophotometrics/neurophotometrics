#ifndef _IMAGES_H_
#define _IMAGES_H_
#include <avr/io.h>

#define BLOCKS_FOR_EACH_IMAGE 2
#define COLUMNS_ON_EACH_PAGE 4

void init_images_ios(void);

void alloc_image(uint16_t image_index, uint16_t column_index, uint16_t * column_pixeis);
uint16_t * get_image(uint16_t image_index, uint16_t column_index);

void display_image_index(uint16_t index);

void write_index_on_screen(uint8_t index);

#endif /* _IMAGES_H_ */