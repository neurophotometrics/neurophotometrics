using System;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    public partial class FP3002ResetDialog : Form
    {
        public FP3002ResetDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            resetTimer.Start();
            base.OnLoad(e);
        }

        private void resetTimer_Tick(object sender, EventArgs e)
        {
            progressBar.Increment(resetTimer.Interval);
            if (progressBar.Value >= progressBar.Maximum)
            {
                Close();
            }
        }
    }
}
