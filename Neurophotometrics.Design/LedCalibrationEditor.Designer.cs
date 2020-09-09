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
            this.label470 = new System.Windows.Forms.Label();
            this.slider470 = new Bonsai.Design.Slider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label470, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.slider470, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(459, 54);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label470
            // 
            this.label470.AutoSize = true;
            this.label470.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label470.Location = new System.Drawing.Point(2, 0);
            this.label470.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label470.Name = "label470";
            this.label470.Size = new System.Drawing.Size(63, 54);
            this.label470.TabIndex = 1;
            this.label470.Text = "Power (%)";
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
            this.slider470.Size = new System.Drawing.Size(384, 46);
            this.slider470.TabIndex = 4;
            this.slider470.Value = 9600D;
            // 
            // LedCalibrationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LedCalibrationEditor";
            this.Size = new System.Drawing.Size(459, 54);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label470;
        private Bonsai.Design.Slider slider470;
    }
}
