using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.UserControls
{
    public partial class SettingSlider : UserControl
    {
        private readonly Timer ButtonTimer;
        private int ButtonChangeSize = 0;
        private int ButtonDuration;

        public event EventHandler ValueChanged;

        private const int DefaultLowSpeedRepeatDelay = 500;
        private const int DefaultHighSpeedRepeatDelay = 2000;
        private const int DefaultLowSpeedRepeatPeriod = 300;
        private const int DefaultHighSpeedRepeatPeriod = 100;

        public int LowSpeedRepeatDelay { get; set; } = DefaultLowSpeedRepeatDelay;
        public int HighSpeedRepeatDelay { get; set; } = DefaultHighSpeedRepeatDelay;
        public int LowSpeedRepeatPeriod { get; set; } = DefaultLowSpeedRepeatPeriod;
        public int HighSpeedRepeatPeriod { get; set; } = DefaultHighSpeedRepeatPeriod;

        public SettingSlider()
        {
            InitializeComponent();
            ButtonTimer = new Timer() { Interval = LowSpeedRepeatDelay };
            ButtonTimer.Tick += InternalTimer_Tick;
            Value_Text.Enabled = false;
            Value_Text.Visible = false;
        }

        internal void SetMaxLabelWidth(string longestString)
        {
            var currentValue = Value_Label.Text;
            Value_Label.Text = longestString;
            var prefWidth = Value_Label.PreferredWidth;
            Value_Label.AutoSize = false;
            Value_Label.Width = prefWidth;
            Value_Label.Text = currentValue;
        }

        internal bool GetIsTypingPower()
        {
            return Value_Text.Focused;
        }

        public int GetTrackBarValue()
        {
            return Slider_TrackBar.Value;
        }

        public void SetTrackBarValue(int trackBarValue)
        {
            Slider_TrackBar.Value = trackBarValue;
            Value_Label.Text = ConvertTrackBarToText(Slider_TrackBar.Value);
        }

        public void SetTrackBarMin(int min)
        {
            Slider_TrackBar.Minimum = min;
        }

        public void SetTrackBarMax(int max)
        {
            Slider_TrackBar.Maximum = max;
        }

        public void SetTrackBarSmallChange(int smallChange)
        {
            Slider_TrackBar.SmallChange = smallChange;
        }

        public void SetTrackBarLargeChange(int largeChange)
        {
            Slider_TrackBar.LargeChange = largeChange;
        }

        public virtual ushort ConvertTrackBarToRegisterValue(int trackBarValue)
        {
            return (ushort)trackBarValue;
        }

        public virtual string ConvertTrackBarToText(int trackBarValue)
        {
            return trackBarValue.ToString();
        }

        public virtual int ConvertTextToTrackBar(string text)
        {
            return int.Parse(text);
        }

        public virtual bool GetIsKeyValid(char keyChar)
        {
            return true;
        }

        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        private void Slider_TrackBar_Scroll(object sender, EventArgs e)
        {
            TryUpdateValueFromTrackBar();
        }

        private void Value_Label_Click(object sender, EventArgs e)
        {
            TryEnableTextEdit();
        }

        private void Value_Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            var keyChar = e.KeyChar;
            TryApplyKeyFilter(keyChar, e);
        }

        private void Value_Text_LostFocus(object sender, EventArgs e)
        {
            TryAcceptText();
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
                TryStartUpdatingValueFromButton(button.Text);
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            StopUpdatingValueFromButton();
        }

        private void InternalTimer_Tick(object sender, EventArgs e)
        {
            TryUpdateValueFromButton();
            UpdateButtonTimer();
        }

        private void TryUpdateValueFromTrackBar()
        {
            try
            {
                UpdateValueFromTrackBar();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryEnableTextEdit()
        {
            try
            {
                EnableTextEdit();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryApplyKeyFilter(char keyChar, KeyPressEventArgs args)
        {
            try
            {
                ApplyKeyFilter(keyChar, args);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryAcceptText()
        {
            try
            {
                AcceptText();
            }
            catch (Exception ex)
            {
                Value_Text.LostFocus -= Value_Text_LostFocus;
                MessageBox.Show(ex.Message, "Error: Cannot Accept Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Value_Text.Focus();
                Value_Text.LostFocus += Value_Text_LostFocus;
            }
        }

        private void TryStartUpdatingValueFromButton(string buttonText)
        {
            try
            {
                StartUpdatingValueFromButton(buttonText);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryUpdateValueFromButton()
        {
            try
            {
                UpdateValueFromButton();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateValueFromTrackBar()
        {
            Value_Label.Text = ConvertTrackBarToText(Slider_TrackBar.Value);

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void EnableTextEdit()
        {
            Focus();
            ReplaceLabelWithTextBox();
            DisableOtherControls();
        }

        private void ApplyKeyFilter(char keyChar, KeyPressEventArgs args)
        {
            if (keyChar == (char)Keys.Enter)
            {
                args.Handled = true;
                Focus();
                return;
            }
            var isValid = GetIsKeyValid(keyChar);
            args.Handled = !isValid;
        }

        private void AcceptText()
        {
            Slider_TrackBar.Value = Math.Min(Slider_TrackBar.Maximum, Math.Max(Slider_TrackBar.Minimum, ConvertTextToTrackBar(Value_Text.Text)));
            UpdateValueFromTrackBar();
            EnableOtherControls();
            ReplaceTextBoxWithLabel();
        }

        private void StartUpdatingValueFromButton(string buttonText)
        {
            ButtonChangeSize = GetButtonChangeSize(buttonText);
            UpdateValueFromButton();
            ButtonDuration = 0;
            ButtonTimer.Tag = DateTime.Now;
            ButtonTimer.Start();
        }

        private void StopUpdatingValueFromButton()
        {
            ButtonTimer.Stop();
            ButtonTimer.Interval = LowSpeedRepeatDelay;
            ButtonDuration = 0;
        }

        private int GetButtonChangeSize(string buttonText)
        {
            switch (buttonText)
            {
                case " - ":
                    return -1 * Slider_TrackBar.SmallChange;

                case "- -":
                    return -1 * Slider_TrackBar.LargeChange;

                case " + ":
                    return Slider_TrackBar.SmallChange;

                case "+ +":
                    return Slider_TrackBar.LargeChange;

                default:
                    return 1;
            }
        }

        private void UpdateValueFromButton()
        {
            var newTrackBarValue = ClampToNearestTick();
            newTrackBarValue = Math.Min(Slider_TrackBar.Maximum, Math.Max(Slider_TrackBar.Minimum, newTrackBarValue));
            Slider_TrackBar.Value = newTrackBarValue;
            UpdateValueFromTrackBar();
        }

        private int ClampToNearestTick()
        {
            if (Slider_TrackBar.Value % Math.Abs(ButtonChangeSize) == 0)
                return Slider_TrackBar.Value + ButtonChangeSize;

            if (ButtonChangeSize == 0)
                return Slider_TrackBar.Value;
            else if (ButtonChangeSize < 0)
                return Slider_TrackBar.Value / ButtonChangeSize * ButtonChangeSize;
            else
                return ((Slider_TrackBar.Value + 2) / ButtonChangeSize + 1) * ButtonChangeSize;
        }

        private void UpdateButtonTimer()
        {
            ButtonDuration += ButtonTimer.Interval;
            if (ButtonDuration < HighSpeedRepeatDelay)
                ButtonTimer.Interval = LowSpeedRepeatPeriod;
            else
                ButtonTimer.Interval = HighSpeedRepeatPeriod;
        }

        private void ReplaceLabelWithTextBox()
        {
            Value_Text.Text = Regex.Replace(Value_Label.Text, "[^.0-9]", "");
            Value_Text.Size = Value_Label.Size;
            Value_Label.Visible = false;
            Value_Label.Enabled = false;
            Value_Label.Click -= Value_Label_Click;
            Top_TableLayoutPanel.Controls.Remove(Value_Label);
            Top_TableLayoutPanel.Controls.Add(Value_Text, 5, 0);
            Value_Text.Enabled = true;
            Value_Text.Visible = true;
            Value_Text.LostFocus += Value_Text_LostFocus;
            Value_Text.Focus();
        }

        private void ReplaceTextBoxWithLabel()
        {
            Value_Text.LostFocus -= Value_Text_LostFocus;
            Value_Text.Visible = false;
            Value_Text.Enabled = false;
            Top_TableLayoutPanel.Controls.Remove(Value_Text);
            Top_TableLayoutPanel.Controls.Add(Value_Label, 5, 0);
            Value_Label.Visible = true;
            Value_Label.Enabled = true;
            Value_Label.Click += Value_Label_Click;
        }

        private void DisableOtherControls()
        {
            Dec_Lg_Button.Enabled = false;
            Dec_Sm_Button.Enabled = false;
            Inc_Lg_Button.Enabled = false;
            Inc_Sm_Button.Enabled = false;
            Slider_TrackBar.Enabled = false;
        }

        private void EnableOtherControls()
        {
            Dec_Lg_Button.Enabled = true;
            Dec_Sm_Button.Enabled = true;
            Inc_Lg_Button.Enabled = true;
            Inc_Sm_Button.Enabled = true;
            Slider_TrackBar.Enabled = true;
        }
    }
}