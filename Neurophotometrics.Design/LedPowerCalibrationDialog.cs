using Bonsai.Design;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class LedPowerCalibrationDialog : Form
    {
        readonly LedPowerConverter converter;
        static readonly FrameFlags[] Constant = new[] { FrameFlags.L470 | FrameFlags.L560 | FrameFlags.L415 };
        const ushort DefaultTriggerPeriod = 25000;
        const ushort DefaultDwellTime = 24500;

        public LedPowerCalibrationDialog(FP3002Configuration configuration)
        {
            InitializeComponent();
            converter = new LedPowerConverter();
            slider470.Converter = slider560.Converter = slider415.Converter = converter;
            slider470.Value = configuration.L470;
            slider560.Value = configuration.L560;
            slider415.Value = configuration.L415;
            Commands = Observable.Merge(
                SetTriggerMode(Constant, DefaultTriggerPeriod, DefaultDwellTime).ToObservable(Scheduler.Immediate),
                FromSlider(slider470, ConfigurationRegisters.DacL470),
                FromSlider(slider560, ConfigurationRegisters.DacL560),
                FromSlider(slider415, ConfigurationRegisters.DacL415),
                ClearTriggerMode(configuration.TriggerState, configuration.TriggerPeriod, configuration.DwellTime));
        }

        public IObservable<HarpMessage> Commands { get; private set; }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            slider470.Size = slider560.Size = slider415.Size = new Size(25, 10);
            tableLayoutPanel.Controls.Add(slider470, 1, 0);
            tableLayoutPanel.Controls.Add(slider560, 1, 1);
            tableLayoutPanel.Controls.Add(slider415, 1, 2);
        }

        private IEnumerable<HarpMessage> SetTriggerMode(FrameFlags[] pattern, int triggerPeriod, int dwellTime)
        {
            Console.WriteLine(pattern.Length);
            var triggerState = TriggerHelper.FromFrameFlags(pattern);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, triggerState);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)pattern.Length);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerPeriod, (ushort)triggerPeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerTimeUpdateOutputs, (ushort)dwellTime);
        }

        private IObservable<HarpMessage> ClearTriggerMode(FrameFlags[] pattern, int triggerPeriod, int dwellTime)
        {
            return Observable.FromEventPattern<FormClosingEventHandler, FormClosingEventArgs>(
                handler => FormClosing += handler,
                handler => FormClosing -= handler)
                .SelectMany(evt => SetTriggerMode(pattern, triggerPeriod, dwellTime).ToObservable(Scheduler.Immediate));
        }

        private IObservable<HarpMessage> FromSlider(Slider slider, int address)
        {
            return Observable.FromEventPattern(
                handler => slider.ValueChanged += handler,
                handler => slider.ValueChanged -= handler)
                .Select(evt => HarpCommand.WriteUInt16(address, (ushort)slider.Value));
        }

        public DialogResult ShowDialog(IWin32Window owner, IObservable<HarpMessage> source)
        {
            var handleDestroyed = Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler);
            var loadDialog = Observable.FromEventPattern(
                handler => Load += handler,
                handler => Load -= handler);
            using (var subscription = loadDialog.Take(1).SelectMany(evt => source.TakeUntil(handleDestroyed)).Subscribe())
            {
                return ShowDialog(owner);
            }
        }
    }
}
