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
            this.editorLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid = new Neurophotometrics.Design.PropertyGrid();
            this.testButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.editorLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel.Controls.Add(this.imageBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.editorLayoutPanel, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(479, 503);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // imageBox
            // 
            this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox.Image = null;
            this.imageBox.Location = new System.Drawing.Point(0, 0);
            this.imageBox.Margin = new System.Windows.Forms.Padding(0);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(479, 353);
            this.imageBox.TabIndex = 0;
            // 
            // editorLayoutPanel
            // 
            this.editorLayoutPanel.ColumnCount = 2;
            this.editorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.editorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.editorLayoutPanel.Controls.Add(this.propertyGrid, 0, 0);
            this.editorLayoutPanel.Controls.Add(this.testButton, 1, 0);
            this.editorLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorLayoutPanel.Location = new System.Drawing.Point(3, 356);
            this.editorLayoutPanel.Name = "editorLayoutPanel";
            this.editorLayoutPanel.RowCount = 1;
            this.editorLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.93694F));
            this.editorLayoutPanel.Size = new System.Drawing.Size(473, 144);
            this.editorLayoutPanel.TabIndex = 4;
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsVisibleIfAvailable = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(345, 138);
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // testButton
            // 
            this.testButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testButton.Location = new System.Drawing.Point(354, 3);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(116, 138);
            this.testButton.TabIndex = 2;
            this.testButton.Text = "Test Stimulation";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // LaserCalibrationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 503);
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(497, 550);
            this.Name = "LaserCalibrationDialog";
            this.Text = "LaserCalibrationDialog";
            this.tableLayoutPanel.ResumeLayout(false);
            this.editorLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Bonsai.Vision.Design.ImageBox imageBox;
        private System.Windows.Forms.Button testButton;
        private Neurophotometrics.Design.PropertyGrid propertyGrid;
        private System.Windows.Forms.TableLayoutPanel editorLayoutPanel;
    }
}