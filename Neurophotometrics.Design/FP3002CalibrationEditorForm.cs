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
                handler => resetSettingsButton.Click += handler,
                handler => resetSettingsButton.Click -= handler)
                .Select(evt => ShouldResetPersistentRegisters())
                .Where(result => result != DialogResult.Cancel)
                .SelectMany(result => ResetRegisters(true))
                .Publish().RefCount();

            var loadSettings = Observable.FromEventPattern(
                handler => loadSettingsButton.Click += handler,
                handler => loadSettingsButton.Click -= handler)
                .Where(evt => openFileDialog.ShowDialog(this) == DialogResult.OK)
                .Select(evt => LoadSettings(openFileDialog.FileName))
                .Do(SetActiveConfiguration)
                .SelectMany(result => WriteRegisters(savePersistent: false));

            var saveSettings = Observable.FromEventPattern(
                handler => saveSettingsButton.Click += handler,
                handler => saveSettingsButton.Click -= handler)
                .Where(evt => saveFileDialog.ShowDialog(this) == DialogResult.OK)
                .Do(evt => SaveSettings(saveFileDialog.FileName, configuration))
                .Select(evt => ShouldSavePersistentRegisters())
                .SelectMany(result => WriteRegisters(result == DialogResult.Yes));

            var valueChanged = Observable.FromEventPattern<PropertyValueChangedEventHandler, PropertyValueChangedEventArgs>(
                handler => propertyGrid.PropertyValueChanged += handler,
                handler => propertyGrid.PropertyValueChanged -= handler)
                .Do(evt => HandlePropertyValueChanged(evt.EventArgs))
                .SelectMany(evt => WritePropertyRegister(evt.EventArgs.ChangedItem.PropertyDescriptor.Name));

            var storeDeviceSettings = Observable.Merge(loadSettings, saveSettings, valueChanged).Publish().RefCount();
            return device.Generate(storeDeviceSettings.Merge(resetDeviceSettings))
                .Where(IsReadMessage).Do(ReadRegister)
                .Throttle(TimeSpan.FromSeconds(0.2)).ObserveOn(propertyGrid).Do(message =>
                {
                    propertyGrid.Refresh();
                    SetConnectionStatus(ConnectionStatus.Ready);
                })
                .DelaySubscription(TimeSpan.FromSeconds(0.2))
                .TakeUntil(resetDeviceSettings.Merge(storeDeviceSettings)
                    .Where(ConfigurationRegisters.Reset)
                    .Delay(TimeSpan.FromSeconds(1)))
                .Do(x => { }, () => BeginInvoke((Action)ResetDevice));
        }

        private void ResetDevice()
        {
            CloseDevice();
            SetConnectionStatus(ConnectionStatus.Reset);
            using (var resetDialog = new FP3002ResetDialog())
            {
                resetDialog.ShowDialog(this);
            }
            OpenDevice();
        }

        private void OpenDevice()
        {
            SetConnectionStatus(ConnectionStatus.Open);
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

        private void ReadRegister(HarpMessage message)
        {
            switch (message.Address)
            {
                case ConfigurationRegisters.Config:
                    configuration.Config = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.ScreenBrightness:
                    configuration.ScreenBrightness = message.GetPayloadByte();
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
                case ConfigurationRegisters.DacL415:
                    configuration.L415 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL470:
                    configuration.L470 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacL560:
                    configuration.L560 = message.GetPayloadUInt16();
                    break;
                case ConfigurationRegisters.DacLaser:
                    configuration.PulseAmplitude = message.GetPayloadUInt16();
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
            configuration.Validate();
            propertyGrid.Refresh();
        }

        private void HandlePropertyValueChanged(PropertyValueChangedEventArgs e)
        {
            ValidateSettings();
            if (e.ChangedItem.PropertyDescriptor.Name == nameof(configuration.TriggerState))
            {
                SetTriggerState();
            }
        }

        private DialogResult ShouldSavePersistentRegisters()
        {
            ValidateSettings();
            SetTriggerState();
            return MessageBox.Show(this,
                Properties.Resources.SavePersistentRegisters_Question,
                Text, MessageBoxButtons.YesNo);
        }

        private DialogResult ShouldResetPersistentRegisters()
        {
            return MessageBox.Show(this,
                Properties.Resources.ResetPersistentRegisters_Question,
                Text, MessageBoxButtons.OKCancel);
        }

        IEnumerable<HarpMessage> ResetRegisters(bool resetDefault)
        {
            var resetMode = resetDefault ? ResetMode.RestoreDefault : ResetMode.RestoreEeprom;
            yield return HarpCommand.Reset(resetMode);
        }

        IEnumerable<HarpMessage> WritePropertyRegister(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(configuration.ClockSynchronizer):
                case nameof(configuration.Output0Routing):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.Config, (ushort)configuration.Config);
                    break;
                case nameof(configuration.ScreenBrightness):
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.ScreenBrightness, (byte)configuration.ScreenBrightness);
                    break;
                case nameof(configuration.FrameRate):
                case nameof(configuration.ExposureTime):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerPeriod, (ushort)configuration.TriggerPeriod);
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerTimeUpdateOutputs, (ushort)configuration.DwellTime);
                    break;
                case nameof(configuration.TriggerState):
                    var triggerState = configuration.TriggerState;
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, TriggerHelper.FromFrameFlags(triggerState));
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)triggerState.Length);
                    break;
                case nameof(configuration.L415):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL415, (ushort)configuration.L415);
                    break;
                case nameof(configuration.L470):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)configuration.L470);
                    break;
                case nameof(configuration.L560):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL560, (ushort)configuration.L560);
                    break;
                case nameof(configuration.PulseAmplitude):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacLaser, (ushort)configuration.PulseAmplitude);
                    break;
                case nameof(configuration.DigitalOutput0):
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.Out0Conf, (byte)configuration.DigitalOutput0);
                    break;
                case nameof(configuration.DigitalOutput1):
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.Out1Conf, (byte)configuration.DigitalOutput1);
                    break;
                case nameof(configuration.DigitalInput0):
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.In0Conf, (byte)configuration.DigitalInput0);
                    break;
                case nameof(configuration.DigitalInput1):
                    yield return HarpCommand.WriteByte(ConfigurationRegisters.In1Conf, (byte)configuration.DigitalInput1);
                    break;
                case nameof(configuration.LaserWavelength):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimWavelength, (ushort)configuration.LaserWavelength);
                    break;
                case nameof(configuration.PulseFrequency):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimPeriod, (ushort)configuration.PulsePeriod);
                    break;
                case nameof(configuration.PulseWidth):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimOn, (ushort)configuration.PulseWidth);
                    break;
                case nameof(configuration.PulseCount):
                    yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimReps, (ushort)configuration.PulseCount);
                    break;
                default: yield break;
            }
        }

        IEnumerable<HarpMessage> WriteRegisters(bool savePersistent)
        {
            var triggerState = configuration.TriggerState;
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.Config, (ushort)configuration.Config);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.ScreenBrightness, (byte)configuration.ScreenBrightness);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerState, TriggerHelper.FromFrameFlags(triggerState));
            yield return HarpCommand.WriteByte(ConfigurationRegisters.TriggerStateLength, (byte)triggerState.Length);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerPeriod, (ushort)configuration.TriggerPeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.TriggerTimeUpdateOutputs, (ushort)configuration.DwellTime);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL415, (ushort)configuration.L415);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)configuration.L470);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL560, (ushort)configuration.L560);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacLaser, (ushort)configuration.PulseAmplitude);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.Out0Conf, (byte)configuration.DigitalOutput0);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.Out1Conf, (byte)configuration.DigitalOutput1);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.In0Conf, (byte)configuration.DigitalInput0);
            yield return HarpCommand.WriteByte(ConfigurationRegisters.In1Conf, (byte)configuration.DigitalInput1);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimWavelength, (ushort)configuration.LaserWavelength);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimPeriod, (ushort)configuration.PulsePeriod);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimOn, (ushort)configuration.PulseWidth);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.StimReps, (ushort)configuration.PulseCount);
            if (savePersistent)
            {
                yield return HarpCommand.Reset(ResetMode.Save);
            }
        }

        IEnumerable<HarpMessage> StartStimulation()
        {
            yield return HarpCommand.WriteByte(ConfigurationRegisters.StimStart, (byte)CommandMode.Start);
        }

        IEnumerable<HarpMessage> LaserCalibration()
        {
            const ushort CalibrationPower = (ushort)(ushort.MaxValue * 0.1);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL415, LedPowerConverter.MinLedPower);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, LedPowerConverter.MinLedPower);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL560, LedPowerConverter.MinLedPower);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacLaser, CalibrationPower);
        }

        IEnumerable<HarpMessage> RestoreCalibration()
        {
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL415, (ushort)configuration.L415);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL470, (ushort)configuration.L470);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacL560, (ushort)configuration.L560);
            yield return HarpCommand.WriteUInt16(ConfigurationRegisters.DacLaser, (ushort)configuration.PulseAmplitude);
        }

        void SetActiveConfiguration(FP3002Configuration activeConfiguration)
        {
            propertyGrid.SelectedObject = activeConfiguration;
            configuration = activeConfiguration;
            ValidateSettings();
            SetTriggerState();
        }

        static FP3002Configuration LoadSettings(string fileName)
        {
            using (var reader = XmlReader.Create(fileName))
            {
                var serializer = new XmlSerializer(typeof(FP3002Configuration));
                var configuration = (FP3002Configuration)serializer.Deserialize(reader);
                return configuration;
            }
        }

        static void SaveSettings(string fileName, FP3002Configuration configuration)
        {
            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true }))
            {
                var serializer = new XmlSerializer(typeof(FP3002Configuration));
                serializer.Serialize(writer, configuration);
            }
        }

        void SetConnectionStatus(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Open:
                    connectionStatusLabel.Text = "Connecting to device...";
                    break;
                case ConnectionStatus.Ready:
                    connectionStatusLabel.Text = $"Ready ({instance.PortName})";
                    break;
                case ConnectionStatus.Reset:
                    connectionStatusLabel.Text = "Resetting device...";
                    break;
                default:
                    break;
            }
        }

        enum ConnectionStatus
        {
            Open,
            Ready,
            Reset
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

        private void setupRegionsButton_Click(object sender, EventArgs e)
        {
            CloseDevice();
            using (var ledCalibration = new LedCalibrationEditor(configuration))
            using (var calibrationDialog = new FP3001CalibrationEditorForm(instance, photometry.Process(instance.Generate(ledCalibration.Commands)), serviceProvider))
            {
                calibrationDialog.AddCalibrationControl(ledCalibration);
                calibrationDialog.Text = setupRegionsButton.Text;
                calibrationDialog.Icon = Icon;
                calibrationDialog.ShowDialog(this);
            }

            OpenDevice();
        }

        private void setupLaserButton_Click(object sender, EventArgs e)
        {
            CloseDevice();

            using (var calibrationDialog = new LaserCalibrationDialog(configuration))
            {
                var setLaserPower = Observable.FromEventPattern(
                    handler => calibrationDialog.Load += handler,
                    handler => calibrationDialog.Load -= handler)
                    .SelectMany(evt => LaserCalibration());
                var stimulationTest = Observable.FromEventPattern(
                    handler => calibrationDialog.StimulationTest += handler,
                    handler => calibrationDialog.StimulationTest -= handler)
                    .SelectMany(evt => StartStimulation());
                var valueChanged = Observable.FromEventPattern<PropertyValueChangedEventHandler, PropertyValueChangedEventArgs>(
                    handler => calibrationDialog.PropertyValueChanged += handler,
                    handler => calibrationDialog.PropertyValueChanged -= handler)
                    .SelectMany(evt => WritePropertyRegister(evt.EventArgs.ChangedItem.PropertyDescriptor.Name));
                var resetLaserPower = Observable.FromEventPattern(
                    handler => calibrationDialog.HandleDestroyed += handler,
                    handler => calibrationDialog.HandleDestroyed -= handler)
                    .SelectMany(evt => RestoreCalibration());
                var commands = Observable.Merge(setLaserPower, stimulationTest, valueChanged, resetLaserPower);
                calibrationDialog.Text = setupLaserButton.Text;
                calibrationDialog.Icon = Icon;
                calibrationDialog.ShowDialog(this, photometry.Process(instance.Generate(commands)));
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
            e.Row.Cells[Led.Name].Value = nameof(FrameFlags.L415);
            e.Row.Cells[Out0.Name].Value = false;
            e.Row.Cells[Out1.Name].Value = false;
        }
    }
}
