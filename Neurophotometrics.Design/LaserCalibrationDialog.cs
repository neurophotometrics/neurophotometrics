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
        object Instance;

        public LaserCalibrationDialog(object instance)
        {
            InitializeComponent();
            Instance = instance;
            propertyGrid.SelectedObject = new StimulationTypeDescriptor_Align(Instance);
        }

        public event EventHandler AlignLaser
        {
            add { alignLaserButton.Click += value; }
            remove { alignLaserButton.Click -= value; }
        }

        public event EventHandler MeasLaserPower
        {
            add { measLaserPwrButton.Click += value; }
            remove { measLaserPwrButton.Click -= value; }
        }

        public event EventHandler TrigLaser
        {
            add { trigLaserButton.Click += value; }
            remove { trigLaserButton.Click -= value; }
        }

        public event PropertyValueChangedEventHandler PropertyValueChanged
        {
            add { propertyGrid.PropertyValueChanged += value; }
            remove { propertyGrid.PropertyValueChanged -= value; }
        }

        public string TrigLaserText
        {
            get { return trigLaserButton.Text; }
            set { trigLaserButton.Text = value; }
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

        class StimulationTypeDescriptor_Align : CustomTypeDescriptor
        {
            public StimulationTypeDescriptor_Align(object instance)
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
                    if ((property.Category == FP3002Configuration.StimulationPulseCategory ||
                        property.Category == FP3002Configuration.LaserPowerCategory) &&
                        property.Name != nameof(FP3002Configuration.LaserWavelength))
                    {
                        stimulationProperties.Add(properties[i]);
                    }
                    

                }

                return new PropertyDescriptorCollection(stimulationProperties.ToArray());
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

        private void alignLaserButton_Click(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = new StimulationTypeDescriptor_Align(Instance);
            
        }

        private void measLaserPwrButton_Click(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = new StimulationTypeDescriptor_Meas(Instance);
        }

        private void trigLaserButton_Click(object sender, EventArgs e)
        {
            if(trigLaserButton.Text == "Trigger Laser")
            {
                trigLaserButton.Text = "Stop Laser";
            }
            else if(trigLaserButton.Text == "Stop Laser")
            {
                trigLaserButton.Text = "Trigger Laser";
            }
        }
    }
}
