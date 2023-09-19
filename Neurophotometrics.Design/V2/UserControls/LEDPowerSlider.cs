using Neurophotometrics.Design.V2.Converters;

using System;

namespace Neurophotometrics.Design.V2.UserControls
{
    public class LedPowerSlider : SettingSlider
    {
        private const int SmallChange = 1000;
        private const int LargeChange = 5000;

        public string LEDName { get; set; }

        public event RegSettingChangedEventHandler LEDPowerChanged;

        public LedPowerSlider() : base()
        {
            ConfigureTrackBar();
            FormatControl();
            ValueChanged += LEDPowerSlider_ValueChanged;
        }

        private void FormatControl()
        {
            SetMaxLabelWidth("100.0%");
        }

        private void ConfigureTrackBar()
        {
            SetTrackBarMin((int)(LedPowerConverter.ConvertRegisterValueToPercent(LedPowerConverter.Min) * 1000));
            SetTrackBarMax((int)(LedPowerConverter.ConvertRegisterValueToPercent(LedPowerConverter.Max) * 1000));
            SetTrackBarSmallChange(SmallChange);
            SetTrackBarLargeChange(LargeChange);
        }

        internal void SetLEDPower(ushort ledPower)
        {
            var percent = LedPowerConverter.ConvertRegisterValueToPercent(ledPower);
            var trackBarValue = (int)(percent * 1000);
            SetTrackBarValue(trackBarValue);
        }

        public override ushort ConvertTrackBarToRegisterValue(int trackBarValue)
        {
            var percent = trackBarValue / 1000.0;
            return LedPowerConverter.ConvertPercentToRegisterValue(percent);
        }

        public override string ConvertTrackBarToText(int trackBarValue)
        {
            var percent = trackBarValue / 1000.0;
            return LedPowerConverter.ConvertPercentToPercentString(percent, 1);
        }

        public override int ConvertTextToTrackBar(string text)
        {
            var percent = LedPowerConverter.ConvertPercentStringToPercent(text);
            var trackBarValue = (int)(percent * 1000);

            return trackBarValue;
        }

        public override bool GetIsKeyValid(char keyChar)
        {
            return KeyFilteringHelpers.IsDecimalNumeric(keyChar);
        }

        private void LEDPowerSlider_ValueChanged(object sender, EventArgs e)
        {
            TryUpdateLEDPower();
        }

        private void TryUpdateLEDPower()
        {
            try
            {
                UpdateLEDPower();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        public void UpdateLEDPower()
        {
            var percent = GetTrackBarValue() / 1000.0;
            var ledPower = LedPowerConverter.ConvertPercentToRegisterValue(percent);
            if (LEDPowerChanged != null)
                LEDPowerChanged.Invoke(this, new RegSettingChangedEventArgs(LEDName, ledPower));
        }
    }
}