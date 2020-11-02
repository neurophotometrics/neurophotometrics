using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Bonsai.Harp;
using Bonsai.Design;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Drawing;

namespace Neurophotometrics.Design
{
    partial class LedCalibrationEditor : UserControl
    {
        readonly LedPowerConverter converter;
        static readonly FrameFlags[] Constant = new[] { FrameFlags.L470 };
        const ushort DefaultCalibrationPower = LedPowerConverter.MaxLedPower;
        const ushort DefaultTriggerPeriod = 25000;
        const ushort DefaultDwellTime = 24500;

        public LedCalibrationEditor(FP3002Configuration configuration)
        {
            InitializeComponent();
            converter = new LedPowerConverter();
            slider470.Converter = converter;
            slider470.Value = DefaultCalibrationPower;
            Commands = Observable.Merge(
                SetTriggerMode(Constant, DefaultCalibrationPower, DefaultTriggerPeriod, DefaultDwellTime).ToObservable(Scheduler.Immediate),
                FromSlider(slider470, ConfigurationRegisters.DacL470),
                ClearTriggerMode(configuration.TriggerState, configuration.L470, configuration.TriggerPeriod, configuration.TriggerTimeUpdateOutputs));
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            slider470.Size = new Size(25, 10);
            tableLayoutPanel.Controls.Add(slider470, 1, 0);
        }

        private IEnumerable<HarpMessage> SetTriggerMode(FrameFlags[] pattern, int ledPower, int triggerPeriod, int dwellTime)
        {
            var triggerState = TriggerHelper.FromFrameFlags(pattern);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, triggerState);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)pattern.Length);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerPeriod, (ushort)triggerPeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerTimeUpdateOutputs, (ushort)dwellTime);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)ledPower);
        }

        private IObservable<HarpMessage> ClearTriggerMode(FrameFlags[] pattern, int ledPower, int triggerPeriod, int dwellTime)
        {
            return Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler)
                .SelectMany(evt => SetTriggerMode(pattern, ledPower, triggerPeriod, dwellTime).ToObservable(Scheduler.Immediate));
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
