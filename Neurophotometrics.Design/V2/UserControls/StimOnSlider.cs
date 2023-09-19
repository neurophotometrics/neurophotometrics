using Neurophotometrics.Design.V2.Converters;

using System;

namespace Neurophotometrics.Design.V2.UserControls
{
    public class StimOnSlider : SettingSlider
    {
        private const int SmallChange = 1;
        private const int LargeChange = 10;

        public event RegSettingChangedEventHandler StimOnChanged;

        public StimOnSlider() : base()
        {
            ConfigureTrackBar();
            FormatControl();
            ValueChanged += StimOnSlider_ValueChanged;
        }

        private void FormatControl()
        {
            SetMaxLabelWidth("200.0 Hz");
        }

        private void ConfigureTrackBar()
        {
            SetTrackBarMin(StimOnConverter.Min);
            SetTrackBarMax(StimOnConverter.Max);
            SetTrackBarSmallChange(SmallChange);
            SetTrackBarLargeChange(LargeChange);
        }

        internal ushort GetStimOn()
        {
            var trackBarValue = GetTrackBarValue();
            var stimOn = ConvertTrackBarToRegisterValue(trackBarValue);
            return stimOn;
        }

        internal void SetStimOn(ushort stimOn)
        {
            SetTrackBarValue(stimOn);
        }

        internal void SetStimOnLimit(ushort stimPeriod)
        {
            var stimOn = GetStimOn();
            if (stimOn > stimPeriod)
            {
                SetTrackBarValue(stimPeriod);
                UpdateStimOn();
            }

            SetTrackBarMax(stimPeriod);
        }

        public override string ConvertTrackBarToText(int trackBarValue)
        {
            var pulseWidth = (ushort)trackBarValue;
            return StimOnConverter.ConvertRegisterValueToPulseWidthString(pulseWidth);
        }

        public override int ConvertTextToTrackBar(string text)
        {
            var regisiterValue = StimOnConverter.ConvertPulseWidthStringToRegisterValue(text);

            return regisiterValue;
        }

        public override bool GetIsKeyValid(char keyChar)
        {
            return KeyFilteringHelpers.IsIntegerNumeric(keyChar);
        }

        private void StimOnSlider_ValueChanged(object sender, EventArgs e)
        {
            TryUpdateStimOn();
        }

        private void TryUpdateStimOn()
        {
            try
            {
                UpdateStimOn();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateStimOn()
        {
            var stimOn = (ushort)GetTrackBarValue();
            if (StimOnChanged != null)
                StimOnChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.StimOn), stimOn));
        }
    }
}