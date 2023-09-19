using Neurophotometrics.Design.V2.Converters;

using System;

namespace Neurophotometrics.Design.V2.UserControls
{
    public class TriggerPeriodSlider : SettingSlider
    {
        private const int SmallChange = 1000;
        private const int LargeChange = 5000;

        public event RegSettingChangedEventHandler TriggerPeriodChanged;

        public TriggerPeriodSlider() : base()
        {
            ConfigureTrackBar();
            ValueChanged += TriggerPeriodSlider_ValueChanged;
        }

        private void ConfigureTrackBar()
        {
            SetTrackBarMin((int)(TriggerPeriodConverter.ConvertPeriodMicrosToFreqHz(TriggerPeriodConverter.MaxMicros) * 1000));
            SetTrackBarMax((int)(TriggerPeriodConverter.ConvertPeriodMicrosToFreqHz(TriggerPeriodConverter.MinMicros) * 1000));
            SetTrackBarSmallChange(SmallChange);
            SetTrackBarLargeChange(LargeChange);
        }

        internal void SetTriggerPeriod(ushort triggerPeriod)
        {
            var freqHz = TriggerPeriodConverter.ConvertPeriodMicrosToFreqHz(triggerPeriod);
            var trackBarValue = (int)(freqHz * 1000);
            SetTrackBarValue(trackBarValue);
        }

        public override ushort ConvertTrackBarToRegisterValue(int trackBarValue)
        {
            var freqHz = trackBarValue / 1000.0;
            return TriggerPeriodConverter.ConvertFreqHzToPeriodMicros(freqHz);
        }

        public override string ConvertTrackBarToText(int trackBarValue)
        {
            var freqHz = trackBarValue / 1000.0;
            return TriggerPeriodConverter.ConvertFreqHzToFreqString(freqHz, 1);
        }

        public override int ConvertTextToTrackBar(string text)
        {
            var freqHz = TriggerPeriodConverter.ConvertFreqStringToFreqHz(text);
            var trackBarValue = (int)(freqHz * 1000);

            return trackBarValue;
        }

        public override bool GetIsKeyValid(char keyChar)
        {
            return KeyFilteringHelpers.IsDecimalNumeric(keyChar);
        }

        private void TriggerPeriodSlider_ValueChanged(object sender, EventArgs e)
        {
            TryUpdateTriggerPeriod();
        }

        private void TryUpdateTriggerPeriod()
        {
            try
            {
                UpdateTriggerPeriod();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateTriggerPeriod()
        {
            var freqHz = GetTrackBarValue() / 1000.0;
            var triggerPeriod = TriggerPeriodConverter.ConvertFreqHzToPeriodMicros(freqHz);
            if (TriggerPeriodChanged != null)
                TriggerPeriodChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.TriggerPeriod), triggerPeriod));
        }
    }
}