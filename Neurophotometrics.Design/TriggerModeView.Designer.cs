namespace Neurophotometrics.Design
{
    partial class TriggerModeView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.triggerPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // triggerPanel
            // 
            this.triggerPanel.BackColor = System.Drawing.SystemColors.Window;
            this.triggerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerPanel.Location = new System.Drawing.Point(0, 0);
            this.triggerPanel.Name = "triggerPanel";
            this.triggerPanel.Size = new System.Drawing.Size(150, 150);
            this.triggerPanel.TabIndex = 0;
            this.triggerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.triggerPanel_Paint);
            // 
            // TriggerModeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.triggerPanel);
            this.Name = "TriggerModeView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel triggerPanel;
    }
}
