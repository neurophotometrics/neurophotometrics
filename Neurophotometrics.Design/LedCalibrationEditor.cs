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
        static readonly FrameFlags[] Constant = new[] { FrameFlags.L415 | FrameFlags.L470 | FrameFlags.L560 };

        public LedCalibrationEditor(FP3002Configuration configuration)
        {
            InitializeComponent();
            converter = new PowerConverter();
            slider415.Converter = slider470.Converter = slider560.Converter = converter;
            slider415.Value = configuration.L415;
            slider470.Value = configuration.L470;
            slider560.Value = configuration.L560;
            Commands = Observable.Merge(
                SetTriggerMode(Constant).ToObservable(Scheduler.Immediate),
                FromSlider(slider415, ConfigurationRegisters.DacL415),
                FromSlider(slider470, ConfigurationRegisters.DacL470),
                FromSlider(slider560, ConfigurationRegisters.DacL560),
                ClearTriggerMode(configuration.TriggerState));
        }

        private IEnumerable<HarpMessage> SetTriggerMode(FrameFlags[] pattern)
        {
            var triggerState = TriggerHelper.FromFrameFlags(pattern);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, triggerState);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)pattern.Length);
        }

        private IObservable<HarpMessage> ClearTriggerMode(FrameFlags[] pattern)
        {
            return Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler)
                .SelectMany(evt => SetTriggerMode(pattern).ToObservable(Scheduler.Immediate));
        }

        private IObservable<HarpMessage> FromSlider(Slider slider, int address)
        {
            return Observable.FromEventPattern(
                handler => slider.ValueChanged += handler,
                handler => slider.ValueChanged -= handler)
                .Select(evt => HarpCommand.WriteUInt16(address, (ushort)slider.Value));
        }

        public IObservable<HarpMessage> Commands { get; private set; }
    }
}
