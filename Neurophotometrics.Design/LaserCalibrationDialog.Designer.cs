namespace Neurophotometrics.Design
{
    partial class LaserCalibrationDialog
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
            this.imageBox = new Bonsai.Vision.Design.ImageBox();
            this.trigLaserButton = new System.Windows.Forms.Button();
            this.editorLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid = new Neurophotometrics.Design.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.alignLaserButton = new System.Windows.Forms.Button();
            this.measLaserPwrButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.editorLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 363F));
            this.tableLayoutPanel.Controls.Add(this.imageBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.trigLaserButton, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.editorLayoutPanel, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(361, 459);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // imageBox
            // 
            this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox.Image = null;
            this.imageBox.ImageScale = 1D;
            this.imageBox.Location = new System.Drawing.Point(0, 0);
            this.imageBox.Margin = new System.Windows.Forms.Padding(0);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(363, 281);
            this.imageBox.TabIndex = 0;
            // 
            // trigLaserButton
            // 
            this.trigLaserButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trigLaserButton.Location = new System.Drawing.Point(4, 283);
            this.trigLaserButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.trigLaserButton.Name = "trigLaserButton";
            this.trigLaserButton.Size = new System.Drawing.Size(355, 24);
            this.trigLaserButton.TabIndex = 5;
            this.trigLaserButton.Text = "Trigger Laser";
            this.trigLaserButton.UseVisualStyleBackColor = true;
            this.trigLaserButton.Click += new System.EventHandler(this.trigLaserButton_Click);
            // 
            // editorLayoutPanel
            // 
            this.editorLayoutPanel.ColumnCount = 2;
            this.editorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.editorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.editorLayoutPanel.Controls.Add(this.propertyGrid, 0, 0);
            this.editorLayoutPanel.Controls.Add(this.splitContainer1, 1, 0);
            this.editorLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorLayoutPanel.Location = new System.Drawing.Point(2, 311);
            this.editorLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.editorLayoutPanel.Name = "editorLayoutPanel";
            this.editorLayoutPanel.RowCount = 1;
            this.editorLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.editorLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.editorLayoutPanel.Size = new System.Drawing.Size(359, 146);
            this.editorLayoutPanel.TabIndex = 4;
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsVisibleIfAvailable = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(2, 2);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(263, 142);
            this.propertyGrid.SplitterDistance = 1.2D;
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(270, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.alignLaserButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.measLaserPwrButton);
            this.splitContainer1.Size = new System.Drawing.Size(86, 140);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 4;
            // 
            // alignLaserButton
            // 
            this.alignLaserButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alignLaserButton.Location = new System.Drawing.Point(0, 0);
            this.alignLaserButton.Margin = new System.Windows.Forms.Padding(2);
            this.alignLaserButton.Name = "alignLaserButton";
            this.alignLaserButton.Size = new System.Drawing.Size(86, 65);
            this.alignLaserButton.TabIndex = 2;
            this.alignLaserButton.Text = "Align Laser";
            this.alignLaserButton.UseVisualStyleBackColor = true;
            this.alignLaserButton.Click += new System.EventHandler(this.alignLaserButton_Click);
            // 
            // measLaserPwrButton
            // 
            this.measLaserPwrButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measLaserPwrButton.Location = new System.Drawing.Point(0, 0);
            this.measLaserPwrButton.Margin = new System.Windows.Forms.Padding(2);
            this.measLaserPwrButton.Name = "measLaserPwrButton";
            this.measLaserPwrButton.Size = new System.Drawing.Size(86, 71);
            this.measLaserPwrButton.TabIndex = 1;
            this.measLaserPwrButton.Text = "Measure Power";
            this.measLaserPwrButton.UseVisualStyleBackColor = true;
            this.measLaserPwrButton.Click += new System.EventHandler(this.measLaserPwrButton_Click);
            // 
            // LaserCalibrationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 459);
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(377, 454);
            this.Name = "LaserCalibrationDialog";
            this.Text = "LaserCalibrationDialog";
            this.tableLayoutPanel.ResumeLayout(false);
            this.editorLayoutPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Bonsai.Vision.Design.ImageBox imageBox;
        private System.Windows.Forms.Button alignLaserButton;
        private Neurophotometrics.Design.PropertyGrid propertyGrid;
        private System.Windows.Forms.TableLayoutPanel editorLayoutPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button measLaserPwrButton;
        private System.Windows.Forms.Button trigLaserButton;
    }
}