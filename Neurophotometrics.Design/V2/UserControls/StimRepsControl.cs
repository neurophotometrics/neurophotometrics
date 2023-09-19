using Neurophotometrics.Design.V2.Converters;

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.UserControls
{
    public partial class StimRepsControl : UserControl
    {
        private ushort StimReps;

        public event RegSettingChangedEventHandler StimRepsChanged;

        private readonly TextBox StimRepsValue_Text;

        public StimRepsControl()
        {
            InitializeComponent();
            StimRepsValue_Text = new TextBox()
            {
                Dock = DockStyle.Left,
                Enabled = false,
                Font = Top_TableLayoutPanel.Font,
                Visible = false
            };
            StimRepsValue_Label.Text = StimRepsConverter.Default.ToString();

            SetComboBoxWidth();
        }

        private void SetComboBoxWidth()
        {
            var maxWidth = 0;

            var tempLabel = new Label();
            foreach (var obj in PulseTrainType_ComboBox.Items)
            {
                tempLabel.Text = obj.ToString();
                if (tempLabel.PreferredWidth > maxWidth)
                    maxWidth = tempLabel.PreferredWidth;
            }

            PulseTrainType_ComboBox.Width = maxWidth * 2;
        }

        internal void TrySetStimReps(ushort stimReps)
        {
            try
            {
                SetStimReps(stimReps);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void SetStimReps(ushort stimReps)
        {
            StimReps = stimReps;
            StimRepsValue_Label.Text = stimReps.ToString();
            SetPulseTrainType(stimReps);
        }

        private void SetPulseTrainType(ushort stimReps)
        {
            if (stimReps == StimRepsConverter.Max)
            {
                PulseTrainType_ComboBox.SelectedIndex = 1;
                StimReps_Label.Enabled = false;
                StimReps_Label.Visible = false;
                StimRepsValue_Label.Enabled = false;
                StimRepsValue_Label.Visible = false;
            }
            else
            {
                PulseTrainType_ComboBox.SelectedIndex = 0;
                StimReps_Label.Enabled = true;
                StimReps_Label.Visible = true;
                StimRepsValue_Label.Enabled = true;
                StimRepsValue_Label.Visible = true;
            }
        }

        internal ushort GetStimReps()
        {
            return StimReps;
        }

        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        private void PulseTrainType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PulseTrainType_ComboBox.SelectedIndex == 0)
            {
                StimReps = ushort.Parse(StimRepsValue_Label.Text);
                StimReps_Label.Enabled = true;
                StimReps_Label.Visible = true;
                StimRepsValue_Label.Enabled = true;
                StimRepsValue_Label.Visible = true;
            }
            else
            {
                StimReps = StimRepsConverter.Max;
                StimReps_Label.Enabled = false;
                StimReps_Label.Visible = false;
                StimRepsValue_Label.Enabled = false;
                StimRepsValue_Label.Visible = false;
            }
            if (StimRepsChanged != null)
                StimRepsChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.StimReps), GetStimReps()));
        }

        private void StimRepsValue_Label_Click(object sender, EventArgs e)
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

        private void StimRepsValue_Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ApplyKeyFilter(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StimRepsValue_Text_LostFocus(object sender, EventArgs e)
        {
            try
            {
                AcceptText();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void EnableTextEdit()
        {
            StimRepsValue_Text.Text = Regex.Replace(StimRepsValue_Label.Text, "[^.0-9]", "");
            Top_TableLayoutPanel.Controls.Remove(StimRepsValue_Label);
            Top_TableLayoutPanel.Controls.Add(StimRepsValue_Text, 2, 0);
            StimRepsValue_Label.Enabled = false;
            StimRepsValue_Label.Visible = false;
            StimRepsValue_Text.Enabled = true;
            StimRepsValue_Text.Visible = true;
            StimRepsValue_Text.Focus();
            StimRepsValue_Text.KeyPress += StimRepsValue_Text_KeyPress;
            StimRepsValue_Text.LostFocus += StimRepsValue_Text_LostFocus;
        }

        private void ApplyKeyFilter(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                Focus();
                return;
            }
            var isValid = KeyFilteringHelpers.IsIntegerNumeric(e.KeyChar);
            e.Handled = !isValid;
        }

        private void AcceptText()
        {
            var value = (ushort)Math.Min(ushort.MaxValue, int.Parse(StimRepsValue_Text.Text));
            StimReps = value;
            StimRepsValue_Label.Text = value.ToString();
            Top_TableLayoutPanel.Controls.Remove(StimRepsValue_Text);
            Top_TableLayoutPanel.Controls.Add(StimRepsValue_Label, 2, 0);
            StimRepsValue_Label.Enabled = true;
            StimRepsValue_Label.Visible = true;
            StimRepsValue_Text.Enabled = false;
            StimRepsValue_Text.Visible = false;
            StimRepsValue_Text.KeyPress -= StimRepsValue_Text_KeyPress;
            StimRepsValue_Text.LostFocus -= StimRepsValue_Text_LostFocus;

            if (StimRepsChanged != null)
                StimRepsChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.PulseTrain.StimReps), GetStimReps()));
        }
    }
}