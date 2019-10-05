using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics
{
    [Flags]
    public enum TriggerEvents : byte
    {
        L410 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        LExtra = 1 << 3,
        Output0 = 1 << 4,
        Output1 = 1 << 5,
        Stimulation = 1 << 6,
        Input = 1 << 7
    }
}
