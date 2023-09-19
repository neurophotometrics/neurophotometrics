namespace Neurophotometrics.Design.V1.Editors
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
            this.Image_PictureBox.Size = new System.Drawing.Size(693, 431);
            this.Image_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image_PictureBox.TabIndex = 0;
            this.Image_PictureBox.TabStop = false;
            this.Image_PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseDown);
            this.Image_PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseMove);
            this.Image_PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Image_PictureBox_MouseUp);
            this.Image_PictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Image_PictureBox_PreviewKeyDown);
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
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(693, 447);
            this.Top_TableLayoutPanel.TabIndex = 1;
            this.Top_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Info_TableLayoutPanel
            // 
            this.Info_TableLayoutPanel.AutoSize = true;
            this.Info_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Info_TableLayoutPanel.ColumnCount = 3;
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Info_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Info_TableLayoutPanel.Controls.Add(this.ROICount_Label, 0, 0);
            this.Info_TableLayoutPanel.Controls.Add(this.ROICountVal_Label, 1, 0);
            this.Info_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Info_TableLayoutPanel.Location = new System.Drawing.Point(0, 431);
            this.Info_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Info_TableLayoutPanel.Name = "Info_TableLayoutPanel";
            this.Info_TableLayoutPanel.RowCount = 1;
            this.Info_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Info_TableLayoutPanel.Size = new System.Drawing.Size(693, 16);
            this.Info_TableLayoutPanel.TabIndex = 1;
            this.Info_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // ROICount_Label
            // 
            this.ROICount_Label.AutoSize = true;
            this.ROICount_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROICount_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ROICount_Label.Location = new System.Drawing.Point(4, 0);
            this.ROICount_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ROICount_Label.Name = "ROICount_Label";
            this.ROICount_Label.Size = new System.Drawing.Size(76, 16);
            this.ROICount_Label.TabIndex = 1;
            this.ROICount_Label.Text = "ROI Count:";
            this.ROICount_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ROICount_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // ROICountVal_Label
            // 
            this.ROICountVal_Label.AutoSize = true;
            this.ROICountVal_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROICountVal_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ROICountVal_Label.Location = new System.Drawing.Point(88, 0);
            this.ROICountVal_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ROICountVal_Label.Name = "ROICountVal_Label";
            this.ROICountVal_Label.Size = new System.Drawing.Size(15, 16);
            this.ROICountVal_Label.TabIndex = 2;
            this.ROICountVal_Label.Text = "0";
            this.ROICountVal_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ROICountVal_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // ROIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ROIControl";
            this.Size = new System.Drawing.Size(693, 447);
            this.Click += new System.EventHandler(this.StrayClick);
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
    }
}
