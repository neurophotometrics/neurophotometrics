namespace Neurophotometrics.Design
{
    partial class FP3002CalibrationEditorForm
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
            this.visualizerPanel = new System.Windows.Forms.Panel();
            this.menuLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.setupGroupBox = new System.Windows.Forms.GroupBox();
            this.setupButton = new System.Windows.Forms.Button();
            this.fileGroupBox = new System.Windows.Forms.GroupBox();
            this.fileLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.restoreDeviceSettingsButton = new System.Windows.Forms.Button();
            this.storeDeviceSettingsButton = new System.Windows.Forms.Button();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.loadSettingsButton = new System.Windows.Forms.Button();
            this.propertyGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerGroupBox = new System.Windows.Forms.GroupBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.propertyGrid = new Neurophotometrics.Design.PropertyGrid();
            this.triggerModeView = new Neurophotometrics.Design.TriggerModeView();
            this.tableLayoutPanel.SuspendLayout();
            this.visualizerPanel.SuspendLayout();
            this.menuLayoutPanel.SuspendLayout();
            this.setupGroupBox.SuspendLayout();
            this.fileGroupBox.SuspendLayout();
            this.fileLayoutPanel.SuspendLayout();
            this.propertyGroupBox.SuspendLayout();
            this.triggerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 353F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.visualizerPanel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.propertyGroupBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.triggerGroupBox, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1061, 591);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // visualizerPanel
            // 
            this.visualizerPanel.Controls.Add(this.menuLayoutPanel);
            this.visualizerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizerPanel.Location = new System.Drawing.Point(3, 3);
            this.visualizerPanel.Name = "visualizerPanel";
            this.visualizerPanel.Size = new System.Drawing.Size(347, 585);
            this.visualizerPanel.TabIndex = 1;
            // 
            // menuLayoutPanel
            // 
            this.menuLayoutPanel.ColumnCount = 1;
            this.menuLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.menuLayoutPanel.Controls.Add(this.setupGroupBox, 0, 1);
            this.menuLayoutPanel.Controls.Add(this.fileGroupBox, 0, 0);
            this.menuLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.menuLayoutPanel.Name = "menuLayoutPanel";
            this.menuLayoutPanel.RowCount = 2;
            this.menuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.menuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.menuLayoutPanel.Size = new System.Drawing.Size(347, 585);
            this.menuLayoutPanel.TabIndex = 1;
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Controls.Add(this.setupButton);
            this.setupGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupGroupBox.Location = new System.Drawing.Point(3, 295);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(341, 287);
            this.setupGroupBox.TabIndex = 1;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Setup";
            // 
            // setupButton
            // 
            this.setupButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupButton.Location = new System.Drawing.Point(3, 27);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(335, 257);
            this.setupButton.TabIndex = 1;
            this.setupButton.Text = "Calibrate Power and Regions";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // fileGroupBox
            // 
            this.fileGroupBox.Controls.Add(this.fileLayoutPanel);
            this.fileGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileGroupBox.Location = new System.Drawing.Point(3, 3);
            this.fileGroupBox.Name = "fileGroupBox";
            this.fileGroupBox.Size = new System.Drawing.Size(341, 286);
            this.fileGroupBox.TabIndex = 2;
            this.fileGroupBox.TabStop = false;
            this.fileGroupBox.Text = "Load / Save";
            // 
            // fileLayoutPanel
            // 
            this.fileLayoutPanel.ColumnCount = 1;
            this.fileLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fileLayoutPanel.Controls.Add(this.restoreDeviceSettingsButton, 0, 3);
            this.fileLayoutPanel.Controls.Add(this.storeDeviceSettingsButton, 0, 2);
            this.fileLayoutPanel.Controls.Add(this.saveSettingsButton, 0, 1);
            this.fileLayoutPanel.Controls.Add(this.loadSettingsButton, 0, 0);
            this.fileLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileLayoutPanel.Location = new System.Drawing.Point(3, 27);
            this.fileLayoutPanel.Name = "fileLayoutPanel";
            this.fileLayoutPanel.RowCount = 4;
            this.fileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.fileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.fileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.fileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.fileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.fileLayoutPanel.Size = new System.Drawing.Size(335, 256);
            this.fileLayoutPanel.TabIndex = 0;
            // 
            // restoreDeviceSettingsButton
            // 
            this.restoreDeviceSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restoreDeviceSettingsButton.Location = new System.Drawing.Point(3, 195);
            this.restoreDeviceSettingsButton.Name = "restoreDeviceSettingsButton";
            this.restoreDeviceSettingsButton.Size = new System.Drawing.Size(329, 58);
            this.restoreDeviceSettingsButton.TabIndex = 3;
            this.restoreDeviceSettingsButton.Text = "Restore Device Settings";
            this.restoreDeviceSettingsButton.UseVisualStyleBackColor = true;
            // 
            // storeDeviceSettingsButton
            // 
            this.storeDeviceSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.storeDeviceSettingsButton.Location = new System.Drawing.Point(3, 131);
            this.storeDeviceSettingsButton.Name = "storeDeviceSettingsButton";
            this.storeDeviceSettingsButton.Size = new System.Drawing.Size(329, 58);
            this.storeDeviceSettingsButton.TabIndex = 2;
            this.storeDeviceSettingsButton.Text = "Store Device Settings";
            this.storeDeviceSettingsButton.UseVisualStyleBackColor = true;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveSettingsButton.Location = new System.Drawing.Point(3, 67);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(329, 58);
            this.saveSettingsButton.TabIndex = 1;
            this.saveSettingsButton.Text = "Save to File...";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // loadSettingsButton
            // 
            this.loadSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadSettingsButton.Location = new System.Drawing.Point(3, 3);
            this.loadSettingsButton.Name = "loadSettingsButton";
            this.loadSettingsButton.Size = new System.Drawing.Size(329, 58);
            this.loadSettingsButton.TabIndex = 0;
            this.loadSettingsButton.Text = "Load from File...";
            this.loadSettingsButton.UseVisualStyleBackColor = true;
            this.loadSettingsButton.Click += new System.EventHandler(this.loadSettingsButton_Click);
            // 
            // propertyGroupBox
            // 
            this.propertyGroupBox.Controls.Add(this.propertyGrid);
            this.propertyGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGroupBox.Location = new System.Drawing.Point(356, 3);
            this.propertyGroupBox.Name = "propertyGroupBox";
            this.propertyGroupBox.Size = new System.Drawing.Size(348, 585);
            this.propertyGroupBox.TabIndex = 2;
            this.propertyGroupBox.TabStop = false;
            this.propertyGroupBox.Text = "FP3002 Configuration";
            // 
            // triggerGroupBox
            // 
            this.triggerGroupBox.Controls.Add(this.triggerModeView);
            this.triggerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerGroupBox.Location = new System.Drawing.Point(710, 3);
            this.triggerGroupBox.Name = "triggerGroupBox";
            this.triggerGroupBox.Size = new System.Drawing.Size(348, 585);
            this.triggerGroupBox.TabIndex = 3;
            this.triggerGroupBox.TabStop = false;
            this.triggerGroupBox.Text = "Trigger Sequence";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "XML files|*.xml|All files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "FP3002Config.xml";
            this.saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 27);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(342, 555);
            this.propertyGrid.TabIndex = 2;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // triggerModeView
            // 
            this.triggerModeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerModeView.Location = new System.Drawing.Point(3, 27);
            this.triggerModeView.Name = "triggerModeView";
            this.triggerModeView.Size = new System.Drawing.Size(342, 555);
            this.triggerModeView.TabIndex = 0;
            // 
            // FP3002CalibrationEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 591);
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FP3002CalibrationEditorForm";
            this.Text = "FP3002 Setup";
            this.tableLayoutPanel.ResumeLayout(false);
            this.visualizerPanel.ResumeLayout(false);
            this.menuLayoutPanel.ResumeLayout(false);
            this.setupGroupBox.ResumeLayout(false);
            this.fileGroupBox.ResumeLayout(false);
            this.fileLayoutPanel.ResumeLayout(false);
            this.propertyGroupBox.ResumeLayout(false);
            this.triggerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button loadSettingsButton;
        private System.Windows.Forms.Panel visualizerPanel;
        private Neurophotometrics.Design.PropertyGrid propertyGrid;
        private System.Windows.Forms.TableLayoutPanel menuLayoutPanel;
        private System.Windows.Forms.GroupBox setupGroupBox;
        private System.Windows.Forms.GroupBox fileGroupBox;
        private System.Windows.Forms.TableLayoutPanel fileLayoutPanel;
        private System.Windows.Forms.Button restoreDeviceSettingsButton;
        private System.Windows.Forms.Button storeDeviceSettingsButton;
        private System.Windows.Forms.Button saveSettingsButton;
        private System.Windows.Forms.GroupBox propertyGroupBox;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox triggerGroupBox;
        private TriggerModeView triggerModeView;
    }
}