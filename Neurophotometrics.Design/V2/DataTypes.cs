using System;

namespace Neurophotometrics.Design.V2
{
    public delegate void RegSettingChangedEventHandler(object sender, RegSettingChangedEventArgs args);

    public class RegSettingChangedEventArgs : EventArgs
    {
        public RegSettingChangedEventArgs(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }

    public static class KeyFilteringHelpers
    {
        public static bool IsIntegerNumeric(char keyChar)
        {
            if (char.IsDigit(keyChar) || char.IsControl(keyChar))
                return true;
            else
                return false;
        }

        public static bool IsDecimalNumeric(char keyChar)
        {
            if (char.IsDigit(keyChar) || char.IsControl(keyChar) || keyChar == '.')
                return true;
            else
                return false;
        }
    }

    public class PulseTrain
    {
        public ushort LaserWavelength { get; set; }
        public ushort LaserAmplitude { get; set; }
        public ushort StimPeriod { get; set; }
        public ushort StimOn { get; set; }
        public ushort StimReps { get; set; }
    }

    public class LedPowers
    {
        private const ushort LedPowerDefault = 0;

        public ushort L415 { get; set; } = LedPowerDefault;

        public ushort L470 { get; set; } = LedPowerDefault;

        public ushort L560 { get; set; } = LedPowerDefault;
    }
}