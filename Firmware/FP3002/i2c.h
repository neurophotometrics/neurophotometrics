#ifndef _I2C_H_
#define _I2C_H_

#include "i2c_user.h"


//*****************************************************************************
// If not defined, define bool, true and false
//*****************************************************************************
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif

//*****************************************************************************
// Define I2C structure for the devices
//*****************************************************************************
#ifndef MAX_I2C_DATA
	#define MAX_I2C_DATA 32
#endif

typedef struct {
	unsigned char add;
	unsigned char reg;
	unsigned char reg_val;
	unsigned char data[MAX_I2C_DATA];
} i2c_dev_t;


//*****************************************************************************
// Check if any of the four I2C buses is used
//*****************************************************************************
#ifdef I2C0_CLK
	#ifndef I2C0_PORT
		#error "Bus I2C0 is enabled but I2C0_PORT is not defined"
	#endif
	#ifndef I2C0_SDA
		#error "Bus I2C0 is enabled but I2C0_SDA is not defined"
	#endif
	#ifndef I2C0_SCL
		#error "Bus I2C0 is enabled but I2C0_SCL is not defined"
	#endif
	#ifndef F_CPU
		#error "Bus I2C0 is enabled but F_CPU is not defined"
	#endif
	#if   F_CPU == 32000000
	#elif F_CPU == 16000000
	#elif F_CPU ==  8000000
	#elif F_CPU ==  4000000
	#elif F_CPU ==  2000000
	#elif F_CPU ==  1000000
	#else
	#error "I2C0: Unsupported F_CPU value for system clock"
	#endif
#endif

#ifdef I2C1_CLK
	#ifndef I2C1_PORT
		#error "Bus I2C1 is enabled but I2C1_PORT is not defined"
	#endif
	#ifndef I2C1_SDA
		#error "Bus I2C1 is enabled but I2C1_SDA is not defined"
	#endif
	#ifndef I2C1_SCL
		#error "Bus I2C1 is enabled but I2C1_SCL is not defined"
	#endif
	#ifndef F_CPU
		#error "Bus I2C1 is enabled but F_CPU is not defined"
	#endif
	#if   F_CPU == 32000000
	#elif F_CPU == 16000000
	#elif F_CPU ==  8000000
	#elif F_CPU ==  4000000
	#elif F_CPU ==  2000000
	#elif F_CPU ==  1000000
	#else
	#error "I2C1: Unsupported F_CPU value for system clock"
	#endif
#endif

#ifdef I2C2_CLK
	#ifndef I2C2_PORT
		#error "Bus I2C2 is enabled but I2C2_PORT is not defined"
	#endif
	#ifndef I2C2_SDA
		#error "Bus I2C2 is enabled but I2C2_SDA is not defined"
	#endif
	#ifndef I2C2_SCL
		#error "Bus I2C2 is enabled but I2C2_SCL is not defined"
	#endif
	#ifndef F_CPU
		#error "Bus I2C2 is enabled but F_CPU is not defined"
	#endif
	#if   F_CPU == 32000000
	#elif F_CPU == 16000000
	#elif F_CPU ==  8000000
	#elif F_CPU ==  4000000
	#elif F_CPU ==  2000000
	#elif F_CPU ==  1000000
	#else
	#error "I2C2: Unsupported F_CPU value for system clock"
	#endif
#endif

#ifdef I2C3_CLK
	#ifndef I2C3_PORT
		#error "Bus I2C3 is enabled but I2C3_PORT is not defined"
	#endif
	#ifndef I2C3_SDA
		#error "Bus I2C3 is enabled but I2C3_SDA is not defined"
	#endif
	#ifndef I2C3_SCL
		#error "Bus I2C3 is enabled but I2C3_SCL is not defined"
	#endif
	#ifndef F_CPU
		#error "Bus I2C3 is enabled but F_CPU is not defined"
	#endif
	#if   F_CPU == 32000000
	#elif F_CPU == 16000000
	#elif F_CPU ==  8000000
	#elif F_CPU ==  4000000
	#elif F_CPU ==  2000000
	#elif F_CPU ==  1000000
	#else
	#error "I2C3: Unsupported F_CPU value for system clock"
	#endif
#endif



//*****************************************************************************
// Define SCL delays
//*****************************************************************************
#ifdef I2C0_CLK
	#define tCLK_I2C0 I2C0_CLK
#endif
#ifdef I2C1_CLK
	#define tCLK_I2C1 I2C1_CLK
#endif
#ifdef I2C2_CLK
	#define tCLK_I2C2 I2C2_CLK
#endif
#ifdef I2C3_CLK
	#define tCLK_I2C3 I2C3_CLK
#endif

