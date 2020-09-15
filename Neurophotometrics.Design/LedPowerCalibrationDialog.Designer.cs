namespace Neurophotometrics.Design
{
    partial class LedPowerCalibrationDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label415 = new System.Windows.Forms.Label();
            this.label560 = new System.Windows.Forms.Label();
            this.label470 = new System.Windows.Forms.Label();
            this.slider470 = new Bonsai.Design.Slider();
            this.slider560 = new Bonsai.Design.Slider();
            this.slider415 = new Bonsai.Design.Slider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label415, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label560, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label470, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(459, 148);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label415
            // 
            this.label415.AutoSize = true;
            this.label415.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label415.Location = new System.Drawing.Point(2, 98);
            this.label415.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label415.Name = "label415";
            this.label415.Size = new System.Drawing.Size(63, 50);
            this.label415.TabIndex = 4;
            this.label415.Text = "L415 (%)";
            this.label415.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label560
            // 
            this.label560.AutoSize = true;
            this.label560.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label560.Location = new System.Drawing.Point(2, 49);
            this.label560.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label560.Name = "label560";
            this.label560.Size = new System.Drawing.Size(63, 49);
            this.label560.TabIndex = 3;
            this.label560.Text = "L560 (%)";
            this.label560.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label470
            // 
            this.label470.AutoSize = true;
            this.label470.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label470.Location = new System.Drawing.Point(2, 0);
            this.label470.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label470.Name = "label470";
            this.label470.Size = new System.Drawing.Size(63, 49);
            this.label470.TabIndex = 2;
            this.label470.Text = "L470 (%)";
            this.label470.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // slider470
            // 
            this.slider470.Converter = null;
            this.slider470.DecimalPlaces = null;
            this.slider470.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider470.Location = new System.Drawing.Point(71, 4);
            this.slider470.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.slider470.Maximum = 35200D;
            this.slider470.Minimum = 9600D;
            this.slider470.Name = "slider470";
            this.slider470.Size = new System.Drawing.Size(384, 59);
            this.slider470.TabIndex = 5;
            this.slider470.Value = 9600D;
            // 
            // slider560
            // 
            this.slider560.Converter = null;
            this.slider560.DecimalPlaces = null;
            this.slider560.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider560.Location = new System.Drawing.Point(71, 71);
            this.slider560.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.slider560.Maximum = 35200D;
            this.slider560.Minimum = 9600D;
            this.slider560.Name = "slider560";
            this.slider560.Size = new System.Drawing.Size(384, 59);
            this.slider560.TabIndex = 6;
            this.slider560.Value = 9600D;
            // 
            // slider415
            // 
            this.slider415.Converter = null;
            this.slider415.DecimalPlaces = null;
            this.slider415.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider415.Location = new System.Drawing.Point(71, 138);
            this.slider415.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.slider415.Maximum = 35200D;
            this.slider415.Minimum = 9600D;
            this.slider415.Name = "slider415";
            this.slider415.Size = new System.Drawing.Size(384, 61);
            this.slider415.TabIndex = 7;
            this.slider415.Value = 9600D;
            // 
            // LedPowerCalibrationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 148);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 180);
            this.Name = "LedPowerCalibrationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LedPowerCalibrationDialog";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label415;
        private System.Windows.Forms.Label label560;
        private System.Windows.Forms.Label label470;
        private Bonsai.Design.Slider slider470;
        private Bonsai.Design.Slider slider560;
        private Bonsai.Design.Slider slider415;
    }
}