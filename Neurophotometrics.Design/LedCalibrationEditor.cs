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
        LedPowerConverter converter;
        static readonly FrameFlags[] Constant = new[] { FrameFlags.L470 };
        const ushort DefaultCalibrationPower = 12800;

        public LedCalibrationEditor(FP3002Configuration configuration)
        {
            InitializeComponent();
            converter = new LedPowerConverter();
            slider470.Converter = converter;
            slider470.Value = DefaultCalibrationPower;
            Commands = Observable.Merge(
                SetTriggerMode(Constant, DefaultCalibrationPower).ToObservable(Scheduler.Immediate),
                FromSlider(slider470, ConfigurationRegisters.DacL470),
                ClearTriggerMode(configuration.TriggerState, configuration.L470));
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            slider470.Size = new Size(25, 10);
            tableLayoutPanel.Controls.Add(slider470, 1, 0);
        }

        private IEnumerable<HarpMessage> SetTriggerMode(FrameFlags[] pattern, int ledPower)
        {
            var triggerState = TriggerHelper.FromFrameFlags(pattern);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, triggerState);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)pattern.Length);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)ledPower);
        }

        private IObservable<HarpMessage> ClearTriggerMode(FrameFlags[] pattern, int ledPower)
        {
            return Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler)
                .SelectMany(evt => SetTriggerMode(pattern, ledPower).ToObservable(Scheduler.Immediate));
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
