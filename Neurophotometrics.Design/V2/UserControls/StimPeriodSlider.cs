using Neurophotometrics.Design.V2.Converters;

using System;

namespace Neurophotometrics.Design.V2.UserControls
{
    public class StimPeriodSlider : SettingSlider
    {
        private const int SmallChange = 1000;
        private const int LargeChange = 5000;

        public event RegSettingChangedEventHandler StimPeriodChanged;

        public StimPeriodSlider() : base()
        {
            ConfigureTrackBar();
            FormatControl();
            ValueChanged += StimPeriodSlider_ValueChanged;
        }

        private void FormatControl()
        {
            SetMaxLabelWidth("200.0 Hz");
        }

        private void ConfigureTrackBar()
        {
            SetTrackBarMin((int)(StimPeriodConverter.ConvertPeriodMillisToFreqHz(StimPeriodConverter.MaxMillis) * 1000));
            SetTrackBarMax((int)(StimPeriodConverter.ConvertPeriodMillisToFreqHz(StimPeriodConverter.MinMillis) * 1000));
            SetTrackBarSmallChange(SmallChange);
            SetTrackBarLargeChange(LargeChange);
        }

        internal ushort GetStimPeriod()
        {
            var trackBarValue = GetTrackBarValue();
            var stimPeriod = ConvertTrackBarToRegisterValue(trackBarValue);
            return stimPeriod;
        }

        internal void SetStimPeriod(ushort stimPeriod)
        {
            var freqHz = StimPeriodConverter.ConvertPeriodMillisToFreqHz(stimPeriod);
            var trackBarValue = (int)(freqHz * 1000);
            SetTrackBarValue(trackBarValue);
        }

        public override ushort ConvertTrackBarToRegisterValue(int trackBarValue)
        {
            var freqHz = trackBarValue / 1000.0;
            return StimPeriodConverter.ConvertFreqHzToPeriodMillis(freqHz);
        }

        public override string ConvertTrackBarToText(int trackBarValue)
        {
            var freqHz = trackBarValue / 1000.0;
            var reg = StimPeriodConverter.ConvertFreqHzToPeriodMillis(freqHz);
            if (1000 / reg == 0)
                return StimPeriodConverter.ConvertPeriodMillisToFreqString(reg, 3);
            else
                return StimPeriodConverter.ConvertPeriodMillisToFreqString(reg, 1);
        }

        public override int ConvertTextToTrackBar(string text)
        {
            var freqHz = StimPeriodConverter.ConvertFreqStringToFreqHz(text);
            var trackBarValue = (int)(freqHz * 1000);

            return trackBarValue;
        }

        public override bool GetIsKeyValid(char keyChar)
        {
            return KeyFilteringHelpers.IsDecimalNumeric(keyChar);
        }

        private void StimPeriodSlider_ValueChanged(object sender, EventArgs e)
        {
            TryUpdateStimPeriod();
        }

        private void TryUpdateStimPeriod()
        {
            try
            {
                UpdateStimPeriod();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateStimPeriod()
        {
            var freqHz = GetTrackBarValue() / 1000.0;
            var stimPeriod = StimPeriodConverter.ConvertFreqHzToPeriodMillis(freqHz);
            StimPeriodChanged?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.StimPeriod), stimPeriod));
        }
    }
}