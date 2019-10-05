using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bonsai.Harp;
using Bonsai.Design;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Neurophotometrics.Design
{
    partial class LedCalibrationEditor : UserControl
    {
        public LedCalibrationEditor(TriggerMode triggerMode)
        {
            InitializeComponent();
            Commands = Observable.Merge(
                SetTriggerMode(TriggerMode.Constant).ToObservable(Scheduler.Immediate),
                FromSlider(slider410, ConfigurationRegisters.RawPotL410),
                FromSlider(slider470, ConfigurationRegisters.RawPotL470),
                FromSlider(slider560, ConfigurationRegisters.RawPotL560),
                ClearTriggerMode(triggerMode));
        }

        private IEnumerable<HarpMessage> SetTriggerMode(TriggerMode triggerMode)
        {
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.TriggerState, TriggerHelper.ToTriggerState(triggerMode));
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.TriggerStateLength, TriggerHelper.GetTriggerStateLength(triggerMode));
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
                .Select(evt => HarpMessage.FromByte(MessageType.Write, address, (byte)(slider.Value * 0.01 * byte.MaxValue)));
        }

        public IObservable<HarpMessage> Commands { get; private set; }
    }
}
