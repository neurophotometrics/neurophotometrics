using Neurophotometrics.Design.V2.Converters;

using System;

namespace Neurophotometrics.Design.V2.UserControls
{
    public class LaserAmplitudeSlider : SettingSlider
    {
        private const int SmallChange = 1000;
        private const int LargeChange = 5000;

        private const double HighAmplitudeCutOff = 50.0;

        public event RegSettingChangedEventHandler LaserAmplitudeChanged;

        public LaserAmplitudeSlider() : base()
        {
            ConfigureTrackBar();
            FormatControl();
            ValueChanged += LaserAmplitudeSlider_ValueChanged;
        }

        private void FormatControl()
        {
            SetMaxLabelWidth("200.0 Hz");
        }

        private void ConfigureTrackBar()
        {
            SetTrackBarMin((int)(LaserAmplitudeConverter.ConvertRegisterValueToPercent(LaserAmplitudeConverter.Min) * 1000));
            SetTrackBarMax((int)(LaserAmplitudeConverter.ConvertRegisterValueToPercent(LaserAmplitudeConverter.Max) * 1000));
            SetTrackBarSmallChange(SmallChange);
            SetTrackBarLargeChange(LargeChange);
        }

        internal ushort GetLaserAmplitude()
        {
            var trackBarValue = GetTrackBarValue();
            var amplitude = ConvertTrackBarToRegisterValue(trackBarValue);
            return amplitude;
        }

        internal void SetLaserAmplitude(ushort laserAmplitude)
        {
            var percent = LaserAmplitudeConverter.ConvertRegisterValueToPercent(laserAmplitude);
            var trackBarValue = (int)(percent * 1000);
            SetTrackBarValue(trackBarValue);
        }

        internal bool GetIsHighAmplitude()
        {
            var trackBarValue = GetTrackBarValue();
            var percent = trackBarValue / 1000.0;
            return percent >= HighAmplitudeCutOff;
        }

        public override ushort ConvertTrackBarToRegisterValue(int trackBarValue)
        {
            var percent = trackBarValue / 1000.0;
            return LaserAmplitudeConverter.ConvertPercentToRegisterValue(percent);
        }

        public override string ConvertTrackBarToText(int trackBarValue)
        {
            var percent = trackBarValue / 1000.0;
            return LaserAmplitudeConverter.ConvertPercentToPercentString(percent, 1);
        }

        public override int ConvertTextToTrackBar(string text)
        {
            var percent = LaserAmplitudeConverter.ConvertPercentStringToPercent(text);
            var trackBarValue = (int)(percent * 1000);

            return trackBarValue;
        }

        public override bool GetIsKeyValid(char keyChar)
        {
            return KeyFilteringHelpers.IsDecimalNumeric(keyChar);
        }

        private void LaserAmplitudeSlider_ValueChanged(object sender, EventArgs e)
        {
            TryUpdateLaserPower();
        }

        private void TryUpdateLaserPower()
        {
            try
            {
                UpdateLaserPower();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        public void UpdateLaserPower()
        {
            var percent = GetTrackBarValue() / 1000.0;
            var laserPower = LaserAmplitudeConverter.ConvertPercentToRegisterValue(percent);
            LaserAmplitudeChanged?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.LaserAmplitude), laserPower));
        }
    }
}