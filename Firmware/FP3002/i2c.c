#include "i2c.h"
#include "i2c_user.h"
#include "cpu.h"


uint8_t slowly_state = 0;

//*****************************************************************************
// I2C0 functions
//*****************************************************************************
#ifdef I2C0_CLK
static void i2c0_start(void);
static void i2c0_stop(void);

void i2c0_init(void)
{
	io_pin2out(&I2C0_PORT, I2C0_SDA, OUT_IO_WIREDAND, IN_EN_IO_EN);	// SDA0
	io_pin2out(&I2C0_PORT, I2C0_SCL, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// SCL0
	
	clear_SCL0;	_delay_us(10);
	set_SDA0;	_delay_us(10);
	
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;

	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	toggle_SCL0; tCLK_I2C0; toggle_SCL0; tCLK_I2C0;
	
	i2c0_stop();
}

static void i2c0_start(void)
{
	tBUF;
	clear_SDA0;	
	tHDSTA;
	clear_SCL0;	
	tCLK_I2C0;
}

static void i2c0_stop(void)
{
	clear_SDA0_and_SCL0;
	tCLK_I2C0;
	set_SCL0;	
	tSUSTO;	
	set_SDA0;
}

bool i2c0_wReg(i2c_dev_t* dev)
{
	i2c0_start();
	
	uint8_t add = (dev->add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL0;	if (add & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;								   clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}

	clear_SCL0;	if (dev->reg & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;

	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}

	clear_SCL0;	if (dev->reg_val & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg_val & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}
		
	i2c0_stop();
	return true;
}

bool i2c0_wReg_slowly(i2c_dev_t* dev)
{
	switch (slowly_state)
	{
		case 0:
			i2c0_start();
			break;
	
	
		// Each cycle
		//   __
		//__|
		case 1: clear_SCL0;	if ((dev->add << 1) & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 2: clear_SCL0;	if ((dev->add << 1) & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 3: clear_SCL0;	if ((dev->add << 1) & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 4: clear_SCL0;	if ((dev->add << 1) & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 5: clear_SCL0;	if ((dev->add << 1) & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 6: clear_SCL0;	if ((dev->add << 1) & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 7: clear_SCL0;	if ((dev->add << 1) & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 8: clear_SCL0;											   clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
	
		case 9:
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			if(read_SDA0)
			{
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			break;

		case 10: clear_SCL0;	if (dev->reg & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 11: clear_SCL0;	if (dev->reg & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 12: clear_SCL0;	if (dev->reg & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 13: clear_SCL0;	if (dev->reg & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 14: clear_SCL0;	if (dev->reg & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 15: clear_SCL0;	if (dev->reg & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 16: clear_SCL0;	if (dev->reg & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 17: clear_SCL0;	if (dev->reg & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;

		case 18:
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			if(read_SDA0)
			{
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			break;

		case 19: clear_SCL0;	if (dev->reg_val & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 20: clear_SCL0;	if (dev->reg_val & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 21: clear_SCL0;	if (dev->reg_val & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 22: clear_SCL0;	if (dev->reg_val & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 23: clear_SCL0;	if (dev->reg_val & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 24: clear_SCL0;	if (dev->reg_val & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 25: clear_SCL0;	if (dev->reg_val & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
		case 26: clear_SCL0;	if (dev->reg_val & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
		case 27:
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			
			i2c0_stop();
			slowly_state = 0;
			return true;
	}
	
	slowly_state++;
	return false;
}

bool i2c0_rReg(i2c_dev_t* dev, uint8_t bytes2read)
{
	if (bytes2read > MAX_I2C_DATA)
		return false;
	i2c0_start();
	
	uint8_t add = (dev->add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL0;	if (add & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;														 clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}

	clear_SCL0;	if (dev->reg & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (dev->reg & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}
	
	/* Repeat Start */
	clear_SCL0;
	set_SDA0;
	tCLK_I2C0;
	set_SCL0;
	tSUSTA;
	clear_SDA0;
	tHDSTA;
	
	clear_SCL0;	if (add & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;	if (add & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0;
	clear_SCL0;									 set_SDA0;							tCLK_I2C0; set_SCL0; tCLK_I2C0;
	
	clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
	if(read_SDA0) {
		i2c0_stop();
		return false;
	}
	
	uint8_t byte;
	
	for (uint8_t i = 0; i < bytes2read; i++) {
		byte = 0;
		set_SDA0;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x80;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x40;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x20;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x10;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x08;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x04;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x02;
		clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) byte |= 0x01;
		
		clear_SCL0;	if ( i == bytes2read-1) set_SDA0; else clear_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
		clear_SCL0;
		dev->data[i] = byte;
		if (i==0) dev->reg_val = byte;
	}

	i2c0_stop();
	return true;
}

bool i2c0_rReg_slowly(i2c_dev_t* dev, uint8_t bytes2read)
{
	switch (slowly_state)
	{
		case 0:
			if (bytes2read > 2)
				return true;
		
			i2c0_start();
			break;
	
		// Each cycle
		//   __
		//__|
		case 1: clear_SCL0;	if ((dev->add << 1) & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 2: clear_SCL0;	if ((dev->add << 1) & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 3: clear_SCL0;	if ((dev->add << 1) & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 4: clear_SCL0;	if ((dev->add << 1) & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 5: clear_SCL0;	if ((dev->add << 1) & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 6: clear_SCL0;	if ((dev->add << 1) & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 7: clear_SCL0;	if ((dev->add << 1) & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 8: clear_SCL0;											   clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
	
		case 9:
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			if(read_SDA0)
			{
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			break;

		case 10: clear_SCL0;	if (dev->reg & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 11: clear_SCL0;	if (dev->reg & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 12: clear_SCL0;	if (dev->reg & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 13: clear_SCL0;	if (dev->reg & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 14: clear_SCL0;	if (dev->reg & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 15: clear_SCL0;	if (dev->reg & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 16: clear_SCL0;	if (dev->reg & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 17: clear_SCL0;	if (dev->reg & 0x01) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		
		case 18:
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			if(read_SDA0)
			{
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			break;
		
		case 19:	
			/* Repeat Start */
			clear_SCL0;
			set_SDA0;
			tCLK_I2C0;
			set_SCL0;
			tSUSTA;
			clear_SDA0;
			tHDSTA;
			break;
	
		case 20: clear_SCL0;	if ((dev->add << 1) & 0x80) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 21: clear_SCL0;	if ((dev->add << 1) & 0x40) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 22: clear_SCL0;	if ((dev->add << 1) & 0x20) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 23: clear_SCL0;	if ((dev->add << 1) & 0x10) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 24: clear_SCL0;	if ((dev->add << 1) & 0x08) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 25: clear_SCL0;	if ((dev->add << 1) & 0x04) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 26: clear_SCL0;	if ((dev->add << 1) & 0x02) set_SDA0; else clear_SDA0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
		case 27: clear_SCL0;								set_SDA0;					tCLK_I2C0; set_SCL0; tCLK_I2C0; break;
	
		case 28: 
			clear_SCL0;	set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			if(read_SDA0)
			{
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			break;
		
		case 29:
			dev->reg_val = 0;
			dev->data[0] = 0;		
			set_SDA0;
			break;
		
		case 30: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x80; dev->data[0] |= 0x80;} break;
		case 31: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x40; dev->data[0] |= 0x40;} break;
		case 32: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x20; dev->data[0] |= 0x20;} break;
		case 33: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x10; dev->data[0] |= 0x10;} break;
		case 34: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x08; dev->data[0] |= 0x08;} break;
		case 35: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x04; dev->data[0] |= 0x04;} break;
		case 36: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x02; dev->data[0] |= 0x02;} break;
		case 37: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->reg_val |= 0x01; dev->data[0] |= 0x01;} break;
		
		case 38:
			clear_SCL0;
			if (bytes2read == 1)
			{
				set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
				clear_SCL0;
				
				i2c0_stop();
				slowly_state = 0;
				return true;
			}
			else
			{
				clear_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
				clear_SCL0;
			}
			break;
		
		case 39: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x80;} break;
		case 40: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x40;} break;
		case 41: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x20;} break;
		case 42: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x10;} break;
		case 43: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x08;} break;
		case 44: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x04;} break;
		case 45: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x02;} break;
		case 46: clear_SCL0;	tCLK_I2C0; set_SCL0; tCLK_I2C0; if (read_SDA0) {dev->data[1] |= 0x01;} break;		
		
		case 47:
			clear_SCL0;
			set_SDA0; tCLK_I2C0; set_SCL0; tCLK_I2C0;
			clear_SCL0;
			
			i2c0_stop();
			slowly_state = 0;
			return true;
	}
	
	slowly_state++;
	return false;
}
#endif

//*****************************************************************************
// I2C1 functions
//*****************************************************************************
#ifdef I2C1_CLK
static void i2c1_start(void);
static void i2c1_stop(void);

void i2c1_init(void)
{
	io_pin2out(I2C1_PORT, I2C1_SDA, OUT_IO_WIREDAND, IN_EN_IO_EN);	// SDA1
	io_pin2out(I2C1_PORT, I2C1_SCL, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// SCL1
	
	clear_SCL1;	_delay_us(10);
	set_SDA1;	_delay_us(10);
	
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	toggle_SCL1; tCLK_I2C1; toggle_SCL1; tCLK_I2C1;
	
	i2c1_stop();
}

static void i2c1_start(void)
{
	tBUF;
	clear_SDA1;	
	tHDSTA;
	clear_SCL1;	
	tCLK_I2C1;
}

static void i2c1_stop(void)
{
	clear_SDA1_and_SCL1;
	tCLK_I2C1;
	set_SCL1;	
	tSUSTO;	
	set_SDA1;
}

bool i2c1_wReg(i2c_dev_t* dev)
{
	i2c1_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL1;
	if (add & 0x80)
		set_SDA1;
	else
		clear_SDA1;
	tCLK_I2C1;
	set_SCL1;
	tCLK_I2C1;
	clear_SCL1;	if (add & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;														 clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	
	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}

	clear_SCL1;	if (dev->reg_add & 0x80) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x01) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;

	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}

	clear_SCL1;	if (dev->reg_value & 0x80) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_value & 0x01) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	
	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}
		
	i2c1_stop();
	return true;
}

bool i2c1_rReg(i2c_dev_t* dev, uint8_t bytes2read)
{
	if (bytes2read > MAX_I2C_DATA)
		return false;
	i2c1_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL1;	if (add & 0x80) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;														 clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	
	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}

	clear_SCL1;	if (dev->reg_add & 0x80) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (dev->reg_add & 0x01) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	
	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}
	
	/* Repeat Start */
	clear_SCL1;
	set_SDA1;
	tCLK_I2C1;
	set_SCL1;
	tSUSTA;
	clear_SDA1;
	tHDSTA;
	
	clear_SCL1;	if (add & 0x80) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x40) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x20) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x10) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x08) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x04) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;	if (add & 0x02) set_SDA1; else clear_SDA1;	tCLK_I2C1; set_SCL1; tCLK_I2C1;
	clear_SCL1;									 set_SDA1;							tCLK_I2C1; set_SCL1; tCLK_I2C1;
	
	clear_SCL1;	set_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
	if(read_SDA1) {
		i2c1_stop();
		return false;
	}
	
	uint8_t byte;
	
	for (uint8_t i = 0; i < bytes2read; i++) {		
		byte = 0;
		set_SDA1;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x80;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x40;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x20;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x10;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x08;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x04;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x02;
		clear_SCL1;	tCLK_I2C1; set_SCL1; tCLK_I2C1; if (read_SDA1) byte |= 0x01;
		
		clear_SCL1;	if ( i == bytes2read-1) set_SDA1; else clear_SDA1; tCLK_I2C1; set_SCL1; tCLK_I2C1;
		clear_SCL1;
		dev->data[i] = byte;
		if (i==0) dev->reg_value = byte;
	}

	i2c1_stop();
	return true;
}
#endif


//*****************************************************************************
// I2C2 functions
//*****************************************************************************
#ifdef I2C2_CLK
static void i2c2_start(void);
static void i2c2_stop(void);

void i2c2_init(void)
{
	io_pin2out(I2C2_PORT, I2C2_SDA, OUT_IO_WIREDAND, IN_EN_IO_EN);	// SDA2
	io_pin2out(I2C2_PORT, I2C2_SCL, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// SCL2
		
	clear_SCL2;	_delay_us(10);
	set_SDA2;	_delay_us(10);
	
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	toggle_SCL2; tCLK_I2C2; toggle_SCL2; tCLK_I2C2;
	
	i2c2_stop();
}

static void i2c2_start(void)
{
	tBUF;
	clear_SDA2;	
	tHDSTA;
	clear_SCL2;	
	tCLK_I2C2;
}

static void i2c2_stop(void)
{
	clear_SDA2_and_SCL2;
	tCLK_I2C2;
	set_SCL2;	
	tSUSTO;	
	set_SDA2;
}

bool i2c2_wReg(i2c_dev_t* dev)
{
	i2c2_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL2;
	if (add & 0x80)
		set_SDA2;
	else
		clear_SDA2;
	tCLK_I2C2;
	set_SCL2;
	tCLK_I2C2;
	clear_SCL2;	if (add & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;														 clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	
	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}

	clear_SCL2;	if (dev->reg_add & 0x80) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x01) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;

	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}

	clear_SCL2;	if (dev->reg_value & 0x80) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_value & 0x01) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	
	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}
		
	i2c2_stop();
	return true;
}

bool i2c2_rReg(i2c_dev_t* dev, uint8_t bytes2read)
{
	if (bytes2read > MAX_I2C_DATA)
		return false;
	i2c2_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL2;	if (add & 0x80) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;														 clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	
	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}

	clear_SCL2;	if (dev->reg_add & 0x80) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (dev->reg_add & 0x01) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	
	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}
	
	/* Repeat Start */
	clear_SCL2;
	set_SDA2;
	tCLK_I2C2;
	set_SCL2;
	tSUSTA;
	clear_SDA2;
	tHDSTA;
	
	clear_SCL2;	if (add & 0x80) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x40) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x20) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x10) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x08) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x04) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;	if (add & 0x02) set_SDA2; else clear_SDA2;	tCLK_I2C2; set_SCL2; tCLK_I2C2;
	clear_SCL2;									 set_SDA2;							tCLK_I2C2; set_SCL2; tCLK_I2C2;
	
	clear_SCL2;	set_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
	if(read_SDA2) {
		i2c2_stop();
		return false;
	}
	
	uint8_t byte;
	
	for (uint8_t i = 0; i < bytes2read; i++) {		
		byte = 0;
		set_SDA2;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x80;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x40;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x20;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x10;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x08;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x04;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x02;
		clear_SCL2;	tCLK_I2C2; set_SCL2; tCLK_I2C2; if (read_SDA2) byte |= 0x01;
		
		clear_SCL2;	if ( i == bytes2read-1) set_SDA2; else clear_SDA2; tCLK_I2C2; set_SCL2; tCLK_I2C2;
		clear_SCL2;
		dev->data[i] = byte;
		if (i==0) dev->reg_value = byte;
	}

	i2c2_stop();
	return true;
}
#endif


//*****************************************************************************
// I2C3 functions
//*****************************************************************************
#ifdef I2C3_CLK
static void i2c3_start(void);
static void i2c3_stop(void);

void i2c3_init(void)
{
	io_pin2out(I2C3_PORT, I2C3_SDA, OUT_IO_WIREDAND, IN_EN_IO_EN);	// SDA3
	io_pin2out(I2C3_PORT, I2C3_SCL, OUT_IO_DIGITAL, IN_EN_IO_DIS);	// SCL3
	
	clear_SCL3;	_delay_us(10);
	set_SDA3;	_delay_us(10);
	
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	toggle_SCL3; tCLK_I2C3; toggle_SCL3; tCLK_I2C3;
	
	i2c3_stop();
}

static void i2c3_start(void)
{
	tBUF;
	clear_SDA3;	
	tHDSTA;
	clear_SCL3;	
	tCLK_I2C3;
}

static void i2c3_stop(void)
{
	clear_SDA3_and_SCL3;
	tCLK_I2C3;
	set_SCL3;	
	tSUSTO;	
	set_SDA3;
}

bool i2c3_wReg(i2c_dev_t* dev)
{
	i2c3_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL3;
	if (add & 0x80)
		set_SDA3;
	else
		clear_SDA3;
	tCLK_I2C3;
	set_SCL3;
	tCLK_I2C3;
	clear_SCL3;	if (add & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;														 clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	
	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}

	clear_SCL3;	if (dev->reg_add & 0x80) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x01) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;

	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}

	clear_SCL3;	if (dev->reg_value & 0x80) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_value & 0x01) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	
	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}
		
	i2c3_stop();
	return true;
}

bool i2c3_rReg(i2c_dev_t* dev, uint8_t bytes2read)
{
	if (bytes2read > MAX_I2C_DATA)
		return false;
	i2c3_start();
	
	uint8_t add = (dev->dev_add << 1);
	
	// Each cycle
	//   __
	//__|
	clear_SCL3;	if (add & 0x80) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;														 clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	
	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}

	clear_SCL3;	if (dev->reg_add & 0x80) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (dev->reg_add & 0x01) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	
	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}
	
	/* Repeat Start */
	clear_SCL3;
	set_SDA3;
	tCLK_I2C3;
	set_SCL3;
	tSUSTA;
	clear_SDA3;
	tHDSTA;
	
	clear_SCL3;	if (add & 0x80) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x40) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x20) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x10) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x08) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x04) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;	if (add & 0x02) set_SDA3; else clear_SDA3;	tCLK_I2C3; set_SCL3; tCLK_I2C3;
	clear_SCL3;									 set_SDA3;							tCLK_I2C3; set_SCL3; tCLK_I2C3;
	
	clear_SCL3;	set_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
	if(read_SDA3) {
		i2c3_stop();
		return false;
	}
	
	uint8_t byte;
	
	for (uint8_t i = 0; i < bytes2read; i++) {		
		byte = 0;
		set_SDA3;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x80;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x40;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x20;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x10;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x08;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x04;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x02;
		clear_SCL3;	tCLK_I2C3; set_SCL3; tCLK_I2C3; if (read_SDA3) byte |= 0x01;
		
		clear_SCL3;	if ( i == bytes2read-1) set_SDA3; else clear_SDA3; tCLK_I2C3; set_SCL3; tCLK_I2C3;
		clear_SCL3;
		dev->data[i] = byte;
		if (i==0) dev->reg_value = byte;
	}

	i2c3_stop();
	return true;
}
#endif