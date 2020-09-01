namespace Neurophotometrics.Design
{
    partial class LedCalibrationEditor
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label415 = new System.Windows.Forms.Label();
            this.label470 = new System.Windows.Forms.Label();
            this.label560 = new System.Windows.Forms.Label();
            this.slider415 = new Bonsai.Design.Slider();
            this.slider470 = new Bonsai.Design.Slider();
            this.slider560 = new Bonsai.Design.Slider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label415, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label470, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label560, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.slider415, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.slider470, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.slider560, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(689, 240);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label415
            // 
            this.label415.AutoSize = true;
            this.label415.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label415.Location = new System.Drawing.Point(3, 0);
            this.label415.Name = "label415";
            this.label415.Size = new System.Drawing.Size(94, 80);
            this.label415.TabIndex = 0;
            this.label415.Text = "415nm";
            this.label415.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label470
            // 
            this.label470.AutoSize = true;
            this.label470.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label470.Location = new System.Drawing.Point(3, 80);
            this.label470.Name = "label470";
            this.label470.Size = new System.Drawing.Size(94, 80);
            this.label470.TabIndex = 1;
            this.label470.Text = "470nm";
            this.label470.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label560
            // 
            this.label560.AutoSize = true;
            this.label560.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label560.Location = new System.Drawing.Point(3, 160);
            this.label560.Name = "label560";
            this.label560.Size = new System.Drawing.Size(94, 80);
            this.label560.TabIndex = 2;
            this.label560.Text = "560nm";
            this.label560.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // slider415
            // 
            this.slider415.Converter = null;
            this.slider415.DecimalPlaces = null;
            this.slider415.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider415.Maximum = 30000D;
            this.slider415.Minimum = 0D;
            this.slider415.Name = "slider415";
            this.slider415.Size = new System.Drawing.Size(577, 68);
            this.slider415.TabIndex = 3;
            this.slider415.Value = 0D;
            // 
            // slider470
            // 
            this.slider470.Converter = null;
            this.slider470.DecimalPlaces = null;
            this.slider470.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider470.Maximum = 30000D;
            this.slider470.Minimum = 0D;
            this.slider470.Name = "slider470";
            this.slider470.Size = new System.Drawing.Size(577, 68);
            this.slider470.TabIndex = 4;
            this.slider470.Value = 0D;
            // 
            // slider560
            // 
            this.slider560.Converter = null;
            this.slider560.DecimalPlaces = null;
            this.slider560.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slider560.Maximum = 30000D;
            this.slider560.Minimum = 0D;
            this.slider560.Name = "slider560";
            this.slider560.Size = new System.Drawing.Size(577, 68);
            this.slider560.TabIndex = 4;
            this.slider560.Value = 0D;
            // 
            // LedCalibrationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "LedCalibrationEditor";
            this.Size = new System.Drawing.Size(689, 240);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label415;
        private System.Windows.Forms.Label label470;
        private System.Windows.Forms.Label label560;
        private Bonsai.Design.Slider slider415;
        private Bonsai.Design.Slider slider470;
        private Bonsai.Design.Slider slider560;
    }
}
