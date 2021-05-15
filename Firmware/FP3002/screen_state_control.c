#include "screen_state_control.h"
#include "screen.h"
#include "app_ios_and_regs.h"
#include <stdbool.h>

extern AppRegs app_regs;

bool bonsai_is_on = false;

void update_screen_indication(void)
{
	uint8_t image_index;
	image_index  = bonsai_is_on ? _BONSAI_IS_ON_BM : 0;
	image_index |= read_KEY_SWITCH ? _KEY_SWITCH_IS_ON_BM : 0;

	
	/* If switch is ON and internal laser is enabled, maybe laser is ON */
	if (read_KEY_SWITCH && read_EN_INT_LASER)
	{
		/* If timer is working, laser is ON */
		/* If DOUT0 is high, laser is ON */
		if (TCE0_CTRLA != 0 || read_OUT0)
		{
			image_index |= _LASER_IS_ON_BM;
		}
		
		/* If camera's channel C and D are enabled, laser interleave mode is ON */
		if (TCC0_INTCTRLB & 0xF0)
		{
			image_index |= _LASER_IS_ON_BM;
		}
	}
	
	switch (app_regs.REG_STIM_WAVELENGTH)
	{
		case 450:
			image_index |= _WL_450_IS_SELECTED_GM;
			break;
			
		case 635:
			image_index |= _WL_635_IS_SELECTED_GM;
			break;
			
		default:
			image_index |= _WL_NONE_IS_SELECTED_GM;
	}
	
	display_image(image_index);
}