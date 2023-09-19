namespace Neurophotometrics.Design.V2.Editors
{
    partial class ROIControl
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
            this.Image_PictureBox = new System.Windows.Forms.PictureBox();
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Info_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ROICount_Label = new System.Windows.Forms.Label();
            this.ROICountVal_Label = new System.Windows.Forms.Label();
            this.L470_Label = new System.Windows.Forms.Label();
            this.L470Power_Slider = new Neurophotometrics.Design.V2.UserControls.LedPowerSlider();
            ((System.ComponentModel.ISupportInitialize)(this.Image_PictureBox)).BeginInit();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.Info_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Image_PictureBox
            // 
            this.Image_PictureBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Image_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Image_PictureBox.Location = new System.Drawing.Point(0, 0);
            this.Image_PictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.Image_PictureBox.Name = "Image_PictureBox";
            this.Image_PictureBox.Size = new System.Drawing.Size(462, 323);
            this.Image_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image_PictureBox.TabIndex = 0;
            this.Image_PictureBox.TabStop = false;
            this.Image_PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseDown);
            this.Image_PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseMove);
            this.Image_PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseUp);
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 1;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Controls.Add(this.Image_PictureBox, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Info_TableLayoutPanel, 0, 1);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 2;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(462, 348);
            this.Top_TableLayoutPanel.TabIndex = 1;
            // 
            // Info_TableLayoutPanel
            // 
            this.Info_TableLayoutPanel.AutoSize = true;
            this.Info_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Info_TableLayoutPanel.ColumnCount = 4;
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Info_TableLayoutPanel.Controls.Add(this.ROICount_Label, 2, 0);
            this.Info_TableLayoutPanel.Controls.Add(this.ROICountVal_Label, 3, 0);
            this.Info_TableLayoutPanel.Controls.Add(this.L470_Label, 0, 0);
            this.Info_TableLayoutPanel.Controls.Add(this.L470Power_Slider, 1, 0);
            this.Info_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Info_TableLayoutPanel.Location = new System.Drawing.Point(0, 323);
            this.Info_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Info_TableLayoutPanel.Name = "Info_TableLayoutPanel";
            this.Info_TableLayoutPanel.RowCount = 1;
            this.Info_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Info_TableLayoutPanel.Size = new System.Drawing.Size(462, 25);
            this.Info_TableLayoutPanel.TabIndex = 1;
            // 
            // ROICount_Label
            // 
            this.ROICount_Label.AutoSize = true;
            this.ROICount_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROICount_Label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ROICount_Label.Location = new System.Drawing.Point(338, 0);
            this.ROICount_Label.Name = "ROICount_Label";
            this.ROICount_Label.Size = new System.Drawing.Size(97, 25);
            this.ROICount_Label.TabIndex = 1;
            this.ROICount_Label.Text = "ROI Count:";
            this.ROICount_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ROICountVal_Label
            // 
            this.ROICountVal_Label.AutoSize = true;
            this.ROICountVal_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROICountVal_Label.Location = new System.Drawing.Point(441, 0);
            this.ROICountVal_Label.Name = "ROICountVal_Label";
            this.ROICountVal_Label.Size = new System.Drawing.Size(18, 25);
            this.ROICountVal_Label.TabIndex = 2;
            this.ROICountVal_Label.Text = "0";
            this.ROICountVal_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L470_Label
            // 
            this.L470_Label.AutoSize = true;
            this.L470_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L470_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L470_Label.Location = new System.Drawing.Point(3, 0);
            this.L470_Label.Name = "L470_Label";
            this.L470_Label.Size = new System.Drawing.Size(55, 25);
            this.L470_Label.TabIndex = 4;
            this.L470_Label.Text = "L470";
            this.L470_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L470Power_Slider
            // 
            this.L470Power_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L470Power_Slider.HighSpeedRepeatDelay = 2000;
            this.L470Power_Slider.HighSpeedRepeatPeriod = 100;
            this.L470Power_Slider.LEDName = null;
            this.L470Power_Slider.Location = new System.Drawing.Point(61, 0);
            this.L470Power_Slider.LowSpeedRepeatDelay = 500;
            this.L470Power_Slider.LowSpeedRepeatPeriod = 300;
            this.L470Power_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.L470Power_Slider.Name = "L470Power_Slider";
            this.L470Power_Slider.Size = new System.Drawing.Size(274, 25);
            this.L470Power_Slider.TabIndex = 5;
            this.L470Power_Slider.TabStop = false;
            // 
            // ROIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ROIControl";
            this.Size = new System.Drawing.Size(462, 348);
            ((System.ComponentModel.ISupportInitialize)(this.Image_PictureBox)).EndInit();
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            this.Info_TableLayoutPanel.ResumeLayout(false);
            this.Info_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image_PictureBox;
        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Info_TableLayoutPanel;
        private System.Windows.Forms.Label ROICount_Label;
        private System.Windows.Forms.Label ROICountVal_Label;
        private System.Windows.Forms.Label L470_Label;
        private UserControls.LedPowerSlider L470Power_Slider;
    }
}
