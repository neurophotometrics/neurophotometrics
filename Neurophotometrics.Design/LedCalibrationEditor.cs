using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Bonsai.Harp;
using Bonsai.Design;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Neurophotometrics.Design
{
    partial class LedCalibrationEditor : UserControl
    {
        PowerConverter converter;

        public LedCalibrationEditor(FP3002Configuration configuration)
        {
            InitializeComponent();
            converter = new PowerConverter();
            slider410.Converter = slider470.Converter = slider560.Converter = converter;
            slider410.Value = configuration.L410;
            slider470.Value = configuration.L470;
            slider560.Value = configuration.L560;
            Commands = Observable.Merge(
                SetTriggerMode(TriggerMode.Constant).ToObservable(Scheduler.Immediate),
                FromSlider(slider410, ConfigurationRegisters.DacL410),
                FromSlider(slider470, ConfigurationRegisters.DacL470),
                FromSlider(slider560, ConfigurationRegisters.DacL560),
                ClearTriggerMode(configuration.TriggerMode));
        }

        private IEnumerable<HarpMessage> SetTriggerMode(TriggerMode triggerMode)
        {
            yield return HarpMessage.FromByte(ConfigurationRegisters.TriggerState, MessageType.Write, TriggerHelper.ToTriggerState(triggerMode));
            yield return HarpMessage.FromByte(ConfigurationRegisters.TriggerStateLength, MessageType.Write, TriggerHelper.GetTriggerStateLength(triggerMode));
        }

        private IObservable<HarpMessage> ClearTriggerMode(TriggerMode triggerMode)
        {
            return Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler)
                .SelectMany(evt => SetTriggerMode(triggerMode).ToObservable(Scheduler.Immediate));
        }

        private IObservable<HarpMessage> FromSlider(Slider slider, int address)
        {
            return Observable.FromEventPattern(
                handler => slider.ValueChanged += handler,
                handler => slider.ValueChanged -= handler)
                .Select(evt => HarpMessage.FromByte(address, MessageType.Write, (byte)slider.Value));
        }

        public IObservable<HarpMessage> Commands { get; private set; }
    }
}
