using Neurophotometrics.Design.Reflection;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Neurophotometrics.Design.Editors
{
    public partial class SliderControl2 : UserControl
    {

        #region Private Members

        private readonly Timer Timer;
        private readonly TextBox Value_Text = new TextBox();
        private string currentButtonPressed;

        private int _Min;
        private int _Max;
        private double _StepSize;
        private int _Value;
        private bool _TextEditEnabled;

        #endregion

        #region Public Members
        public byte Decimals { get; set; } = 3;
        public RegisterConverter Converter { get; set; }
        public KeyFilterType KeyFilter { get; set; }
        public int FirstDelay { get; set; } = 500; // Delay before first repeat in milliseconds
        public int LoSpeedWait { get; set; } = 300; // Delay between repeats before LoHiChangeTime
        public int HiSpeedWait { get; set; } = 100; // Delay between repeats after LoHiChangeTime
        public int LoHiChangeTime { get; set; } = 2000; // Changeover time between slow repeats and fast repeats
        public int Min
        {
            get { return _Min; }
            set
            {
                _Min = value;

                if(Converter != null)
                {
                    var UIVal = Converter.ConvertFromRegToDouble(value);
                    var trackBarVal = (int)Math.Round(UIVal * Math.Pow(10, Decimals));
                    if (!Converter.IsInverted()) Slider_TrackBar.Minimum = trackBarVal;
                    else Slider_TrackBar.Maximum = trackBarVal;
                }
            }
        }
        public int Max
        {
            get { return _Max; }
            set
            {
                _Max = value;

                if (Converter != null)
                {
                    var UIVal = Converter.ConvertFromRegToDouble(value);
                    var trackBarVal = (int)Math.Round(UIVal * Math.Pow(10, Decimals));
                    if (!Converter.IsInverted()) Slider_TrackBar.Maximum = trackBarVal;
                    else Slider_TrackBar.Minimum = trackBarVal;

                }
            }
        }
        public double StepSize
        {
            get { return _StepSize; }
            set
            {
                if(Converter != null)
                {
                    _StepSize = value;
                    var trackBarVal = (int)Math.Round(_StepSize * Math.Pow(10, Decimals));
                    Slider_TrackBar.SmallChange = trackBarVal;
                    Slider_TrackBar.LargeChange = Slider_TrackBar.SmallChange * 5;
                }
            }
        }
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = Math.Min(Max, Math.Max(Min, value));
                if(Converter != null)
                {
                    var UIVal = Converter.ConvertFromRegToDouble(_Value);
                    var trackBarVal = (int)Math.Round(UIVal * Math.Pow(10, Decimals));
                    Slider_TrackBar.Value = Math.Min(Slider_TrackBar.Maximum, Math.Max(Slider_TrackBar.Minimum, trackBarVal));
                    Value_Label.Text = Converter.ConvertToInvariantString(_Value);
                }
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

        public SliderControl2()
        {
            InitializeComponent();
            Timer = new Timer
            {
                Interval = FirstDelay
            };
            Timer.Tick += InternalTimer_Tick;
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
            try
            {
                if (Converter != null)
                {
                    var UIVal = Slider_TrackBar.Value / Math.Pow(10, Decimals);
                    _Value = (int)Convert.ChangeType(Converter.ConvertToReg(UIVal), typeof(int));
                    Value_Label.Text = Converter.ConvertToInvariantString(Value);
                    SafeInvokeValueChanged();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: Slider_TrackBar_Scroll\nMessage: {ex.Message}");
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    currentButtonPressed = button.Text;
                    UpdateValue_FromButton();
                }
                Timer.Tag = DateTime.Now;
                Timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Button_MouseDown\nMessage: {ex.Message}");
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Timer.Stop();
                Timer.Interval = FirstDelay;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Button_MouseUp\nMessage: {ex.Message}");
            }
        }

        private void InternalTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateValue_FromButton();
                var elapsed = DateTime.Now - ((DateTime)Timer.Tag);
                if (elapsed.TotalMilliseconds < LoHiChangeTime)
                {
                    Timer.Interval = LoSpeedWait;
                }
                else
                {
                    Timer.Interval = HiSpeedWait;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: InternalTimer_Tick\nMessage: {ex.Message}");
            }
        }

        private void Value_Label_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Value_Label_Click\nMessage: {ex.Message}");
            }
        }

        private void Text_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Text_Value_KeyPress\nMessage: {ex.Message}");
            }
        }

        private void Value_Text_LostFocus(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Value_Text_LostFocus\nMessage: {ex.Message}");
            }
        }
        private void StrayClick(object sender, EventArgs e)
        {
            try
            {
                Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: StrayClick\nMessage: {ex.Message}");
            }
        }

        #endregion

        #region Helper Functions

        private void UpdateValue_FromButton()
        {
            try
            {
                var changeSize = currentButtonPressed.Length == 1 ? Slider_TrackBar.SmallChange : Slider_TrackBar.LargeChange;
                var changeDirection = currentButtonPressed.Contains("+") ? 1 : -1;

                var diff = Slider_TrackBar.Value - Slider_TrackBar.Minimum;
                var ticks = 0;
                if (diff % changeSize == 0)
                    ticks = diff / changeSize + changeDirection;
                else
                    ticks = changeDirection == 1 ? (int)Math.Ceiling((double)diff / changeSize) : (int)Math.Floor((double)diff / changeSize);

                var newTrackBarValue = ticks * changeSize + Slider_TrackBar.Minimum;

                newTrackBarValue = Math.Min(Slider_TrackBar.Maximum, Math.Max(Slider_TrackBar.Minimum, newTrackBarValue));
                var UIVal = newTrackBarValue / Math.Pow(10, Decimals);
                _Value = (int)Convert.ChangeType(Converter.ConvertToReg(UIVal), typeof(int));
                Value_Label.Text = Converter.ConvertToInvariantString(Value);
                SafeInvokeValueChanged();
                Slider_TrackBar.Value = newTrackBarValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: UpdateValue_FromButton\nMessage: {ex.Message}");
            }
        }

        public void ConvertToLongValueLabel()
        {
            try
            {
                Width += (int)Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width;
                Top_TableLayoutPanel.ColumnStyles[Top_TableLayoutPanel.ColumnCount - 1].Width *= 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: ConvertToLongValueLabel\nMessage: {ex.Message}");
            }
        }

        public void ConvertToTwoValues()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: ConvertToTwoValues\nMessage: {ex.Message}");
            }
        }

        public void UpdateSecondValue(string label)
        {
            try
            {
                if (Top_TableLayoutPanel.Controls.ContainsKey("Value_Label2"))
                {
                    var secondValueLabel = (Label)Top_TableLayoutPanel.Controls.Find("Value_Label2", false)[0];
                    secondValueLabel.Text = $"{label}%";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: UpdateSecondValue\nMessage: {ex.Message}");
            }
        }

        public bool AcceptText(TextBox textBox)
        {
            try
            {
                var success = true;
                textBox.LostFocus -= Value_Text_LostFocus;
                textBox.KeyPress -= Text_Value_KeyPress;
                var text = textBox.Text;
                try
                {
                    if (Converter != null) Value = Convert.ToInt32(Converter.ConvertFromInvariantString(text));
                    else Value = Convert.ToInt32(text);
                    if (Converter != null) Value_Label.Text = Converter.ConvertToInvariantString(Value);
                    else Value_Label.Text = Value.ToString(CultureInfo.InvariantCulture);
                    //Slider_TrackBar.Value = Value;
                    Top_TableLayoutPanel.Controls.Remove(Value_Text);
                    Top_TableLayoutPanel.Controls.Add(Value_Label, 5, 0);
                    SafeInvokeValueChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    textBox.Focus();
                    success = false;
                }

                textBox.LostFocus += Value_Text_LostFocus;
                textBox.KeyPress += Text_Value_KeyPress;
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: AcceptText\nMessage: {ex.Message}");
            }
            return false;
        }

        #endregion
    }
}
