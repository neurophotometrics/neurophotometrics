using Neurophotometrics.Design.Properties;

using System;
using System.Text.RegularExpressions;

namespace Neurophotometrics.Design.V2.Converters
{
    public static class StimPeriodConverter
    {
        public const ushort MinMillis = 2;
        public const ushort MaxMillis = 60000;
        public const ushort DefaultMillis = 1000;

        public static ushort ConvertFreqStringToPeriodMillis(string freqString)
        {
            var freqHz = ConvertFreqStringToFreqHz(freqString);
            var periodMillis = ConvertFreqHzToPeriodMillis(freqHz);

            return periodMillis;
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

        public static string ConvertPeriodMillisToFreqString(ushort periodMillis, int decimals)
        {
            var freqHz = ConvertPeriodMillisToFreqHz(periodMillis);
            var freqString = ConvertFreqHzToFreqString(freqHz, decimals);
            return freqString;
        }

        public static double ConvertPeriodMillisToFreqHz(ushort periodMillis)
        {
            periodMillis = Math.Min(MaxMillis, Math.Max(MinMillis, periodMillis));
            var freqHz = 1000.0 / periodMillis;
            return freqHz;
        }

        public static string ConvertFreqHzToFreqString(double freqHz, int decimals)
        {
            return string.Format($"{{0:F{decimals}}}", freqHz) + " Hz";
        }

        public static ushort ConvertFreqHzToPeriodMillis(double freqHz)
        {
            var periodMillis = (ushort)(1000.0 / freqHz);
            return Math.Min(MaxMillis, Math.Max(MinMillis, periodMillis));
        }

        internal static ushort ConvertStringToPeriodMillis(string periodMillisString)
        {
            return ushort.Parse(periodMillisString);
        }
    }
}