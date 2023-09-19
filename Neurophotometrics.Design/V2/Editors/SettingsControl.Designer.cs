
namespace Neurophotometrics.Design.Editors
{
    partial class SettingsControl
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
            this.Save_Button = new System.Windows.Forms.Button();
            this.Load_Button = new System.Windows.Forms.Button();
            this.Settings_PropGrid = new System.Windows.Forms.PropertyGrid();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Top_TableLayoutPanel.ColumnCount = 2;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Top_TableLayoutPanel.Controls.Add(this.Save_Button, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Load_Button, 1, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Settings_PropGrid, 0, 1);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 2;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(296, 495);
            this.Top_TableLayoutPanel.TabIndex = 0;
            // 
            // Save_Button
            // 
            this.Save_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Save_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Save_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save_Button.Location = new System.Drawing.Point(3, 3);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(142, 24);
            this.Save_Button.TabIndex = 0;
            this.Save_Button.TabStop = false;
            this.Save_Button.Text = "Save Settings";
            this.Save_Button.UseVisualStyleBackColor = true;
            // 
            // Load_Button
            // 
            this.Load_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Load_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Load_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Load_Button.Location = new System.Drawing.Point(151, 3);
            this.Load_Button.Name = "Load_Button";
            this.Load_Button.Size = new System.Drawing.Size(142, 24);
            this.Load_Button.TabIndex = 1;
            this.Load_Button.TabStop = false;
            this.Load_Button.Text = "Load Settings";
            this.Load_Button.UseVisualStyleBackColor = true;
            // 
            // Settings_PropGrid
            // 
            this.Top_TableLayoutPanel.SetColumnSpan(this.Settings_PropGrid, 2);
            this.Settings_PropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Settings_PropGrid.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_PropGrid.Location = new System.Drawing.Point(3, 33);
            this.Settings_PropGrid.Name = "Settings_PropGrid";
            this.Settings_PropGrid.Size = new System.Drawing.Size(290, 459);
            this.Settings_PropGrid.TabIndex = 2;
            this.Settings_PropGrid.TabStop = false;
            this.Settings_PropGrid.ToolbarVisible = false;
            this.Settings_PropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Settings_PropGrid_PropertyValueChanged);
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(296, 495);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.PropertyGrid Settings_PropGrid;
    }
}
