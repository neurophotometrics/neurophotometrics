namespace Neurophotometrics.Design
{
    partial class SecondaryLaserCalibrationDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trigSecondaryLaser = new System.Windows.Forms.Button();
            this.propertyGrid1 = new Bonsai.Design.PropertyGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.trigSecondaryLaser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.3125F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.6875F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(361, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // trigSecondaryLaser
            // 
            this.trigSecondaryLaser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trigSecondaryLaser.Location = new System.Drawing.Point(4, 2);
            this.trigSecondaryLaser.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.trigSecondaryLaser.Name = "trigSecondaryLaser";
            this.trigSecondaryLaser.Size = new System.Drawing.Size(353, 35);
            this.trigSecondaryLaser.TabIndex = 0;
            this.trigSecondaryLaser.Text = "Trigger Laser";
            this.trigSecondaryLaser.UseVisualStyleBackColor = true;
            this.trigSecondaryLaser.Click += new System.EventHandler(this.trigSecondaryLaser_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CommandsVisibleIfAvailable = false;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(2, 41);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.Size = new System.Drawing.Size(357, 149);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // SecondaryLaserCalibrationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 192);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SecondaryLaserCalibrationDialog";
            this.Text = "SecondaryLaserCalibrationDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button trigSecondaryLaser;
        private Bonsai.Design.PropertyGrid propertyGrid1;
    }
}