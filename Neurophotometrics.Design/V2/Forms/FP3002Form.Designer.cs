namespace Neurophotometrics.Design.V2.Forms
{
    partial class FP3002Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FP3002Form));
            this.Cal_GroupBox = new System.Windows.Forms.GroupBox();
            this.Cal_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PrevNext_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Load_Button = new System.Windows.Forms.Button();
            this.Prev_Button = new System.Windows.Forms.Button();
            this.Next_Button = new System.Windows.Forms.Button();
            this.Save_Button = new System.Windows.Forms.Button();
            this.PersistReg_Button = new System.Windows.Forms.Button();
            this.Desc_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DescTitle_Label = new System.Windows.Forms.Label();
            this.DescText_Label = new System.Windows.Forms.Label();
            this.Editors_TabControl = new System.Windows.Forms.TabControl();
            this.ROICalibration_TabPage = new System.Windows.Forms.TabPage();
            this.Regions_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ROIControl_Editor = new Neurophotometrics.Design.V2.Editors.ROIControl();
            this.Regions_PhotometryDataVisualizer = new Neurophotometrics.Design.V2.Visualizers.PhotometryDataVisualizer();
            this.TrigSeqCalibration_TabPage = new System.Windows.Forms.TabPage();
            this.TrigSeq_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TrigSeq_SignalTimingVisualizer = new Neurophotometrics.Design.V2.Visualizers.SignalTimingVisualizer();
            this.TrigSeqControl_Editor = new Neurophotometrics.Design.V2.Editors.TrigSeqControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.FPS_Label = new System.Windows.Forms.Label();
            this.TriggerPeriod_Slider = new Neurophotometrics.Design.V2.UserControls.TriggerPeriodSlider();
            this.LEDCalibration_TabPage = new System.Windows.Forms.TabPage();
            this.LEDPowers_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LEDPowersControl_Editor = new Neurophotometrics.Design.V2.Editors.LEDPowersControl();
            this.LaserCalibration_TabPage = new System.Windows.Forms.TabPage();
            this.Laser_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PulseTrainControl_Editor = new Neurophotometrics.Design.V2.Editors.PulseTrainControl();
            this.Miscellaneous_TabPage = new System.Windows.Forms.TabPage();
            this.MiscellaneousControl_Editor = new Neurophotometrics.Design.V2.Editors.DigitalIOsControl();
            this.Cal_GroupBox.SuspendLayout();
            this.Cal_TableLayoutPanel.SuspendLayout();
            this.PrevNext_TableLayoutPanel.SuspendLayout();
            this.Desc_TableLayoutPanel.SuspendLayout();
            this.Editors_TabControl.SuspendLayout();
            this.ROICalibration_TabPage.SuspendLayout();
            this.Regions_TableLayoutPanel.SuspendLayout();
            this.TrigSeqCalibration_TabPage.SuspendLayout();
            this.TrigSeq_TableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.LEDCalibration_TabPage.SuspendLayout();
            this.LEDPowers_TableLayoutPanel.SuspendLayout();
            this.LaserCalibration_TabPage.SuspendLayout();
            this.Laser_TableLayoutPanel.SuspendLayout();
            this.Miscellaneous_TabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cal_GroupBox
            // 
            this.Cal_GroupBox.Controls.Add(this.Cal_TableLayoutPanel);
            this.Cal_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cal_GroupBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cal_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.Cal_GroupBox.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.Cal_GroupBox.Name = "Cal_GroupBox";
            this.Cal_GroupBox.Padding = new System.Windows.Forms.Padding(0);
            this.Cal_GroupBox.Size = new System.Drawing.Size(1418, 765);
            this.Cal_GroupBox.TabIndex = 3;
            this.Cal_GroupBox.TabStop = false;
            this.Cal_GroupBox.Text = "Calibration";
            // 
            // Cal_TableLayoutPanel
            // 
            this.Cal_TableLayoutPanel.ColumnCount = 1;
            this.Cal_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Cal_TableLayoutPanel.Controls.Add(this.PrevNext_TableLayoutPanel, 0, 2);
            this.Cal_TableLayoutPanel.Controls.Add(this.Desc_TableLayoutPanel, 0, 1);
            this.Cal_TableLayoutPanel.Controls.Add(this.Editors_TabControl, 0, 0);
            this.Cal_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cal_TableLayoutPanel.Location = new System.Drawing.Point(0, 19);
            this.Cal_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Cal_TableLayoutPanel.Name = "Cal_TableLayoutPanel";
            this.Cal_TableLayoutPanel.RowCount = 3;
            this.Cal_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Cal_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Cal_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Cal_TableLayoutPanel.Size = new System.Drawing.Size(1418, 746);
            this.Cal_TableLayoutPanel.TabIndex = 0;
            // 
            // PrevNext_TableLayoutPanel
            // 
            this.PrevNext_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PrevNext_TableLayoutPanel.ColumnCount = 8;
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PrevNext_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PrevNext_TableLayoutPanel.Controls.Add(this.Load_Button, 5, 0);
            this.PrevNext_TableLayoutPanel.Controls.Add(this.Prev_Button, 0, 0);
            this.PrevNext_TableLayoutPanel.Controls.Add(this.Next_Button, 7, 0);
            this.PrevNext_TableLayoutPanel.Controls.Add(this.Save_Button, 4, 0);
            this.PrevNext_TableLayoutPanel.Controls.Add(this.PersistReg_Button, 3, 0);
            this.PrevNext_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrevNext_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrevNext_TableLayoutPanel.Location = new System.Drawing.Point(0, 716);
            this.PrevNext_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PrevNext_TableLayoutPanel.Name = "PrevNext_TableLayoutPanel";
            this.PrevNext_TableLayoutPanel.RowCount = 1;
            this.PrevNext_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PrevNext_TableLayoutPanel.Size = new System.Drawing.Size(1418, 30);
            this.PrevNext_TableLayoutPanel.TabIndex = 1;
            // 
            // Load_Button
            // 
            this.Load_Button.AutoSize = true;
            this.Load_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Load_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Load_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Load_Button.Location = new System.Drawing.Point(805, 0);
            this.Load_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Load_Button.Name = "Load_Button";
            this.Load_Button.Size = new System.Drawing.Size(126, 30);
            this.Load_Button.TabIndex = 3;
            this.Load_Button.TabStop = false;
            this.Load_Button.Text = "Load All Settings";
            this.Load_Button.UseVisualStyleBackColor = true;
            this.Load_Button.Click += new System.EventHandler(this.Load_Button_Click);
            // 
            // Prev_Button
            // 
            this.Prev_Button.AutoSize = true;
            this.Prev_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Prev_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Prev_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prev_Button.Location = new System.Drawing.Point(0, 0);
            this.Prev_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Prev_Button.Name = "Prev_Button";
            this.Prev_Button.Size = new System.Drawing.Size(73, 30);
            this.Prev_Button.TabIndex = 0;
            this.Prev_Button.TabStop = false;
            this.Prev_Button.Text = "Previous";
            this.Prev_Button.UseVisualStyleBackColor = true;
            this.Prev_Button.Visible = false;
            this.Prev_Button.Click += new System.EventHandler(this.Button_Click);
            // 
            // Next_Button
            // 
            this.Next_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Next_Button.Location = new System.Drawing.Point(1314, 0);
            this.Next_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Next_Button.Name = "Next_Button";
            this.Next_Button.Size = new System.Drawing.Size(103, 30);
            this.Next_Button.TabIndex = 1;
            this.Next_Button.TabStop = false;
            this.Next_Button.Text = "Next";
            this.Next_Button.UseVisualStyleBackColor = true;
            this.Next_Button.Click += new System.EventHandler(this.Button_Click);
            // 
            // Save_Button
            // 
            this.Save_Button.AutoSize = true;
            this.Save_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Save_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Save_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save_Button.Location = new System.Drawing.Point(679, 0);
            this.Save_Button.Margin = new System.Windows.Forms.Padding(0);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(126, 30);
            this.Save_Button.TabIndex = 2;
            this.Save_Button.TabStop = false;
            this.Save_Button.Text = "Save All Settings";
            this.Save_Button.UseVisualStyleBackColor = true;
            this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
            // 
            // PersistReg_Button
            // 
            this.PersistReg_Button.AutoSize = true;
            this.PersistReg_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PersistReg_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PersistReg_Button.Location = new System.Drawing.Point(456, 0);
            this.PersistReg_Button.Margin = new System.Windows.Forms.Padding(0);
            this.PersistReg_Button.Name = "PersistReg_Button";
            this.PersistReg_Button.Size = new System.Drawing.Size(223, 30);
            this.PersistReg_Button.TabIndex = 4;
            this.PersistReg_Button.Text = "Write To Persistent Registers";
            this.PersistReg_Button.UseVisualStyleBackColor = true;
            this.PersistReg_Button.Click += new System.EventHandler(this.PersistReg_Button_Click);
            // 
            // Desc_TableLayoutPanel
            // 
            this.Desc_TableLayoutPanel.AutoSize = true;
            this.Desc_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Desc_TableLayoutPanel.BackColor = System.Drawing.Color.LightGray;
            this.Desc_TableLayoutPanel.ColumnCount = 1;
            this.Desc_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Desc_TableLayoutPanel.Controls.Add(this.DescTitle_Label, 0, 0);
            this.Desc_TableLayoutPanel.Controls.Add(this.DescText_Label, 0, 1);
            this.Desc_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Desc_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Desc_TableLayoutPanel.Location = new System.Drawing.Point(0, 670);
            this.Desc_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Desc_TableLayoutPanel.Name = "Desc_TableLayoutPanel";
            this.Desc_TableLayoutPanel.RowCount = 2;
            this.Desc_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Desc_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Desc_TableLayoutPanel.Size = new System.Drawing.Size(1418, 46);
            this.Desc_TableLayoutPanel.TabIndex = 0;
            // 
            // DescTitle_Label
            // 
            this.DescTitle_Label.AutoSize = true;
            this.DescTitle_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescTitle_Label.Location = new System.Drawing.Point(0, 0);
            this.DescTitle_Label.Margin = new System.Windows.Forms.Padding(0);
            this.DescTitle_Label.Name = "DescTitle_Label";
            this.DescTitle_Label.Size = new System.Drawing.Size(1418, 16);
            this.DescTitle_Label.TabIndex = 0;
            this.DescTitle_Label.Text = "Emission Calibration:";
            this.DescTitle_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DescText_Label
            // 
            this.DescText_Label.AutoSize = true;
            this.DescText_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescText_Label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescText_Label.Location = new System.Drawing.Point(0, 16);
            this.DescText_Label.Margin = new System.Windows.Forms.Padding(0);
            this.DescText_Label.Name = "DescText_Label";
            this.DescText_Label.Size = new System.Drawing.Size(1418, 30);
            this.DescText_Label.TabIndex = 1;
            this.DescText_Label.Text = resources.GetString("DescText_Label.Text");
            // 
            // Editors_TabControl
            // 
            this.Editors_TabControl.Controls.Add(this.ROICalibration_TabPage);
            this.Editors_TabControl.Controls.Add(this.TrigSeqCalibration_TabPage);
            this.Editors_TabControl.Controls.Add(this.LEDCalibration_TabPage);
            this.Editors_TabControl.Controls.Add(this.LaserCalibration_TabPage);
            this.Editors_TabControl.Controls.Add(this.Miscellaneous_TabPage);
            this.Editors_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editors_TabControl.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Editors_TabControl.Location = new System.Drawing.Point(0, 0);
            this.Editors_TabControl.Margin = new System.Windows.Forms.Padding(0);
            this.Editors_TabControl.Multiline = true;
            this.Editors_TabControl.Name = "Editors_TabControl";
            this.Editors_TabControl.SelectedIndex = 0;
            this.Editors_TabControl.Size = new System.Drawing.Size(1418, 670);
            this.Editors_TabControl.TabIndex = 0;
            this.Editors_TabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.Editors_TabControl_Selecting);
            this.Editors_TabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.Editors_TabControl_Selected);
            this.Editors_TabControl.Deselected += new System.Windows.Forms.TabControlEventHandler(this.Editors_TabControl_Deselected);
            // 
            // ROICalibration_TabPage
            // 
            this.ROICalibration_TabPage.Controls.Add(this.Regions_TableLayoutPanel);
            this.ROICalibration_TabPage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ROICalibration_TabPage.Location = new System.Drawing.Point(4, 25);
            this.ROICalibration_TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.ROICalibration_TabPage.Name = "ROICalibration_TabPage";
            this.ROICalibration_TabPage.Size = new System.Drawing.Size(1410, 641);
            this.ROICalibration_TabPage.TabIndex = 0;
            this.ROICalibration_TabPage.Text = "Emission Alignment";
            this.ROICalibration_TabPage.UseVisualStyleBackColor = true;
            // 
            // Regions_TableLayoutPanel
            // 
            this.Regions_TableLayoutPanel.ColumnCount = 2;
            this.Regions_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Regions_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Regions_TableLayoutPanel.Controls.Add(this.ROIControl_Editor, 0, 0);
            this.Regions_TableLayoutPanel.Controls.Add(this.Regions_PhotometryDataVisualizer, 1, 0);
            this.Regions_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Regions_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Regions_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Regions_TableLayoutPanel.Name = "Regions_TableLayoutPanel";
            this.Regions_TableLayoutPanel.RowCount = 1;
            this.Regions_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Regions_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 641F));
            this.Regions_TableLayoutPanel.Size = new System.Drawing.Size(1410, 641);
            this.Regions_TableLayoutPanel.TabIndex = 0;
            // 
            // ROIControl_Editor
            // 
            this.ROIControl_Editor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ROIControl_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROIControl_Editor.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ROIControl_Editor.Location = new System.Drawing.Point(0, 0);
            this.ROIControl_Editor.Margin = new System.Windows.Forms.Padding(0);
            this.ROIControl_Editor.Name = "ROIControl_Editor";
            this.ROIControl_Editor.Regions = null;
            this.ROIControl_Editor.Size = new System.Drawing.Size(705, 641);
            this.ROIControl_Editor.TabIndex = 0;
            // 
            // Regions_PhotometryDataVisualizer
            // 
            this.Regions_PhotometryDataVisualizer.AutoSize = true;
            this.Regions_PhotometryDataVisualizer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Regions_PhotometryDataVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Regions_PhotometryDataVisualizer.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Regions_PhotometryDataVisualizer.Location = new System.Drawing.Point(708, 4);
            this.Regions_PhotometryDataVisualizer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Regions_PhotometryDataVisualizer.Name = "Regions_PhotometryDataVisualizer";
            this.Regions_PhotometryDataVisualizer.Size = new System.Drawing.Size(699, 633);
            this.Regions_PhotometryDataVisualizer.TabIndex = 1;
            // 
            // TrigSeqCalibration_TabPage
            // 
            this.TrigSeqCalibration_TabPage.Controls.Add(this.TrigSeq_TableLayoutPanel);
            this.TrigSeqCalibration_TabPage.Location = new System.Drawing.Point(4, 25);
            this.TrigSeqCalibration_TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.TrigSeqCalibration_TabPage.Name = "TrigSeqCalibration_TabPage";
            this.TrigSeqCalibration_TabPage.Size = new System.Drawing.Size(1410, 641);
            this.TrigSeqCalibration_TabPage.TabIndex = 3;
            this.TrigSeqCalibration_TabPage.Text = "Excitation Sequence";
            this.TrigSeqCalibration_TabPage.UseVisualStyleBackColor = true;
            // 
            // TrigSeq_TableLayoutPanel
            // 
            this.TrigSeq_TableLayoutPanel.ColumnCount = 2;
            this.TrigSeq_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TrigSeq_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.TrigSeq_TableLayoutPanel.Controls.Add(this.TrigSeq_SignalTimingVisualizer, 1, 1);
            this.TrigSeq_TableLayoutPanel.Controls.Add(this.TrigSeqControl_Editor, 0, 0);
            this.TrigSeq_TableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.TrigSeq_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrigSeq_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TrigSeq_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TrigSeq_TableLayoutPanel.Name = "TrigSeq_TableLayoutPanel";
            this.TrigSeq_TableLayoutPanel.RowCount = 2;
            this.TrigSeq_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TrigSeq_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TrigSeq_TableLayoutPanel.Size = new System.Drawing.Size(1410, 641);
            this.TrigSeq_TableLayoutPanel.TabIndex = 0;
            // 
            // TrigSeq_SignalTimingVisualizer
            // 
            this.TrigSeq_SignalTimingVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrigSeq_SignalTimingVisualizer.Location = new System.Drawing.Point(355, 35);
            this.TrigSeq_SignalTimingVisualizer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TrigSeq_SignalTimingVisualizer.Name = "TrigSeq_SignalTimingVisualizer";
            this.TrigSeq_SignalTimingVisualizer.Size = new System.Drawing.Size(1052, 602);
            this.TrigSeq_SignalTimingVisualizer.TabIndex = 0;
            // 
            // TrigSeqControl_Editor
            // 
            this.TrigSeqControl_Editor.AutoSize = true;
            this.TrigSeqControl_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrigSeqControl_Editor.Location = new System.Drawing.Point(3, 4);
            this.TrigSeqControl_Editor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TrigSeqControl_Editor.Name = "TrigSeqControl_Editor";
            this.TrigSeq_TableLayoutPanel.SetRowSpan(this.TrigSeqControl_Editor, 2);
            this.TrigSeqControl_Editor.Size = new System.Drawing.Size(346, 633);
            this.TrigSeqControl_Editor.TabIndex = 1;
            this.TrigSeqControl_Editor.TriggerSequence = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.FPS_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TriggerPeriod_Slider, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(352, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1058, 31);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // FPS_Label
            // 
            this.FPS_Label.AutoSize = true;
            this.FPS_Label.BackColor = System.Drawing.SystemColors.ControlDark;
            this.FPS_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FPS_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPS_Label.Location = new System.Drawing.Point(3, 0);
            this.FPS_Label.Name = "FPS_Label";
            this.FPS_Label.Size = new System.Drawing.Size(86, 31);
            this.FPS_Label.TabIndex = 0;
            this.FPS_Label.Text = "Frame Rate:";
            this.FPS_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TriggerPeriod_Slider
            // 
            this.TriggerPeriod_Slider.AutoSize = true;
            this.TriggerPeriod_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TriggerPeriod_Slider.BackColor = System.Drawing.SystemColors.ControlDark;
            this.TriggerPeriod_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TriggerPeriod_Slider.HighSpeedRepeatDelay = 2000;
            this.TriggerPeriod_Slider.HighSpeedRepeatPeriod = 100;
            this.TriggerPeriod_Slider.Location = new System.Drawing.Point(92, 0);
            this.TriggerPeriod_Slider.LowSpeedRepeatDelay = 500;
            this.TriggerPeriod_Slider.LowSpeedRepeatPeriod = 300;
            this.TriggerPeriod_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.TriggerPeriod_Slider.Name = "TriggerPeriod_Slider";
            this.TriggerPeriod_Slider.Size = new System.Drawing.Size(966, 31);
            this.TriggerPeriod_Slider.TabIndex = 1;
            // 
            // LEDCalibration_TabPage
            // 
            this.LEDCalibration_TabPage.Controls.Add(this.LEDPowers_TableLayoutPanel);
            this.LEDCalibration_TabPage.Location = new System.Drawing.Point(4, 25);
            this.LEDCalibration_TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.LEDCalibration_TabPage.Name = "LEDCalibration_TabPage";
            this.LEDCalibration_TabPage.Size = new System.Drawing.Size(1410, 641);
            this.LEDCalibration_TabPage.TabIndex = 1;
            this.LEDCalibration_TabPage.Text = "Excitation Power";
            this.LEDCalibration_TabPage.UseVisualStyleBackColor = true;
            // 
            // LEDPowers_TableLayoutPanel
            // 
            this.LEDPowers_TableLayoutPanel.ColumnCount = 1;
            this.LEDPowers_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LEDPowers_TableLayoutPanel.Controls.Add(this.LEDPowersControl_Editor, 0, 0);
            this.LEDPowers_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LEDPowers_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LEDPowers_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LEDPowers_TableLayoutPanel.Name = "LEDPowers_TableLayoutPanel";
            this.LEDPowers_TableLayoutPanel.RowCount = 1;
            this.LEDPowers_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LEDPowers_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.LEDPowers_TableLayoutPanel.Size = new System.Drawing.Size(1410, 641);
            this.LEDPowers_TableLayoutPanel.TabIndex = 0;
            // 
            // LEDPowersControl_Editor
            // 
            this.LEDPowersControl_Editor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LEDPowersControl_Editor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LEDPowersControl_Editor.Location = new System.Drawing.Point(0, 173);
            this.LEDPowersControl_Editor.Margin = new System.Windows.Forms.Padding(0);
            this.LEDPowersControl_Editor.Name = "LEDPowersControl_Editor";
            this.LEDPowersControl_Editor.Padding = new System.Windows.Forms.Padding(0, 48, 0, 48);
            this.LEDPowersControl_Editor.Size = new System.Drawing.Size(1410, 294);
            this.LEDPowersControl_Editor.TabIndex = 0;
            // 
            // LaserCalibration_TabPage
            // 
            this.LaserCalibration_TabPage.Controls.Add(this.Laser_TableLayoutPanel);
            this.LaserCalibration_TabPage.Location = new System.Drawing.Point(4, 25);
            this.LaserCalibration_TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.LaserCalibration_TabPage.Name = "LaserCalibration_TabPage";
            this.LaserCalibration_TabPage.Size = new System.Drawing.Size(1410, 641);
            this.LaserCalibration_TabPage.TabIndex = 2;
            this.LaserCalibration_TabPage.Text = "Opto-Stimulation";
            this.LaserCalibration_TabPage.UseVisualStyleBackColor = true;
            // 
            // Laser_TableLayoutPanel
            // 
            this.Laser_TableLayoutPanel.ColumnCount = 2;
            this.Laser_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Laser_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Laser_TableLayoutPanel.Controls.Add(this.PulseTrainControl_Editor, 0, 0);
            this.Laser_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Laser_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Laser_TableLayoutPanel.Name = "Laser_TableLayoutPanel";
            this.Laser_TableLayoutPanel.RowCount = 1;
            this.Laser_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Laser_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.Laser_TableLayoutPanel.Size = new System.Drawing.Size(1410, 641);
            this.Laser_TableLayoutPanel.TabIndex = 0;
            // 
            // PulseTrainControl_Editor
            // 
            this.PulseTrainControl_Editor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PulseTrainControl_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PulseTrainControl_Editor.Location = new System.Drawing.Point(0, 0);
            this.PulseTrainControl_Editor.Margin = new System.Windows.Forms.Padding(0);
            this.PulseTrainControl_Editor.Name = "PulseTrainControl_Editor";
            this.PulseTrainControl_Editor.Size = new System.Drawing.Size(705, 641);
            this.PulseTrainControl_Editor.TabIndex = 0;
            // 
            // Miscellaneous_TabPage
            // 
            this.Miscellaneous_TabPage.Controls.Add(this.MiscellaneousControl_Editor);
            this.Miscellaneous_TabPage.Location = new System.Drawing.Point(4, 25);
            this.Miscellaneous_TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.Miscellaneous_TabPage.Name = "Miscellaneous_TabPage";
            this.Miscellaneous_TabPage.Size = new System.Drawing.Size(1410, 641);
            this.Miscellaneous_TabPage.TabIndex = 4;
            this.Miscellaneous_TabPage.Text = "Digital IOs";
            this.Miscellaneous_TabPage.UseVisualStyleBackColor = true;
            // 
            // MiscellaneousControl_Editor
            // 
            this.MiscellaneousControl_Editor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MiscellaneousControl_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiscellaneousControl_Editor.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MiscellaneousControl_Editor.Location = new System.Drawing.Point(0, 0);
            this.MiscellaneousControl_Editor.Name = "MiscellaneousControl_Editor";
            this.MiscellaneousControl_Editor.Size = new System.Drawing.Size(1410, 641);
            this.MiscellaneousControl_Editor.TabIndex = 0;
            // 
            // FP3002Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 771);
            this.Controls.Add(this.Cal_GroupBox);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(720, 405);
            this.Name = "FP3002Form";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FP3002 Setup";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FP3002Form_KeyDown);
            this.Cal_GroupBox.ResumeLayout(false);
            this.Cal_TableLayoutPanel.ResumeLayout(false);
            this.Cal_TableLayoutPanel.PerformLayout();
            this.PrevNext_TableLayoutPanel.ResumeLayout(false);
            this.PrevNext_TableLayoutPanel.PerformLayout();
            this.Desc_TableLayoutPanel.ResumeLayout(false);
            this.Desc_TableLayoutPanel.PerformLayout();
            this.Editors_TabControl.ResumeLayout(false);
            this.ROICalibration_TabPage.ResumeLayout(false);
            this.Regions_TableLayoutPanel.ResumeLayout(false);
            this.Regions_TableLayoutPanel.PerformLayout();
            this.TrigSeqCalibration_TabPage.ResumeLayout(false);
            this.TrigSeq_TableLayoutPanel.ResumeLayout(false);
            this.TrigSeq_TableLayoutPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.LEDCalibration_TabPage.ResumeLayout(false);
            this.LEDPowers_TableLayoutPanel.ResumeLayout(false);
            this.LaserCalibration_TabPage.ResumeLayout(false);
            this.Laser_TableLayoutPanel.ResumeLayout(false);
            this.Miscellaneous_TabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox Cal_GroupBox;
        private System.Windows.Forms.TabControl Editors_TabControl;
        private System.Windows.Forms.TabPage ROICalibration_TabPage;
        private System.Windows.Forms.TabPage LEDCalibration_TabPage;
        private System.Windows.Forms.TabPage LaserCalibration_TabPage;
        private System.Windows.Forms.TabPage TrigSeqCalibration_TabPage;
        private Editors.LEDPowersControl LEDPowersControl_Editor;
        private Editors.PulseTrainControl PulseTrainControl_Editor;
        private Editors.ROIControl ROIControl_Editor;
        private System.Windows.Forms.TableLayoutPanel Cal_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Desc_TableLayoutPanel;
        private System.Windows.Forms.Label DescTitle_Label;
        private System.Windows.Forms.Label DescText_Label;
        private System.Windows.Forms.TableLayoutPanel PrevNext_TableLayoutPanel;
        private System.Windows.Forms.Button Prev_Button;
        private System.Windows.Forms.Button Next_Button;
        private System.Windows.Forms.TableLayoutPanel TrigSeq_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Regions_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel LEDPowers_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Laser_TableLayoutPanel;
        private Visualizers.SignalTimingVisualizer TrigSeq_SignalTimingVisualizer;
        private Visualizers.PhotometryDataVisualizer Regions_PhotometryDataVisualizer;
        private Editors.TrigSeqControl TrigSeqControl_Editor;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label FPS_Label;
        private System.Windows.Forms.TabPage Miscellaneous_TabPage;
        private Editors.DigitalIOsControl MiscellaneousControl_Editor;
        private UserControls.TriggerPeriodSlider TriggerPeriod_Slider;
        private System.Windows.Forms.Button PersistReg_Button;
    }
}