#ifndef _MEMORY_H_
#define _MEMORY_H_

#include "cpu.h"


/************************************************************************/
/* Memory organization                                                  */
/************************************************************************/
#define PAGES_PER_MEM_2G 131072
#define PAGES_PER_MEM_4G 262144
#define BYTES_PER_PAGE 2048
#define PAGES_PER_BLOCK 64


/************************************************************************/
/* Commands list                                                        */
/************************************************************************/
#define MEM_REG_PAGE_READ 0x00
#define MEM_REG_BLOCK_ERASE 0x60
#define MEM_REG_READ_STATUS_REG 0x70
#define MEM_REG_PAGE_PROGRAM 0x80
#define MEM_REG_READ_ID 0x90

/************************************************************************/
/* Definition of the board                                              */
/************************************************************************/
//#define PCB_1V0
#define PCB_1V1


/************************************************************************/
/* Definition of pins                                                   */
/************************************************************************/
#ifdef PCB_1V0
	// CLE @ PA3 as output
	#define set_MEM_CLE  PORTA_OUTSET = (1<<3) // LATDSET = (1 << 13)
	#define clr_MEM_CLE  PORTA_OUTCLR = (1<<3) // LATDCLR = (1 << 13)
	#define tgl_MEM_CLE  PORTA_OUTTGL = (1<<3) // LATDINV = (1 << 13)
#endif

#ifdef PCB_1V1
	// CLE @ PK4 as output
	#define set_MEM_CLE  PORTK_OUTSET = (1<<4) // LATDSET = (1 << 13)
	#define clr_MEM_CLE  PORTK_OUTCLR = (1<<4) // LATDCLR = (1 << 13)
	#define tgl_MEM_CLE  PORTK_OUTTGL = (1<<4) // LATDINV = (1 << 13)
#endif

// ALE @ PA4 as output
#define set_MEM_ALE  PORTA_OUTSET = (1<<4) // LATGSET = (1 << 7)
#define clr_MEM_ALE  PORTA_OUTCLR = (1<<4) // LATGCLR = (1 << 7)
#define tgl_MEM_ALE  PORTA_OUTTGL = (1<<4) // LATGINV = (1 << 7)

#ifdef PCB_1V0
	// WP# @ PA7 as output
	#define set_MEM_WP  PORTA_OUTSET = (1<<7) // LATGSET = (1 << 8)
	#define clr_MEM_WP  PORTA_OUTCLR = (1<<7) // LATGCLR = (1 << 8)
	#define tgl_MEM_WP  PORTA_OUTTGL = (1<<7) // LATGINV = (1 << 8)
#endif

#ifdef PCB_1V1
	// WP# @ PC5 as output
	#define set_MEM_WP  PORTC_OUTSET = (1<<5) // LATGSET = (1 << 8)
	#define clr_MEM_WP  PORTC_OUTCLR = (1<<5) // LATGCLR = (1 << 8)
	#define tgl_MEM_WP  PORTC_OUTTGL = (1<<5) // LATGINV = (1 << 8)
#endif

#ifdef PCB_1V0
	// R/B# @ PA0 as INPUT
	#define read_MEM_BUSY read_io(PORTA, 0) // (PORTG & (1 << 9))
#endif

#ifdef PCB_1V1
	// R/B# @ PK7 as INPUT
	#define read_MEM_BUSY read_io(PORTK, 7) // (PORTG & (1 << 9))
#endif

#ifdef PCB_1V0
	// CE# @ PA2 as output
	#define set_MEM_CE  PORTA_OUTSET = (1<<2) // LATDSET = (1 << 12)
	#define clr_MEM_CE  PORTA_OUTCLR = (1<<2) // LATDCLR = (1 << 12)
	#define tgl_MEM_CE  PORTA_OUTTGL = (1<<2) // LATDINV = (1 << 12)
#endif

#ifdef PCB_1V1
	// CE# @ PK5 as output
	#define set_MEM_CE  PORTK_OUTSET = (1<<5) // LATDSET = (1 << 12)
	#define clr_MEM_CE  PORTK_OUTCLR = (1<<5) // LATDCLR = (1 << 12)
	#define tgl_MEM_CE  PORTK_OUTTGL = (1<<5) // LATDINV = (1 << 12)
#endif

// WE# @ PA5 as output
#define set_MEM_WE  PORTA_OUTSET = (1<<5) // LATCSET = (1 << 3)
#define clr_MEM_WE  PORTA_OUTCLR = (1<<5) // LATCCLR = (1 << 3)
#define tgl_MEM_WE  PORTA_OUTTGL = (1<<5) // LATCINV = (1 << 3)

#ifdef PCB_1V0
	// RE# @ PA1 as output
	#define set_MEM_RE  PORTA_OUTSET = (1<<1) // LATCSET = (1 << 4)
	#define clr_MEM_RE  PORTA_OUTCLR = (1<<1) // LATCCLR = (1 << 4)
	#define tgl_MEM_RE  PORTA_OUTTGL = (1<<1) // LATCINV = (1 << 4)
#endif

#ifdef PCB_1V1
	// RE# @ PK6 as output
	#define set_MEM_RE  PORTK_OUTSET = (1<<6) // LATCSET = (1 << 4)
	#define clr_MEM_RE  PORTK_OUTCLR = (1<<6) // LATCCLR = (1 << 4)
	#define tgl_MEM_RE  PORTK_OUTTGL = (1<<6) // LATCINV = (1 << 4)
#endif

// IO0-7 @ PB0-7 as inputs
#define to_output_MEM_DATA PORTB_DIR = 0xFF       // TRISECLR = 0xFF
#define to_input_MEM_DATA  PORTB_DIR = 0          // TRISESET = 0xFF
#define write_MEM_DATA(value)  PORTB_OUT = value  // LATE = value
#define read_MEM_DATA PORTB_IN


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void init_memory_ios(void);

uint8_t read_memory_size(void);
uint8_t block_erase (int32_t block_index);
uint8_t program_memory_without_spare (int32_t page_address, uint8_t *page);
void read_memory_without_spare (int32_t page_address, uint8_t *page);

#endif /* _MEMORY_H_ */