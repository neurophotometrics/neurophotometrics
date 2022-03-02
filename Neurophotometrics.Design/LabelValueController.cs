using System;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    class LabelValueController : ContainerControl
    {
        private int layoutCount = 0;

        private const int DefaultHeight = 30;
        private const int DefaultWidth = 100;
        private const int VerticalMargin = 5;
        public LabelValueController()
        {
            // Initialize Component
            Label = new Label();
            Value = new Label();

            Label.SuspendLayout();
            Value.SuspendLayout();
            SuspendLayout();

            //
            // Label
            //
            Label.Name = "label";
            Label.Size = new System.Drawing.Size(DefaultWidth / 2, DefaultHeight - VerticalMargin);
            Label.Text = "Label:";
            Label.Click += new System.EventHandler(label_Click);
            //
            // Value
            //
            Value.Name = "value";
            Value.Size = new System.Drawing.Size(DefaultWidth / 2, DefaultHeight - VerticalMargin);
            Value.Text = "ON";

            Controls.Add(Label);
            Controls.Add(Value);

            Name = "LabelValueController";
            Size = new System.Drawing.Size(DefaultHeight, DefaultWidth);
        }

        public Label Label { get; set; }

        public Label Value { get; set; }

        private void label_Click(object sender, EventArgs e)
        {
            if (Label.CanFocus)
            {
                Label.Focus();
            }
        }
    }
}