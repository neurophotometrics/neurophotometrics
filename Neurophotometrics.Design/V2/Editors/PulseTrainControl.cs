using Neurophotometrics.Design.V2.Converters;

using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class PulseTrainControl : UserControl
    {
        public event RegSettingChangedEventHandler PulseTrainSettingsChanged;

        public event RegSettingChangedEventHandler PulseTrainFP3002Changed;

        private bool IsMeasuringPower = false;
        private bool IsHighAmplitude = false;
        private bool IsAligningLaser = false;
        private byte StimKeySwitchState;

        private const ushort MeasurePowerStimPeriod_Default = 30000;
        private const ushort MeasurePowerStimPeriod_Long = 35000;
        private const ushort MeasurePowerStimOn = 30000;

        private const ushort AlignLaserAmplitude_Default = ushort.MaxValue / 10;
        private const ushort AlignLaserAmplitude_Max = (ushort)(ushort.MaxValue * 0.3);
        private const ushort AlignLaserStimPeriod = 25;
        private const ushort AlignLaserStimOn = 1;

        private const ushort CalibrationStimReps = 1;

        private const string StartCalibrateLaserCommandName = "StartCalLaser";
        private const string StopCalibrateLaserCommandName = "StopCalLaser";

        public PulseTrainControl()
        {
            InitializeComponent();

            Wavelength_ComboBox.DataSource = LaserWavelengthConverter.TextToWavelength.ToList();
            Wavelength_ComboBox.DisplayMember = "Key";
            Wavelength_ComboBox.ValueMember = "Value";

            ConfigureDutyCycleColumnLabels();

            Amplitude_Slider.LaserAmplitudeChanged += Slider_SettingValueChanged;
            StimPeriod_Slider.StimPeriodChanged += Slider_SettingValueChanged;
            StimOn_Slider.StimOnChanged += Slider_SettingValueChanged;
            StimReps_Control.StimRepsChanged += Slider_SettingValueChanged;
        }

        private void ConfigureDutyCycleColumnLabels()
        {
            DummyLabel_Amplitude.ForeColor = Color.Transparent;
            DummyLabel_StimPeriod.ForeColor = Color.Transparent;
            DummyLabel_Amplitude.BackColor = Color.Transparent;
            DummyLabel_StimPeriod.BackColor = Color.Transparent;
            DutyCycle_Label.BackColor = Color.Transparent;

            var currentText = DutyCycle_Label.Text;
            DutyCycle_Label.Text = "100%";
            var prefWidth = DutyCycle_Label.PreferredWidth;
            DutyCycle_Label.AutoSize = false;
            DummyLabel_Amplitude.AutoSize = false;
            DummyLabel_StimPeriod.AutoSize = false;
            DutyCycle_Label.Width = prefWidth;
            DummyLabel_StimPeriod.Width = prefWidth;
            DummyLabel_Amplitude.Width = prefWidth;
            DutyCycle_Label.Text = currentText;
        }

        internal void ResetTab()
        {
            Wavelength_ComboBox.Enabled = true;
            Amplitude_Slider.Enabled = true;
            StimPeriod_Slider.Enabled = true;
            StimOn_Slider.Enabled = true;
            StimReps_Control.Enabled = true;

            var wavelength = LaserWavelengthConverter.GetWavelength(Wavelength_ComboBox.SelectedValue);
            MeasPower_Button.Enabled = LaserWavelengthConverter.IsLaserEnabled(wavelength);
            AlignLaser_Button.Enabled = LaserWavelengthConverter.Is635LaserEnabled(wavelength);

            AlignLaser_Button.Text = "Align Laser";
            MeasPower_Button.Text = "Measure Power";

            IsMeasuringPower = false;
            IsAligningLaser = false;
        }

        internal void TryUpdatePulseTrain(RegSettingChangedEventArgs args)
        {
            try
            {
                UpdatePulseTrain(args);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdatePulseTrain(RegSettingChangedEventArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(FP3002Settings.PulseTrain):
                    var pulseTrain = (PulseTrain)arg.Value;
                    Wavelength_ComboBox.SelectedValue = arg.Value;
                    Amplitude_Slider.SetLaserAmplitude(pulseTrain.LaserAmplitude);
                    StimPeriod_Slider.SetStimPeriod(pulseTrain.StimPeriod);
                    StimOn_Slider.SetStimOnLimit(pulseTrain.StimPeriod);
                    StimOn_Slider.SetStimOn(pulseTrain.StimOn);
                    StimReps_Control.TrySetStimReps(pulseTrain.StimReps);
                    break;

                case nameof(FP3002Settings.PulseTrain.LaserWavelength):
                    Wavelength_ComboBox.SelectedValue = arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.LaserAmplitude):
                    Amplitude_Slider.SetLaserAmplitude((ushort)arg.Value);
                    break;

                case nameof(FP3002Settings.PulseTrain.StimPeriod):
                    StimPeriod_Slider.SetStimPeriod((ushort)arg.Value);
                    StimOn_Slider.SetStimOnLimit((ushort)arg.Value);
                    UpdateDutyCycle();
                    break;

                case nameof(FP3002Settings.PulseTrain.StimOn):
                    StimOn_Slider.SetStimOn((ushort)arg.Value);
                    UpdateDutyCycle();
                    break;

                case nameof(FP3002Settings.PulseTrain.StimReps):
                    StimReps_Control.TrySetStimReps((ushort)arg.Value);
                    break;

                case nameof(FP3002Settings.StimKeySwitchState):
                    StimKeySwitchState = (byte)arg.Value;
                    break;
            }
        }

        private void UpdateDutyCycle()
        {
            var pulseWidth = StimOn_Slider.GetStimOn();
            var period = StimPeriod_Slider.GetStimPeriod();
            var dutyCycle = pulseWidth * 100.0f / period;
            DutyCycle_Label.Text = dutyCycle.ToString("0.#") + "%";
            DummyLabel_Amplitude.Text = dutyCycle.ToString("0.#") + "%";
            DummyLabel_StimPeriod.Text = dutyCycle.ToString("0.#") + "%";
        }

        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        private void Slider_SettingValueChanged(object sender, RegSettingChangedEventArgs args)
        {
            TryHandleSettingValueChanged(args);
        }

        private void TryHandleSettingValueChanged(RegSettingChangedEventArgs args)
        {
            try
            {
                HandleSettingValueChanged(args);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void HandleSettingValueChanged(RegSettingChangedEventArgs args)
        {
            EnsureDutyCycle(args);
            EnsureAmplitude(ref args);

            PulseTrainSettingsChanged?.Invoke(this, args);
        }

        private void EnsureDutyCycle(RegSettingChangedEventArgs args)
        {
            if (args.Name == nameof(FP3002Settings.PulseTrain.StimPeriod))
            {
                StimOn_Slider.SetStimOnLimit((ushort)args.Value);
                UpdateDutyCycle();
            }
            else if (args.Name == nameof(FP3002Settings.PulseTrain.StimOn))
                UpdateDutyCycle();
            else if (args.Name == nameof(FP3002Settings.PulseTrain.LaserAmplitude) && IsMeasuringPower)
            {
                UpdatePeriod();
            }
        }

        private void UpdatePeriod()
        {
            var isHighAmplitude = Amplitude_Slider.GetIsHighAmplitude();
            var isChanged = isHighAmplitude != IsHighAmplitude;

            var calibrationStimPeriod = MeasurePowerStimPeriod_Default;
            IsHighAmplitude = Amplitude_Slider.GetIsHighAmplitude();
            if (IsHighAmplitude)
                calibrationStimPeriod = MeasurePowerStimPeriod_Long;

            if (isChanged)
                PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.StimPeriod), calibrationStimPeriod));
        }

        private void EnsureAmplitude(ref RegSettingChangedEventArgs args)
        {
            if (args.Name != nameof(FP3002Settings.PulseTrain.LaserAmplitude))
                return;

            if (IsAligningLaser)
            {
                args.Value = Math.Min((ushort)args.Value, AlignLaserAmplitude_Max);
                Amplitude_Slider.SetLaserAmplitude((ushort)args.Value); PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.LaserAmplitude), (ushort)args.Value));
            }
            else if (IsMeasuringPower)
                PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.LaserAmplitude), (ushort)args.Value));
        }

        private void Wavelength_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryUpdateWavelength();
        }

        private void TryUpdateWavelength()
        {
            try
            {
                UpdateWavelength();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateWavelength()
        {
            var wavelength = LaserWavelengthConverter.GetWavelength(Wavelength_ComboBox.SelectedValue);
            MeasPower_Button.Enabled = LaserWavelengthConverter.IsLaserEnabled(wavelength);
            AlignLaser_Button.Enabled = LaserWavelengthConverter.Is635LaserEnabled(wavelength);

            PulseTrainSettingsChanged?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.LaserWavelength), wavelength));
        }

        private void MeasPower_Button_Click(object sender, EventArgs e)
        {
            if (IsAligningLaser) return;

            if (IsMeasuringPower)
                TryStopCalibratingLaser();
            else
                TryStartMeasuringPower();
        }

        private void TryStartMeasuringPower()
        {
            try
            {
                StartMeasuringPower();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StartMeasuringPower()
        {
            if (StimKeySwitchState == 0)
            {
                MessageBox.Show(Properties.Resources.MsgBox_Warning_StimKeySwitchOff, "Key-lock Switch OFF:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var wavelength = LaserWavelengthConverter.GetWavelength(Wavelength_ComboBox.SelectedValue);
            if (!LaserWavelengthConverter.IsLaserEnabled(wavelength))
            {
                MessageBox.Show(Properties.Resources.MsgBox_Warning_NoLaserWavelengthSpecified, "Laser Wavelength Not Specified:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AlignLaser_Button.Enabled = false;
            Wavelength_ComboBox.Enabled = false;
            StimPeriod_Slider.Enabled = false;
            StimOn_Slider.Enabled = false;
            StimReps_Control.Enabled = false;

            MeasPower_Button.Text = "Stop Measuring Power";

            var measurePowerPulseTrain = GetMeasurePowerPulseTrain();
            PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(StartCalibrateLaserCommandName, measurePowerPulseTrain));

            IsMeasuringPower = true;
        }

        private PulseTrain GetMeasurePowerPulseTrain()
        {
            var calibrationStimPeriod = MeasurePowerStimPeriod_Default;
            IsHighAmplitude = Amplitude_Slider.GetIsHighAmplitude();
            if (IsHighAmplitude)
                calibrationStimPeriod = MeasurePowerStimPeriod_Long;

            var measurePowerPulseTrain = new PulseTrain()
            {
                LaserWavelength = (ushort)Wavelength_ComboBox.SelectedValue,
                LaserAmplitude = Amplitude_Slider.GetLaserAmplitude(),
                StimPeriod = calibrationStimPeriod,
                StimOn = MeasurePowerStimOn,
                StimReps = CalibrationStimReps
            };

            return measurePowerPulseTrain;
        }

        private void AlignLaser_Button_Click(object sender, EventArgs e)
        {
            if (IsMeasuringPower) return;

            if (IsAligningLaser)
                TryStopCalibratingLaser();
            else
                TryStartAligningLaser();
        }

        private void TryStartAligningLaser()
        {
            try
            {
                StartAligningLaser();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StartAligningLaser()
        {
            if (StimKeySwitchState == 0)
            {
                MessageBox.Show(Properties.Resources.MsgBox_Warning_StimKeySwitchOff, "Key lock Switch OFF:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var wavelength = LaserWavelengthConverter.GetWavelength(Wavelength_ComboBox.SelectedValue);
            if (!LaserWavelengthConverter.IsLaserEnabled(wavelength))
            {
                MessageBox.Show(Properties.Resources.MsgBox_Warning_NoLaserWavelengthSpecified, "Laser Wavelength Not Specified:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MeasPower_Button.Enabled = false;
            Wavelength_ComboBox.Enabled = false;
            StimPeriod_Slider.Enabled = false;
            StimOn_Slider.Enabled = false;
            StimReps_Control.Enabled = false;

            AlignLaser_Button.Text = "Stop Aligning Laser";

            var alignLaserPulseTrain = GetAlignLaserPulseTrain();
            Amplitude_Slider.SetLaserAmplitude(alignLaserPulseTrain.LaserAmplitude);
            PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(StartCalibrateLaserCommandName, alignLaserPulseTrain));

            IsAligningLaser = true;
        }

        private PulseTrain GetAlignLaserPulseTrain()
        {
            var alignLaserPulseTrain = new PulseTrain()
            {
                LaserWavelength = (ushort)Wavelength_ComboBox.SelectedValue,
                LaserAmplitude = AlignLaserAmplitude_Default,
                StimPeriod = AlignLaserStimPeriod,
                StimOn = AlignLaserStimOn,
                StimReps = CalibrationStimReps
            };

            return alignLaserPulseTrain;
        }

        private void TryStopCalibratingLaser()
        {
            try
            {
                StopCalibratingLaser();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StopCalibratingLaser()
        {
            var wavelength = LaserWavelengthConverter.GetWavelength(Wavelength_ComboBox.SelectedValue);
            MeasPower_Button.Enabled = LaserWavelengthConverter.IsLaserEnabled(wavelength);
            AlignLaser_Button.Enabled = LaserWavelengthConverter.Is635LaserEnabled(wavelength);
            Wavelength_ComboBox.Enabled = true;
            Amplitude_Slider.Enabled = true;
            StimPeriod_Slider.Enabled = true;
            StimOn_Slider.Enabled = true;
            StimReps_Control.Enabled = true;

            MeasPower_Button.Text = "Measure Power";
            AlignLaser_Button.Text = "Align Laser";

            var settingsPulseTrain = GetSettingsPulseTrain();
            PulseTrainFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(StopCalibrateLaserCommandName, settingsPulseTrain));

            IsMeasuringPower = false;
            IsAligningLaser = false;
        }

        private PulseTrain GetSettingsPulseTrain()
        {
            var settingsPulseTrain = new PulseTrain()
            {
                LaserWavelength = (ushort)Wavelength_ComboBox.SelectedValue,
                LaserAmplitude = Amplitude_Slider.GetLaserAmplitude(),
                StimPeriod = StimPeriod_Slider.GetStimPeriod(),
                StimOn = StimOn_Slider.GetStimOn(),
                StimReps = StimReps_Control.GetStimReps()
            };
            return settingsPulseTrain;
        }
    }
}