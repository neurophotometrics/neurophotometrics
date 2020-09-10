namespace Neurophotometrics.Design
{
    partial class FP3001CalibrationEditorForm
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
            this.calibrateRoiButton = new System.Windows.Forms.Button();
            this.visualizerPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.calibrateRoiButton, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.visualizerPanel, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(459, 378);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // calibrateRoiButton
            // 
            this.calibrateRoiButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrateRoiButton.Location = new System.Drawing.Point(2, 2);
            this.calibrateRoiButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.calibrateRoiButton.Name = "calibrateRoiButton";
            this.calibrateRoiButton.Size = new System.Drawing.Size(455, 34);
            this.calibrateRoiButton.TabIndex = 0;
            this.calibrateRoiButton.Text = "Calibrate Regions";
            this.calibrateRoiButton.UseVisualStyleBackColor = true;
            this.calibrateRoiButton.Click += new System.EventHandler(this.calibrateRoiButton_Click);
            // 
            // visualizerPanel
            // 
            this.visualizerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizerPanel.Location = new System.Drawing.Point(2, 40);
            this.visualizerPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.visualizerPanel.Name = "visualizerPanel";
            this.visualizerPanel.Size = new System.Drawing.Size(455, 336);
            this.visualizerPanel.TabIndex = 1;
            // 
            // FP3001CalibrationEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 378);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = global::Neurophotometrics.Design.Properties.Resources.Neurophotometrics;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimumSize = new System.Drawing.Size(470, 420);
            this.Name = "FP3001CalibrationEditorForm";
            this.Text = "FP3001 Setup";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button calibrateRoiButton;
        private System.Windows.Forms.Panel visualizerPanel;
    }
}