using Neurophotometrics.Design.Properties;

using System;
using System.Text.RegularExpressions;

namespace Neurophotometrics.Design.V2.Converters
{
    public static class TriggerPeriodConverter
    {
        public const ushort MinMicros = 5000;
        public const ushort MaxMicros = 62500;
        public const ushort DefaultMicros = 62500;

        public static ushort ConvertFreqStringToPeriodMicros(string freqString)
        {
            var freqHz = ConvertFreqStringToFreqHz(freqString);
            var periodMicros = ConvertFreqHzToPeriodMicros(freqHz);

            return periodMicros;
        }

        public static double ConvertFreqStringToFreqHz(string freqString)
        {
            // Remove any Non-Numeric characters
            freqString = Regex.Replace(freqString, "[^.0-9]", "");

            // Find user-readable value
            var success = double.TryParse(freqString, out var freqHz);

            if (!success)
                throw new ArgumentException(Resources.MsgBox_Warning_InvalidPropertyValue.Replace("{0}", freqString));

            return freqHz;
        }

        public static string ConvertPeriodMicrosToFreqString(ushort periodMicros, int decimals)
        {
            var freqHz = ConvertPeriodMicrosToFreqHz(periodMicros);
            var freqString = ConvertFreqHzToFreqString(freqHz, decimals);
            return freqString;
        }

        public static double ConvertPeriodMicrosToFreqHz(ushort periodMicros)
        {
            periodMicros = Math.Min(MaxMicros, Math.Max(MinMicros, periodMicros));
            var freqHz = 1000000.0 / periodMicros;
            return freqHz;
        }

        public static string ConvertFreqHzToFreqString(double freqHz, int decimals)
        {
            return string.Format($"{{0:F{decimals}}}", freqHz) + " Hz";
        }

        public static ushort ConvertFreqHzToPeriodMicros(double freqHz)
        {
            var periodMicros = (ushort)(1000000.0 / freqHz);
            return Math.Min(MaxMicros, Math.Max(MinMicros, periodMicros));
        }

        internal static ushort ConvertStringToPeriodMicros(string periodMicrosString)
        {
            return ushort.Parse(periodMicrosString);
        }
    }
}