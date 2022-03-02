using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    public partial class SecondaryLaserCalibrationDialog : Form
    {
        object Instance;

        public SecondaryLaserCalibrationDialog(object instance)
        {
            InitializeComponent();
            Instance = instance;
            propertyGrid1.SelectedObject = new StimulationTypeDescriptor_Meas(Instance);
        }

        public event EventHandler TrigLaser
        {
            add { trigSecondaryLaser.Click += value; }
            remove { trigSecondaryLaser.Click -= value; }
        }

        public event PropertyValueChangedEventHandler PropertyValueChanged
        {
            add { propertyGrid1.PropertyValueChanged += value; }
            remove { propertyGrid1.PropertyValueChanged -= value; }
        }

        public string TrigLaserText
        {
            get { return trigSecondaryLaser.Text; }
            set { trigSecondaryLaser.Text = value; }
        }

        public DialogResult ShowDialog(IWin32Window owner, IObservable<PhotometryDataFrame> source)
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

        class StimulationTypeDescriptor_Meas : CustomTypeDescriptor
        {
            public StimulationTypeDescriptor_Meas(object instance)
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
                    if (property.Name == nameof(FP3002Configuration.LaserAmplitude))
                    {
                        stimulationProperties.Add(properties[i]);
                    }
                }

                return new PropertyDescriptorCollection(stimulationProperties.ToArray());
            }
        }

        private void trigSecondaryLaser_Click(object sender, EventArgs e)
        {
            if (trigSecondaryLaser.Text == "Trigger Laser")
            {
                trigSecondaryLaser.Text = "Stop Laser";
            }
            else if (trigSecondaryLaser.Text == "Stop Laser")
            {
                trigSecondaryLaser.Text = "Trigger Laser";
            }
        }
    }
}
