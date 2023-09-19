namespace Neurophotometrics.Design.V2.Editors
{
    partial class LEDPowersControl
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
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.L415_Label = new System.Windows.Forms.Label();
            this.L560_Label = new System.Windows.Forms.Label();
            this.L470_Label = new System.Windows.Forms.Label();
            this.L415_Button = new System.Windows.Forms.Button();
            this.L470_Button = new System.Windows.Forms.Button();
            this.L560_Button = new System.Windows.Forms.Button();
            this.L415_Slider = new Neurophotometrics.Design.V2.UserControls.LedPowerSlider();
            this.L470_Slider = new Neurophotometrics.Design.V2.UserControls.LedPowerSlider();
            this.L560_Slider = new Neurophotometrics.Design.V2.UserControls.LedPowerSlider();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 3;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Top_TableLayoutPanel.Controls.Add(this.L415_Label, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.L560_Label, 0, 2);
            this.Top_TableLayoutPanel.Controls.Add(this.L470_Label, 0, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.L415_Button, 2, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.L470_Button, 2, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.L560_Button, 2, 2);
            this.Top_TableLayoutPanel.Controls.Add(this.L415_Slider, 1, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.L470_Slider, 1, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.L560_Slider, 1, 2);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 3;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(726, 163);
            this.Top_TableLayoutPanel.TabIndex = 0;
            // 
            // L415_Label
            // 
            this.L415_Label.AutoSize = true;
            this.L415_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L415_Label.Enabled = false;
            this.L415_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L415_Label.Location = new System.Drawing.Point(0, 0);
            this.L415_Label.Margin = new System.Windows.Forms.Padding(0);
            this.L415_Label.Name = "L415_Label";
            this.L415_Label.Size = new System.Drawing.Size(78, 54);
            this.L415_Label.TabIndex = 1;
            this.L415_Label.Text = "415 nm";
            this.L415_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L560_Label
            // 
            this.L560_Label.AutoSize = true;
            this.L560_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L560_Label.Enabled = false;
            this.L560_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L560_Label.Location = new System.Drawing.Point(0, 108);
            this.L560_Label.Margin = new System.Windows.Forms.Padding(0);
            this.L560_Label.Name = "L560_Label";
            this.L560_Label.Size = new System.Drawing.Size(78, 55);
            this.L560_Label.TabIndex = 2;
            this.L560_Label.Text = "560 nm";
            this.L560_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L470_Label
            // 
            this.L470_Label.AutoSize = true;
            this.L470_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L470_Label.Enabled = false;
            this.L470_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L470_Label.Location = new System.Drawing.Point(0, 54);
            this.L470_Label.Margin = new System.Windows.Forms.Padding(0);
            this.L470_Label.Name = "L470_Label";
            this.L470_Label.Size = new System.Drawing.Size(78, 54);
            this.L470_Label.TabIndex = 3;
            this.L470_Label.Text = "470 nm";
            this.L470_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L415_Button
            // 
            this.L415_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L415_Button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L415_Button.Location = new System.Drawing.Point(636, 0);
            this.L415_Button.Margin = new System.Windows.Forms.Padding(0);
            this.L415_Button.Name = "L415_Button";
            this.L415_Button.Size = new System.Drawing.Size(90, 54);
            this.L415_Button.TabIndex = 4;
            this.L415_Button.TabStop = false;
            this.L415_Button.Text = "Edit";
            this.L415_Button.UseVisualStyleBackColor = true;
            this.L415_Button.Click += new System.EventHandler(this.L415_Button_Click);
            // 
            // L470_Button
            // 
            this.L470_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L470_Button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L470_Button.Location = new System.Drawing.Point(636, 54);
            this.L470_Button.Margin = new System.Windows.Forms.Padding(0);
            this.L470_Button.Name = "L470_Button";
            this.L470_Button.Size = new System.Drawing.Size(90, 54);
            this.L470_Button.TabIndex = 5;
            this.L470_Button.TabStop = false;
            this.L470_Button.Text = "Edit";
            this.L470_Button.UseVisualStyleBackColor = true;
            this.L470_Button.Click += new System.EventHandler(this.L470_Button_Click);
            // 
            // L560_Button
            // 
            this.L560_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L560_Button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L560_Button.Location = new System.Drawing.Point(636, 108);
            this.L560_Button.Margin = new System.Windows.Forms.Padding(0);
            this.L560_Button.Name = "L560_Button";
            this.L560_Button.Size = new System.Drawing.Size(90, 55);
            this.L560_Button.TabIndex = 6;
            this.L560_Button.TabStop = false;
            this.L560_Button.Text = "Edit";
            this.L560_Button.UseVisualStyleBackColor = true;
            this.L560_Button.Click += new System.EventHandler(this.L560_Button_Click);
            // 
            // L415_Slider
            // 
            this.L415_Slider.AutoSize = true;
            this.L415_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.L415_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L415_Slider.Enabled = false;
            this.L415_Slider.HighSpeedRepeatDelay = 2000;
            this.L415_Slider.HighSpeedRepeatPeriod = 100;
            this.L415_Slider.LEDName = null;
            this.L415_Slider.Location = new System.Drawing.Point(78, 0);
            this.L415_Slider.LowSpeedRepeatDelay = 500;
            this.L415_Slider.LowSpeedRepeatPeriod = 300;
            this.L415_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.L415_Slider.Name = "L415_Slider";
            this.L415_Slider.Size = new System.Drawing.Size(558, 54);
            this.L415_Slider.TabIndex = 7;
            this.L415_Slider.TabStop = false;
            // 
            // L470_Slider
            // 
            this.L470_Slider.AutoSize = true;
            this.L470_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.L470_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L470_Slider.Enabled = false;
            this.L470_Slider.HighSpeedRepeatDelay = 2000;
            this.L470_Slider.HighSpeedRepeatPeriod = 100;
            this.L470_Slider.LEDName = null;
            this.L470_Slider.Location = new System.Drawing.Point(78, 54);
            this.L470_Slider.LowSpeedRepeatDelay = 500;
            this.L470_Slider.LowSpeedRepeatPeriod = 300;
            this.L470_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.L470_Slider.Name = "L470_Slider";
            this.L470_Slider.Size = new System.Drawing.Size(558, 54);
            this.L470_Slider.TabIndex = 8;
            this.L470_Slider.TabStop = false;
            // 
            // L560_Slider
            // 
            this.L560_Slider.AutoSize = true;
            this.L560_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.L560_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.L560_Slider.Enabled = false;
            this.L560_Slider.HighSpeedRepeatDelay = 2000;
            this.L560_Slider.HighSpeedRepeatPeriod = 100;
            this.L560_Slider.LEDName = null;
            this.L560_Slider.Location = new System.Drawing.Point(78, 108);
            this.L560_Slider.LowSpeedRepeatDelay = 500;
            this.L560_Slider.LowSpeedRepeatPeriod = 300;
            this.L560_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.L560_Slider.Name = "L560_Slider";
            this.L560_Slider.Size = new System.Drawing.Size(558, 55);
            this.L560_Slider.TabIndex = 9;
            this.L560_Slider.TabStop = false;
            // 
            // LEDPowersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LEDPowersControl";
            this.Size = new System.Drawing.Size(726, 163);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Label L415_Label;
        private System.Windows.Forms.Label L560_Label;
        private System.Windows.Forms.Label L470_Label;
        private System.Windows.Forms.Button L415_Button;
        private System.Windows.Forms.Button L470_Button;
        private System.Windows.Forms.Button L560_Button;
        private UserControls.LedPowerSlider L415_Slider;
        private UserControls.LedPowerSlider L470_Slider;
        private UserControls.LedPowerSlider L560_Slider;
    }
}