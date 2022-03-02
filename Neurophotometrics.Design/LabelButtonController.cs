using System;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    class LabelButtonController : ContainerControl
    {
        private int layoutCount = 0;

        private const int DefaultHeight = 30;
        private const int DefaultWidth = 100;
        private const int VerticalMargin = 5;
        public LabelButtonController()
        {
            // Initialize Component
            Label = new Label();
            Button = new Button();

            Label.SuspendLayout();
            Button.SuspendLayout();
            SuspendLayout();
            //
            // Label
            //
            Label.Name = "label";
            Label.Size = new System.Drawing.Size(DefaultWidth / 2, DefaultHeight - VerticalMargin);
            Label.Text = "Label:";
            Label.Click += new System.EventHandler(label_Click);
            //
            // Button
            //
            Button.Name = "button";
            Button.Size = new System.Drawing.Size(DefaultWidth / 2, DefaultHeight - VerticalMargin);
            Button.Text = "ON";
            Button.BackColor = System.Drawing.Color.White;
            Button.ForeColor = System.Drawing.Color.Red;

            Controls.Add(Label);
            Controls.Add(Button);

            Name = "LabelButtonController";
            Size = new System.Drawing.Size(DefaultHeight, DefaultWidth);

        }

        public Label Label { get; set; }

        public Button Button { get; set; }

        private void label_Click(object sender, EventArgs e)
        {
            if (Label.CanFocus)
            {
                Label.Focus();
            }
        }
    }
}
