using Bonsai.Design;
using Bonsai.Harp;
using Neurophotometrics.Design.Properties;
using Neurophotometrics.Design.V2.Editors;
using Neurophotometrics.V2;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Forms
{
    internal partial class FP3002Form : Form
    {
        private const ushort DefaultTriggerPeriod = 25000;
        private const ushort DefaultTriggerPeriodUpdateOutputs = 24500;
        private const int WAIT_TIME = 200;
        private const int TIMEOUT = 5000;
        private const int CLOSE_TIME = 100;

        private FP3002 FP3002;
        private PhotometryData PhotometryData;
        private SettingsConfigurator SettingsConfigurator;
        private CompositeDisposable Subscriptions;

        private IObservable<RegSettingChangedEventArgs> UIComms;
        private IObservable<PhotometryDataFrame> FP3002Comms;

        public event RegSettingChangedEventHandler TabChanged;

        public event EventHandler ReadyToAcquireEvent;

        private event EventHandler WriteSettings;

        private event EventHandler WriteToPersistentRegistersEvent;

        private event EventHandler ResetPersistentRegistersEvent;

        private bool ReadyToAcquire;
        private bool BeganWritingSettings;

        public FP3002Form(FP3002 fp3002)
        {
            InitializeComponent();
            InitializeMembers(fp3002);
            InitializePiping();
            FP3002.SetTriggerPeriod(DefaultTriggerPeriod);
            FP3002.SetExposureTime(DefaultTriggerPeriodUpdateOutputs);
            StartSubscription();

            SplashScreen.CloseSplash();

            if (!ReadyToAcquire)
                CloseWithErrorMessage(Resources.MsgBox_Warning_VerifyDeviceConnected);
            else
                TryValidateFirmware();
        }

        private void InitializeMembers(FP3002 fp3002)
        {
            FP3002 = fp3002;
            PhotometryData = new PhotometryData();
            SettingsConfigurator = new SettingsConfigurator();
            ROIControl_Editor.Regions = FP3002.Regions;
            SetFPSHeight();
            Icon = Icon.FromHandle(Resources.Neurophotometrics.GetHicon());
        }

        private void SetFPSHeight()
        {
            var fpsLabelHeight = FPS_Label.PreferredHeight;
            var fpsLabelWidth = FPS_Label.Width;
            var triggerPeriodSliderHeight = TriggerPeriod_Slider.PreferredSize.Height;
            var triggerPeriodSliderWidth = TriggerPeriod_Slider.Width;
            var newHeight = Math.Max(fpsLabelHeight, triggerPeriodSliderHeight) * 2;
            FPS_Label.AutoSize = false;
            TriggerPeriod_Slider.AutoSize = false;
            FPS_Label.Size = new Size(fpsLabelWidth, newHeight);
            TriggerPeriod_Slider.Size = new Size(triggerPeriodSliderWidth, newHeight);
        }

        private void InitializePiping()
        {
            // Layer 3: UI Communication
            var EditorsToSettings = CreateEditorsToSettingsCommunication();
            var SettingsToEditors = CreateSettingsToEditorsCommunication();

            UIComms = Observable.Merge(EditorsToSettings, SettingsToEditors)
                .ObserveOn(TrigSeq_SignalTimingVisualizer)
                .Do(arg => TrigSeq_SignalTimingVisualizer.TryUpdateSignals(arg));

            //Layer 2: FP3002 Commands
            var UISettingsToFP3002 = CreateUISettingsToFP3002Communication();
            var HarpMessagesToFP3002 = CreateHarpMessagesToFP3002Communication();

            var FP3002Commands = Observable.Merge(HarpMessagesToFP3002, UISettingsToFP3002);

            // Layer 1: FP3002 Output Routing
            var FP3002Routing = FP3002.Generate(FP3002Commands)
                .Do(ProcessMessages);

            // Layer 0: Photometry Data Routing
            FP3002Comms = PhotometryData.Process(FP3002Routing)
                .ObserveOn(Scheduler.Default)
                .Do(UpdateDataVisualizer)
                .Sample(TimeSpan.FromMilliseconds(25.0))
                .Do(UpdateROIControl)
                .Sample(TimeSpan.FromMilliseconds(50.0))
                .Do(UpdateUI);
        }

        private void TryValidateFirmware()
        {
            try
            {
                ValidateFirmware();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ValidateFirmware()
        {
            var deviceFirmware = SettingsConfigurator.TryGetFirmwareMetadata();
            var targetFirmware = FP3002.DeviceInfo.TryGetTargetFirmwareMetaData();

            if (deviceFirmware == null || targetFirmware == null)
                return;

            if (!targetFirmware.Supports(nameof(FP3002), deviceFirmware.HardwareVersion))
                CloseWithErrorMessage(Resources.MsgBox_Error_UnsupportedHWVersion);
            else if (ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt))
                UpdateFW();
            else if (deviceFirmware.FirmwareVersion > targetFirmware.FirmwareVersion)
                CloseWithErrorMessage(Resources.MsgBox_Error_UpdateDriverVersion);
            else if (!deviceFirmware.FirmwareVersion.Satisfies(targetFirmware.FirmwareVersion))
            {
                var updateFWRes = MessageBox.Show(this, Resources.MsgBox_Question_UpdateDeviceFW, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (updateFWRes == DialogResult.Yes)
                    UpdateFW();
                else if (deviceFirmware.FirmwareVersion.Major < targetFirmware.FirmwareVersion.Major)
                    CloseWithErrorMessage(Resources.MsgBox_Error_UnsupportedFWVersion);
            }
        }


        private void CloseWithErrorMessage(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
        private void UpdateFW()
        {
            DisposeSubscription();
            var targetDeviceFirmware = FP3002.DeviceInfo.TryGetTargetFirmware();
            using (var firmwareDialog = new FP3002FirmwareDialog(FP3002.PortName, targetDeviceFirmware))
            {
                firmwareDialog.ShowDialog(this);
            }

            ResetFP3002();
        }

        private void ResetFP3002()
        {
            ReadyToAcquire = false;
            Regions_PhotometryDataVisualizer.Clear();
            SplashScreen.ShowSplash();
            TryFindSystem();
            FP3002.SetTriggerPeriod(DefaultTriggerPeriod);
            FP3002.SetExposureTime(DefaultTriggerPeriodUpdateOutputs);
            StartSubscription();
            SplashScreen.CloseSplash();

            if (!ReadyToAcquire)
                CloseWithErrorMessage(Resources.MsgBox_Warning_CouldNotReconnectSystem);
            else
                ReadyToAcquireEvent?.Invoke(this, EventArgs.Empty);
        }

        private void TryFindSystem()
        {
            try
            {
                FP3002.TryFindSystem();
            }
            catch (TimeoutException)
            {
                SplashScreen.CloseSplash();
                CloseWithErrorMessage(Resources.MsgBox_Warning_CouldNotReconnectSystem);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StartSubscription()
        {
            Subscriptions = new CompositeDisposable(UIComms.Subscribe(), FP3002Comms.Subscribe());
            var totalWaitTime = 0;
            while (!ReadyToAcquire)
            {
                Thread.Sleep(WAIT_TIME);
                totalWaitTime += WAIT_TIME;
                if (totalWaitTime >= TIMEOUT)
                    throw new TimeoutException();
            }
        }

        private void UpdateDataVisualizer(PhotometryDataFrame pdf)
        {
            Regions_PhotometryDataVisualizer.TryUpdatePlots(pdf.Activities, pdf.SystemTimestamp);
        }

        private void UpdateROIControl(PhotometryDataFrame pdf)
        {
            ROIControl_Editor.TryUpdateNewFrame(pdf.PhotometryImage);
        }

        private void UpdateUI(PhotometryDataFrame pdf)
        {
            Regions_PhotometryDataVisualizer.TryRefreshPlot();
        }

        private IObservable<RegSettingChangedEventArgs> CreateEditorsToSettingsCommunication()
        {
            var ROIToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => ROIControl_Editor.ROIsChanged += handler,
                handler => ROIControl_Editor.ROIsChanged -= handler)
                .Select(evt => evt.EventArgs)
                .Do(arg => FP3002.Regions = (List<PhotometryRegion>)arg.Value);
            var TrigSeqToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => TrigSeqControl_Editor.TrigSeqChanged += handler,
                handler => TrigSeqControl_Editor.TrigSeqChanged -= handler)
                .Select(evt => evt.EventArgs);
            var LEDPowerToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => LEDPowersControl_Editor.LEDPowerSettingChanged += handler,
                handler => LEDPowersControl_Editor.LEDPowerSettingChanged -= handler)
                .Select(evt => evt.EventArgs);
            var PulseTrainToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => PulseTrainControl_Editor.PulseTrainSettingsChanged += handler,
                handler => PulseTrainControl_Editor.PulseTrainSettingsChanged -= handler)
                .Select(evt => evt.EventArgs);
            var FPSToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => TriggerPeriod_Slider.TriggerPeriodChanged += handler,
                handler => TriggerPeriod_Slider.TriggerPeriodChanged -= handler)
                .Select(evt => evt.EventArgs);
            var MiscToSettings = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => MiscellaneousControl_Editor.SettingChanged += handler,
                handler => MiscellaneousControl_Editor.SettingChanged -= handler)
                .Select(evt => evt.EventArgs);

            var EditorsToSettings = Observable.Merge(TrigSeqToSettings, ROIToSettings, LEDPowerToSettings, PulseTrainToSettings, FPSToSettings, MiscToSettings)
                .Do(arg => SettingsConfigurator.UpdateProp(arg));
            return EditorsToSettings;
        }

        private IObservable<RegSettingChangedEventArgs> CreateSettingsToEditorsCommunication()
        {
            var SettingsEvents = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                   handler => SettingsConfigurator.SettingChanged += handler,
                   handler => SettingsConfigurator.SettingChanged -= handler)
                   .Select(evt => evt.EventArgs);

            var SettingsToTrigSeq = SettingsEvents.Where(IsTrigSeq).Do(TrigSeqControl_Editor.TryUpdateTrigSeq);
            var SettingsToLEDPower = SettingsEvents.Where(IsLEDPower).Do(LEDPowersControl_Editor.TryUpdateLEDPowers);
            var SettingsToPulseTrain = SettingsEvents.Where(IsPulseTrain).Do(PulseTrainControl_Editor.TryUpdatePulseTrain);
            var SettingsToFPS = SettingsEvents.Where(IsFPS).Do(UpdateFPS);
            var SettingsToMisc = SettingsEvents.Where(IsMisc).Do(MiscellaneousControl_Editor.TryUpdateMiscSetting);

            var SetttingsToEditors = Observable.Merge(SettingsToTrigSeq, SettingsToLEDPower, SettingsToPulseTrain, SettingsToFPS, SettingsToMisc);
            return SetttingsToEditors;
        }

        private IObservable<HarpMessage> CreateUISettingsToFP3002Communication()
        {
            var ROIToFP3002 = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                   handler => ROIControl_Editor.LEDPowerFP3002Changed += handler,
                   handler => ROIControl_Editor.LEDPowerFP3002Changed -= handler)
                   .Select(evt => evt.EventArgs);
            var TabControlToFP3002 = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => TabChanged += handler,
                handler => TabChanged -= handler)
                .Select(evt => evt.EventArgs);
            var LEDPowerToFP3002 = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => LEDPowersControl_Editor.LEDPowerFP3002Changed += handler,
                handler => LEDPowersControl_Editor.LEDPowerFP3002Changed -= handler)
                .Select(evt => evt.EventArgs);
            var PulseTrainToFP3002 = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => PulseTrainControl_Editor.PulseTrainFP3002Changed += handler,
                handler => PulseTrainControl_Editor.PulseTrainFP3002Changed -= handler)
                .Select(evt => evt.EventArgs)
                .Do(HandleStartAndStopLaserCalibration);
            var DigitalIOsToFP3002 = Observable.FromEventPattern<RegSettingChangedEventHandler, RegSettingChangedEventArgs>(
                handler => MiscellaneousControl_Editor.FP3002RegisterChanged += handler,
                handler => MiscellaneousControl_Editor.FP3002RegisterChanged -= handler)
                .Select(evt => evt.EventArgs);

            var UIToFP3002 = Observable.Merge(ROIToFP3002, TabControlToFP3002, LEDPowerToFP3002, PulseTrainToFP3002, DigitalIOsToFP3002)
                .SelectMany(arg => SettingsConfigurator.WritePropToReg(arg));
            return UIToFP3002;
        }

        private IObservable<HarpMessage> CreateHarpMessagesToFP3002Communication()
        {
            var CaliSettingToFP3002 = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => ReadyToAcquireEvent += handler,
                handler => ReadyToAcquireEvent -= handler)
                .Do(evt => SettingsConfigurator.NotifyVisualizers())
                .SelectMany(evt => SettingsConfigurator.GenCaliSettings());
            var StoreSettings = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => WriteSettings += handler,
                handler => WriteSettings -= handler)
                .SelectMany(evt => SettingsConfigurator.StoreSettings());
            var WritePersistentRegisters = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => WriteToPersistentRegistersEvent += handler,
                handler => WriteToPersistentRegistersEvent -= handler)
                .SelectMany(evt => SettingsConfigurator.WriteToPersistentRegisters());
            var ResetPersistentRegisters = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => ResetPersistentRegistersEvent += handler,
                handler => ResetPersistentRegistersEvent -= handler)
                .Select(evt => SettingsConfigurator.GenResetPersistentRegisters());
            var HarpMessagesToFP3002 = Observable.Merge(CaliSettingToFP3002, StoreSettings, WritePersistentRegisters, ResetPersistentRegisters);
            return HarpMessagesToFP3002;
        }

        private void HandleStartAndStopLaserCalibration(RegSettingChangedEventArgs arg)
        {
            if (arg.Name == "StartCalLaser" && ((PulseTrain)arg.Value).StimPeriod <= 1000)
            {
                ROIControl_Editor.DisableUI();
                ROIControl_Editor.SetLEDToPD();
                Regions_TableLayoutPanel.Controls.Remove(ROIControl_Editor);
                Laser_TableLayoutPanel.Controls.Remove(TrigSeq_SignalTimingVisualizer);
                Laser_TableLayoutPanel.Controls.Add(ROIControl_Editor, 1, 0);
                ROIControl_Editor.SetImageScale(1);
            }
            else if (arg.Name == "StopCalLaser" && Laser_TableLayoutPanel.Controls.Contains(ROIControl_Editor))
            {
                Laser_TableLayoutPanel.Controls.Remove(ROIControl_Editor);
                Laser_TableLayoutPanel.Controls.Add(TrigSeq_SignalTimingVisualizer, 1, 0);
                Regions_TableLayoutPanel.Controls.Add(ROIControl_Editor, 0, 0);
                ROIControl_Editor.SetImageScale(11);
                ROIControl_Editor.ResetTab();
            }
        }

        private void ProcessMessages(HarpMessage msg)
        {
            ProcessReadMessages(msg);
            ProcessStartMessage(msg);
        }

        private void ProcessReadMessages(HarpMessage msg)
        {
            if (!IsReadMessage(msg)) return;

            SettingsConfigurator.TryReadRegister(msg);
            if (ReadyToAcquire)
                SettingsConfigurator.NotifyChangeInReg(msg);
        }

        private void ProcessStartMessage(HarpMessage msg)
        {
            if (!IsStartMessage(msg) || IsReadMessage(msg) || ReadyToAcquire) return;

            var isStart = msg.GetPayloadByte();
            if (isStart == (byte)(AcquisitionModes.StopExternalCamera | AcquisitionModes.StopPhotometry))
                ReadyToAcquire = true;
        }

        private void UpdateFPS(RegSettingChangedEventArgs arg)
        {
            TriggerPeriod_Slider.SetTriggerPeriod((ushort)arg.Value);
        }

        private static bool IsTrigSeq(RegSettingChangedEventArgs args)
        {
            return args.Name == nameof(FP3002Settings.TriggerSequence) || args.Name == nameof(FP3002Settings.TriggerPeriod) || args.Name == nameof(FP3002Settings.TriggerSequenceLength);
        }

        private static bool IsLEDPower(RegSettingChangedEventArgs args)
        {
            return args.Name == nameof(FP3002Settings.LedPowers) || args.Name == nameof(FP3002Settings.LedPowers.L415) ||
                   args.Name == nameof(FP3002Settings.LedPowers.L470) || args.Name == nameof(FP3002Settings.LedPowers.L560);
        }

        private static bool IsPulseTrain(RegSettingChangedEventArgs args)
        {
            return args.Name == nameof(FP3002Settings.PulseTrain) || args.Name == nameof(FP3002Settings.PulseTrain.LaserWavelength) ||
                   args.Name == nameof(FP3002Settings.PulseTrain.LaserAmplitude) || args.Name == nameof(FP3002Settings.PulseTrain.StimPeriod) ||
                   args.Name == nameof(FP3002Settings.PulseTrain.StimOn) || args.Name == nameof(FP3002Settings.PulseTrain.StimReps) ||
                   args.Name == nameof(FP3002Settings.StimKeySwitchState);
        }

        private static bool IsMisc(RegSettingChangedEventArgs args)
        {
            return args.Name == nameof(FP3002Settings.ClockSynchronizer) || args.Name == nameof(FP3002Settings.ScreenBrightness) ||
                   args.Name == nameof(FP3002Settings.DigitalInput0) || args.Name == nameof(FP3002Settings.DigitalInput1) ||
                   args.Name == nameof(FP3002Settings.DigitalOutput0) || args.Name == nameof(FP3002Settings.Output1Routing);
        }

        private static bool IsFPS(RegSettingChangedEventArgs args)
        {
            return args.Name == nameof(FP3002Settings.TriggerPeriod);
        }

        private static bool IsReadMessage(HarpMessage message)
        {
            return message.MessageType == MessageType.Read || (message.MessageType == MessageType.Event && message.Address == Registers.StimKeySwitchState);
        }

        private static bool IsStartMessage(HarpMessage message)
        {
            return message.Address == Registers.Start;
        }

        protected override void OnLoad(EventArgs e)
        {
            ReadyToAcquireEvent?.Invoke(this, EventArgs.Empty);
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!BeganWritingSettings)
            {
                e.Cancel = true;
                WriteSettings?.Invoke(this, EventArgs.Empty);
                BeganWritingSettings = true;
                RunAsyncDelay();
            }
            else
            {
                if (Subscriptions != null)
                {
                    Subscriptions.Dispose();
                    Subscriptions = null;
                }
                base.OnFormClosing(e);
            }
        }

        private void RunAsyncDelay()
        {
            Task.Run(async () =>
            {
                await Task.Delay(CLOSE_TIME);
                Invoke((MethodInvoker)delegate {
                    Close();
                });
            });
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DisposeSubscription();
            base.OnFormClosed(e);
        }

        private void DisposeSubscription()
        {
            if (Subscriptions != null)
            {
                Subscriptions.Dispose();
                Subscriptions = null;
            }
        }

        private void Load_Button_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSettings();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void LoadSettings()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files|*.xml|All files|*.*";
                var result = openFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var settingsSerializer = new FP3002SettingsSerializer(openFileDialog.FileName);
                    settingsSerializer.TryReadSettings();
                    SettingsConfigurator.Settings = settingsSerializer.GetSettings();
                    SettingsConfigurator.NotifyVisualizers();
                    ROIControl_Editor.Regions = SettingsConfigurator.Settings.Regions;
                    ROIControl_Editor.UpdateRegionChannels();
                    FP3002.Regions = ROIControl_Editor.Regions;
                    SettingsConfigurator.Settings.Regions = ROIControl_Editor.Regions;
                    Regions_PhotometryDataVisualizer.Clear();
                }
            }
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void SaveSettings()
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = "FP3002Config.xml";
                saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
                var result = saveFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    SettingsConfigurator.Settings.Regions = ROIControl_Editor.Regions;
                    var settingsSerializer = new FP3002SettingsSerializer(saveFileDialog.FileName, SettingsConfigurator.Settings);
                    settingsSerializer.TryWriteSettings();
                }
            }
        }

        private void Editors_TabControl_Deselected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == ROICalibration_TabPage)
            {
                ROIControl_Editor.ResetTab();
            }
            else if (e.TabPage == TrigSeqCalibration_TabPage)
            {
                TrigSeq_TableLayoutPanel.Controls.Remove(TrigSeq_SignalTimingVisualizer);
            }
            else if (e.TabPage == LEDCalibration_TabPage)
            {
                LEDPowersControl_Editor.ResetTab();
            }
            else if (e.TabPage == LaserCalibration_TabPage)
            {
                PulseTrainControl_Editor.ResetTab();

                if (Laser_TableLayoutPanel.Controls.Contains(ROIControl_Editor))
                {
                    Laser_TableLayoutPanel.Controls.Remove(ROIControl_Editor);
                    Laser_TableLayoutPanel.Controls.Add(TrigSeq_SignalTimingVisualizer);
                    Regions_TableLayoutPanel.Controls.Add(ROIControl_Editor, 0, 0);
                    ROIControl_Editor.ResetTab();
                }
                else
                {
                    Laser_TableLayoutPanel.Controls.RemoveAt(1);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Editors_TabControl.SelectedIndex += sender == Next_Button ? 1 : -1;
        }

        private void Editors_TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == TrigSeqCalibration_TabPage)
            {
                TrigSeq_TableLayoutPanel.Controls.Add(TrigSeq_SignalTimingVisualizer, 1, 1);
            }
            else if (e.TabPage == LaserCalibration_TabPage)
            {
                Laser_TableLayoutPanel.Controls.Add(TrigSeq_SignalTimingVisualizer, 1, 0);
            }

            TabChanged?.Invoke(this, new RegSettingChangedEventArgs("TabPage", e.TabPageIndex));
        }

        private void Editors_TabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == ROICalibration_TabPage)
            {
                Prev_Button.Visible = false;
                Next_Button.Visible = true;
                ROIControl_Editor.SetImageScale(11);
                ROIControl_Editor.EnableUI();
                DescTitle_Label.Text = "Emission Alignment Calibration:";
                DescText_Label.Text = Resources.EditorDesc_ROIs;
            }
            else if (e.TabPage == TrigSeqCalibration_TabPage)
            {
                Prev_Button.Visible = true;
                Next_Button.Visible = true;
                DescTitle_Label.Text = "Excitation Sequence Calibration:";
                DescText_Label.Text = Resources.EditorDesc_TrigSeq;
                TrigSeq_SignalTimingVisualizer.Invalidate();
            }
            else if (e.TabPage == LEDCalibration_TabPage)
            {
                Prev_Button.Visible = true;
                Next_Button.Visible = true;
                DescTitle_Label.Text = "Excitation Power Calibration:";
                DescText_Label.Text = Resources.EditorDesc_LEDs;
            }
            else if (e.TabPage == LaserCalibration_TabPage)
            {
                Prev_Button.Visible = true;
                Next_Button.Visible = true;
                DescTitle_Label.Text = "Opto-Stimulation Calibration:";
                DescText_Label.Text = Resources.EditorDesc_Laser_Base;
                TrigSeq_SignalTimingVisualizer.Invalidate();
                MessageBox.Show(Resources.MsgBox_Question_EnableOpto, "Enable Opto-Stimulation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else if (e.TabPage == Miscellaneous_TabPage)
            {
                Prev_Button.Visible = true;
                Next_Button.Visible = false;
                DescTitle_Label.Text = "Digital IOs Calibration:";
                DescText_Label.Text = Resources.EditorDesc_Misc;
            }
        }

        private void FP3002Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (Editors_TabControl.SelectedTab == ROICalibration_TabPage)
                ROIControl_Editor.TryUpdateKeyDown(e);

            if (e.Shift && e.Control && e.KeyCode == Keys.R)
                TryResetPersistentRegisters();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab && Editors_TabControl.SelectedTab == ROICalibration_TabPage)
            {
                ROIControl_Editor.TabSelectROI();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PersistReg_Button_Click(object sender, EventArgs e)
        {
            TryWriteToPersistentRegisters();
        }

        private void TryWriteToPersistentRegisters()
        {
            try
            {
                WriteToPersistentRegisters();
            }
            catch (TimeoutException ex)
            {
                SplashScreen.CloseSplash();
                MessageBox.Show(Resources.MsgBox_Error_TimeoutFindSystem, $"Error: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void WriteToPersistentRegisters()
        {
            var result = MessageBox.Show(Resources.MsgBox_Question_WriteToPersistentRegisters, "Write To Persistent Registers:", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            WriteToPersistentRegistersEvent?.Invoke(this, EventArgs.Empty);

            DisposeSubscription();
            using (var resetDialog = new FP3002ResetDialog())
            {
                resetDialog.ShowDialog(this);
            }
            ResetFP3002();
        }

        private void TryResetPersistentRegisters()
        {
            try
            {
                ResetPersistentRegisters();
            }
            catch (TimeoutException ex)
            {
                SplashScreen.CloseSplash();
                MessageBox.Show(Resources.MsgBox_Error_TimeoutFindSystem, $"Error: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ResetPersistentRegisters()
        {
            var result = MessageBox.Show(Resources.MsgBox_Question_ResetPersistentRegisters, "Reset Persistent Registers:", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            ResetPersistentRegistersEvent?.Invoke(this, EventArgs.Empty);

            DisposeSubscription();
            using (var resetDialog = new FP3002ResetDialog())
            {
                resetDialog.ShowDialog(this);
            }

            ResetFP3002();
        }
    }
}