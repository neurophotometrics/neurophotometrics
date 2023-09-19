
namespace Neurophotometrics.Design.Editors
{
    partial class SliderControl
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
            this.Dec_Lg_Button = new System.Windows.Forms.Button();
            this.Dec_Sm_Button = new System.Windows.Forms.Button();
            this.Inc_Sm_Button = new System.Windows.Forms.Button();
            this.Inc_Lg_Button = new System.Windows.Forms.Button();
            this.Slider_TrackBar = new System.Windows.Forms.TrackBar();
            this.Value_Label = new System.Windows.Forms.Label();
            this.Top_TableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Slider_TrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 6;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.Top_TableLayoutPanel.Controls.Add(this.Dec_Lg_Button, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Dec_Sm_Button, 1, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Inc_Sm_Button, 3, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Inc_Lg_Button, 4, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Slider_TrackBar, 2, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Value_Label, 5, 0);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 1;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(375, 30);
            this.Top_TableLayoutPanel.TabIndex = 0;
            this.Top_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Dec_Lg_Button
            // 
            this.Dec_Lg_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dec_Lg_Button.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dec_Lg_Button.Location = new System.Drawing.Point(0, 0);
            this.Dec_Lg_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Dec_Lg_Button.Name = "Dec_Lg_Button";
            this.Dec_Lg_Button.Size = new System.Drawing.Size(40, 30);
            this.Dec_Lg_Button.TabIndex = 0;
            this.Dec_Lg_Button.Text = "- -";
            this.Dec_Lg_Button.UseVisualStyleBackColor = true;
            this.Dec_Lg_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Dec_Lg_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // Dec_Sm_Button
            // 
            this.Dec_Sm_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dec_Sm_Button.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dec_Sm_Button.Location = new System.Drawing.Point(40, 0);
            this.Dec_Sm_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Dec_Sm_Button.Name = "Dec_Sm_Button";
            this.Dec_Sm_Button.Size = new System.Drawing.Size(40, 30);
            this.Dec_Sm_Button.TabIndex = 1;
            this.Dec_Sm_Button.Text = "-";
            this.Dec_Sm_Button.UseVisualStyleBackColor = true;
            this.Dec_Sm_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Dec_Sm_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // Inc_Sm_Button
            // 
            this.Inc_Sm_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inc_Sm_Button.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Inc_Sm_Button.Location = new System.Drawing.Point(225, 0);
            this.Inc_Sm_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Inc_Sm_Button.Name = "Inc_Sm_Button";
            this.Inc_Sm_Button.Size = new System.Drawing.Size(40, 30);
            this.Inc_Sm_Button.TabIndex = 2;
            this.Inc_Sm_Button.Text = "+";
            this.Inc_Sm_Button.UseVisualStyleBackColor = true;
            this.Inc_Sm_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Inc_Sm_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // Inc_Lg_Button
            // 
            this.Inc_Lg_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inc_Lg_Button.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Inc_Lg_Button.Location = new System.Drawing.Point(265, 0);
            this.Inc_Lg_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Inc_Lg_Button.Name = "Inc_Lg_Button";
            this.Inc_Lg_Button.Size = new System.Drawing.Size(40, 30);
            this.Inc_Lg_Button.TabIndex = 3;
            this.Inc_Lg_Button.Text = "+ +";
            this.Inc_Lg_Button.UseVisualStyleBackColor = true;
            this.Inc_Lg_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Inc_Lg_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // Slider_TrackBar
            // 
            this.Slider_TrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Slider_TrackBar.Location = new System.Drawing.Point(80, 0);
            this.Slider_TrackBar.Margin = new System.Windows.Forms.Padding(0);
            this.Slider_TrackBar.Name = "Slider_TrackBar";
            this.Slider_TrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Slider_TrackBar.Size = new System.Drawing.Size(145, 30);
            this.Slider_TrackBar.TabIndex = 4;
            this.Slider_TrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.Slider_TrackBar.Scroll += new System.EventHandler(this.Slider_TrackBar_Scroll);
            // 
            // Value_Label
            // 
            this.Value_Label.AutoSize = true;
            this.Value_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Value_Label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Value_Label.Location = new System.Drawing.Point(305, 0);
            this.Value_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Value_Label.Name = "Value_Label";
            this.Value_Label.Size = new System.Drawing.Size(70, 30);
            this.Value_Label.TabIndex = 5;
            this.Value_Label.Text = "99.999 Hz";
            this.Value_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SliderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SliderControl";
            this.Size = new System.Drawing.Size(375, 30);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Slider_TrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Button Dec_Lg_Button;
        private System.Windows.Forms.Button Dec_Sm_Button;
        private System.Windows.Forms.Button Inc_Sm_Button;
        private System.Windows.Forms.Button Inc_Lg_Button;
        private System.Windows.Forms.TrackBar Slider_TrackBar;
        private System.Windows.Forms.Label Value_Label;
    }
}
