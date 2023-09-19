namespace Neurophotometrics.Design.V2.Visualizers
{
    partial class PlotSettingGridView
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
            this.PlotSetting_DataGridView = new System.Windows.Forms.DataGridView();
            this.SettingNamesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SettingValuesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PlotSetting_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PlotSetting_DataGridView
            // 
            this.PlotSetting_DataGridView.AllowUserToAddRows = false;
            this.PlotSetting_DataGridView.AllowUserToDeleteRows = false;
            this.PlotSetting_DataGridView.AllowUserToResizeColumns = false;
            this.PlotSetting_DataGridView.AllowUserToResizeRows = false;
            this.PlotSetting_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlotSetting_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SettingNamesColumn,
            this.SettingValuesColumn});
            this.PlotSetting_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.PlotSetting_DataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.PlotSetting_DataGridView.MultiSelect = false;
            this.PlotSetting_DataGridView.Name = "PlotSetting_DataGridView";
            this.PlotSetting_DataGridView.RowHeadersVisible = false;
            this.PlotSetting_DataGridView.RowHeadersWidth = 62;
            this.PlotSetting_DataGridView.RowTemplate.Height = 28;
            this.PlotSetting_DataGridView.Size = new System.Drawing.Size(367, 610);
            this.PlotSetting_DataGridView.TabIndex = 0;
            this.PlotSetting_DataGridView.TabStop = false;
            // 
            // SettingNamesColumn
            // 
            this.SettingNamesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.SettingNamesColumn.HeaderText = "SettingNames";
            this.SettingNamesColumn.MinimumWidth = 8;
            this.SettingNamesColumn.Name = "SettingNamesColumn";
            this.SettingNamesColumn.ReadOnly = true;
            this.SettingNamesColumn.Width = 27;
            // 
            // SettingValuesColumn
            // 
            this.SettingValuesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.SettingValuesColumn.HeaderText = "SettingValues";
            this.SettingValuesColumn.MinimumWidth = 8;
            this.SettingValuesColumn.Name = "SettingValuesColumn";
            this.SettingValuesColumn.Width = 27;
            // 
            // PlotSettingGridView
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.PlotSetting_DataGridView);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlotSettingGridView";
            this.Size = new System.Drawing.Size(367, 610);
            ((System.ComponentModel.ISupportInitialize)(this.PlotSetting_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView PlotSetting_DataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettingNamesColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettingValuesColumn;
    }
}
