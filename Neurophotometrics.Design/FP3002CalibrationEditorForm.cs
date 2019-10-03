using Bonsai.Design;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Neurophotometrics.Design
{
    public partial class FP3002CalibrationEditorForm : Form
    {
        FP3002 instance;
        FP3002Configuration configuration;
        IObservable<HarpMessage> device;
        IServiceProvider serviceProvider;
        IDisposable subscription;

        public FP3002CalibrationEditorForm(FP3002 capture, IServiceProvider provider)
        {
            InitializeComponent();
            instance = capture;
            device = CreateDevice();
            serviceProvider = provider;
            configuration = new FP3002Configuration();
            propertyGrid.SelectedObject = configuration;
        }

        private IObservable<HarpMessage> CreateDevice()
        {
            var device = new Device
            {
                PortName = instance.PortName,
                Heartbeat = EnableType.Disable,
                IgnoreErrors = true
            };

            var restoreDeviceSettings = Observable.FromEventPattern(
                handler => restoreDeviceSettingsButton.Click += handler,
                handler => restoreDeviceSettingsButton.Click -= handler);

            var storeDeviceSettings = Observable.FromEventPattern(
                handler => storeDeviceSettingsButton.Click += handler,
                handler => storeDeviceSettingsButton.Click -= handler)
                .SelectMany(evt => SerializeSettings());

            return device.Generate(storeDeviceSettings)
                .Where(IsReadMessage).Do(ParseSettings)
                .Throttle(TimeSpan.FromSeconds(0.2)).ObserveOn(propertyGrid).Do(message => propertyGrid.Refresh())
                .DelaySubscription(TimeSpan.FromSeconds(0.2))
                .TakeUntil(restoreDeviceSettings).Repeat();
        }

        private void OpenDevice()
        {
            subscription = device.Subscribe();
        }

        private void CloseDevice()
        {
            if (subscription != null)
            {
                subscription.Dispose();
                subscription = null;
            }
        }

        private static bool IsPhotometryEvent(HarpMessage input)
        {
            return input.Address == (byte)FP3002EventType.Photometry && input.MessageType == MessageType.Event && input.Error == false;
        }

        private static IObservable<PhotometryDataFrame> ProcessPhotometry(IObservable<HarpMessage> source)
        {
            return source.Where(IsPhotometryEvent).Select(input => ((PhotometryHarpMessage)input).PhotometryData);
        }

        private static bool IsReadMessage(HarpMessage message)
        {
            return message.MessageType == MessageType.Read;
        }

        private void ParseSettings(HarpMessage message)
        {
            switch (message.Address)
            {
                case ConfigurationRegisters.TriggerState:
                    byte[] triggerState;
                    message.GetPayloadByte(out triggerState);
                    configuration.TriggerMode = TriggerHelper.FromTriggerState(triggerState);
                    break;
                case ConfigurationRegisters.TriggerPeriod:
                    configuration.TriggerFrequency = 1000000 / message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.TriggerTime:
                    configuration.TriggerTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.PreTriggerTime:
                    configuration.PreTriggerTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DigitalOutput0:
                    configuration.DigitalOutput0 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.DigitalOutput1:
                    configuration.DigitalOutput1 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.DigitalInput0:
                    configuration.DigitalInput0 = (DigitalInputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.StimPeriod:
                    configuration.StimFrequency = 1000 / message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.StimTime:
                    configuration.StimTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.StimRepetitions:
                    configuration.StimRepetitions = message.GetPayloadUInt16();
                    break;
                default:
                    break;
            }
        }

        IEnumerable<HarpMessage> SerializeSettings()
        {
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.TriggerState, TriggerHelper.ToTriggerState(configuration.TriggerMode));
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.TriggerStateLength, TriggerHelper.GetTriggerStateLength(configuration.TriggerMode));
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.TriggerPeriod, (ushort)(1000000 / configuration.TriggerFrequency));
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.TriggerTime, (ushort)configuration.TriggerTime);
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.PreTriggerTime, (ushort)configuration.PreTriggerTime);
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.DigitalOutput0, (byte)configuration.DigitalOutput0);
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.DigitalOutput1, (byte)configuration.DigitalOutput1);
            yield return HarpMessage.FromByte(MessageType.Write, ConfigurationRegisters.DigitalInput0, (byte)configuration.DigitalInput0);
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.StimPeriod, (ushort)(1000 / configuration.StimFrequency));
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.StimTime, (ushort)configuration.StimTime);
            yield return HarpMessage.FromUInt16(MessageType.Write, ConfigurationRegisters.StimRepetitions, (ushort)configuration.StimRepetitions);
        }

        protected override void OnLoad(EventArgs e)
        {
            OpenDevice();
            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CloseDevice();
            base.OnFormClosed(e);
        }

        private void loadSettingsButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (var reader = XmlReader.Create(openFileDialog.FileName))
                {
                    var serializer = new XmlSerializer(typeof(FP3002Configuration));
                    configuration = (FP3002Configuration)serializer.Deserialize(reader);
                }
            }
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (var writer = XmlWriter.Create(saveFileDialog.FileName, new XmlWriterSettings { Indent = true }))
                {
                    var serializer = new XmlSerializer(typeof(FP3002Configuration));
                    serializer.Serialize(writer, configuration);
                }
            }
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            CloseDevice();
            using (var calibrationDialog = new FP3001CalibrationEditorForm(instance, ProcessPhotometry(instance.Generate()), serviceProvider))
            {
                calibrationDialog.Text = setupButton.Text;
                calibrationDialog.ShowDialog(this);
            }

            OpenDevice();
        }
    }
}
