#include "memory.h"

/************************************************************************/
/* GPIOs                                                                */
/************************************************************************/
void init_memory_ios(void)
{	
	// IO0-7 @ PB0-7 as inputs
	io_pin2in(&PORTB, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 1, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 3, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 4, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 5, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 6, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	io_pin2in(&PORTB, 7, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	
	#ifdef PCB_1V0
		// CLE @ PA3 as output
		io_pin2out(&PORTA, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
	#ifdef PCB_1V1
		// CLE @ PK4 as output
		io_pin2out(&PORTK, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif

	// ALE @ PA4 as output
	io_pin2out(&PORTA, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);
	
	#ifdef PCB_1V0
		// WP# @ PA7 as output
		io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
	#ifdef PCB_1V1
		// WP# @ PC5 as output
		io_pin2out(&PORTC, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
	#ifdef PCB_1V0
		// R/B# @ PA0 as INPUT
		io_pin2in(&PORTA, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	#endif
	
	#ifdef PCB_1V1
		// R/B# @ PK7 as INPUT
		io_pin2in(&PORTK, 7, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	#endif
	
	#ifdef PCB_1V0
		// CE# @ PA2 as output
		io_pin2out(&PORTA, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
	#ifdef PCB_1V1
		// CE# @ PK5 as output
		io_pin2out(&PORTK, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif

	// WE# @ PA5 as output
	io_pin2out(&PORTA, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);
	
	#ifdef PCB_1V0
		// RE# @ PA1 as output
		io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
	#ifdef PCB_1V1
		// RE# @ PK6 as output
		io_pin2out(&PORTK, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);
	#endif
	
    set_MEM_WP;     // Removes hardware protection against modification
    set_MEM_CE;     // Un-select the memory
    clr_MEM_CLE;    // Disable Command Latch Enable
    clr_MEM_ALE;    // Disable Address Latch Enable
    set_MEM_WE;     // Set Write Enable
    set_MEM_RE;     // Set Read Enable	
}


/*
 * Return the memory size in Gbits of the available memory.
 */
uint8_t read_memory_size(void)
{
    uint8_t manufacturer_code;
    uint8_t device_identifier;
    uint8_t byte3;
    uint8_t byte4;
    uint8_t byte5;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;

    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_ID);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable

    /* Write Address 1 Cycle */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_ALE;    // Each instruction is 31.25 ns
    clr_MEM_ALE;    // Each instruction is 31.25 ns
    clr_MEM_ALE;    // Each instruction is 31.25 ns

    /* Configure data port to input */
    to_input_MEM_DATA;

    /* Read Maker Code */
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    manufacturer_code = read_MEM_DATA;
    set_MEM_RE;

    /* Read Device Code */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    device_identifier = read_MEM_DATA;
    set_MEM_RE;       

    /* Read Byte 3 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte3 = read_MEM_DATA;
    set_MEM_RE;

     /* Read Byte 4 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte4 = read_MEM_DATA;
    set_MEM_RE;

    /* Read Byte 5 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte5 = read_MEM_DATA;
    set_MEM_RE;

    /* De-select memory */
    set_MEM_CE;
    
    switch (device_identifier)
    {
        case 0xDA: return 2;
        case 0xDC: return 4;
        default: return -1;
    }
}

/*
 * Deletes a memory Block.
 */
uint8_t block_erase (int32_t block_index)
{
    int32_t block_address = block_index * (int32_t)64;
    
    uint8_t row_add_1 = block_address & 0xFF;
    uint8_t row_add_2 = (block_address >> 8) & 0xFF;
    uint8_t row_add_3 = (block_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_BLOCK_ERASE);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0xD0);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 31.25 ns
    clr_MEM_CLE;    // Each instruction is 31.25 ns
    clr_MEM_CLE;    // Each instruction is 31.25 ns
    clr_MEM_CLE;    // Each instruction is 31.25 ns  
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);
    
    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
      
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 31.25 ns
    clr_MEM_CLE;    // Each instruction is 31.25 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    uint8_t byte;
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

/*
 * Writes a Page.
 */
uint8_t program_memory_without_spare (int32_t page_address, uint8_t *page)
{
    uint8_t row_add_1 = page_address & 0xFF;
    uint8_t row_add_2 = (page_address >> 8) & 0xFF;
    uint8_t row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_PROGRAM);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Wait tADL (min. 70 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Program Page */
    for (uint16_t i = 0; i < 2048; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(page[i]);
        set_MEM_WE;
    }    
    
    /* Program empty Header */
    for (uint8_t i = 0; i < 64; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(0);
        set_MEM_WE;
    }
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x10);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);

    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    uint8_t byte;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

void read_memory_without_spare (int32_t page_address, uint8_t *page)
{
    uint8_t row_add_1 = page_address & 0xFF;
    uint8_t row_add_2 = (page_address >> 8) & 0xFF;
    uint8_t row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_READ);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable

    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x30);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read page */
    for (uint16_t i = 0; i < 2048; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;
        page[i] = (unsigned char) (read_MEM_DATA & 0xFF);
        set_MEM_RE;
    }
 
    /* Read spare */
    for (uint8_t i = 0; i < 64; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;
        set_MEM_RE;
    }
    
    /* De-select memory */
    set_MEM_CE;
}