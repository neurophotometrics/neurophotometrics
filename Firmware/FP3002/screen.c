#include "screen.h"
#include "uart0.h"
#include "app_ios_and_regs.h"

extern AppRegs app_regs;

bool screen_is_connected = true;

void init_screen_serial(void)
{
	uart0_init(0, 2, false);
	uart0_enable();
}


void screen_wakeup_now(void)
{	
	if (screen_is_connected)
		uart0_xmit_now_byte(CMD_MASK_FUNC | CMD_FUNC_SCREEN_WAKEUP);
}

void screen_clean(void)
{
	if (screen_is_connected)
	{
		uint8_t cmd = CMD_MASK_FUNC | CMD_FUNC_SCREEN_CLEAN;
		uart0_xmit(&cmd, 1);
	}
}
void screen_clean_now(void)
{
	if (screen_is_connected)
		uart0_xmit_now_byte(CMD_MASK_FUNC | CMD_FUNC_SCREEN_CLEAN);
}

void screen_send_to_bootloader(void)
{
	if (screen_is_connected)
		uart0_xmit_now_byte(CMD_MASK_FUNC | CMD_FUNC_JUMP_TO_BOOTLOADER);	
}


/* Range = [0:15] */

void screen_set_bright(uint8_t bright)
{
	if (screen_is_connected)
	{
		bright = (bright > 15) ? CMD_MASK_SCREEN_BRIGHT_MASK | 15 : CMD_MASK_SCREEN_BRIGHT_MASK | bright;
		uart0_xmit(&bright, 1);
	}
}
void screen_set_bright_now(uint8_t bright)
{
	if (screen_is_connected)
	{
		bright = (bright > 15) ? CMD_MASK_SCREEN_BRIGHT_MASK | 15 : CMD_MASK_SCREEN_BRIGHT_MASK | bright;
		uart0_xmit_now_byte(bright);
	}
}

void screen_get_versions(void)
{
	if (screen_is_connected)
	{
		uint8_t cmd = CMD_MASK_VERSION_MASK | CMD_FUNC_ALL_REQUEST;
		uart0_xmit(&cmd, 1);
	}
}

void display_image(uint8_t image_index)
{
	if (screen_is_connected)
	{
		uint8_t cmd = CMD_BIT_MASK_DISPLAY_INDEX | (image_index & CMD_MASK_IMAGE_INDEX);
		uart0_xmit(&cmd, 1);
	}
}
void display_image_now_byte(uint8_t image_index)
{
	if (screen_is_connected)
	{		
		uint8_t cmd = CMD_BIT_MASK_DISPLAY_INDEX | (image_index & CMD_MASK_IMAGE_INDEX);
		uart0_xmit_now_byte(cmd);
	}
}


void uart0_rcv_byte_callback(uint8_t byte)
{
	uint8_t reply_mask = byte & 0xE0;
	
	switch(reply_mask)
	{
		case CMD_REPLY_FW_H_MASK: app_regs.REG_SCREEN_FW_VERSION_H = byte & 0x1F; break;
		case CMD_REPLY_FW_L_MASK: app_regs.REG_SCREEN_FW_VERSION_L = byte & 0x1F; break;
		case CMD_REPLY_HW_H_MASK: app_regs.REG_SCREEN_HW_VERSION_H = byte & 0x1F; break;
		case CMD_REPLY_HW_L_MASK: app_regs.REG_SCREEN_HW_VERSION_L = byte & 0x1F; break;		
		case CMD_REPLY_ASS_MASK: app_regs.REG_SCREEN_ASSEMBLY_VERSION = byte & 0x1F; break;
	}
}