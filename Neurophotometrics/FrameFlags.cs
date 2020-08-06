using System;

namespace Neurophotometrics
{
    [Flags]
    public enum FrameFlags : ushort
    {
        L410 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        Output0 = 1 << 3,
        Output1 = 1 << 4,
        Stimulation = 1 << 5,
        CameraGpio0 = 1 << 6,
        CameraGpio1 = 1 << 7,
        Input0 = 1 << 8,
        Input1 = 1 << 9
    }
}
