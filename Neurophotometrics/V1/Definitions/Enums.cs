using System;
using System.ComponentModel;

namespace Neurophotometrics.V1.Definitions
{
    public enum RegionChannel : byte
    {
        Unspecified,
        Red,
        Green
    }

    [Flags]
    public enum FrameFlags : byte
    {
        None = 0,
        L415 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        All = L415 | L470 | L560
    }

    public enum TriggerMode
    {
        [Description("Constant")]
        Constant,

        [Description("Trigger 1")]
        Trigger1,

        [Description("Trigger 2")]
        Trigger2,

        [Description("Trigger 3")]
        Trigger3
    }
}