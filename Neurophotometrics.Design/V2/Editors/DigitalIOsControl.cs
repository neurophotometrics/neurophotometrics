using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class DigitalIOsControl : UserControl
    {
        private Dictionary<string, object> ClockConfig_Pairs;
        private Dictionary<string, object> DIConfig_Pairs;
        private Dictionary<string, object> DOConfig_Pairs;
        private Dictionary<string, object> DORoute_Pairs;

        public event RegSettingChangedEventHandler SettingChanged;

        public event RegSettingChangedEventHandler FP3002RegisterChanged;

        public DigitalIOsControl()
        {
            InitializeComponent();
            TryInitComboBoxes();
            TryInitKeyDefs();
        }

        private void TryInitKeyDefs()
        {
            try
            {
                InitKeyDefs();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void InitKeyDefs()
        {
            KeyDefs_RichTextBox.Rtf = @"{\rtf1\pc "
                                    + @"\b\ul Digital Inputs\b0\ul0 \par "
                                    + @"\b Events:\b0  Samples a +5V TTL signal, generating timestamps on the specified type of voltage change.\par "
                                    + @"\b Control:\b0  Allows a +5V TTL signal to control data acquisition of the FP3002 system and/or an external camera.\par "
                                    + @"\b Start Stimulation:\b0  Allows a +5V TTL signal to start a finite or continuous pulse train.\par \par"
                                    + @"\b\ul Digital Output 0\b0\ul0 \par "
                                    + @"\b Software:\b0  The system will output a +5V TTL signal generated within Bonsai, using the Digital Output Node.\par "
                                    + @"\b Strobe:\b0  Outputs a +5V TTL signal that will be HIGH while the camera is exposing, and LOW between frames.\par "
                                    + @"\b Trigger State:\b0  Outputs a +5V TTL will be HIGH while an LED is ON and LOW while an LED is OFF.\par \par "
                                    + @"\b\ul Digital Output 1\b0\ul0 \par "
                                    + @"\b Both:\b0  Internal signal used to control the laser will be sent to both the laser and the Digital Output 1 port. \par "
                                    + @"\b BNC:\b0  Internal signal will be sent to the Digital Output 1 port only. Timing can be configured by changing laser settings.\par "
                                    + @"\b Laser:\b0  Internal signal will be sent to the laser only.\par \par";
        }

        private void TryInitComboBoxes()
        {
            try
            {
                InitComboBoxes();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void InitComboBoxes()
        {
            var ClockConfig_Fields = typeof(ClockSynchronizerConfiguration).GetFields()
                    .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Any());
            ClockConfig_Pairs = new Dictionary<string, object>();
            foreach (var fi in ClockConfig_Fields)
            {
                var value = fi.GetValue(null);
                var da = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                var desc = da.Description;

                ClockConfig_Pairs[desc] = value;
            }

            ClockConfig_ComboBox.DataSource = ClockConfig_Pairs.ToList();
            ClockConfig_ComboBox.DisplayMember = "Key";
            ClockConfig_ComboBox.ValueMember = "Value";

            var DIConfig_Fields = typeof(DigitalInputConfiguration).GetFields()
                .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Any());
            DIConfig_Pairs = new Dictionary<string, object>();
            foreach (var fi in DIConfig_Fields)
            {
                var value = fi.GetValue(null);
                var da = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                var desc = da.Description;

                DIConfig_Pairs[desc] = value;
            }

            DI0_ComboBox.DataSource = DIConfig_Pairs.ToList();
            DI1_ComboBox.DataSource = DIConfig_Pairs.ToList();
            DI0_ComboBox.DisplayMember = "Key";
            DI1_ComboBox.DisplayMember = "Key";
            DI0_ComboBox.ValueMember = "Value";
            DI1_ComboBox.ValueMember = "Value";

            var DOConfig_Fields = typeof(DigitalOutputConfiguration).GetFields()
                .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Any());
            DOConfig_Pairs = new Dictionary<string, object>();
            foreach (var fi in DOConfig_Fields)
            {
                var value = fi.GetValue(null);
                var da = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                var desc = da.Description;

                DOConfig_Pairs[desc] = value;
            }

            DO0_ComboBox.DataSource = DOConfig_Pairs.ToList();
            DO0_ComboBox.DisplayMember = "Key";
            DO0_ComboBox.ValueMember = "Value";

            var DORoute_Fields = typeof(DigitalOutputRouting).GetFields()
                .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Any());
            DORoute_Pairs = new Dictionary<string, object>();
            foreach (var fi in DORoute_Fields)
            {
                var value = fi.GetValue(null);
                var da = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                var desc = da.Description;

                DORoute_Pairs[desc] = value;
            }

            DO1_ComboBox.DataSource = DORoute_Pairs.ToList();
            DO1_ComboBox.DisplayMember = "Key";
            DO1_ComboBox.ValueMember = "Value";
        }

        internal void TryUpdateMiscSetting(RegSettingChangedEventArgs arg)
        {
            try
            {
                UpdateMiscSetting(arg);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateMiscSetting(RegSettingChangedEventArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(FP3002Settings.ClockSynchronizer):
                    ClockConfig_ComboBox.SelectedValue = (ClockSynchronizerConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.ScreenBrightness):
                    ScreenBrightness_Slider.Value = (byte)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalInput0):
                    DI0_ComboBox.SelectedValue = (DigitalInputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalInput1):
                    DI1_ComboBox.SelectedValue = (DigitalInputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalOutput0):
                    DO0_ComboBox.SelectedValue = (DigitalOutputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.Output1Routing):
                    DO1_ComboBox.SelectedValue = (DigitalOutputRouting)arg.Value;
                    break;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateDigitalIOSetting(sender);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateDigitalIOSetting(object sender)
        {
            if (sender is ComboBox cb)
            {
                if (cb.SelectedValue is ClockSynchronizerConfiguration newClockConfig)
                {
                    SafeInvokeMiscSettingChanged(nameof(FP3002Settings.ClockSynchronizer), newClockConfig);
                }
                else if (cb.SelectedValue is DigitalInputConfiguration newDIConfig)
                {
                    var name = cb.Name.Contains("DI0") ? nameof(FP3002Settings.DigitalInput0) : nameof(FP3002Settings.DigitalInput1);
                    SafeInvokeMiscSettingChanged(name, newDIConfig);
                }
                else if (cb.SelectedValue is DigitalOutputConfiguration newDOConfig)
                {
                    SafeInvokeMiscSettingChanged(nameof(FP3002Settings.DigitalOutput0), newDOConfig);
                }
                else if (cb.SelectedValue is DigitalOutputRouting newD1Route)
                {
                    SafeInvokeMiscSettingChanged(nameof(FP3002Settings.Output1Routing), newD1Route);
                }
            }
        }

        private void ScreenBrightness_Slider_Scroll(object sender, EventArgs e)
        {
            try
            {
                UpdateScreenBrightness();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateScreenBrightness()
        {
            if (SettingChanged != null)
                SettingChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.ScreenBrightness), (byte)Math.Min(byte.MaxValue, ScreenBrightness_Slider.Value)));
            if (FP3002RegisterChanged != null)
                FP3002RegisterChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.ScreenBrightness), (byte)Math.Min(byte.MaxValue, ScreenBrightness_Slider.Value)));
        }

        private void SafeInvokeMiscSettingChanged(string name, object value)
        {
            if (SettingChanged != null)
                SettingChanged.Invoke(this, new RegSettingChangedEventArgs(name, value));
        }
    }
}