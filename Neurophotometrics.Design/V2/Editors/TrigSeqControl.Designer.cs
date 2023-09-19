
namespace Neurophotometrics.Design.V2.Editors
{
    partial class TrigSeqControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TrigSeq_DataGridView = new System.Windows.Forms.DataGridView();
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Remove_Button = new System.Windows.Forms.Button();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LEDName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LEDFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.TrigSeq_DataGridView)).BeginInit();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrigSeq_DataGridView
            // 
            this.TrigSeq_DataGridView.AllowUserToResizeColumns = false;
            this.TrigSeq_DataGridView.AllowUserToResizeRows = false;
            this.TrigSeq_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TrigSeq_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.TrigSeq_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TrigSeq_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.LEDName,
            this.LEDFlag});
            this.TrigSeq_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrigSeq_DataGridView.Location = new System.Drawing.Point(3, 3);
            this.TrigSeq_DataGridView.Margin = new System.Windows.Forms.Padding(1);
            this.TrigSeq_DataGridView.MultiSelect = false;
            this.TrigSeq_DataGridView.Name = "TrigSeq_DataGridView";
            this.TrigSeq_DataGridView.RowHeadersVisible = false;
            this.TrigSeq_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.TrigSeq_DataGridView.Size = new System.Drawing.Size(156, 308);
            this.TrigSeq_DataGridView.TabIndex = 0;
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Top_TableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.Top_TableLayoutPanel.ColumnCount = 1;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Top_TableLayoutPanel.Controls.Add(this.TrigSeq_DataGridView, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Remove_Button, 0, 1);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 2;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(162, 346);
            this.Top_TableLayoutPanel.TabIndex = 1;
            // 
            // Remove_Button
            // 
            this.Remove_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Remove_Button.Location = new System.Drawing.Point(5, 317);
            this.Remove_Button.Name = "Remove_Button";
            this.Remove_Button.Size = new System.Drawing.Size(152, 24);
            this.Remove_Button.TabIndex = 1;
            this.Remove_Button.Text = "Remove Selected";
            this.Remove_Button.UseVisualStyleBackColor = true;
            this.Remove_Button.Click += new System.EventHandler(this.Remove_Button_Click);
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Index.DefaultCellStyle = dataGridViewCellStyle2;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 62;
            // 
            // LEDName
            // 
            this.LEDName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LEDName.DefaultCellStyle = dataGridViewCellStyle3;
            this.LEDName.DisplayStyleForCurrentCellOnly = true;
            this.LEDName.HeaderText = "LED";
            this.LEDName.Name = "LEDName";
            this.LEDName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // LEDFlag
            // 
            this.LEDFlag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LEDFlag.DefaultCellStyle = dataGridViewCellStyle4;
            this.LEDFlag.HeaderText = "Flag";
            this.LEDFlag.Name = "LEDFlag";
            this.LEDFlag.ReadOnly = true;
            this.LEDFlag.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LEDFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LEDFlag.Width = 35;
            // 
            // TrigSeqControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Name = "TrigSeqControl";
            this.Size = new System.Drawing.Size(162, 346);
            ((System.ComponentModel.ISupportInitialize)(this.TrigSeq_DataGridView)).EndInit();
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView TrigSeq_DataGridView;
        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.Button Remove_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewComboBoxColumn LEDName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LEDFlag;
    }
}
