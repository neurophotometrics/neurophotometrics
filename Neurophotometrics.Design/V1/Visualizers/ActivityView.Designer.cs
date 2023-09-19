
namespace Neurophotometrics.Design.V1.Visualizers
{
    partial class ActivityView
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
            this.Title_Panel = new System.Windows.Forms.Panel();
            this.Logo_PictureBox = new System.Windows.Forms.PictureBox();
            this.Title_Label = new System.Windows.Forms.Label();
            this.Plots_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Settings_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Capacity_Label = new System.Windows.Forms.Label();
            this.CapacityValue_Label = new System.Windows.Forms.Label();
            this.ConfigPlots_Button = new System.Windows.Forms.Button();
            this.CapacityValue_Text = new System.Windows.Forms.TextBox();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.Title_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo_PictureBox)).BeginInit();
            this.Settings_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.Top_TableLayoutPanel.ColumnCount = 1;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Controls.Add(this.Title_Panel, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Plots_TableLayoutPanel, 0, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.Settings_TableLayoutPanel, 0, 2);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 3;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(960, 540);
            this.Top_TableLayoutPanel.TabIndex = 0;
            this.Top_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Title_Panel
            // 
            this.Title_Panel.BackColor = System.Drawing.SystemColors.Control;
            this.Title_Panel.Controls.Add(this.Logo_PictureBox);
            this.Title_Panel.Controls.Add(this.Title_Label);
            this.Title_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Title_Panel.Location = new System.Drawing.Point(0, 0);
            this.Title_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.Title_Panel.Name = "Title_Panel";
            this.Title_Panel.Size = new System.Drawing.Size(960, 30);
            this.Title_Panel.TabIndex = 0;
            this.Title_Panel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Logo_PictureBox
            // 
            this.Logo_PictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.Logo_PictureBox.Image = global::Neurophotometrics.Design.Properties.Resources.Logo;
            this.Logo_PictureBox.Location = new System.Drawing.Point(0, 0);
            this.Logo_PictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.Logo_PictureBox.Name = "Logo_PictureBox";
            this.Logo_PictureBox.Size = new System.Drawing.Size(30, 30);
            this.Logo_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo_PictureBox.TabIndex = 0;
            this.Logo_PictureBox.TabStop = false;
            this.Logo_PictureBox.Click += new System.EventHandler(this.StrayClick);
            // 
            // Title_Label
            // 
            this.Title_Label.BackColor = System.Drawing.SystemColors.Control;
            this.Title_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Title_Label.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title_Label.Location = new System.Drawing.Point(0, 0);
            this.Title_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Title_Label.Name = "Title_Label";
            this.Title_Label.Size = new System.Drawing.Size(960, 30);
            this.Title_Label.TabIndex = 1;
            this.Title_Label.Text = "Photometry Data Visualizer";
            this.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Title_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // Plots_TableLayoutPanel
            // 
            this.Plots_TableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.Plots_TableLayoutPanel.ColumnCount = 1;
            this.Plots_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Plots_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Plots_TableLayoutPanel.Location = new System.Drawing.Point(3, 33);
            this.Plots_TableLayoutPanel.Name = "Plots_TableLayoutPanel";
            this.Plots_TableLayoutPanel.RowCount = 1;
            this.Plots_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Plots_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Plots_TableLayoutPanel.Size = new System.Drawing.Size(954, 465);
            this.Plots_TableLayoutPanel.TabIndex = 1;
            this.Plots_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Settings_TableLayoutPanel
            // 
            this.Settings_TableLayoutPanel.AutoSize = true;
            this.Settings_TableLayoutPanel.ColumnCount = 4;
            this.Settings_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Settings_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Settings_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Settings_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Settings_TableLayoutPanel.Controls.Add(this.Capacity_Label, 0, 0);
            this.Settings_TableLayoutPanel.Controls.Add(this.CapacityValue_Label, 1, 0);
            this.Settings_TableLayoutPanel.Controls.Add(this.ConfigPlots_Button, 3, 0);
            this.Settings_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Settings_TableLayoutPanel.Location = new System.Drawing.Point(0, 501);
            this.Settings_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Settings_TableLayoutPanel.Name = "Settings_TableLayoutPanel";
            this.Settings_TableLayoutPanel.RowCount = 1;
            this.Settings_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Settings_TableLayoutPanel.Size = new System.Drawing.Size(960, 39);
            this.Settings_TableLayoutPanel.TabIndex = 2;
            this.Settings_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Capacity_Label
            // 
            this.Capacity_Label.AutoSize = true;
            this.Capacity_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Capacity_Label.Location = new System.Drawing.Point(3, 0);
            this.Capacity_Label.Name = "Capacity_Label";
            this.Capacity_Label.Size = new System.Drawing.Size(74, 39);
            this.Capacity_Label.TabIndex = 0;
            this.Capacity_Label.Text = "Capacity:";
            this.Capacity_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Capacity_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // CapacityValue_Label
            // 
            this.CapacityValue_Label.AutoSize = true;
            this.CapacityValue_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CapacityValue_Label.Location = new System.Drawing.Point(83, 0);
            this.CapacityValue_Label.Name = "CapacityValue_Label";
            this.CapacityValue_Label.Size = new System.Drawing.Size(44, 39);
            this.CapacityValue_Label.TabIndex = 1;
            this.CapacityValue_Label.Text = "1000";
            this.CapacityValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CapacityValue_Label.Click += new System.EventHandler(this.CapacityValue_Label_Click);
            // 
            // ConfigPlots_Button
            // 
            this.ConfigPlots_Button.AutoSize = true;
            this.ConfigPlots_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigPlots_Button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigPlots_Button.Location = new System.Drawing.Point(757, 0);
            this.ConfigPlots_Button.Margin = new System.Windows.Forms.Padding(0);
            this.ConfigPlots_Button.Name = "ConfigPlots_Button";
            this.ConfigPlots_Button.Size = new System.Drawing.Size(203, 39);
            this.ConfigPlots_Button.TabIndex = 2;
            this.ConfigPlots_Button.Text = "Configure Plots";
            this.ConfigPlots_Button.UseVisualStyleBackColor = true;
            this.ConfigPlots_Button.Click += new System.EventHandler(this.ConfigPlots_Button_Click);
            // 
            // CapacityValue_Text
            // 
            this.CapacityValue_Text.Enabled = false;
            this.CapacityValue_Text.Location = new System.Drawing.Point(0, 0);
            this.CapacityValue_Text.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.CapacityValue_Text.Multiline = true;
            this.CapacityValue_Text.Name = "CapacityValue_Text";
            this.CapacityValue_Text.Size = new System.Drawing.Size(960, 540);
            this.CapacityValue_Text.TabIndex = 0;
            this.CapacityValue_Text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CapacityValue_Text_KeyPress);
            // 
            // ActivityView
            // 
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Controls.Add(this.CapacityValue_Text);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(650, 350);
            this.Name = "ActivityView";
            this.Size = new System.Drawing.Size(960, 540);
            this.Click += new System.EventHandler(this.StrayClick);
            this.Resize += new System.EventHandler(this.ActivityView_Resize);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            this.Title_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo_PictureBox)).EndInit();
            this.Settings_TableLayoutPanel.ResumeLayout(false);
            this.Settings_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Panel Title_Panel;
        private System.Windows.Forms.PictureBox Logo_PictureBox;
        private System.Windows.Forms.Label Title_Label;
        private System.Windows.Forms.TableLayoutPanel Plots_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Settings_TableLayoutPanel;
        private System.Windows.Forms.Label Capacity_Label;
        private System.Windows.Forms.Label CapacityValue_Label;
        private System.Windows.Forms.TextBox CapacityValue_Text;
        private System.Windows.Forms.Button ConfigPlots_Button;
    }
}
