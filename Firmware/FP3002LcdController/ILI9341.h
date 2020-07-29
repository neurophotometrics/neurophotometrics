#ifndef _ILI9341_H_
#define _ILI9341_H_

#include "cpu.h"

/*
	External pins IM [3:0] are equal to 0 by default, this means that we are in
	mode 8080 MCU 8-bit bus interface Ⅰ.
	
	CS  WR  RD  D/C
	L   ^   H   L   Write command code.
	L   H   ^   H   Read internal status.
	L   ^   H   H   Write parameter or display data.
	L   H   ^   H   Reads parameter or display data.	
*/


/************************************************************************/
/* Definition of pins                                                   */
/************************************************************************/
#define LCD_CS_ACTIVE	PORTC_OUTCLR = 1 // clear_io(PORTC, 0)
#define LCD_CS_IDLE		PORTC_OUTSET = 1 // set_io(PORTC, 0)
#define LCD_CD_ACTIVE	PORTC_OUTCLR = 2 // clear_io(PORTC, 1)
#define LCD_CD_IDLE		PORTC_OUTSET = 2 // set_io(PORTC, 1)
#define LCD_WR_ACTIVE	PORTC_OUTCLR = 4 // clear_io(PORTC, 2)
#define LCD_WR_IDLE		PORTC_OUTSET = 4 // set_io(PORTC, 2)
#define LCD_RD_ACTIVE	PORTC_OUTCLR = 8 // clear_io(PORTC, 3)
#define LCD_RD_IDLE		PORTC_OUTSET = 8 // set_io(PORTC, 3)
#define LCD_RST_ACTIVE	PORTC_OUTCLR = 16 // clear_io(PORTC, 4)
#define LCD_RST_IDLE	PORTC_OUTSET = 16 // set_io(PORTC, 4)


#define LCD_WR_STROBE	{ LCD_WR_ACTIVE; LCD_WR_IDLE; }
#define LCD_RD_STROBE	{ LCD_RD_ACTIVE; _delay_ms(1); LCD_RD_IDLE; _delay_ms(1); }

#define LCD_CD_COMMAND	LCD_CD_ACTIVE
#define LCD_CD_DATA		LCD_CD_IDLE

#define LCD_Write8(a)	{ PORTE_OUT = a; LCD_WR_STROBE; }
#define LCD_Read8(a)	{ LCD_RD_ACTIVE; _delay_ms(1); _delay_ms(1); a = PORTE_IN; _delay_ms(1); LCD_RD_IDLE; _delay_ms(1); }
	

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void init_lcd_ios(void);
void reset_lcd(void);

void set_backlight_intensity(uint16_t intensity);
bool check_lcd_ID(void);
void lcd_begin(void);

void draw_a_pixel_color(int16_t x, int16_t y, uint16_t color);
void draw_a_pixel_rgb(int16_t x, int16_t y, uint8_t R, uint8_t G, uint8_t B);
void disable_all_pixels(void);

#define _DRAW_SMILE 0
uint8_t draw_letter(
	char letter,
	uint16_t X,
	uint16_t Y,
	uint8_t R,
	uint8_t G,
	uint8_t B,
	uint8_t R_back,
	uint8_t G_back,
	uint8_t B_back
);

uint8_t draw_space_between_letters(
	uint16_t X,
	uint16_t Y,
	uint8_t R_back,
	uint8_t G_back,
	uint8_t B_back
);

uint8_t draw_other(uint8_t character, uint16_t X, uint16_t Y, uint8_t R, uint8_t G, uint8_t B);


/************************************************************************/
/* Screen resolution                                                    */
/************************************************************************/
#define LCD_MAX_X  320
#define LCD_MAX_Y  240


/************************************************************************/
/* Registers                                                            */
/************************************************************************/
#define ILI9341_SOFTRESET       0x01
#define ILI9341_SLEEPIN         0x10
#define ILI9341_SLEEPOUT        0x11
#define ILI9341_NORMALDISP      0x13
#define ILI9341_INVERTOFF       0x20
#define ILI9341_INVERTON        0x21
#define ILI9341_GAMMASET        0x26
#define ILI9341_DISPLAYOFF      0x28
#define ILI9341_DISPLAYON       0x29
#define ILI9341_COLADDRSET      0x2A
#define ILI9341_PAGEADDRSET     0x2B
#define ILI9341_MEMORYWRITE     0x2C
#define ILI9341_PIXELFORMAT     0x3A
#define ILI9341_FRAMECONTROL    0xB1
#define ILI9341_DISPLAYFUNC     0xB6
#define ILI9341_ENTRYMODE       0xB7
#define ILI9341_POWERCONTROL1   0xC0
#define ILI9341_POWERCONTROL2   0xC1
#define ILI9341_VCOMCONTROL1	0xC5
#define ILI9341_VCOMCONTROL2	0xC7
#define ILI9341_MEMCONTROL		0x36
#define ILI9341_MADCTL			0x36

#define ILI9341_MADCTL_MY		0x80
#define ILI9341_MADCTL_MX		0x40
#define ILI9341_MADCTL_MV		0x20
#define ILI9341_MADCTL_ML		0x10
#define ILI9341_MADCTL_RGB		0x00
#define ILI9341_MADCTL_BGR		0x08
#define ILI9341_MADCTL_MH		0x04


#endif /* _ILI9341_H_ */