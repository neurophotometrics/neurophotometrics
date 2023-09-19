
namespace Neurophotometrics.Design.V1.Forms
{
    partial class FP3001Form
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
            this.FPS_Label = new System.Windows.Forms.Label();
            this.FPSValue_Label = new System.Windows.Forms.Label();
            this.Mode_Label = new System.Windows.Forms.Label();
            this.Mode_ComboBox = new System.Windows.Forms.ComboBox();
            this.ROIEditor = new Neurophotometrics.Design.V1.Editors.ROIControl();
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FPS_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Mode_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FPDataVisualizer = new Neurophotometrics.Design.V1.Visualizers.PhotometryDataVisualizer();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.FPS_TableLayoutPanel.SuspendLayout();
            this.Mode_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FPS_Label
            // 
            this.FPS_Label.AutoSize = true;
            this.FPS_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FPS_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPS_Label.Location = new System.Drawing.Point(3, 0);
            this.FPS_Label.Name = "FPS_Label";
            this.FPS_Label.Size = new System.Drawing.Size(38, 36);
            this.FPS_Label.TabIndex = 3;
            this.FPS_Label.Text = "FPS:";
            this.FPS_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FPS_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // FPSValue_Label
            // 
            this.FPSValue_Label.AutoSize = true;
            this.FPSValue_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FPSValue_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPSValue_Label.Location = new System.Drawing.Point(47, 0);
            this.FPSValue_Label.Name = "FPSValue_Label";
            this.FPSValue_Label.Size = new System.Drawing.Size(49, 36);
            this.FPSValue_Label.TabIndex = 5;
            this.FPSValue_Label.Text = "100 Hz";
            this.FPSValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FPSValue_Label.Click += new System.EventHandler(this.FPSValue_Label_Click);
            // 
            // Mode_Label
            // 
            this.Mode_Label.AutoSize = true;
            this.Mode_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mode_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mode_Label.Location = new System.Drawing.Point(0, 0);
            this.Mode_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Mode_Label.Name = "Mode_Label";
            this.Mode_Label.Size = new System.Drawing.Size(47, 36);
            this.Mode_Label.TabIndex = 4;
            this.Mode_Label.Text = "Mode:";
            this.Mode_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Mode_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // Mode_ComboBox
            // 
            this.Mode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Mode_ComboBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mode_ComboBox.FormattingEnabled = true;
            this.Mode_ComboBox.Location = new System.Drawing.Point(53, 6);
            this.Mode_ComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.Mode_ComboBox.Name = "Mode_ComboBox";
            this.Mode_ComboBox.Size = new System.Drawing.Size(121, 24);
            this.Mode_ComboBox.TabIndex = 6;
            this.Mode_ComboBox.TabStop = false;
            this.Mode_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Mode_ComboBox_SelectedIndexChanged);
            // 
            // ROIEditor
            // 
            this.ROIEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROIEditor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ROIEditor.Location = new System.Drawing.Point(0, 42);
            this.ROIEditor.Margin = new System.Windows.Forms.Padding(0);
            this.ROIEditor.Name = "ROIEditor";
            this.ROIEditor.Regions = null;
            this.ROIEditor.Size = new System.Drawing.Size(433, 404);
            this.ROIEditor.TabIndex = 0;
            this.ROIEditor.TabStop = false;
            this.ROIEditor.Click += new System.EventHandler(this.StrayClick);
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.ColumnCount = 2;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Top_TableLayoutPanel.Controls.Add(this.FPS_TableLayoutPanel, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Mode_TableLayoutPanel, 1, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.ROIEditor, 0, 1);
            this.Top_TableLayoutPanel.Controls.Add(this.FPDataVisualizer, 1, 1);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 2;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(867, 446);
            this.Top_TableLayoutPanel.TabIndex = 7;
            // 
            // FPS_TableLayoutPanel
            // 
            this.FPS_TableLayoutPanel.AutoSize = true;
            this.FPS_TableLayoutPanel.ColumnCount = 3;
            this.FPS_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FPS_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FPS_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FPS_TableLayoutPanel.Controls.Add(this.FPS_Label, 0, 0);
            this.FPS_TableLayoutPanel.Controls.Add(this.FPSValue_Label, 1, 0);
            this.FPS_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FPS_TableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.FPS_TableLayoutPanel.Name = "FPS_TableLayoutPanel";
            this.FPS_TableLayoutPanel.RowCount = 1;
            this.FPS_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FPS_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.FPS_TableLayoutPanel.Size = new System.Drawing.Size(427, 36);
            this.FPS_TableLayoutPanel.TabIndex = 0;
            // 
            // Mode_TableLayoutPanel
            // 
            this.Mode_TableLayoutPanel.AutoSize = true;
            this.Mode_TableLayoutPanel.ColumnCount = 3;
            this.Mode_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Mode_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Mode_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Mode_TableLayoutPanel.Controls.Add(this.Mode_Label, 0, 0);
            this.Mode_TableLayoutPanel.Controls.Add(this.Mode_ComboBox, 1, 0);
            this.Mode_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mode_TableLayoutPanel.Location = new System.Drawing.Point(436, 3);
            this.Mode_TableLayoutPanel.Name = "Mode_TableLayoutPanel";
            this.Mode_TableLayoutPanel.RowCount = 1;
            this.Mode_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Mode_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Mode_TableLayoutPanel.Size = new System.Drawing.Size(428, 36);
            this.Mode_TableLayoutPanel.TabIndex = 1;
            // 
            // FPDataVisualizer
            // 
            this.FPDataVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FPDataVisualizer.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPDataVisualizer.Location = new System.Drawing.Point(436, 46);
            this.FPDataVisualizer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FPDataVisualizer.Name = "FPDataVisualizer";
            this.FPDataVisualizer.Size = new System.Drawing.Size(428, 396);
            this.FPDataVisualizer.TabIndex = 2;
            // 
            // FP3001Form
            // 
            this.ClientSize = new System.Drawing.Size(867, 446);
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FP3001Form";
            this.Text = "FP3001 Setup";
            this.Click += new System.EventHandler(this.StrayClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FP3001Form_KeyDown);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Top_TableLayoutPanel.PerformLayout();
            this.FPS_TableLayoutPanel.ResumeLayout(false);
            this.FPS_TableLayoutPanel.PerformLayout();
            this.Mode_TableLayoutPanel.ResumeLayout(false);
            this.Mode_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Editors.ROIControl ROIEditor;
        private System.Windows.Forms.Label Mode_Label;
        private System.Windows.Forms.Label FPS_Label;
        private System.Windows.Forms.Label FPSValue_Label;
        private System.Windows.Forms.ComboBox Mode_ComboBox;
        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Mode_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel FPS_TableLayoutPanel;
        private Visualizers.PhotometryDataVisualizer FPDataVisualizer;
    }
}
