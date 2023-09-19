using Neurophotometrics.Design.Properties;

using System;
using System.Text.RegularExpressions;

namespace Neurophotometrics.Design.V2.Converters
{
    public static class StimOnConverter
    {
        public const ushort Min = 1;
        public const ushort Max = 60000;

        public static ushort ConvertPulseWidthStringToRegisterValue(string pulseWidthString)
        {
            // Remove any Non-Numeric characters
            pulseWidthString = Regex.Replace(pulseWidthString, "[^.0-9]", "");

            // Find user-readable value
            var success = int.TryParse(pulseWidthString, out var pulseWidth);

            if (!success)
                throw new ArgumentException(Resources.MsgBox_Warning_InvalidPropertyValue.Replace("{0}", pulseWidthString));

            return (ushort)Math.Min(Max, Math.Max(Min, pulseWidth));
        }

        public static string ConvertRegisterValueToPulseWidthString(ushort registerValue)
        {
            return registerValue.ToString() + " ms";
        }
    }
}