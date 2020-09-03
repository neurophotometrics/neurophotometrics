using System;
using System.ComponentModel;
using System.Linq;

namespace Neurophotometrics
{
    class FrameFlagsConverter : EnumConverter
    {
        public FrameFlagsConverter()
            : base(typeof(FrameFlags))
        {
            var values = ((FrameFlags[])Enum.GetValues(typeof(FrameFlags))).Distinct().ToArray();
            Values = new StandardValuesCollection(values);
        }
    }
}
