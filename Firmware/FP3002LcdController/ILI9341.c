#include "ILI9341.h"
#define F_CPU 32000000
#include <util/delay.h>

// https://learn.adafruit.com/adafruit-2-dot-8-color-tft-touchscreen-breakout-v2/downloads
// https://www.adafruit.com/product/1770

/************************************************************************/
/* GPIOs                                                                */
/************************************************************************/
static void lcd_data_to_out(void)
{
	/*
	io_pin2out(&PORTE, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D0
	io_pin2out(&PORTE, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D1
	io_pin2out(&PORTE, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D2
	io_pin2out(&PORTE, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D3
	io_pin2out(&PORTE, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D4
	io_pin2out(&PORTE, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D5
	io_pin2out(&PORTE, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D6
	io_pin2out(&PORTE, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);		// D7
	*/
	PORTE_DIR = 0xFF;
}

static void lcd_data_to_in(void)
{
	/*
	io_pin2in(&PORTE, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D0
	io_pin2in(&PORTE, 1, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D1
	io_pin2in(&PORTE, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D2
	io_pin2in(&PORTE, 3, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D3
	io_pin2in(&PORTE, 4, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D4
	io_pin2in(&PORTE, 5, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D5
	io_pin2in(&PORTE, 6, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D6
	io_pin2in(&PORTE, 7, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);	// D7
	*/
	
	PORTE_DIR = 0;
}

