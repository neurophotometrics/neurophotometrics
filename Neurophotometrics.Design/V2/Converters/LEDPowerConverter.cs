using Neurophotometrics.Design.Properties;

using System;
using System.Text.RegularExpressions;

namespace Neurophotometrics.Design.V2.Converters
{
    public static class LedPowerConverter
    {
        public const ushort Min = 9600;
        public const ushort Max = 35200;
        public const ushort Default = 0;
        public const double PDPercent = 12.5;

        public static ushort GetRegisterValueAtPDPercent()
        {
            return ConvertPercentToRegisterValue(PDPercent);
        }

        public static ushort ConvertPercentStringToRegisterValue(string percentString)
        {
            var percent = ConvertPercentStringToPercent(percentString);
            var registerValue = ConvertPercentToRegisterValue(percent);

            return registerValue;
        }

        public static double ConvertPercentStringToPercent(string percentString)
        {
            // Remove any Non-Numeric characters
            percentString = Regex.Replace(percentString, "[^.0-9]", "");

            // Find user-readable value
            var success = double.TryParse(percentString, out var percent);

            if (!success)
                throw new ArgumentException(Resources.MsgBox_Warning_InvalidPropertyValue.Replace("{0}", percentString));

            return percent;
        }

        public static string ConvertRegisterValueToPercentString(ushort registerValue, int decimals)
        {
            var percent = ConvertRegisterValueToPercent(registerValue);
            var percentString = ConvertPercentToPercentString(percent, decimals);
            return percentString;
        }

        public static double ConvertRegisterValueToPercent(ushort registerValue)
        {
            registerValue = Math.Min(Max, Math.Max(Min, registerValue));
            var percent = 100.0 * (registerValue - Min) / (Max - Min);
            return percent;
        }

        public static string ConvertPercentToPercentString(double percent, int decimals)
        {
            return string.Format($"{{0:F{decimals}}}", percent) + "%";
        }

        public static ushort ConvertPercentToRegisterValue(double percent)
        {
            if (percent <= 0) return Default;

            var registerValue = (ushort)(percent * 0.01 * (Max - Min) + Min);
            return Math.Min(Max, Math.Max(Min, registerValue));
        }

        internal static ushort ConvertStringToRegisterValue(string regValueString)
        {
            return ushort.Parse(regValueString);
        }
    }
}