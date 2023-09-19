using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Neurophotometrics.Design.Editors
{
    public partial class SliderControl : UserControl
    {

        #region Private Members

        private readonly Timer internalTimer;
        private readonly TextBox Value_Text = new TextBox();
        private string currentButtonPressed;

        private int _Value;
        private bool _TextEditEnabled;

        #endregion

        #region Public Members
        public TypeConverter Converter { get; set; }
        public KeyFilterType KeyFilter { get; set; }
        public bool LeftToRight { get; set; }
        public RightToLeft Flipped { get; set; }
        public bool Success { get; set; }
        public int FirstDelay { get; set; } = 500; // Delay before first repeat in milliseconds
        public int LoSpeedWait { get; set; } = 300; // Delay between repeats before LoHiChangeTime
        public int HiSpeedWait { get; set; } = 100; // Delay between repeats after LoHiChangeTime
        public int LoHiChangeTime { get; set; } = 2000; // Changeover time between slow repeats and fast repeats
        public int Minimum
        {
            get { return Slider_TrackBar.Minimum; }
            set
            {
                Slider_TrackBar.Minimum = value;
                Slider_TrackBar.Value = Slider_TrackBar.Minimum;
                Slider_TrackBar.SmallChange = (Maximum - Minimum) / 100;
                Slider_TrackBar.LargeChange = (Maximum - Minimum) / 20;
            }
        }
        public int Maximum
        {
            get { return Slider_TrackBar.Maximum; }
            set
            {
                Slider_TrackBar.Maximum = value;
                Slider_TrackBar.SmallChange = (Maximum - Minimum) / 100;
                Slider_TrackBar.LargeChange = (Maximum - Minimum) / 20;
            }
        }
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                Slider_TrackBar.Value = _Value < Minimum ? Minimum : _Value;
                if (Converter != null) Value_Label.Text = Converter.ConvertToInvariantString(_Value);
                else Value_Label.Text = _Value.ToString(CultureInfo.InvariantCulture);
            }
        }
        public bool TextEditEnabled
        {
            get { return _TextEditEnabled; }
            set
            {
                _TextEditEnabled = value;
                if (_TextEditEnabled)
                {
                    Value_Label.Click += Value_Label_Click;
                    Value_Text.Dock = DockStyle.Fill;
                    Value_Text.KeyPress += Text_Value_KeyPress;
                    Value_Text.LostFocus += Value_Text_LostFocus;
                }
            }
        }

        #endregion 

        public SliderControl()
        {
            InitializeComponent();
            internalTimer = new Timer
            {
                Interval = FirstDelay
            };
            internalTimer.Tick += InternalTimer_Tick;
        }

        #region Event Handling

        public event EventHandler ValueChanged;

        private void SafeInvokeValueChanged()
        {
            try
            {
                if (ValueChanged != null)
                    ValueChanged.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: SafeInvokeValueChanged\nMessage: {ex.Message}");
            }
        }

        private void Slider_TrackBar_Scroll(object sender, EventArgs e)
        {
            Value = Slider_TrackBar.Value;
            if (Converter != null) Value_Label.Text = Converter.ConvertToInvariantString(Value);
            else Value_Label.Text = Value.ToString(CultureInfo.InvariantCulture);
            SafeInvokeValueChanged();
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                currentButtonPressed = button.Text;
                UpdateValue_FromButton();
            }
            internalTimer.Tag = DateTime.Now;
            internalTimer.Start();
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            internalTimer.Stop();
            internalTimer.Interval = FirstDelay;
        }

        private void InternalTimer_Tick(object sender, EventArgs e)
        {
            UpdateValue_FromButton();
            var elapsed = DateTime.Now - ((DateTime)internalTimer.Tag);
            if (elapsed.TotalMilliseconds < LoHiChangeTime)
            {
                internalTimer.Interval = LoSpeedWait;
            }
            else
            {
                internalTimer.Interval = HiSpeedWait;
            }
        }

        private void Value_Label_Click(object sender, EventArgs e)
        {
            Focus();
            Value_Text.Text = Regex.Replace(Value_Label.Text, "[^.0-9]", "");
            Top_TableLayoutPanel.Controls.Remove(Value_Label);
            Top_TableLayoutPanel.Controls.Add(Value_Text, 5, 0);
            Value_Text.Focus();
            Dec_Lg_Button.Enabled = false;
            Dec_Sm_Button.Enabled = false;
            Inc_Lg_Button.Enabled = false;
            Inc_Sm_Button.Enabled = false;
            Slider_TrackBar.Enabled = false;
        }

        private void Text_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            var keyChar = e.KeyChar;
            bool invalidCharEntered;
            switch (KeyFilter)
            {
                case KeyFilterType.IntegerNumeric:
                    invalidCharEntered = !KeyFilteringHelpers.IsIntegerNumeric(keyChar);
                    break;
                case KeyFilterType.DecimalNumeric:
                    invalidCharEntered = !KeyFilteringHelpers.IsDecimalNumeric(keyChar);
                    break;
                default:
                    invalidCharEntered = false;
                    break;
            }

            // Check for the flag being set in the KeyDown event.
            if (invalidCharEntered == true)
            {
                // Stop the character from being entered into the control.
                e.Handled = true;
            }
        }

        private void Value_Text_LostFocus(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                try
                {
                    var success = AcceptText(textBox);

                    if (success)
                    {
                        Dec_Lg_Button.Enabled = true;
                        Dec_Sm_Button.Enabled = true;
                        Inc_Lg_Button.Enabled = true;
                        Inc_Sm_Button.Enabled = true;
                        Slider_TrackBar.Enabled = true;
                    }
                }
                catch
                {
                    textBox.LostFocus -= Value_Text_LostFocus;
                    MessageBox.Show($"Unable to process input: {textBox.Text}");
                    textBox.Focus();
                    textBox.LostFocus += Value_Text_LostFocus;
                }
            }
        }
        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        #endregion

        #region Helper Functions

        private void UpdateValue_FromButton()
        {
            var multiplier = Flipped == RightToLeft.Yes ? -1 : 1;
            var changeSize = currentButtonPressed.Length == 1 ? Slider_TrackBar.SmallChange : Slider_TrackBar.LargeChange;
            var changeDirection = (Flipped == RightToLeft.Yes) ^ currentButtonPressed.Contains("+") ? 1 : -1;

            var value = Value + changeDirection * changeSize;

            // Go to nearest Small Change tick
            var ticks = (value - Minimum) / changeSize + (changeDirection == -1 && (value - Minimum) % changeSize != 0 ? 1 : 0);
            value = ticks * changeSize + Minimum;

            if (value > Maximum) Value = Maximum;
            else if (value < Minimum) Value = Minimum;
            else Value = value;

            if (Converter != null) Value_Label.Text = Converter.ConvertToInvariantString(Value);
            else Value_Label.Text = Value.ToString(CultureInfo.InvariantCulture);
            SafeInvokeValueChanged();
            Slider_TrackBar.Value = Value;
        }

        public void ConvertToLongValueLabel()
        {
            Width += (int)Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width;
            Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width *= 2;
        }

        public void ConvertToTwoValues()
        {
            var totalWidth = Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width;
            Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width = totalWidth * 0.5f;

            var colStyle = Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1];
            Top_TableLayoutPanel.ColumnCount += 1;
            Top_TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(colStyle.SizeType, totalWidth * 0.5f));

            var newLabel = new Label
            {
                Name = "Value_Label2",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = Value_Label.Font
            };

            Top_TableLayoutPanel.Controls.Add(newLabel);
        }

        public void UpdateSecondValue(string label)
        {
            if (Top_TableLayoutPanel.Controls.ContainsKey("Value_Label2"))
            {
                var secondValueLabel = (Label)Top_TableLayoutPanel.Controls.Find("Value_Label2", false)[0];
                secondValueLabel.Text = $"{label}%";
            }
        }

        public bool AcceptText(TextBox textBox)
        {
            Success = true;
            textBox.LostFocus -= Value_Text_LostFocus;
            textBox.KeyPress -= Text_Value_KeyPress;
            var text = textBox.Text;
            try
            {
                if (Converter != null) Value = Convert.ToInt32(Converter.ConvertFromInvariantString(text));
                else Value = Convert.ToInt32(text);
                if (Converter != null) Value_Label.Text = Converter.ConvertToInvariantString(Value);
                else Value_Label.Text = Value.ToString(CultureInfo.InvariantCulture);
                Slider_TrackBar.Value = Value;
                Top_TableLayoutPanel.Controls.Remove(Value_Text);
                Top_TableLayoutPanel.Controls.Add(Value_Label, 5, 0);
                SafeInvokeValueChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                textBox.Focus();
                Success = false;
            }

            textBox.LostFocus += Value_Text_LostFocus;
            textBox.KeyPress += Text_Value_KeyPress;
            return Success;
        }

        #endregion
    }
}
