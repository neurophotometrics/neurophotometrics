using Bonsai.Harp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class FP3002FirmwareDialog : Form
    {
        readonly Func<Task> updateFirmwareAsync;

        public FP3002FirmwareDialog(string portName, DeviceFirmware firmware)
        {
            InitializeComponent();
            var progress = new Progress<int>(ReportProgress);
            updateFirmwareAsync = () => Bootloader.UpdateFirmwareAsync(portName, firmware, progress);
        }

        protected override void OnLoad(EventArgs e)
        {
            updateFirmwareAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show(this, task.Exception.InnerException.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else DialogResult = DialogResult.OK;
                Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            base.OnLoad(e);
        }

        private void ReportProgress(int value)
        {
            progressBar.Value = value;
        }
    }
}