void init_lcd_ios(void)
{
	lcd_data_to_out();										// D0-7
	
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// CS
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// C/D
	io_pin2out(&PORTC, 2, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// WR
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// RD
	io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// RST
	
	LCD_CS_IDLE;
	LCD_CD_IDLE;
	LCD_WR_IDLE;
	LCD_RD_IDLE;
	LCD_RST_IDLE;
	
	_delay_ms(100);
	
	/* Back-light pin to output */
	io_pin2out(&PORTD, 0, OUT_IO_DIGITAL, IN_EN_IO_DIS);
		
	/* Initialize back-light at 100 Hz */
	/* Above 200 Hz we get flickering in some screens (on one unit we can still see it at 250 Hz) */
	timer_type0_pwm(&TCD0, TIMER_PRESCALER_DIV256, 125*10, 0, INT_LEVEL_OFF, INT_LEVEL_OFF);
}

void reset_lcd(void)
{
	LCD_CS_IDLE;
	LCD_WR_IDLE;
	LCD_RD_IDLE;
	
	LCD_RST_ACTIVE;
	_delay_ms(1);
	LCD_RST_IDLE;
	_delay_ms(150);
	
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(0x00);
	for(uint8_t i=0; i<3; i++) LCD_WR_STROBE; // Three extra 0x00s
	LCD_CS_IDLE;
}


/************************************************************************/
/* Register access                                                      */
/************************************************************************/
static void write_lcd_register8b(uint8_t reg, uint8_t content)
{
	LCD_CS_ACTIVE; _delay_ms(1);
	LCD_CD_COMMAND; _delay_ms(1);
	LCD_Write8(reg); _delay_ms(1);
	LCD_CD_DATA; _delay_ms(1);
	LCD_Write8(content); _delay_ms(1);
	LCD_CS_IDLE; _delay_ms(1);
}

static void write_lcd_register16b(uint16_t reg, uint16_t content)
{
	uint8_t hi, lo;
	
	hi = reg >> 8;
	lo = reg;
	
	LCD_CS_ACTIVE; _delay_ms(1);
	LCD_CD_COMMAND; _delay_ms(1);
	LCD_Write8(hi); _delay_ms(1);
	LCD_Write8(lo); _delay_ms(1);
	
	hi = content >> 8;
	lo = content;
	
	LCD_CD_DATA; _delay_ms(1);
	LCD_Write8(hi); _delay_ms(1);
	LCD_Write8(lo); _delay_ms(1);
	
	LCD_CS_IDLE; _delay_ms(1);
}

static void write_lcd_register32b(uint8_t reg, uint32_t content)
{
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(reg);
	LCD_CD_DATA;
	//_delay_us(10);
	LCD_Write8(content >> 24);
	//_delay_us(10);
	LCD_Write8(content >> 16);
	//_delay_us(10);
	LCD_Write8(content >> 8);
	//_delay_us(10);
	LCD_Write8(content);
	LCD_CS_IDLE;

}

static uint32_t read_lcd_register32b(uint8_t reg)
{
	uint8_t aux8;
	uint32_t content;
	
	LCD_CS_ACTIVE; _delay_ms(1);
	LCD_CD_COMMAND; _delay_ms(1);
	
	LCD_Write8(reg); _delay_ms(1);
	
	lcd_data_to_in(); _delay_ms(1);
	
	LCD_CD_DATA; _delay_ms(1);
	_delay_ms(1); _delay_ms(1);
	
	LCD_Read8(aux8); _delay_ms(1);
	content = aux8;
	content <<= 8;
	LCD_Read8(aux8); _delay_ms(1);
	content |= aux8;
	content <<= 8;
	LCD_Read8(aux8); _delay_ms(1);
	content |= aux8;
	content <<= 8;
	LCD_Read8(aux8); _delay_ms(1);
	content |= aux8;
	
	LCD_CS_IDLE; _delay_ms(1);
	
	lcd_data_to_out();
	
	return content;
}


static void setAddrWindow(int x1, int y1, int x2, int y2)
{
    uint32_t t;

    t = x1;
    t <<= 16;
    t |= x2;
    write_lcd_register32b(ILI9341_COLADDRSET, t);
    t = y1;
    t <<= 16;
    t |= y2;
    write_lcd_register32b(ILI9341_PAGEADDRSET, t);
}


/************************************************************************/
/* LCD drivers                                                          */
/************************************************************************/
/* Intensity range = [0:125] */
void set_backlight_intensity(uint16_t intensity)
{
	TCD0_CCA = (intensity > 125*10) ? 125*10 : intensity;
}


bool check_lcd_ID(void)
{	
	return (read_lcd_register32b(0xD3) & (uint32_t)0xFFFF) == 0x9341 ? true : false;
}

void lcd_begin(void)
{
    write_lcd_register8b(ILI9341_SOFTRESET, 0);
    _delay_ms(50);
    write_lcd_register8b(ILI9341_DISPLAYOFF, 0);

    write_lcd_register8b(ILI9341_POWERCONTROL1, 0x23);
    write_lcd_register8b(ILI9341_POWERCONTROL2, 0x10);
    write_lcd_register16b(ILI9341_VCOMCONTROL1, 0x2B2B);
    write_lcd_register8b(ILI9341_VCOMCONTROL2, 0xC0);
    write_lcd_register8b(ILI9341_MEMCONTROL, ILI9341_MADCTL_MY | ILI9341_MADCTL_BGR);
    write_lcd_register8b(ILI9341_PIXELFORMAT, 0x55);	// Set to 16 bits/pixel
    write_lcd_register16b(ILI9341_FRAMECONTROL, 0x001B);
    
    write_lcd_register8b(ILI9341_ENTRYMODE, 0x07);
    /* writeRegister32(ILI9341_DISPLAYFUNC, 0x0A822700);*/

    write_lcd_register8b(ILI9341_SLEEPOUT, 0);
    _delay_ms(150);
    write_lcd_register8b(ILI9341_DISPLAYON, 0);
    _delay_ms(500);
    setAddrWindow(0, 0, 240-1, 320-1);
	
    return;
}


/************************************************************************/
/* Fill a pixel                                                         */
/************************************************************************/
void draw_a_pixel_color(int16_t x, int16_t y, uint16_t color)
{
    //setAddrWindow(LCD_MAX_Y - 1 - y, LCD_MAX_X - 1 - x, 240-1, 320-1);

	uint32_t t;

    t = LCD_MAX_Y - 1 - y;
    t <<= 16;
    t |= 240-1;
    //write_lcd_register32b(ILI9341_COLADDRSET, t);
	
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(ILI9341_COLADDRSET);
	LCD_CD_DATA;
	//_delay_us(10);
	LCD_Write8(t >> 24);
	//_delay_us(10);
	LCD_Write8(t >> 16);
	//_delay_us(10);
	LCD_Write8(t >> 8);
	//_delay_us(10);
	LCD_Write8(t);
	LCD_CS_IDLE;
	
    t = LCD_MAX_X - 1 - x;
    t <<= 16;
    t |= 320-1;
    //write_lcd_register32b(ILI9341_PAGEADDRSET, t);
	
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(ILI9341_PAGEADDRSET);
	LCD_CD_DATA;
	//_delay_us(10);
	LCD_Write8(t >> 24);
	//_delay_us(10);
	LCD_Write8(t >> 16);
	//_delay_us(10);
	LCD_Write8(t >> 8);
	//_delay_us(10);
	LCD_Write8(t);
	LCD_CS_IDLE;

    LCD_CS_ACTIVE;
    LCD_CD_COMMAND;
    LCD_Write8(0x2C);
    LCD_CD_DATA;
    LCD_Write8(color >> 8);
	LCD_Write8(color);
	LCD_CS_IDLE;
}

/* RGB range should be [0:255] */
void draw_a_pixel_rgb(int16_t x, int16_t y, uint8_t R, uint8_t G, uint8_t B)
{
    //setAddrWindow(LCD_MAX_Y - 1 - y, LCD_MAX_X - 1 - x, 240-1, 320-1);

	uint32_t t;

    t = LCD_MAX_Y - 1 - y;
    t <<= 16;
    t |= 240-1;
    //write_lcd_register32b(ILI9341_COLADDRSET, t);
	
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(ILI9341_COLADDRSET);
	LCD_CD_DATA;
	//_delay_us(10);
	LCD_Write8(t >> 24);
	//_delay_us(10);
	LCD_Write8(t >> 16);
	//_delay_us(10);
	LCD_Write8(t >> 8);
	//_delay_us(10);
	LCD_Write8(t);
	LCD_CS_IDLE;
	
    t = LCD_MAX_X - 1 - x;
    t <<= 16;
    t |= 320-1;
    //write_lcd_register32b(ILI9341_PAGEADDRSET, t);
	
	LCD_CS_ACTIVE;
	LCD_CD_COMMAND;
	LCD_Write8(ILI9341_PAGEADDRSET);
	LCD_CD_DATA;
	//_delay_us(10);
	LCD_Write8(t >> 24);
	//_delay_us(10);
	LCD_Write8(t >> 16);
	//_delay_us(10);
	LCD_Write8(t >> 8);
	//_delay_us(10);
	LCD_Write8(t);
	LCD_CS_IDLE;
	
	// R [15:11]
	// G [10:5]
	// B [4:0]
	// We are not using the G's LSB to make it 5 bits like the other colors
	uint16_t color = ((uint16_t)((R >> 3) & 0x1F) << 11)  | ((uint16_t)((G >> 3) & 0x1F) << 6) | ((B >> 3) & 0x1F) ;

    LCD_CS_ACTIVE;
    LCD_CD_COMMAND;
    LCD_Write8(0x2C);
    LCD_CD_DATA;
    LCD_Write8(color >> 8);
	LCD_Write8(color);
	LCD_CS_IDLE;
}

void disable_all_pixels(void)
{
	for (uint16_t i = 0; i < LCD_MAX_X; i++)
		for (uint16_t j = 0; j < LCD_MAX_Y; j++)
			draw_a_pixel_rgb(i, j, 0, 0, 0);
}

// https://www.123rf.com/photo_78494921_stock-vector-pixel-font-alphabet-letters-and-numbers-retro-videgame-type.html

uint8_t _A[7][5] = {
	{0,1,1,1,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1}};

uint8_t _B[7][5] = {
	{1,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,0}};

uint8_t _E[7][5] = {
	{1,1,1,1,1},
	{1,0,0,0,0},
	{1,0,0,0,0},
	{1,1,1,1,0},
	{1,0,0,0,0},
	{1,0,0,0,0},
	{1,1,1,1,1}};
		
uint8_t _D[7][5] = {
	{1,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,0}};

uint8_t _F[7][5] = {
	{1,1,1,1,1},
	{1,0,0,0,0},
	{1,0,0,0,0},
	{1,1,1,1,0},
	{1,0,0,0,0},
	{1,0,0,0,0},
	{1,0,0,0,0}};

uint8_t _G[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,0},
	{1,0,0,0,0},
	{1,0,0,1,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _H[7][5] = {
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1}};
		
uint8_t _I[7][3] = {
	{1,1,1,},
	{0,1,0,},
	{0,1,0,},
	{0,1,0,},
	{0,1,0,},
	{0,1,0,},
	{1,1,1,}};

uint8_t _M[7][5] = {
	{1,0,0,0,1},
	{1,1,0,1,1},
	{1,0,1,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1}};

uint8_t _N[7][5] = {
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,1,0,0,1},
	{1,0,1,0,1},
	{1,0,0,1,1},
	{1,0,0,0,1},
	{1,0,0,0,1}};

