using System;
using System.ComponentModel;

namespace Neurophotometrics
{
    [Flags]
    [TypeConverter(typeof(FrameFlagsConverter))]
    public enum FrameFlags : ushort
    {
        None = 0,
        L410 = 1 << 0,
        L415 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        Output1 = 1 << 3,
        Output0 = 1 << 4,
        Stimulation = 1 << 5,
        Line2 = 1 << 6,
        Line3 = 1 << 7,
        Input1 = 1 << 8,
        Input0 = 1 << 9
    }
}
