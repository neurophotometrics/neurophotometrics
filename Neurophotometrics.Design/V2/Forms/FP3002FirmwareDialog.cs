﻿using Bonsai.Harp;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Forms
{
    internal partial class FP3002FirmwareDialog : Form
    {
        private readonly Func<Task> UpdateFirmwareAsync;

        public FP3002FirmwareDialog(string portName, DeviceFirmware firmware)
        {
            InitializeComponent();
            var progress = new Progress<int>(ReportProgress);
            UpdateFirmwareAsync = () => Bootloader.UpdateFirmwareAsync(portName, firmware, progress);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            var size = ClientSize;
            ClientSize = new Size((int)(size.Width * factor.Width), (int)(size.Height * factor.Height));
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateFirmwareAsync().ContinueWith(task =>
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