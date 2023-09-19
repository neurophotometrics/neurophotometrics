using Neurophotometrics.Design.Properties;
using Neurophotometrics.V1;
using Neurophotometrics.V1.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V1.Forms
{
    public partial class FP3001Form : Form
    {
        private const int FPS_Max = 100;
        private const int FPS_Min = 5;

        private FP3001 FP3001;
        private Dictionary<string, object> Mode_Pairs;
        private TextBox FPSValue_Text;
        private IObservable<PhotometryDataFrame> FP3001Comms;
        private IDisposable Subscription;

        public FP3001Form(FP3001 fp3001)
        {
            InitializeComponent();
            InitializeMembers(fp3001);
            InitializePiping();
            TryUpdateComponents();
        }

        private void InitializeMembers(FP3001 fp3001)
        {
            FP3001 = fp3001;
            ROIEditor.Regions = FP3001.Regions;
            FPSValue_Text = new TextBox()
            {
                Enabled = false,
                Visible = false
            };
            FPSValue_Text.KeyPress += FPSValue_Text_KeyPress;
            Icon = Icon.FromHandle(Resources.Neurophotometrics.GetHicon());
        }

        private void InitializePiping()
        {
            FP3001Comms = FP3001.Generate()
                .ObserveOn(Scheduler.Default)
                .Do(UpdateDataVisualizer)
                .Sample(TimeSpan.FromMilliseconds(25.0))
                .Do(UpdateROIControl)
                .Sample(TimeSpan.FromMilliseconds(50.0))
                .Do(UpdateUI);
            FP3001.EnsureCameraSerialNumber();
        }

        private void UpdateDataVisualizer(PhotometryDataFrame pdf)
        {
            FPDataVisualizer.TryUpdatePlots(pdf.Activities, pdf.SystemTimestamp);
        }

        private void UpdateROIControl(PhotometryDataFrame pdf)
        {
            ROIEditor.TryUpdateNewFrame(pdf.PhotometryImage);
        }

        private void UpdateUI(PhotometryDataFrame pdf)
        {
            FPDataVisualizer.TryRefreshPlot();
        }

        protected override void OnLoad(EventArgs e)
        {
            Subscription = FP3001Comms.Subscribe();
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Subscription != null)
                Subscription.Dispose();
            base.OnClosing(e);
        }

        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        private void FPSValue_Label_Click(object sender, EventArgs e)
        {
            TryEnableTextEdit();
        }

        private void FPSValue_Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            var keyChar = e.KeyChar;
            TryApplyKeyFilter(keyChar, e);
        }

        private void FPSValue_Text_LostFocus(object sender, EventArgs e)
        {
            TryAcceptText();
        }

        private void Mode_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryUpdateMode();
        }

        private void FP3001Form_KeyDown(object sender, KeyEventArgs e)
        {
            ROIEditor.TryUpdateKeyDown(e.KeyCode);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                ROIEditor.TabSelectROI();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TryUpdateComponents()
        {
            try
            {
                UpdateComponents();
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
                ConsoleLogger.LogError(ex);
            }
        }

        private void TryUpdateMode()
        {
            try
            {
                UpdateMode();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateComponents()
        {
            FPSValue_Label.Text = FP3001.FPS.ToString() + " Hz";

            var modeFields = typeof(TriggerMode).GetFields()
                    .Where(fi => fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Any());

            Mode_Pairs = new Dictionary<string, object>();
            foreach (var fi in modeFields)
            {
                var value = fi.GetValue(null);
                var da = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                var desc = da.Description;

                Mode_Pairs[desc] = value;
            }

            Mode_ComboBox.DataSource = Mode_Pairs.ToList();
            Mode_ComboBox.DisplayMember = "Key";
            Mode_ComboBox.ValueMember = "Value";

            Mode_ComboBox.SelectedValue = FP3001.TriggerMode;
        }

        private void EnableTextEdit()
        {
            Focus();
            FPSValue_Text.Text = Regex.Replace(FPSValue_Label.Text, "[^.0-9]", "");
            FPSValue_Text.Size = FPSValue_Label.Size;
            FPSValue_Label.Visible = false;
            FPSValue_Label.Enabled = false;
            FPSValue_Label.Click -= FPSValue_Label_Click;
            FPS_TableLayoutPanel.Controls.Remove(FPSValue_Label);
            FPS_TableLayoutPanel.Controls.Add(FPSValue_Text, 1, 0);
            FPSValue_Text.Enabled = true;
            FPSValue_Text.Visible = true;
            FPSValue_Text.LostFocus += FPSValue_Text_LostFocus;
            FPSValue_Text.Focus();
        }

        private void ApplyKeyFilter(char keyChar, KeyPressEventArgs args)
        {
            if (keyChar == (char)Keys.Enter)
            {
                args.Handled = true;
                Focus();
                return;
            }
            var isValid = KeyFilteringHelpers.IsIntegerNumeric(keyChar);
            args.Handled = !isValid;
        }

        private void AcceptText()
        {
            var fps = int.Parse(FPSValue_Text.Text);
            fps = Math.Min(FPS_Max, Math.Max(FPS_Min, fps));
            FP3001.FPS = fps;
            FPSValue_Label.Text = fps.ToString() + " Hz";
            FPSValue_Text.Visible = false;
            FPSValue_Text.Enabled = false;
            FPSValue_Text.LostFocus -= FPSValue_Text_LostFocus;
            FPS_TableLayoutPanel.Controls.Remove(FPSValue_Text);
            FPS_TableLayoutPanel.Controls.Add(FPSValue_Label, 1, 0);
            FPSValue_Label.Visible = true;
            FPSValue_Label.Enabled = true;
            FPSValue_Label.Click += FPSValue_Label_Click;
        }

        private void UpdateMode()
        {
            if (Mode_ComboBox.SelectedValue is TriggerMode trigMode)
                FP3001.TriggerMode = trigMode;
        }
    }
}