//*****************************************************************************
// Define IOs interface
//*****************************************************************************
#ifdef I2C0_CLK
	#define set_SDA0 set_io(I2C0_PORT, I2C0_SDA)
	#define clear_SDA0 clear_io(I2C0_PORT, I2C0_SDA)
	#define toggle_SDA0 toggle_io(I2C0_PORT, I2C0_SDA)
	#define set_SCL0 set_io(I2C0_PORT, I2C0_SCL)
	#define clear_SCL0 clear_io(I2C0_PORT, I2C0_SCL)
	#define toggle_SCL0 toggle_io(I2C0_PORT, I2C0_SCL)
	#define read_SDA0 read_io(I2C0_PORT, I2C0_SDA)
	#define set_SDA0_and_SCL0 set_io_mask(I2C0_PORT, (1<<I2C0_SDA) | (1<<I2C0_SCL))
	#define clear_SDA0_and_SCL0 clear_io_mask(I2C0_PORT, (1<<I2C0_SDA) | (1<<I2C0_SCL))
#endif

#ifdef I2C1_CLK
	#define set_SDA1 set_io(I2C1_PORT, I2C1_SDA)
	#define clear_SDA1 clear_io(I2C1_PORT, I2C1_SDA)
	#define toggle_SDA1 toggle_io(I2C1_PORT, I2C1_SDA)
	#define set_SCL1 set_io(I2C1_PORT, I2C1_SCL)
	#define clear_SCL1 clear_io(I2C1_PORT, I2C1_SCL)
	#define toggle_SCL1 toggle_io(I2C1_PORT, I2C1_SCL)
	#define read_SDA1 read_io(I2C1_PORT, I2C1_SDA)
	#define set_SDA1_and_SCL1 set_io_mask(I2C1_PORT, (1<<I2C1_SDA) | (1<<I2C1_SCL))
	#define clear_SDA1_and_SCL1 clear_io_mask(I2C1_PORT, (1<<I2C1_SDA) | (1<<I2C1_SCL))
#endif

#ifdef I2C2_CLK
	#define set_SDA2 set_io(I2C2_PORT, I2C2_SDA)
	#define clear_SDA2 clear_io(I2C2_PORT, I2C2_SDA)
	#define toggle_SDA2 toggle_io(I2C2_PORT, I2C2_SDA)
	#define set_SCL2 set_io(I2C2_PORT, I2C2_SCL)
	#define clear_SCL2 clear_io(I2C2_PORT, I2C2_SCL)
	#define toggle_SCL2 toggle_io(I2C2_PORT, I2C2_SCL)
	#define read_SDA2 read_io(I2C2_PORT, I2C2_SDA)
	#define set_SDA2_and_SCL2 set_io_mask(I2C2_PORT, (1<<I2C2_SDA) | (1<<I2C2_SCL))
	#define clear_SDA2_and_SCL2 clear_io_mask(I2C2_PORT, (1<<I2C2_SDA) | (1<<I2C2_SCL))
#endif

#ifdef I2C3_CLK
	#define set_SDA3 set_io(I2C3_PORT, I2C3_SDA)
	#define clear_SDA3 clear_io(I2C3_PORT, I2C3_SDA)
	#define toggle_SDA3 toggle_io(I2C3_PORT, I2C3_SDA)
	#define set_SCL3 set_io(I2C3_PORT, I2C3_SCL)
	#define clear_SCL3 clear_io(I2C3_PORT, I2C3_SCL)
	#define toggle_SCL3 toggle_io(I2C3_PORT, I2C3_SCL)
	#define read_SDA3 read_io(I2C3_PORT, I2C3_SDA)
	#define set_SDA3_and_SCL3 set_io_mask(I2C3_PORT, (1<<I2C3_SDA) | (1<<I2C3_SCL))
	#define clear_SDA3_and_SCL3 clear_io_mask(I2C3_PORT, (1<<I2C3_SDA) | (1<<I2C3_SCL))
#endif


//*****************************************************************************
// Prototypes
//*****************************************************************************
#ifdef I2C0_CLK
	void i2c0_init(void);
	bool i2c0_wReg(i2c_dev_t* dev);
	bool i2c0_wReg_slowly(i2c_dev_t* dev);
	bool i2c0_rReg(i2c_dev_t* dev, uint8_t bytes2read);
	bool i2c0_rReg_slowly(i2c_dev_t* dev, uint8_t bytes2read);
#endif
#ifdef I2C1_CLK
	void i2c1_init(void);
	bool i2c1_wReg(i2c_dev_t* dev);
	bool i2c1_rReg(i2c_dev_t* dev, uint8_t bytes2read);
#endif
#ifdef I2C2_CLK
	void i2c2_init(void);
	bool i2c2_wReg(i2c_dev_t* dev);
	bool i2c2_rReg(i2c_dev_t* dev, uint8_t bytes2read);
#endif
#ifdef I2C3_CLK
	void i2c3_init(void);
	bool i2c3_wReg(i2c_dev_t* dev);
	bool i2c3_rReg(i2c_dev_t* dev, uint8_t bytes2read);
#endif

#endif
