#include "images.h"
#include "memory.h"
#include "ILI9341.h"
#include "uart0.h"
#include "usb.h"

uint16_t image_rx_buffer[1024];
uint16_t image_read_buffer[1024];

#define INDEX_TEXT_LEN 15
const char index_text[INDEX_TEXT_LEN] = "SAVED AT INDEX ";

void alloc_image(uint16_t image_index, uint16_t column_index, uint16_t * column_pixeis)
{
	set_io(PORTA, 6);
	
	if (column_index == 0)
	{
		/* Erase previous image */
		for (uint8_t i = 0; i<BLOCKS_FOR_EACH_IMAGE; i++)
		{
			block_erase(image_index * BLOCKS_FOR_EACH_IMAGE + i);
		}
		
	}
	
	for (uint8_t i = 0; i<LCD_MAX_Y; i++)
	{
		image_rx_buffer[(column_index % COLUMNS_ON_EACH_PAGE) * LCD_MAX_Y + i] = column_pixeis[i];
	}
	
	if (column_index % COLUMNS_ON_EACH_PAGE == COLUMNS_ON_EACH_PAGE - 1)
	{	
		program_memory_without_spare(
			image_index * BLOCKS_FOR_EACH_IMAGE * PAGES_PER_BLOCK + (column_index - COLUMNS_ON_EACH_PAGE + 1)/4,
			((uint8_t*)(&image_rx_buffer)));
	}
	
	/* Preview image on screen */
	for (uint16_t j = 0; j < LCD_MAX_Y; j++)
		draw_a_pixel_color(column_index, j, column_pixeis[j]);
	
	if (column_index == 319)
	{
		write_index_on_screen(image_index);
		TCC0_CNT = TCC0_PER - 10*32;	// 10 x 1.024 ms
										// 5 ms is enough, using 10 to make sure
	}	
	
	clear_io(PORTA, 6);
}

uint16_t * get_image(uint16_t image_index, uint16_t column_index)
{
	set_io(PORTA, 6);
	
	if (column_index % COLUMNS_ON_EACH_PAGE == 0)
	{	
		read_memory_without_spare(
			image_index * BLOCKS_FOR_EACH_IMAGE * PAGES_PER_BLOCK + column_index/4,
			((uint8_t*)(&image_read_buffer)));
	}
	
	clear_io(PORTA, 6);
	
	return &image_read_buffer[(column_index % COLUMNS_ON_EACH_PAGE) * LCD_MAX_Y];
}

void display_image_index(uint16_t index)
{
	uint16_t * column_raw_image;
	
	for (uint16_t i = 0; i < LCD_MAX_X; i++)
	{
		column_raw_image = get_image(index, i);
			
		for (uint8_t j = 0; j < LCD_MAX_Y; j++)
		{
			draw_a_pixel_color(i, j, *(column_raw_image+j));
		}
	}
}

void write_index_on_screen(uint8_t index)
{
	uint16_t length = 0;
	
	length = length + draw_space_between_letters(length, 240-7, 0, 0, 0);
	
	for (uint8_t i = 0; i < INDEX_TEXT_LEN; i ++)
	{
		length = length + draw_letter(index_text[i], length, 240-7, 255, 255, 255, 0, 0, 0);
		length = length + draw_space_between_letters(length, 240-7, 0, 0, 0);
	}
	
	uint8_t hundreds = (index - (index % 100));
	uint8_t dozens   = (index - hundreds - (index % 10));
	uint8_t units    = index % 10;
	
	length = length + draw_letter(hundreds/100 + 48, length, 240-7, 255, 255, 255, 0, 0, 0);
	length = length + draw_space_between_letters(length, 240-7, 0, 0, 0);
	
	length = length + draw_letter(dozens/10 + 48,length, 240-7, 255, 255, 255, 0, 0, 0);
	length = length + draw_space_between_letters(length, 240-7, 0, 0, 0);
	
	length = length + draw_letter(units + 48,length, 240-7, 255, 255, 255, 0, 0, 0);
	length = length + draw_space_between_letters(length, 240-7, 0, 0, 0);
}
