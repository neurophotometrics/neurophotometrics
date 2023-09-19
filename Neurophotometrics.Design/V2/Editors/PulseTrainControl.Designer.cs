namespace Neurophotometrics.Design.V2.Editors
{
    partial class PulseTrainControl
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
            this.Top_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Buttons_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MeasPower_Button = new System.Windows.Forms.Button();
            this.AlignLaser_Button = new System.Windows.Forms.Button();
            this.Props_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Wavelength_Label = new System.Windows.Forms.Label();
            this.Amplitude_Label = new System.Windows.Forms.Label();
            this.Frequency_Label = new System.Windows.Forms.Label();
            this.PulseWidth_Label = new System.Windows.Forms.Label();
            this.PulseCount_Label = new System.Windows.Forms.Label();
            this.Wavelength_ComboBox = new System.Windows.Forms.ComboBox();
            this.StimReps_Control = new Neurophotometrics.Design.V2.UserControls.StimRepsControl();
            this.StimOn_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StimOn_Slider = new Neurophotometrics.Design.V2.UserControls.StimOnSlider();
            this.DutyCycle_Label = new System.Windows.Forms.Label();
            this.StimPeriod_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DummyLabel_StimPeriod = new System.Windows.Forms.Label();
            this.StimPeriod_Slider = new Neurophotometrics.Design.V2.UserControls.StimPeriodSlider();
            this.Amplitude_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DummyLabel_Amplitude = new System.Windows.Forms.Label();
            this.Amplitude_Slider = new Neurophotometrics.Design.V2.UserControls.LaserAmplitudeSlider();
            this.Top_TableLayoutPanel.SuspendLayout();
            this.Buttons_TableLayoutPanel.SuspendLayout();
            this.Props_TableLayoutPanel.SuspendLayout();
            this.StimOn_TableLayoutPanel.SuspendLayout();
            this.StimPeriod_TableLayoutPanel.SuspendLayout();
            this.Amplitude_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Top_TableLayoutPanel
            // 
            this.Top_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Top_TableLayoutPanel.ColumnCount = 1;
            this.Top_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Controls.Add(this.Buttons_TableLayoutPanel, 0, 0);
            this.Top_TableLayoutPanel.Controls.Add(this.Props_TableLayoutPanel, 0, 1);
            this.Top_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Top_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Top_TableLayoutPanel.Name = "Top_TableLayoutPanel";
            this.Top_TableLayoutPanel.RowCount = 2;
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.Top_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_TableLayoutPanel.Size = new System.Drawing.Size(681, 265);
            this.Top_TableLayoutPanel.TabIndex = 0;
            this.Top_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Buttons_TableLayoutPanel
            // 
            this.Buttons_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Buttons_TableLayoutPanel.ColumnCount = 2;
            this.Buttons_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Buttons_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Buttons_TableLayoutPanel.Controls.Add(this.MeasPower_Button, 0, 0);
            this.Buttons_TableLayoutPanel.Controls.Add(this.AlignLaser_Button, 1, 0);
            this.Buttons_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Buttons_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Buttons_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Buttons_TableLayoutPanel.Name = "Buttons_TableLayoutPanel";
            this.Buttons_TableLayoutPanel.RowCount = 1;
            this.Buttons_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Buttons_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.Buttons_TableLayoutPanel.Size = new System.Drawing.Size(681, 46);
            this.Buttons_TableLayoutPanel.TabIndex = 0;
            this.Buttons_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // MeasPower_Button
            // 
            this.MeasPower_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MeasPower_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeasPower_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MeasPower_Button.Location = new System.Drawing.Point(0, 0);
            this.MeasPower_Button.Margin = new System.Windows.Forms.Padding(0);
            this.MeasPower_Button.Name = "MeasPower_Button";
            this.MeasPower_Button.Size = new System.Drawing.Size(340, 46);
            this.MeasPower_Button.TabIndex = 0;
            this.MeasPower_Button.TabStop = false;
            this.MeasPower_Button.Text = "Measure Power";
            this.MeasPower_Button.UseVisualStyleBackColor = true;
            this.MeasPower_Button.Click += new System.EventHandler(this.MeasPower_Button_Click);
            // 
            // AlignLaser_Button
            // 
            this.AlignLaser_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AlignLaser_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlignLaser_Button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlignLaser_Button.Location = new System.Drawing.Point(340, 0);
            this.AlignLaser_Button.Margin = new System.Windows.Forms.Padding(0);
            this.AlignLaser_Button.Name = "AlignLaser_Button";
            this.AlignLaser_Button.Size = new System.Drawing.Size(341, 46);
            this.AlignLaser_Button.TabIndex = 1;
            this.AlignLaser_Button.TabStop = false;
            this.AlignLaser_Button.Text = "Align Laser";
            this.AlignLaser_Button.UseVisualStyleBackColor = true;
            this.AlignLaser_Button.Click += new System.EventHandler(this.AlignLaser_Button_Click);
            // 
            // Props_TableLayoutPanel
            // 
            this.Props_TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Props_TableLayoutPanel.ColumnCount = 2;
            this.Props_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 204F));
            this.Props_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Props_TableLayoutPanel.Controls.Add(this.Wavelength_Label, 0, 1);
            this.Props_TableLayoutPanel.Controls.Add(this.Amplitude_Label, 0, 2);
            this.Props_TableLayoutPanel.Controls.Add(this.Frequency_Label, 0, 3);
            this.Props_TableLayoutPanel.Controls.Add(this.PulseWidth_Label, 0, 4);
            this.Props_TableLayoutPanel.Controls.Add(this.PulseCount_Label, 0, 5);
            this.Props_TableLayoutPanel.Controls.Add(this.Wavelength_ComboBox, 1, 1);
            this.Props_TableLayoutPanel.Controls.Add(this.StimReps_Control, 1, 5);
            this.Props_TableLayoutPanel.Controls.Add(this.StimOn_TableLayoutPanel, 1, 4);
            this.Props_TableLayoutPanel.Controls.Add(this.StimPeriod_TableLayoutPanel, 1, 3);
            this.Props_TableLayoutPanel.Controls.Add(this.Amplitude_TableLayoutPanel, 1, 2);
            this.Props_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Props_TableLayoutPanel.Location = new System.Drawing.Point(0, 46);
            this.Props_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Props_TableLayoutPanel.Name = "Props_TableLayoutPanel";
            this.Props_TableLayoutPanel.RowCount = 7;
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Props_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Props_TableLayoutPanel.Size = new System.Drawing.Size(681, 219);
            this.Props_TableLayoutPanel.TabIndex = 1;
            this.Props_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // Wavelength_Label
            // 
            this.Wavelength_Label.AutoSize = true;
            this.Wavelength_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Wavelength_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Wavelength_Label.Location = new System.Drawing.Point(0, 6);
            this.Wavelength_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Wavelength_Label.Name = "Wavelength_Label";
            this.Wavelength_Label.Size = new System.Drawing.Size(204, 31);
            this.Wavelength_Label.TabIndex = 0;
            this.Wavelength_Label.Text = "Wavelength";
            this.Wavelength_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Wavelength_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // Amplitude_Label
            // 
            this.Amplitude_Label.AutoSize = true;
            this.Amplitude_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Amplitude_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Amplitude_Label.Location = new System.Drawing.Point(0, 37);
            this.Amplitude_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Amplitude_Label.Name = "Amplitude_Label";
            this.Amplitude_Label.Size = new System.Drawing.Size(204, 43);
            this.Amplitude_Label.TabIndex = 1;
            this.Amplitude_Label.Text = "Amplitude";
            this.Amplitude_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Amplitude_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // Frequency_Label
            // 
            this.Frequency_Label.AutoSize = true;
            this.Frequency_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Frequency_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Frequency_Label.Location = new System.Drawing.Point(0, 80);
            this.Frequency_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Frequency_Label.Name = "Frequency_Label";
            this.Frequency_Label.Size = new System.Drawing.Size(204, 43);
            this.Frequency_Label.TabIndex = 2;
            this.Frequency_Label.Text = "Frequency";
            this.Frequency_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Frequency_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // PulseWidth_Label
            // 
            this.PulseWidth_Label.AutoSize = true;
            this.PulseWidth_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PulseWidth_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PulseWidth_Label.Location = new System.Drawing.Point(0, 123);
            this.PulseWidth_Label.Margin = new System.Windows.Forms.Padding(0);
            this.PulseWidth_Label.Name = "PulseWidth_Label";
            this.PulseWidth_Label.Size = new System.Drawing.Size(204, 43);
            this.PulseWidth_Label.TabIndex = 3;
            this.PulseWidth_Label.Text = "Pulse Width";
            this.PulseWidth_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PulseWidth_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // PulseCount_Label
            // 
            this.PulseCount_Label.AutoSize = true;
            this.PulseCount_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PulseCount_Label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PulseCount_Label.Location = new System.Drawing.Point(0, 166);
            this.PulseCount_Label.Margin = new System.Windows.Forms.Padding(0);
            this.PulseCount_Label.Name = "PulseCount_Label";
            this.PulseCount_Label.Size = new System.Drawing.Size(204, 47);
            this.PulseCount_Label.TabIndex = 4;
            this.PulseCount_Label.Text = "Pulse Train Type";
            this.PulseCount_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PulseCount_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // Wavelength_ComboBox
            // 
            this.Wavelength_ComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Wavelength_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Wavelength_ComboBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Wavelength_ComboBox.FormattingEnabled = true;
            this.Wavelength_ComboBox.Location = new System.Drawing.Point(204, 6);
            this.Wavelength_ComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.Wavelength_ComboBox.Name = "Wavelength_ComboBox";
            this.Wavelength_ComboBox.Size = new System.Drawing.Size(477, 31);
            this.Wavelength_ComboBox.TabIndex = 9;
            this.Wavelength_ComboBox.TabStop = false;
            this.Wavelength_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Wavelength_ComboBox_SelectedIndexChanged);
            // 
            // StimReps_Control
            // 
            this.StimReps_Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StimReps_Control.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StimReps_Control.Location = new System.Drawing.Point(204, 166);
            this.StimReps_Control.Margin = new System.Windows.Forms.Padding(0);
            this.StimReps_Control.Name = "StimReps_Control";
            this.StimReps_Control.Size = new System.Drawing.Size(477, 47);
            this.StimReps_Control.TabIndex = 17;
            this.StimReps_Control.Click += new System.EventHandler(this.StrayClick);
            // 
            // StimOn_TableLayoutPanel
            // 
            this.StimOn_TableLayoutPanel.ColumnCount = 2;
            this.StimOn_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StimOn_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.StimOn_TableLayoutPanel.Controls.Add(this.StimOn_Slider, 0, 0);
            this.StimOn_TableLayoutPanel.Controls.Add(this.DutyCycle_Label, 1, 0);
            this.StimOn_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StimOn_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StimOn_TableLayoutPanel.Location = new System.Drawing.Point(204, 123);
            this.StimOn_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.StimOn_TableLayoutPanel.Name = "StimOn_TableLayoutPanel";
            this.StimOn_TableLayoutPanel.RowCount = 1;
            this.StimOn_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StimOn_TableLayoutPanel.Size = new System.Drawing.Size(477, 43);
            this.StimOn_TableLayoutPanel.TabIndex = 18;
            this.StimOn_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // StimOn_Slider
            // 
            this.StimOn_Slider.AutoSize = true;
            this.StimOn_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StimOn_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StimOn_Slider.HighSpeedRepeatDelay = 2000;
            this.StimOn_Slider.HighSpeedRepeatPeriod = 100;
            this.StimOn_Slider.Location = new System.Drawing.Point(0, 0);
            this.StimOn_Slider.LowSpeedRepeatDelay = 500;
            this.StimOn_Slider.LowSpeedRepeatPeriod = 300;
            this.StimOn_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.StimOn_Slider.Name = "StimOn_Slider";
            this.StimOn_Slider.Size = new System.Drawing.Size(469, 43);
            this.StimOn_Slider.TabIndex = 16;
            this.StimOn_Slider.TabStop = false;
            this.StimOn_Slider.Click += new System.EventHandler(this.StrayClick);
            // 
            // DutyCycle_Label
            // 
            this.DutyCycle_Label.AutoSize = true;
            this.DutyCycle_Label.BackColor = System.Drawing.SystemColors.Control;
            this.DutyCycle_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.DutyCycle_Label.Location = new System.Drawing.Point(473, 0);
            this.DutyCycle_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DutyCycle_Label.Name = "DutyCycle_Label";
            this.DutyCycle_Label.Size = new System.Drawing.Size(0, 43);
            this.DutyCycle_Label.TabIndex = 17;
            this.DutyCycle_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DutyCycle_Label.Click += new System.EventHandler(this.StrayClick);
            // 
            // StimPeriod_TableLayoutPanel
            // 
            this.StimPeriod_TableLayoutPanel.ColumnCount = 2;
            this.StimPeriod_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StimPeriod_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.StimPeriod_TableLayoutPanel.Controls.Add(this.DummyLabel_StimPeriod, 1, 0);
            this.StimPeriod_TableLayoutPanel.Controls.Add(this.StimPeriod_Slider, 0, 0);
            this.StimPeriod_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StimPeriod_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StimPeriod_TableLayoutPanel.Location = new System.Drawing.Point(204, 80);
            this.StimPeriod_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.StimPeriod_TableLayoutPanel.Name = "StimPeriod_TableLayoutPanel";
            this.StimPeriod_TableLayoutPanel.RowCount = 1;
            this.StimPeriod_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.StimPeriod_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.StimPeriod_TableLayoutPanel.Size = new System.Drawing.Size(477, 43);
            this.StimPeriod_TableLayoutPanel.TabIndex = 19;
            this.StimPeriod_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // DummyLabel_StimPeriod
            // 
            this.DummyLabel_StimPeriod.AutoSize = true;
            this.DummyLabel_StimPeriod.BackColor = System.Drawing.SystemColors.Control;
            this.DummyLabel_StimPeriod.Dock = System.Windows.Forms.DockStyle.Left;
            this.DummyLabel_StimPeriod.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DummyLabel_StimPeriod.Location = new System.Drawing.Point(473, 0);
            this.DummyLabel_StimPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DummyLabel_StimPeriod.Name = "DummyLabel_StimPeriod";
            this.DummyLabel_StimPeriod.Size = new System.Drawing.Size(0, 43);
            this.DummyLabel_StimPeriod.TabIndex = 18;
            this.DummyLabel_StimPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DummyLabel_StimPeriod.Click += new System.EventHandler(this.StrayClick);
            // 
            // StimPeriod_Slider
            // 
            this.StimPeriod_Slider.AutoSize = true;
            this.StimPeriod_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StimPeriod_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StimPeriod_Slider.HighSpeedRepeatDelay = 2000;
            this.StimPeriod_Slider.HighSpeedRepeatPeriod = 100;
            this.StimPeriod_Slider.Location = new System.Drawing.Point(0, 0);
            this.StimPeriod_Slider.LowSpeedRepeatDelay = 500;
            this.StimPeriod_Slider.LowSpeedRepeatPeriod = 300;
            this.StimPeriod_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.StimPeriod_Slider.Name = "StimPeriod_Slider";
            this.StimPeriod_Slider.Size = new System.Drawing.Size(469, 43);
            this.StimPeriod_Slider.TabIndex = 15;
            this.StimPeriod_Slider.TabStop = false;
            this.StimPeriod_Slider.Click += new System.EventHandler(this.StrayClick);
            // 
            // Amplitude_TableLayoutPanel
            // 
            this.Amplitude_TableLayoutPanel.ColumnCount = 2;
            this.Amplitude_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Amplitude_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Amplitude_TableLayoutPanel.Controls.Add(this.DummyLabel_Amplitude, 1, 0);
            this.Amplitude_TableLayoutPanel.Controls.Add(this.Amplitude_Slider, 0, 0);
            this.Amplitude_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Amplitude_TableLayoutPanel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Amplitude_TableLayoutPanel.Location = new System.Drawing.Point(204, 37);
            this.Amplitude_TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Amplitude_TableLayoutPanel.Name = "Amplitude_TableLayoutPanel";
            this.Amplitude_TableLayoutPanel.RowCount = 1;
            this.Amplitude_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Amplitude_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.Amplitude_TableLayoutPanel.Size = new System.Drawing.Size(477, 43);
            this.Amplitude_TableLayoutPanel.TabIndex = 20;
            this.Amplitude_TableLayoutPanel.Click += new System.EventHandler(this.StrayClick);
            // 
            // DummyLabel_Amplitude
            // 
            this.DummyLabel_Amplitude.AutoSize = true;
            this.DummyLabel_Amplitude.BackColor = System.Drawing.SystemColors.Control;
            this.DummyLabel_Amplitude.Dock = System.Windows.Forms.DockStyle.Left;
            this.DummyLabel_Amplitude.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DummyLabel_Amplitude.Location = new System.Drawing.Point(473, 0);
            this.DummyLabel_Amplitude.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DummyLabel_Amplitude.Name = "DummyLabel_Amplitude";
            this.DummyLabel_Amplitude.Size = new System.Drawing.Size(0, 43);
            this.DummyLabel_Amplitude.TabIndex = 19;
            this.DummyLabel_Amplitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DummyLabel_Amplitude.Click += new System.EventHandler(this.StrayClick);
            // 
            // Amplitude_Slider
            // 
            this.Amplitude_Slider.AutoSize = true;
            this.Amplitude_Slider.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Amplitude_Slider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Amplitude_Slider.HighSpeedRepeatDelay = 2000;
            this.Amplitude_Slider.HighSpeedRepeatPeriod = 100;
            this.Amplitude_Slider.Location = new System.Drawing.Point(0, 0);
            this.Amplitude_Slider.LowSpeedRepeatDelay = 500;
            this.Amplitude_Slider.LowSpeedRepeatPeriod = 300;
            this.Amplitude_Slider.Margin = new System.Windows.Forms.Padding(0);
            this.Amplitude_Slider.Name = "Amplitude_Slider";
            this.Amplitude_Slider.Size = new System.Drawing.Size(469, 43);
            this.Amplitude_Slider.TabIndex = 14;
            this.Amplitude_Slider.TabStop = false;
            this.Amplitude_Slider.Click += new System.EventHandler(this.StrayClick);
            // 
            // PulseTrainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Top_TableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PulseTrainControl";
            this.Size = new System.Drawing.Size(681, 265);
            this.Click += new System.EventHandler(this.StrayClick);
            this.Top_TableLayoutPanel.ResumeLayout(false);
            this.Buttons_TableLayoutPanel.ResumeLayout(false);
            this.Props_TableLayoutPanel.ResumeLayout(false);
            this.Props_TableLayoutPanel.PerformLayout();
            this.StimOn_TableLayoutPanel.ResumeLayout(false);
            this.StimOn_TableLayoutPanel.PerformLayout();
            this.StimPeriod_TableLayoutPanel.ResumeLayout(false);
            this.StimPeriod_TableLayoutPanel.PerformLayout();
            this.Amplitude_TableLayoutPanel.ResumeLayout(false);
            this.Amplitude_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Top_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Buttons_TableLayoutPanel;
        private System.Windows.Forms.Button MeasPower_Button;
        private System.Windows.Forms.Button AlignLaser_Button;
        private System.Windows.Forms.TableLayoutPanel Props_TableLayoutPanel;
        private System.Windows.Forms.Label Wavelength_Label;
        private System.Windows.Forms.Label Amplitude_Label;
        private System.Windows.Forms.Label Frequency_Label;
        private System.Windows.Forms.Label PulseWidth_Label;
        private System.Windows.Forms.Label PulseCount_Label;
        private System.Windows.Forms.ComboBox Wavelength_ComboBox;
        private UserControls.LaserAmplitudeSlider Amplitude_Slider;
        private UserControls.StimPeriodSlider StimPeriod_Slider;
        private UserControls.StimOnSlider StimOn_Slider;
        private UserControls.StimRepsControl StimReps_Control;
        private System.Windows.Forms.TableLayoutPanel StimOn_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel StimPeriod_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel Amplitude_TableLayoutPanel;
        private System.Windows.Forms.Label DutyCycle_Label;
        private System.Windows.Forms.Label DummyLabel_StimPeriod;
        private System.Windows.Forms.Label DummyLabel_Amplitude;
    }
}