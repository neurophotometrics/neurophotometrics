using System;
using System.ComponentModel;
using System.Globalization;

namespace Neurophotometrics.Design
{
    class PowerConverter : SingleConverter
    {
        const double CurrentScale = 2048.0 / 65536;
        const double CurrentOffset = -300.0;

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            value = ToCurrentValue(Convert.ToInt32(value));
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var result = (float)base.ConvertFrom(context, culture, value);
            return ToRawRegisterValue(result);
        }

        static int ToRawRegisterValue(float value) => (ushort)((value - CurrentOffset) / CurrentScale);
        static float ToCurrentValue(int value) => (float)(value * CurrentScale + CurrentOffset);
    }
}
