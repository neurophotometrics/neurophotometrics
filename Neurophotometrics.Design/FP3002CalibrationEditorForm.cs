using Bonsai.Design;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Neurophotometrics.Design
{
    partial class FP3002CalibrationEditorForm : Form
    {
        readonly FP3002 instance;
        readonly PhotometryData photometry;
        readonly IObservable<HarpMessage> device;
        readonly IServiceProvider serviceProvider;
        readonly StringFormat rowHeaderFormat;
        FP3002Configuration configuration;
        IDisposable subscription;

        public FP3002CalibrationEditorForm(FP3002 capture, IServiceProvider provider)
        {
            InitializeComponent();
            instance = capture;
            device = CreateDevice();
            serviceProvider = provider;
            photometry = new PhotometryData();
            configuration = new FP3002Configuration();
            configuration.ExposureTime = (int)(capture.ExposureTime * 1000);
            propertyGrid.SelectedObject = configuration;
            rowHeaderFormat = new StringFormat();
            rowHeaderFormat.Alignment = StringAlignment.Far;
            rowHeaderFormat.LineAlignment = StringAlignment.Center;
        }

        private IObservable<HarpMessage> CreateDevice()
        {
            var device = new Device
            {
                PortName = instance.PortName,
                Heartbeat = EnableType.Disable,
                IgnoreErrors = true
            };

            var resetDeviceSettings = Observable.FromEventPattern(
                handler => restoreDeviceSettingsButton.Click += handler,
                handler => restoreDeviceSettingsButton.Click -= handler)
                .Select(evt => ShouldResetSettings())
                .Where(result => result != DialogResult.Cancel)
                .SelectMany(result => ResetSettings(true))
                .Publish().RefCount();

            var storeDeviceSettings = Observable.FromEventPattern(
                handler => storeDeviceSettingsButton.Click += handler,
                handler => storeDeviceSettingsButton.Click -= handler)
                .Select(evt => ShouldSaveSettings())
                .Where(result => result != DialogResult.Cancel)
                .SelectMany(result => SerializeSettings(result == DialogResult.Yes))
                .Publish().RefCount();

            return device.Generate(storeDeviceSettings.Merge(resetDeviceSettings))
                .Where(IsReadMessage).Do(ParseSettings)
                .Throttle(TimeSpan.FromSeconds(0.2)).ObserveOn(propertyGrid).Do(message => propertyGrid.Refresh())
                .DelaySubscription(TimeSpan.FromSeconds(0.2))
                .TakeUntil(resetDeviceSettings.Merge(storeDeviceSettings)
                    .Where(ConfigurationRegisters.Reset)
                    .Delay(TimeSpan.FromSeconds(1)))
                .Do(x => { }, () => BeginInvoke((Action)Close));
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

        private static bool IsReadMessage(HarpMessage message)
        {
            return message.MessageType == MessageType.Read;
        }

        private void ParseSettings(HarpMessage message)
        {
            switch (message.Address)
            {
                case ConfigurationRegisters.Config:
                    configuration.Config = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.TriggerState:
                    var triggerState = message.GetPayloadArray<byte>();
                    configuration.TriggerState = TriggerHelper.ToFrameFlags(triggerState);
                    triggerStateView.BeginInvoke((Action)SetTriggerState);
                    break;
                case ConfigurationRegisters.TriggerPeriod:
                    configuration.TriggerPeriod = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.TriggerTimeUpdateOutputs:
                    configuration.DwellTime = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL410:
                    configuration.L410 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL470:
                    configuration.L470 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL560:
                    configuration.L560 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacLaser:
                    configuration.LaserPower = message.GetPayloadUInt16();
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
                case ConfigurationRegisters.StimWavelength:
                    configuration.LaserWavelength = message.GetPayloadUInt16();
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

        private FrameFlags[] GetTriggerState()
        {
            var triggerState = new FrameFlags[triggerStateView.Rows.Count - 1];
            for (int i = 0; i < triggerState.Length; i++)
            {
                var row = triggerStateView.Rows[i].Cells;
                var ledState = (string)row[Led.Name].Value;
                if (Enum.TryParse(ledState, out FrameFlags state) || string.IsNullOrEmpty(ledState))
                {
                    if (true.Equals(row[Out0.Name].Value)) state |= FrameFlags.Output0;
                    if (true.Equals(row[Out1.Name].Value)) state |= FrameFlags.Output1;
                    triggerState[i] = state;
                }
            }
            return triggerState;
        }

        private void SetTriggerState()
        {
            var triggerState = configuration.TriggerState;
            var rows = Array.ConvertAll(configuration.TriggerState, state =>
            {
                var led = (FrameFlags)((int)state & 0x7);
                var output0 = (state & FrameFlags.Output0) != 0;
                var output1 = (state & FrameFlags.Output1) != 0;
                var row = new DataGridViewRow();
                row.CreateCells(triggerStateView, led.ToString(), output0, output1);
                return row;
            });

            triggerStateView.SuspendLayout();
            triggerStateView.Rows.Clear();
            triggerStateView.Rows.AddRange(rows);
            triggerStateView.ResumeLayout();
        }

        private void ValidateSettings()
        {
            instance.ExposureTime = configuration.ExposureTime / 1000.0;
            configuration.Validate();
            propertyGrid.Refresh();
            SetTriggerState();
        }

        private DialogResult ShouldSaveSettings()
        {
            ValidateSettings();
            return MessageBox.Show(this,
                Properties.Resources.SavePermanentRegisters_Question,
                Text, MessageBoxButtons.YesNoCancel);
        }

        private DialogResult ShouldResetSettings()
        {
            return MessageBox.Show(this,
                Properties.Resources.ResetPermanentRegisters_Question,
                Text, MessageBoxButtons.OKCancel);
        }

        IEnumerable<HarpMessage> ResetSettings(bool resetDefault)
        {
            var resetMode = resetDefault ? ResetDeviceConfiguration.ResetDefault : ResetDeviceConfiguration.ResetEeprom;
            yield return HarpCommand.WriteByte(ConfigurationRegisters.Reset, (byte)resetMode);
        }

        IEnumerable<HarpMessage> SerializeSettings(bool saveRegisters)
        {
            var triggerState = configuration.TriggerState;
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.Config, (ushort)configuration.Config);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, TriggerHelper.FromFrameFlags(triggerState));
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)triggerState.Length);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerPeriod, (ushort)configuration.TriggerPeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerTimeUpdateOutputs, (ushort)configuration.DwellTime);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL410, (ushort)configuration.L410);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)configuration.L470);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL560, (ushort)configuration.L560);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacLaser, (ushort)configuration.LaserPower);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.Out0Conf, (byte)configuration.DigitalOutput0);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.Out1Conf, (byte)configuration.DigitalOutput1);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.In0Conf, (byte)configuration.DigitalInput0);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.In1Conf, (byte)configuration.DigitalInput1);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimWavelength, (ushort)configuration.LaserWavelength);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimPeriod, (ushort)configuration.PulsePeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimOn, (ushort)configuration.PulseWidth);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimReps, (ushort)configuration.PulseCount);
            if (saveRegisters)
            {
                yield return HarpCommand.WriteByte(ConfigurationRegisters.Reset, (byte)ResetDeviceConfiguration.Save);
            }
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
            if (e.ChangedItem.PropertyDescriptor.Name == nameof(configuration.TriggerState))
            {
                SetTriggerState();
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
                    propertyGrid.SelectedObject = configuration;
                    ValidateSettings();
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
            using (var calibrationDialog = new FP3001CalibrationEditorForm(instance, photometry.Process(instance.Generate(ledCalibration.Commands)), serviceProvider))
            {
                calibrationDialog.AddCalibrationControl(ledCalibration);
                calibrationDialog.Text = setupButton.Text;
                calibrationDialog.ShowDialog(this);
            }

            OpenDevice();
        }

        private void triggerStateView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var index = e.RowIndex + 1;
            var bounds = (RectangleF)e.RowBounds;
            bounds.Width = triggerStateView.GetColumnDisplayRectangle(0, false).X - triggerStateView.Margin.Left;
            e.Graphics.DrawString(index.ToString(), Font, SystemBrushes.ControlText, bounds, rowHeaderFormat);
        }

        private void triggerStateView_Validated(object sender, EventArgs e)
        {
            if (configuration == null) return;
            configuration.TriggerState = GetTriggerState();
        }

        private void triggerStateView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Led.Name].Value = nameof(FrameFlags.L410);
            e.Row.Cells[Out0.Name].Value = false;
            e.Row.Cells[Out1.Name].Value = false;
        }
    }
}
