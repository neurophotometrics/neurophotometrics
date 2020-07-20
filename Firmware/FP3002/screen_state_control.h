#ifndef _SCREEN_STATE_CONTROL_
#define _SCREEN_STATE_CONTROL_
#include <avr/io.h>

#define _BONSAI_IS_ON_BM (1<<0)
#define _KEY_SWITCH_IS_ON_BM (1<<1)
#define _LASER_IS_ON_BM (1<<2)
#define _WL_NONE_IS_SELECTED_GM (0<<3)
#define _WL_450_IS_SELECTED_GM (1<<3)
#define _WL_635_IS_SELECTED_GM (2<<3)

void update_screen_indication(void);

#endif /* #ifndef _SCREEN_STATE_CONTROL_ */