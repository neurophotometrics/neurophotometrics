using Bonsai.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class LaserCalibrationDialog : Form
    {
        public LaserCalibrationDialog(object instance)
        {
            InitializeComponent();
            propertyGrid.SelectedObject = new StimulationTypeDescriptor(instance);
        }

        public event EventHandler StimulationTest
        {
            add { testButton.Click += value; }
            remove { testButton.Click -= value; }
        }

        public event PropertyValueChangedEventHandler PropertyValueChanged
        {
            add { propertyGrid.PropertyValueChanged += value; }
            remove { propertyGrid.PropertyValueChanged -= value; }
        }

        public DialogResult ShowDialog(IWin32Window owner, IObservable<PhotometryDataFrame> capture)
        {
            var handleDestroyed = Observable.FromEventPattern(
                handler => HandleDestroyed += handler,
                handler => HandleDestroyed -= handler);
            var loadDialog = Observable.FromEventPattern(
                handler => Load += handler,
                handler => Load -= handler)
                .SelectMany(evt => capture.ObserveOn(this))
                .TakeUntil(handleDestroyed);
            using (var subscription = loadDialog.Subscribe(input => imageBox.Image = input.Image))
            {
                return ShowDialog(owner);
            }
        }

        class StimulationTypeDescriptor : CustomTypeDescriptor
        {
            public StimulationTypeDescriptor(object instance)
                : base(TypeDescriptor.GetProvider(instance).GetTypeDescriptor(instance))
            {
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                return base.GetProperties(new Attribute[0]);
            }

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                var stimulationProperties = new List<PropertyDescriptor>();
                var properties = base.GetProperties(attributes);
                for (int i = 0; i < properties.Count; i++)
                {
                    var property = properties[i];
                    if (property.Category == FP3002Configuration.StimulationPulseCategory &&
                        property.Name != nameof(FP3002Configuration.LaserWavelength) &&
                        property.Name != nameof(FP3002Configuration.LaserAmplitude))
                    {
                        stimulationProperties.Add(properties[i]);
                    }
                }

                return new PropertyDescriptorCollection(stimulationProperties.ToArray());
            }
        }
    }
}
