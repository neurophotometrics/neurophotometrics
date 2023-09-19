
namespace Neurophotometrics.Design.V2.Editors
{
    partial class SplashScreen
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
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Text_Label = new System.Windows.Forms.Label();
            this.Splash_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.Logo_PictureBox = new System.Windows.Forms.PictureBox();
            this.Top_TableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 1;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Controls.Add(this.Text_Label, 0, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.Splash_ProgressBar, 0, 2);
            this.Top_TableLayoutPanel.Controls.Add(this.Logo_PictureBox, 0, 0);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 3;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(351, 146);
            this.Top_TableLayoutPanel.TabIndex = 0;
            // 
            // Text_Label
            // 
            this.Text_Label.AutoSize = true;
            this.Text_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Text_Label.Location = new System.Drawing.Point(3, 100);
            this.Text_Label.Name = "Text_Label";
            this.Text_Label.Size = new System.Drawing.Size(345, 26);
            this.Text_Label.TabIndex = 0;
            this.Text_Label.Text = "Connecting to FP3002...";
            this.Text_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Splash_ProgressBar
            // 
            this.Splash_ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Splash_ProgressBar.Location = new System.Drawing.Point(3, 129);
            this.Splash_ProgressBar.MarqueeAnimationSpeed = 10;
            this.Splash_ProgressBar.Name = "Splash_ProgressBar";
            this.Splash_ProgressBar.Size = new System.Drawing.Size(345, 14);
            this.Splash_ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.Splash_ProgressBar.TabIndex = 1;
            // 
            // Logo_PictureBox
            // 
            this.Logo_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logo_PictureBox.Image = global::Neurophotometrics.Design.Properties.Resources.Logo;
            this.Logo_PictureBox.Location = new System.Drawing.Point(3, 3);
            this.Logo_PictureBox.Name = "Logo_PictureBox";
            this.Logo_PictureBox.Size = new System.Drawing.Size(345, 94);
            this.Logo_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo_PictureBox.TabIndex = 2;
            this.Logo_PictureBox.TabStop = false;
            // 
            // SplashScreen
            // 
            this.ClientSize = new System.Drawing.Size(351, 146);
            this.ControlBox = false;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo_PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Label Text_Label;
        private System.Windows.Forms.ProgressBar Splash_ProgressBar;
        private System.Windows.Forms.PictureBox Logo_PictureBox;
    }
}
