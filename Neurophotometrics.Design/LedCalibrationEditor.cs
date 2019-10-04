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

namespace Neurophotometrics.Design
{
    public partial class LedCalibrationEditor : UserControl
    {
        public LedCalibrationEditor()
        {
            InitializeComponent();
            Commands = Observable.Merge(
                FromSlider(slider410, ConfigurationRegisters.RawPotL410),
                FromSlider(slider470, ConfigurationRegisters.RawPotL470),
                FromSlider(slider560, ConfigurationRegisters.RawPotL560));
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
