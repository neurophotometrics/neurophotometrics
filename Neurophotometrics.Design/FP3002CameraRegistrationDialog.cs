using Bonsai.Harp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class FP3002CameraRegistrationDialog : Form
    {
        readonly Func<Task> updateFirmwareAsync;

        public FP3002CameraRegistrationDialog(string portName)
        {
            InitializeComponent();
            var progress = new Progress<int>(ReportProgress);
            updateFirmwareAsync = () => UpdateCameraSerialNumber(portName, progress);
        }

        static async Task UpdateCameraSerialNumber(string portName, IProgress<int> progress)
        {
            using (var device = new AsyncDevice(portName))
            {
                progress.Report(20);
                var serialNumber = SerialNumberHelper.GetCameraSerialNumber();
                progress.Report(50);
                await device.WriteUInt64Async(ConfigurationRegisters.CameraSerialNumber, serialNumber);
                await device.CommandAsync(HarpCommand.Reset(ResetMode.Save));
                progress.Report(100);
            }
        }

        protected override async void OnShown(EventArgs e)
        {
            try { await Task.Run(updateFirmwareAsync); }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ReportProgress(int value)
        {
            progressBar.Value = value;
        }
    }
}
