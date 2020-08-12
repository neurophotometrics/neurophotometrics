using System;
using System.ComponentModel;
using System.Globalization;

namespace Neurophotometrics.Design
{
    class PowerConverter : SingleConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            value = ToPercentPower(Convert.ToInt32(value));
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (float)base.ConvertFrom(context, culture, value);
            return ToRawPotPower(result);
        }

        static int ToRawPotPower(float value) => (ushort)(value * 0.01f * ushort.MaxValue);
        static float ToPercentPower(int value) => (float)Math.Round(100f * value / ushort.MaxValue, 2);
    }
}