uint8_t _O[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _S[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,0},
	{0,1,1,1,0},
	{0,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _T[7][5] = {
	{1,1,1,1,1},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0}};

uint8_t _V[7][5] = {
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,0,1,0},
	{0,0,1,0,0}};

uint8_t _X[7][5] = {
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,0,1,0},
	{0,0,1,0,0},
	{0,1,0,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1}};
				
uint8_t _Y[7][5] = {
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,0,1,0},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0},
	{0,0,1,0,0}};
		
uint8_t _1[7][3] = {
	{0,1,0,},
	{1,1,0,},
	{0,1,0,},
	{0,1,0,},
	{0,1,0,},
	{0,1,0,},
	{1,1,1,}};
		
uint8_t _2[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{0,0,0,0,1},
	{0,0,1,1,0},
	{0,1,0,0,0},
	{1,0,0,0,0},
	{1,1,1,1,1}};

uint8_t _3[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{0,0,0,0,1},
	{0,0,1,1,0},
	{0,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _4[7][5] = {
	{0,0,0,1,1},
	{0,0,1,0,1},
	{0,1,0,0,1},
	{1,0,0,0,1},
	{1,1,1,1,1},
	{0,0,0,0,1},
	{0,0,0,0,1}};

uint8_t _5[7][5] = {
	{1,1,1,1,1},
	{1,0,0,0,0},
	{1,1,1,1,0},
	{0,0,0,0,1},
	{0,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _6[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,0},
	{1,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _7[7][5] = {
	{1,1,1,1,1},
	{0,0,0,0,1},
	{0,0,0,1,0},
	{0,0,1,0,0},
	{0,1,0,0,0},
	{0,1,0,0,0},
	{0,1,0,0,0}};

uint8_t _8[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _9[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,1},
	{0,0,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};

uint8_t _0[7][5] = {
	{0,1,1,1,0},
	{1,0,0,0,1},
	{1,0,0,1,1},
	{1,0,1,0,1},
	{1,1,0,0,1},
	{1,0,0,0,1},
	{0,1,1,1,0}};
			
uint8_t _space[7][2] = {
	{0,0},
	{0,0},
	{0,0},
	{0,0},
	{0,0},
	{0,0},
	{0,0}};
		
uint8_t _dot[7][1] = {
	{0},
	{0},
	{0},
	{0},
	{0},
	{0},
	{1}};
		
uint8_t _twodots[7][1] = {
	{0},
	{0},
	{1},
	{0},
	{0},
	{1},
	{0}};

uint8_t _smile[7][5] = {
	{0,0,0,1,0},
	{0,0,0,0,1},
	{0,1,0,0,1},
	{0,0,0,0,1},
	{0,0,0,0,1},
	{0,1,0,0,1},
	{0,0,0,1,0}};


uint8_t draw_letter(
	char letter,
	uint16_t X,
	uint16_t Y,
	uint8_t R,
	uint8_t G,
	uint8_t B,
	uint8_t R_back,
	uint8_t G_back,
	uint8_t B_back)
{
	uint8_t * ptr;
	uint8_t x, y;
	
	switch (letter)
	{
		case 'A': ptr = _A; x = 5; y = 7; break;
		case 'B': ptr = _B; x = 5; y = 7; break;
		case 'D': ptr = _D; x = 5; y = 7; break;
		case 'E': ptr = _E; x = 5; y = 7; break;
		case 'F': ptr = _F; x = 5; y = 7; break;		
		case 'G': ptr = _G; x = 5; y = 7; break;
		case 'H': ptr = _H; x = 5; y = 7; break;
		case 'I': ptr = _I; x = 3; y = 7; break;
		case 'M': ptr = _M; x = 5; y = 7; break;
		case 'N': ptr = _N; x = 5; y = 7; break;		
		case 'O': ptr = _O; x = 5; y = 7; break;		
		case 'S': ptr = _S; x = 5; y = 7; break;
		case 'T': ptr = _T; x = 5; y = 7; break;
		case 'V': ptr = _V; x = 5; y = 7; break;
		case 'X': ptr = _X; x = 5; y = 7; break;
		case 'Y': ptr = _Y; x = 5; y = 7; break;
		case '1': ptr = _1; x = 3; y = 7; break;
		case '2': ptr = _2; x = 5; y = 7; break;
		case '3': ptr = _3; x = 5; y = 7; break;
		case '4': ptr = _4; x = 5; y = 7; break;
		case '5': ptr = _5; x = 5; y = 7; break;
		case '6': ptr = _6; x = 5; y = 7; break;
		case '7': ptr = _7; x = 5; y = 7; break;
		case '8': ptr = _8; x = 5; y = 7; break;
		case '9': ptr = _9; x = 5; y = 7; break;
		case '0': ptr = _0; x = 5; y = 7; break;
		case ' ': ptr = _space; x = 2; y = 7; break;
		case '.': ptr = _dot; x = 1; y = 7; break;		
		case ':': ptr = _twodots; x = 1; y = 7; break;
		
		default : ptr = _space; x = 2; y = 7; break;
	}
	
	for (uint8_t i = 0; i < x; i++)
		for (uint8_t j = 0; j < y; j++)
		{
			if (*(ptr+(y-j-1)*x+i) == 1)
			{
				draw_a_pixel_rgb(X+i, Y+j, R, G, B);
			}
			else
			{
				draw_a_pixel_rgb(X+i, Y+j, R_back, G_back, B_back);
			}
		}
	
	return x;
}

uint8_t draw_space_between_letters(
	uint16_t X,
	uint16_t Y,
	uint8_t R_back,
	uint8_t G_back,
	uint8_t B_back)
{
	for (uint8_t i = 0; i < 2; i++)
		for (uint8_t j = 0; j < 7; j++)
			draw_a_pixel_rgb(X+i, Y+j, R_back, G_back, B_back);
	
	return 2;
}

uint8_t draw_other(uint8_t other, uint16_t X, uint16_t Y, uint8_t R, uint8_t G, uint8_t B)
{
	uint8_t * ptr;
	uint8_t x, y;
	
	switch (other)
	{
		case _DRAW_SMILE: ptr = _smile; x = 5; y = 7; break;
		
		default : ptr = _space; x = 2; y = 7; break;
	}
	
	for (uint8_t i = 0; i < x; i++)
		for (uint8_t j = 0; j < y; j++)
			if (*(ptr+(y-j-1)*x+i) ==1)
				draw_a_pixel_rgb(X+i, Y+j, R, G, B);
	
	return x;
}