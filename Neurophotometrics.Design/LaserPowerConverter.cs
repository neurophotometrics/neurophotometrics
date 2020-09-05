using System;
using System.ComponentModel;
using System.Globalization;

namespace Neurophotometrics.Design
{
    class LaserPowerConverter : SingleConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            value = ToPercentValue(Convert.ToInt32(value));
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (float)base.ConvertFrom(context, culture, value);
            return ToRawRegisterValue(result);
        }

        static int ToRawRegisterValue(float value) => (ushort)(value * 0.01f * ushort.MaxValue);

        static float ToPercentValue(int value) => (float)Math.Round(100f * value / ushort.MaxValue, 2);
    }
}
