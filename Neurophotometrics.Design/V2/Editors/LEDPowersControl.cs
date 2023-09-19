using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;

using System;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class LEDPowersControl : UserControl
    {
        public event RegSettingChangedEventHandler LEDPowerSettingChanged;

        public event RegSettingChangedEventHandler LEDPowerFP3002Changed;

        public LEDPowersControl()
        {
            InitializeComponent();

            L415_Slider.LEDName = nameof(FP3002Settings.LedPowers.L415);
            L470_Slider.LEDName = nameof(FP3002Settings.LedPowers.L470);
            L560_Slider.LEDName = nameof(FP3002Settings.LedPowers.L560);

            L415_Slider.LEDPowerChanged += LEDPower_Slider_LEDPowerChanged;
            L470_Slider.LEDPowerChanged += LEDPower_Slider_LEDPowerChanged;
            L560_Slider.LEDPowerChanged += LEDPower_Slider_LEDPowerChanged;
        }

        internal void ResetTab()
        {
            L415_Slider.Enabled = false;
            L415_Label.Enabled = false;
            L415_Slider.BackColor = BackColor;
            L415_Label.BackColor = BackColor;
            L415_Button.Text = "Edit";
            if (LEDPowerFP3002Changed != null)
                LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L415), LedPowerConverter.Default));

            L470_Slider.Enabled = false;
            L470_Label.Enabled = false;
            L470_Slider.BackColor = BackColor;
            L470_Label.BackColor = BackColor;
            L470_Button.Text = "Edit";
            if (LEDPowerFP3002Changed != null)
                LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L470), LedPowerConverter.Default));

            L560_Slider.Enabled = false;
            L560_Label.Enabled = false;
            L560_Slider.BackColor = BackColor;
            L560_Label.BackColor = BackColor;
            L560_Button.Text = "Edit";
            if (LEDPowerFP3002Changed != null)
                LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L560), LedPowerConverter.Default));
        }

        internal void TryUpdateLEDPowers(RegSettingChangedEventArgs args)
        {
            try
            {
                UpdateLEDPowers(args);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateLEDPowers(RegSettingChangedEventArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(FP3002Settings.LedPowers):
                    var ledPowers = (LedPowers)arg.Value;
                    L415_Slider.SetLEDPower(ledPowers.L415);
                    L470_Slider.SetLEDPower(ledPowers.L470);
                    L560_Slider.SetLEDPower(ledPowers.L560);
                    break;

                case nameof(FP3002Settings.LedPowers.L415):
                    L415_Slider.SetLEDPower((ushort)arg.Value);
                    break;

                case nameof(FP3002Settings.LedPowers.L470):
                    L470_Slider.SetLEDPower((ushort)arg.Value);
                    break;

                case nameof(FP3002Settings.LedPowers.L560):
                    L560_Slider.SetLEDPower((ushort)arg.Value);
                    break;
            }
        }

        private void LEDPower_Slider_LEDPowerChanged(object sender, RegSettingChangedEventArgs args)
        {
            if (LEDPowerFP3002Changed != null)
                LEDPowerFP3002Changed.Invoke(this, args);

            if (LEDPowerSettingChanged != null)
                LEDPowerSettingChanged.Invoke(this, args);
        }

        private void L415_Button_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleL415();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ToggleL415()
        {
            L415_Button.Text = L415_Button.Text.Equals("Accept") ? "Edit" : "Accept";
            L415_Label.Enabled = !L415_Label.Enabled;
            L415_Slider.Enabled = !L415_Slider.Enabled;

            if (L415_Slider.Enabled)
            {
                L415_Slider.UpdateLEDPower();
                var color = TriggerSequenceConverter.GetLightColor(FrameFlags.L415);
                L415_Slider.BackColor = color;
                L415_Label.BackColor = color;
            }
            else
            {
                L415_Slider.BackColor = BackColor;
                L415_Label.BackColor = BackColor;

                if (LEDPowerFP3002Changed != null)
                    LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L415), LedPowerConverter.Default));
            }
        }

        private void L470_Button_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleL470();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ToggleL470()
        {
            L470_Button.Text = L470_Button.Text.Equals("Accept") ? "Edit" : "Accept";
            L470_Label.Enabled = !L470_Label.Enabled;
            L470_Slider.Enabled = !L470_Slider.Enabled;

            if (L470_Slider.Enabled)
            {
                L470_Slider.UpdateLEDPower();
                var color = TriggerSequenceConverter.GetLightColor(FrameFlags.L470);
                L470_Slider.BackColor = color;
                L470_Label.BackColor = color;
            }
            else
            {
                L470_Slider.BackColor = BackColor;
                L470_Label.BackColor = BackColor;

                if (LEDPowerFP3002Changed != null)
                    LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L470), LedPowerConverter.Default));
            }
        }

        private void L560_Button_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleL560();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ToggleL560()
        {
            L560_Button.Text = L560_Button.Text.Equals("Accept") ? "Edit" : "Accept";
            L560_Label.Enabled = !L560_Label.Enabled;
            L560_Slider.Enabled = !L560_Slider.Enabled;

            if (L560_Slider.Enabled)
            {
                L560_Slider.UpdateLEDPower();
                var color = TriggerSequenceConverter.GetLightColor(FrameFlags.L560);
                L560_Slider.BackColor = color;
                L560_Label.BackColor = color;
            }
            else
            {
                L560_Slider.BackColor = BackColor;
                L560_Label.BackColor = BackColor;

                if (LEDPowerFP3002Changed != null)
                    LEDPowerFP3002Changed.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L560), LedPowerConverter.Default));
            }
        }
    }
}