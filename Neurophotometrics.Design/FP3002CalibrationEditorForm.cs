using Bonsai.Design;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Neurophotometrics.Design
{
    partial class FP3002CalibrationEditorForm : Form
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
                .Do(evt => ValidateSettings())
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
                    var triggerState = message.GetPayload<byte>();
                    configuration.TriggerMode = TriggerHelper.FromTriggerState(triggerState);
                    triggerModeView.TriggerMode = configuration.TriggerMode;
                    break;
                case ConfigurationRegisters.TriggerPeriod:
                    configuration.TriggerPeriod = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.TriggerTime:
                    configuration.ExposureTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.TriggerPreTime:
                    configuration.PreTriggerTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL410:
                    configuration.L410 = message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.DacL470:
                    configuration.L470 = message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.DacL560:
                    configuration.L560 = message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.Out0Conf:
                    configuration.DigitalOutput0 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.Out1Conf:
                    configuration.DigitalOutput1 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.In0Conf:
                    configuration.DigitalInput0 = (DigitalInputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.In1Conf:
                    configuration.DigitalInput1 = (DigitalInputConfiguration)message.GetPayloadByte();
                    break;
                case ConfigurationRegisters.StimPeriod:
                    configuration.PulsePeriod = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.StimOn:
                    configuration.PulseWidth = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.StimReps:
                    configuration.PulseCount = message.GetPayloadUInt16();
                    break;
                default:
                    break;
            }
        }

        private void ValidateSettings()
        {
            configuration.Validate();
            propertyGrid.Refresh();
        }

        IEnumerable<HarpMessage> SerializeSettings()
        {
            yield return HarpMessage.FromByte(ConfigurationRegisters.TriggerState, MessageType.Write, TriggerHelper.ToTriggerState(configuration.TriggerMode));
            yield return HarpMessage.FromByte(ConfigurationRegisters.TriggerStateLength, MessageType.Write, TriggerHelper.GetTriggerStateLength(configuration.TriggerMode));
            yield return HarpMessage.FromUInt16(ConfigurationRegisters.TriggerPeriod, MessageType.Write, (ushort)configuration.TriggerPeriod);
            yield return HarpMessage.FromUInt16(ConfigurationRegisters.TriggerTime, MessageType.Write, (ushort)configuration.ExposureTime);
            yield return HarpMessage.FromByte(ConfigurationRegisters.DacL410, MessageType.Write, (byte)configuration.L410);
            yield return HarpMessage.FromByte(ConfigurationRegisters.DacL470, MessageType.Write, (byte)configuration.L470);
            yield return HarpMessage.FromByte(ConfigurationRegisters.DacL560, MessageType.Write, (byte)configuration.L560);
            yield return HarpMessage.FromByte(ConfigurationRegisters.Out0Conf, MessageType.Write, (byte)configuration.DigitalOutput0);
            yield return HarpMessage.FromByte(ConfigurationRegisters.Out1Conf, MessageType.Write, (byte)configuration.DigitalOutput1);
            yield return HarpMessage.FromByte(ConfigurationRegisters.In0Conf, MessageType.Write, (byte)configuration.DigitalInput0);
            yield return HarpMessage.FromByte(ConfigurationRegisters.In1Conf, MessageType.Write, (byte)configuration.DigitalInput1);
            yield return HarpMessage.FromUInt16(ConfigurationRegisters.StimPeriod, MessageType.Write, (ushort)configuration.PulsePeriod);
            yield return HarpMessage.FromUInt16(ConfigurationRegisters.StimOn, MessageType.Write, (ushort)configuration.PulseWidth);
            yield return HarpMessage.FromUInt16(ConfigurationRegisters.StimReps, MessageType.Write, (ushort)configuration.PulseCount);
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

        private void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            configuration.Validate();
            if (e.ChangedItem.PropertyDescriptor.Name == nameof(configuration.TriggerMode))
            {
                triggerModeView.TriggerMode = configuration.TriggerMode;
            }
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
            using (var ledCalibration = new LedCalibrationEditor(configuration))
            using (var calibrationDialog = new FP3001CalibrationEditorForm(instance, ProcessPhotometry(instance.Generate(ledCalibration.Commands)), serviceProvider))
            {
                calibrationDialog.AddCalibrationControl(ledCalibration);
                calibrationDialog.Text = setupButton.Text;
                calibrationDialog.ShowDialog(this);
            }

            OpenDevice();
        }
    }
}
