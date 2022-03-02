using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

namespace Neurophotometrics.Design
{
    partial class GlobalStatusController
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
        private void InitializeComponent(Tuple<Tuple<int, bool[], bool>, Tuple<bool[], bool[], double[], double[]>> ROIData)
        {
            this.SuspendLayout();

            this.globalPopOutController = new UserControl();
            this.globalConfigController = new UserControl();

            this.capacityController = new LabelValueController();
            this.capacityTextBox = new TextBox();
            this.colorBlindModeController = new LabelButtonController();
            this.globalPopOutControllerLabel = new Label();
            this.globalPopOutControllerLabel1 = new Label();
            this.globalPopOutControllerLabel2 = new Label();
            this.globalPopOutControllerLabel3 = new Label();
            this.globalPopOutControllerLabel4 = new Label();
            this.globalConfigControllerVScroll = new VScrollBar();

            this.localStatusController = new LocalStatusController(ROIData);

            this.components = new System.ComponentModel.Container();

            this.globalPopOutController.SuspendLayout();
            this.globalConfigController.SuspendLayout();
            this.globalPopOutControllerLabel.SuspendLayout();

            //
            // popOutController
            //
            this.globalPopOutController.Name = "popOutController";
            //this.globalPopOutController.BackColor = Color.Beige;
            this.globalPopOutController.BackColor = Color.White;
            this.globalPopOutController.Location = new Point(0, 0);
            this.globalPopOutController.Height = PopOutControllerHeight;
            this.globalPopOutController.Visible = true;
            this.globalPopOutController.Click += new EventHandler(this.popOutController_Click);

            //
            // capacityController
            //
            this.capacityController.Label.Name = "capacityControllerLabel";
            this.capacityController.Label.Bounds = new Rectangle(0, 0, 100, PopOutControllerHeight);
            this.capacityController.Label.Text = "Capacity:";
            this.capacityController.Label.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.capacityController.Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.capacityController.Value.Name = "capacityControllerValue";
            this.capacityController.Value.Bounds = new Rectangle(100, 0, 50, PopOutControllerHeight);
            this.capacityController.Value.Text = 300.ToString(CultureInfo.InvariantCulture);
            this.capacityController.Value.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.capacityController.Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.capacityController.Value.Click += new System.EventHandler(this.editableCapacityLabel_Click);

            this.capacityTextBox.LostFocus += capacityTextBox_LostFocus;
            this.capacityTextBox.KeyDown += capacityTextBox_KeyDown;

            this.capacityController.Name = "capacityController";
            this.capacityController.Bounds = new Rectangle(PopOutControllerHeight + HorizontalMargin, 0, 150, PopOutControllerHeight);
            //this.capacityController.BackColor = System.Drawing.Color.Aqua;
            this.capacityController.Visible = true;

            //
            // colorBlindModeController
            //
            this.colorBlindModeController.Label.Name = "colorBlindModeControllerLabel";
            this.colorBlindModeController.Label.Bounds = new Rectangle(0, 0, 150, PopOutControllerHeight);
            this.colorBlindModeController.Label.Text = "Colorblind Mode:";
            this.colorBlindModeController.Label.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.colorBlindModeController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.colorBlindModeController.Button.Name = "colorBlindModeControllerButton";
            this.colorBlindModeController.Button.Bounds = new Rectangle(150, 0, 75, PopOutControllerHeight);
            this.colorBlindModeController.Button.Text = "False";
            this.colorBlindModeController.Button.ForeColor = Color.FromArgb(220, 50, 32);
            this.colorBlindModeController.Button.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.colorBlindModeController.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.colorBlindModeController.Button.Click += new EventHandler(this.colorBlindModeButton_Click);

            this.colorBlindModeController.Name = "colorBlindModeController";
            this.colorBlindModeController.Bounds = new Rectangle(PopOutControllerHeight + 2 * HorizontalMargin + 150, 0, 225, PopOutControllerHeight);
            this.colorBlindModeController.Visible = true;



            //
            // popOutControllerLabel
            //
            this.globalPopOutControllerLabel.Name = "popOutControllerLabel";
            //this.globalPopOutControllerLabel.Location = new Point(PopOutControllerHeight + HorizontalMargin + 150 + HorizontalMargin, 0);
            //this.globalPopOutControllerLabel.Height = PopOutControllerHeight;
            this.globalPopOutControllerLabel.Bounds = new Rectangle(PopOutControllerHeight + HorizontalMargin + 150 + HorizontalMargin, 0, 150, PopOutControllerHeight);
            this.globalPopOutControllerLabel.Text = "Configure Plots";
            this.globalPopOutControllerLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.globalPopOutControllerLabel.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            //this.globalPopOutControllerLabel.BackColor = Color.Aqua;
            this.globalPopOutControllerLabel.Visible = true;
            this.globalPopOutControllerLabel.Click += new EventHandler(this.popOutController_Click);
            //
            // popOutControllerLabel1
            //
            this.globalPopOutControllerLabel1.Name = "popOutControllerButton1";
            this.globalPopOutControllerLabel1.Bounds = new Rectangle(0, 0, PopOutControllerHeight, PopOutControllerHeight);
            this.globalPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2191);
            this.globalPopOutControllerLabel1.TextAlign = ContentAlignment.MiddleCenter;
            this.globalPopOutControllerLabel1.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Bold);
            //this.globalPopOutControllerLabel1.BackColor = Color.Aqua;
            this.globalPopOutControllerLabel1.Visible = true;
            this.globalPopOutControllerLabel1.Click += new EventHandler(this.popOutController_Click);
            //
            // popOutControllerLabel2
            //
            this.globalPopOutControllerLabel2.Name = "popOutControllerButton2";
            this.globalPopOutControllerLabel2.Size = new Size(PopOutControllerHeight, PopOutControllerHeight);
            this.globalPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2191);
            //this.globalPopOutControllerLabel2.BackColor = Color.Aqua;
            this.globalPopOutControllerLabel2.TextAlign = ContentAlignment.MiddleCenter;
            this.globalPopOutControllerLabel2.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Bold);
            this.globalPopOutControllerLabel2.Visible = true;
            this.globalPopOutControllerLabel2.Click += new EventHandler(this.popOutController_Click);
            //
            // popOutControllerLabel3
            //
            this.globalPopOutControllerLabel3.Name = "popOutControllerButton3";
            this.globalPopOutControllerLabel3.Size = new Size(PopOutControllerHeight, PopOutControllerHeight);
            this.globalPopOutControllerLabel3.Text = char.ConvertFromUtf32(0x2191);
            this.globalPopOutControllerLabel3.TextAlign = ContentAlignment.MiddleCenter;
            this.globalPopOutControllerLabel3.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Bold);
            this.globalPopOutControllerLabel3.Visible = true;
            this.globalPopOutControllerLabel3.Click += new EventHandler(this.popOutController_Click);
            //
            // popOutControllerLabel4
            //
            this.globalPopOutControllerLabel4.Name = "popOutControllerButton4";
            this.globalPopOutControllerLabel4.Size = new Size(PopOutControllerHeight, PopOutControllerHeight);
            this.globalPopOutControllerLabel4.Text = char.ConvertFromUtf32(0x2191);
            this.globalPopOutControllerLabel4.TextAlign = ContentAlignment.MiddleCenter;
            this.globalPopOutControllerLabel4.Font = new System.Drawing.Font("Century Gothic", 8, System.Drawing.FontStyle.Bold);
            this.globalPopOutControllerLabel4.Visible = true;
            this.globalPopOutControllerLabel4.Click += new EventHandler(this.popOutController_Click);
            //
            // globalConfigControllerVScroll
            //
            this.globalConfigControllerVScroll.Name = "vScroll";
            this.globalConfigControllerVScroll.Visible = true;
            //this.globalConfigControllerVScroll.Anchor = AnchorStyles.Right;
            this.globalConfigControllerVScroll.Minimum = 0;
            this.globalConfigControllerVScroll.Maximum = 100;
            this.globalConfigControllerVScroll.SmallChange = 25;
            this.globalConfigControllerVScroll.LargeChange = 50;
            this.globalConfigControllerVScroll.Value = 0;
            this.globalConfigControllerVScroll.ValueChanged += new EventHandler(globalConfigControllerVScroll_ValueChanged);
            //
            // configController
            //
            this.globalConfigController.Name = "configController";
            this.globalConfigController.BackColor = Color.Black;
            this.globalConfigController.Visible = false;
            this.globalConfigController.Click += new EventHandler(globalConfigController_Click);


            //
            // localStatusController
            //
            this.localStatusController.LocalConfigModeChanged += LocalStatusController_LocalConfigModeChanged;
            this.localStatusController.ROIVisibleChanged += LocalStatusController_ROIVisibleChanged;
            this.localStatusController.DeinterleaveChanged += LocalStatusController_DeinterleaveChanged;
            this.localStatusController.AutoScalesChanged += LocalStatusController_AutoScalesChanged;
            this.localStatusController.MinsChanged += LocalStatusController_MinsChanged;
            this.localStatusController.MaxesChanged += LocalStatusController_MaxesChanged;
            this.localStatusController.StateIndexChanged += LocalStatusController_StateIndexChanged;


            this.globalPopOutController.Controls.Add(globalPopOutControllerLabel);
            this.globalPopOutController.Controls.Add(globalPopOutControllerLabel1);
            this.globalPopOutController.Controls.Add(globalPopOutControllerLabel2);
            this.globalPopOutController.Controls.Add(globalPopOutControllerLabel3);
            this.globalPopOutController.Controls.Add(globalPopOutControllerLabel4);
            this.globalPopOutController.Controls.Add(capacityController);
            this.globalPopOutController.Controls.Add(colorBlindModeController);
            this.globalConfigController.Controls.Add(localStatusController);
            this.globalConfigController.Controls.Add(globalConfigControllerVScroll);
            this.Controls.Add(globalPopOutController);
            this.Controls.Add(globalConfigController);

            this.Name = "GlobalStatusController";
        }



        #endregion
        private UserControl globalPopOutController;
        private Label globalPopOutControllerLabel;
        private Label globalPopOutControllerLabel1;
        private Label globalPopOutControllerLabel2;
        private Label globalPopOutControllerLabel3;
        private Label globalPopOutControllerLabel4;
        private LabelValueController capacityController;
        private TextBox capacityTextBox;
        private LabelButtonController colorBlindModeController;
        private UserControl globalConfigController;
        private LocalStatusController localStatusController;
        private VScrollBar globalConfigControllerVScroll;
    }
}
