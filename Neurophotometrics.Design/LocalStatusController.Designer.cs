using System.Windows.Forms;
using System.Drawing;
using System;

namespace Neurophotometrics.Design
{
    partial class LocalStatusController
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
            SuspendLayout();
            /********** INITIALIZE ALL SUB-COMPONENTS **********/
            this.localPopOutController = new UserControl();
            this.localPopOutControllerLabel = new Label();
            this.localPopOutControllerLabel1 = new Label();
            this.localPopOutControllerLabel2 = new Label();
            this.localConfigController = new UserControl();
            this.state = new UserControl();
            this.plotVisibleController = new LabelButtonController();
            this.deinterleaveController = new LabelButtonController();
            this.autoScaleController = new LabelButtonController();
            this.minController = new LabelValueController();
            this.maxController = new LabelValueController();
            this.axisLabelMultiButtonController = new UserControl();
            this.axisLabel = new Label();
            this.axis415Controller = new LabelButtonController();
            this.axis470Controller = new LabelButtonController();
            this.axis560Controller = new LabelButtonController();
            this.autoScaleLabelMultiButtonController = new UserControl();
            this.autoScaleLabel = new Label();
            this.autoScale415Controller = new LabelButtonController();
            this.autoScale470Controller = new LabelButtonController();
            this.autoScale560Controller = new LabelButtonController();
            this.minMaxLabelMultiValue2DController = new UserControl();
            this.minMaxLabels = new UserControl();
            this.Label0 = new Label();
            this.Label1 = new Label();
            this.Label2 = new Label();
            this.minValuesController = new UserControl();
            this.minLabel = new Label();
            this.min415Label = new Label();
            this.min470Label = new Label();
            this.min560Label = new Label();
            this.maxValuesController = new UserControl();
            this.maxLabel = new Label();
            this.max415Label = new Label();
            this.max470Label = new Label();
            this.max560Label = new Label();
            this.textBox = new TextBox();
            /********** SUSPEND LAYOUT OF ALL SUBCOMPONENTS **********/
            this.localPopOutController.SuspendLayout();
            this.localPopOutControllerLabel.SuspendLayout();
            this.localPopOutControllerLabel1.SuspendLayout();
            this.localPopOutControllerLabel2.SuspendLayout();
            this.localConfigController.SuspendLayout();
            this.state.SuspendLayout();
            this.plotVisibleController.SuspendLayout();
            this.deinterleaveController.SuspendLayout();
            this.autoScaleController.SuspendLayout();
            this.minController.SuspendLayout();
            this.maxController.SuspendLayout();
            this.axisLabelMultiButtonController.SuspendLayout();
            this.axis415Controller.SuspendLayout();
            this.axis470Controller.SuspendLayout();
            this.axis560Controller.SuspendLayout();
            this.autoScaleLabelMultiButtonController.SuspendLayout();
            this.autoScale415Controller.SuspendLayout();
            this.autoScale470Controller.SuspendLayout();
            this.autoScale560Controller.SuspendLayout();
            this.minMaxLabelMultiValue2DController.SuspendLayout();
            this.minMaxLabelMultiValue2DController.SuspendLayout();
            this.minMaxLabels.SuspendLayout();
            this.Label0.SuspendLayout();
            this.Label1.SuspendLayout();
            this.Label2.SuspendLayout();
            this.minValuesController.SuspendLayout();
            this.minLabel.SuspendLayout();
            this.min415Label.SuspendLayout();
            this.min470Label.SuspendLayout();
            this.min560Label.SuspendLayout();
            this.maxValuesController.SuspendLayout();
            this.maxLabel.SuspendLayout();
            this.max415Label.SuspendLayout();
            this.max470Label.SuspendLayout();
            this.max560Label.SuspendLayout();
            this.textBox.SuspendLayout();

            /********** LOCAL POP OUT CONTROLLER **********/

            /***** popOutControllerLabel *****/
            this.localPopOutControllerLabel.Name = "localPopOutControllerLabel_" + roiNumber;
            this.localPopOutControllerLabel.Location = new Point(LocalPopOutControllerHeight + HorizontalMargin, 0);
            this.localPopOutControllerLabel.Height = LocalPopOutControllerHeight;
            this.localPopOutControllerLabel.Width = LocalStatusControllerWidth;
            this.localPopOutControllerLabel.Text = "Configure ROI " + roiNumber + " Plot";
            this.localPopOutControllerLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.localPopOutControllerLabel.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            //this.localPopOutControllerLabel.BackColor = Color.Aqua;
            this.localPopOutControllerLabel.Visible = true;
            this.localPopOutControllerLabel.Click += new EventHandler(localPopOutController_Click);
            /***** popOutControllerLabel1 *****/
            this.localPopOutControllerLabel1.Name = "localPopOutControllerLabel1_" + roiNumber;
            this.localPopOutControllerLabel1.Bounds = new Rectangle(0, 0, LocalPopOutControllerHeight, LocalPopOutControllerHeight);
            this.localPopOutControllerLabel1.Text = char.ConvertFromUtf32(0x2193);
            this.localPopOutControllerLabel1.TextAlign = ContentAlignment.MiddleCenter;
            //this.localPopOutControllerLabel1.BackColor = Color.Aqua;
            this.localPopOutControllerLabel1.Visible = true;
            this.localPopOutControllerLabel1.Click += new EventHandler(localPopOutController_Click);
            /***** popOutControllerLabel2 *****/
            this.localPopOutControllerLabel2.Name = "localPopOutControllerLabel1_" + roiNumber;
            this.localPopOutControllerLabel2.Location = new Point(LocalStatusControllerWidth + 2 * HorizontalMargin + LocalPopOutControllerHeight, 0);
            this.localPopOutControllerLabel2.Size = new Size(LocalPopOutControllerHeight, LocalPopOutControllerHeight);
            this.localPopOutControllerLabel2.Text = char.ConvertFromUtf32(0x2193);
            //this.localPopOutControllerLabel2.BackColor = Color.Aqua;
            this.localPopOutControllerLabel2.TextAlign = ContentAlignment.MiddleCenter;
            this.localPopOutControllerLabel2.Visible = true;
            this.localPopOutControllerLabel2.Click += new EventHandler(localPopOutController_Click);
            /***** popOutController *****/
            this.localPopOutController.Name = "localPopOutController_" + roiNumber;
            //this.localPopOutController.BackColor = Color.Beige;
            this.localPopOutController.Location = new Point(0, 0);
            this.localPopOutController.Height = LocalPopOutControllerHeight;
            this.localPopOutController.Visible = true;
            this.localPopOutController.Click += new EventHandler(localPopOutController_Click);
            this.localPopOutController.Controls.Add(localPopOutControllerLabel);
            this.localPopOutController.Controls.Add(localPopOutControllerLabel1);
            this.localPopOutController.Controls.Add(localPopOutControllerLabel2);

            /********** LOCAL CONFIG CONTROLLER **********/

            /***** plotVisibleController *****/
            this.plotVisibleController.Name = "plotVisibleController_" + roiNumber;
            this.plotVisibleController.Bounds = new Rectangle(0, 0, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.plotVisibleController.BackColor = Color.DeepPink;
            this.plotVisibleController.Label.Name = "plotVisibleControllerLabel_" + roiNumber;
            this.plotVisibleController.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.plotVisibleController.Label.Text = "Plot Visible:";
            this.plotVisibleController.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.plotVisibleController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.plotVisibleController.Label.Click += new EventHandler(Null_Click);
            this.plotVisibleController.Button.Name = "plotVisibleControllerButton_" + roiNumber;
            this.plotVisibleController.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.plotVisibleController.Button.Text = "True";
            this.plotVisibleController.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.plotVisibleController.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.plotVisibleController.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.plotVisibleController.Button.Click += new EventHandler(plotVisible_Click);
            /***** deinterleaveController *****/
            this.deinterleaveController.Name = "deinterleaveController_" + roiNumber;
            this.deinterleaveController.Bounds = new Rectangle(0, LocalPopOutControllerHeight + VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.deinterleaveController.BackColor = Color.DeepPink;
            this.deinterleaveController.Label.Name = "deinterleaveControllerLabel_" + roiNumber;
            this.deinterleaveController.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.deinterleaveController.Label.Text = "Deinterleave:";
            this.deinterleaveController.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.deinterleaveController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.deinterleaveController.Label.Click += new EventHandler(Null_Click);
            this.deinterleaveController.Button.Name = "deinterleaveControllerButton_" + roiNumber;
            this.deinterleaveController.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.deinterleaveController.Button.Text = "False";
            this.deinterleaveController.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.deinterleaveController.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.deinterleaveController.Button.ForeColor = Color.FromArgb(220, 50, 32);
            this.deinterleaveController.Button.Click += new EventHandler(deinterleave_Click);
            /***** autoScaleController *****/
            this.autoScaleController.Name = "autoScaleController_" + roiNumber;
            this.autoScaleController.Bounds = new Rectangle(0, 2 * LocalPopOutControllerHeight + 2 * VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.autoScaleController.BackColor = Color.DeepPink;
            this.autoScaleController.Label.Name = "autoScaleControllerLabel_" + roiNumber;
            this.autoScaleController.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.autoScaleController.Label.Text = "AutoScale:";
            this.autoScaleController.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScaleController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.autoScaleController.Label.Click += new EventHandler(Null_Click);
            this.autoScaleController.Button.Name = "autoScaleControllerButton_" + roiNumber;
            this.autoScaleController.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.autoScaleController.Button.Text = "True";
            this.autoScaleController.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScaleController.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.autoScaleController.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.autoScaleController.Button.Click += new EventHandler(autoScale_Click);
            /***** minController *****/
            this.minController.Name = "minController_" + roiNumber;
            this.minController.Bounds = new Rectangle(0, 3 * LocalPopOutControllerHeight + 3 * VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.minController.BackColor = Color.DeepPink;
            this.minController.Label.Name = "minControllerLabel_" + roiNumber;
            this.minController.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.minController.Label.Text = "Min:";
            this.minController.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.minController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.minController.Label.Click += new EventHandler(Null_Click);
            this.minController.Value.Name = "minControllerValue_" + roiNumber;
            this.minController.Value.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.minController.Value.Text = "0.0";
            this.minController.Value.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.minController.Value.TextAlign = ContentAlignment.MiddleCenter;
            this.minController.Value.Click += new EventHandler(min_Click);
            /***** maxController *****/
            this.maxController.Name = "maxController_" + roiNumber;
            this.maxController.Bounds = new Rectangle(0, 4 * LocalPopOutControllerHeight + 4 * VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.maxController.BackColor = Color.DeepPink;
            this.maxController.Label.Name = "maxControllerLabel_" + roiNumber;
            this.maxController.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.maxController.Label.Text = "Max:";
            this.maxController.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.maxController.Label.TextAlign = ContentAlignment.MiddleCenter;
            this.maxController.Label.Click += new EventHandler(Null_Click);
            this.maxController.Value.Name = "maxControllerValue_" + roiNumber;
            this.maxController.Value.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.maxController.Value.Text = "1.0";
            this.maxController.Value.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.maxController.Value.TextAlign = ContentAlignment.MiddleCenter;
            this.maxController.Value.Click += new EventHandler(max_Click);
            /***** textBox *****/
            this.textBox.LostFocus += new EventHandler(textBox_LostFocus);
            this.textBox.KeyDown += textBox_KeyDown;
            /***** axisLabel *****/
            this.axisLabel.Name = "axisLabel_" + roiNumber;
            this.axisLabel.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            //this.axisLabel.BackColor = Color.DeepPink;
            this.axisLabel.Text = "Axes Visible:";
            this.axisLabel.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axisLabel.TextAlign = ContentAlignment.MiddleRight;
            this.axisLabel.Click += new EventHandler(Null_Click);
            /***** axis415Controller *****/
            this.axis415Controller.Name = "axisController_415_" + roiNumber;
            this.axis415Controller.Bounds = new Rectangle(0, LocalPopOutControllerHeight + VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.axis415Controller.BackColor = Color.DeepPink;
            this.axis415Controller.Label.Name = "axisControllerLabel_415_" + roiNumber;
            this.axis415Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.axis415Controller.Label.Text = "L415:";
            this.axis415Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis415Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.axis415Controller.Label.ForeColor = Color.Purple;
            this.axis415Controller.Label.Click += new EventHandler(Null_Click);
            this.axis415Controller.Button.Name = "axisControlleButton_415_" + roiNumber;
            this.axis415Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.axis415Controller.Button.Text = "True";
            this.axis415Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis415Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.axis415Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.axis415Controller.Button.Click += new EventHandler(axisVisible_Click);
            /***** axis470Controller *****/
            this.axis470Controller.Name = "axisController_470_" + roiNumber;
            this.axis470Controller.Bounds = new Rectangle(0, 2 * (LocalPopOutControllerHeight + VerticalMargin), LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.axis470Controller.BackColor = Color.DeepPink;
            this.axis470Controller.Label.Name = "axisControllerLabel_470_" + roiNumber;
            this.axis470Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.axis470Controller.Label.Text = "L470:";
            this.axis470Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis470Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.axis470Controller.Label.ForeColor = Color.Green;
            this.axis470Controller.Label.Click += new EventHandler(Null_Click);
            this.axis470Controller.Button.Name = "axisControllerButton_470_" + roiNumber;
            this.axis470Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.axis470Controller.Button.Text = "True";
            this.axis470Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis470Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.axis470Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.axis470Controller.Button.Click += new EventHandler(axisVisible_Click);
            /***** axis560Controller *****/
            this.axis560Controller.Name = "axisController_560_" + roiNumber;
            this.axis560Controller.Bounds = new Rectangle(0, 3 * (LocalPopOutControllerHeight + VerticalMargin), LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.axis560Controller.BackColor = Color.DeepPink;
            this.axis560Controller.Label.Name = "axisControllerLabel_560_" + roiNumber;
            this.axis560Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.axis560Controller.Label.Text = "L560:";
            this.axis560Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis560Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.axis560Controller.Label.ForeColor = Color.Red;
            this.axis560Controller.Label.Click += new EventHandler(Null_Click);
            this.axis560Controller.Button.Name = "axisControllerButton_560_" + roiNumber;
            this.axis560Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.axis560Controller.Button.Text = "True";
            this.axis560Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.axis560Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.axis560Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.axis560Controller.Button.Click += new EventHandler(axisVisible_Click);
            /***** axisLabelMultiButtonController *****/
            this.axisLabelMultiButtonController.Name = "axisLabelMultiButtonController_" + roiNumber;
            this.axisLabelMultiButtonController.Location = new Point(0, 2 * (LocalPopOutControllerHeight + VerticalMargin));
            //this.axisLabelMultiButtonController.BackColor = Color.DeepPink;
            this.axisLabelMultiButtonController.Click += new EventHandler(Null_Click);
            /***** autoScaleLabel *****/
            this.autoScaleLabel.Name = "autoScaleLabel_" + roiNumber;
            this.autoScaleLabel.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            //this.autoScaleLabel.BackColor = Color.DeepPink;
            this.autoScaleLabel.Text = "AutoScale:";
            this.autoScaleLabel.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScaleLabel.TextAlign = ContentAlignment.MiddleRight;
            this.autoScaleLabel.Click += new EventHandler(Null_Click);
            /***** autoScale415Controller *****/
            this.autoScale415Controller.Name = "autoScaleController_415_" + roiNumber;
            this.autoScale415Controller.Bounds = new Rectangle(0, LocalPopOutControllerHeight + VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.autoScale415Controller.BackColor = Color.DeepPink;
            this.autoScale415Controller.Label.Name = "autoScaleControllerLabel_415_" + roiNumber;
            this.autoScale415Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.autoScale415Controller.Label.Text = "L415:";
            this.autoScale415Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale415Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.autoScale415Controller.Label.ForeColor = Color.Purple;
            this.autoScale415Controller.Label.Click += new EventHandler(Null_Click);
            this.autoScale415Controller.Button.Name = "autoScaleControlleButton_415_" + roiNumber;
            this.autoScale415Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.autoScale415Controller.Button.Text = "True";
            this.autoScale415Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale415Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.autoScale415Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.autoScale415Controller.Button.Click += new EventHandler(autoScaleLED_Click);
            /***** autoScale470Controller *****/
            this.autoScale470Controller.Name = "autoScaleController_470_" + roiNumber;
            this.autoScale470Controller.Bounds = new Rectangle(0, 2 * (LocalPopOutControllerHeight + VerticalMargin), LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.autoScale470Controller.BackColor = Color.DeepPink;
            this.autoScale470Controller.Label.Name = "autoScaleControllerLabel_470_" + roiNumber;
            this.autoScale470Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.autoScale470Controller.Label.Text = "L470:";
            this.autoScale470Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale470Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.autoScale470Controller.Label.ForeColor = Color.Green;
            this.autoScale470Controller.Label.Click += new EventHandler(Null_Click);
            this.autoScale470Controller.Button.Name = "autoScaleControllerButton_470_" + roiNumber;
            this.autoScale470Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.autoScale470Controller.Button.Text = "True";
            this.autoScale470Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale470Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.autoScale470Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.autoScale470Controller.Button.Click += new EventHandler(autoScaleLED_Click);
            /***** autoScale560Controller *****/
            this.autoScale560Controller.Name = "autoScaleController_560_" + roiNumber;
            this.autoScale560Controller.Bounds = new Rectangle(0, 3 * (LocalPopOutControllerHeight + VerticalMargin), LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.autoScale560Controller.BackColor = Color.DeepPink;
            this.autoScale560Controller.Label.Name = "autoScaleControllerLabel_560_" + roiNumber;
            this.autoScale560Controller.Label.Bounds = new Rectangle(0, 0, 125, LocalPopOutControllerHeight);
            this.autoScale560Controller.Label.Text = "L560:";
            this.autoScale560Controller.Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale560Controller.Label.TextAlign = ContentAlignment.MiddleRight;
            this.autoScale560Controller.Label.ForeColor = Color.Red;
            this.autoScale560Controller.Label.Click += new EventHandler(Null_Click);
            this.autoScale560Controller.Button.Name = "autoScaleControllerButton_560_" + roiNumber;
            this.autoScale560Controller.Button.Bounds = new Rectangle(125, 0, 75, LocalPopOutControllerHeight);
            this.autoScale560Controller.Button.Text = "True";
            this.autoScale560Controller.Button.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.autoScale560Controller.Button.TextAlign = ContentAlignment.MiddleCenter;
            this.autoScale560Controller.Button.ForeColor = Color.FromArgb(0, 90, 181);
            this.autoScale560Controller.Button.Click += new EventHandler(autoScaleLED_Click);
            /***** autoScaleLabelMultiButtonController *****/
            this.autoScaleLabelMultiButtonController.Name = "autoScaleLabelMultiButtonController_" + roiNumber;
            //this.autoScaleLabelMultiButtonController.BackColor = Color.DeepPink;
            this.autoScaleLabelMultiButtonController.Click += new EventHandler(Null_Click);
            /***** minMaxLabels *****/
            this.minMaxLabels.Name = "minMaxLabels_" + roiNumber;
            this.minMaxLabels.Bounds = new Rectangle(0, 0, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.minMaxLabels.BackColor = Color.DeepPink;
            this.minMaxLabels.Click += new EventHandler(Null_Click);
            /***** Label0 *****/
            this.Label0.Name = "Label_415_" + roiNumber;
            this.Label0.Bounds = new Rectangle(50, 0, 50, LocalPopOutControllerHeight);
            //this.Label0.BackColor = Color.DeepPink;
            this.Label0.Text = "L415:";
            this.Label0.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.Label0.ForeColor = Color.Purple;
            this.Label0.TextAlign = ContentAlignment.MiddleCenter;
            this.Label0.Click += new EventHandler(Null_Click);
            /***** Label1 *****/
            this.Label1.Name = "Label_470_" + roiNumber;
            this.Label1.Bounds = new Rectangle(100, 0, 50, LocalPopOutControllerHeight);
            //this.Label1.BackColor = Color.DeepPink;
            this.Label1.Text = "L470:";
            this.Label1.ForeColor = Color.Green;
            this.Label1.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.Label1.TextAlign = ContentAlignment.MiddleCenter;
            this.Label1.Click += new EventHandler(Null_Click);
            /***** Label2 *****/
            this.Label2.Name = "Label_560_" + roiNumber;
            this.Label2.Bounds = new Rectangle(150, 0, 50, LocalPopOutControllerHeight);
            //this.Label2.BackColor = Color.DeepPink;
            this.Label2.Text = "L560:";
            this.Label2.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.Label2.ForeColor = Color.Red;
            this.Label2.TextAlign = ContentAlignment.MiddleCenter;
            this.Label2.Click += new EventHandler(Null_Click);
            /***** minValuesController *****/
            this.minValuesController.Name = "minValuesController_" + roiNumber;
            this.minValuesController.Bounds = new Rectangle(0, LocalPopOutControllerHeight + VerticalMargin, LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.minValuesController.BackColor = Color.DeepPink;
            this.minValuesController.Click += new EventHandler(Null_Click);
            /***** minLabel *****/
            this.minLabel.Name = "minLabel_" + roiNumber;
            this.minLabel.Bounds = new Rectangle(0, 0, 50, LocalPopOutControllerHeight);
            //this.minLabel.BackColor = Color.DeepPink;
            this.minLabel.Text = "Min:";
            this.minLabel.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.minLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.minLabel.Click += new EventHandler(Null_Click);
            /***** min415Label *****/
            this.min415Label.Name = "minLabel_415_" + roiNumber;
            this.min415Label.Bounds = new Rectangle(50, 0, 50, LocalPopOutControllerHeight);
            //this.min415Label.BackColor = Color.DeepPink;
            this.min415Label.Text = "0.0";
            this.min415Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.min415Label.ForeColor = Color.Purple;
            this.min415Label.TextAlign = ContentAlignment.MiddleCenter;
            this.min415Label.Click += new EventHandler(minAxis_Click);
            /***** min470Label *****/
            this.min470Label.Name = "minLabel_470_" + roiNumber;
            this.min470Label.Bounds = new Rectangle(100, 0, 50, LocalPopOutControllerHeight);
            //this.min470Label.BackColor = Color.DeepPink;
            this.min470Label.Text = "0.0";
            this.min470Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.min470Label.ForeColor = Color.Green;
            this.min470Label.TextAlign = ContentAlignment.MiddleCenter;
            this.min470Label.Click += new EventHandler(minAxis_Click);
            /***** min560Label *****/
            this.min560Label.Name = "minLabel_560_" + roiNumber;
            this.min560Label.Bounds = new Rectangle(150, 0, 50, LocalPopOutControllerHeight);
            //this.min560Label.BackColor = Color.DeepPink;
            this.min560Label.Text = "0.0";
            this.min560Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.min560Label.ForeColor = Color.Red;
            this.min560Label.TextAlign = ContentAlignment.MiddleCenter;
            this.min560Label.Click += new EventHandler(minAxis_Click);
            /***** maxValuesController *****/
            this.maxValuesController.Name = "maxValuesController_" + roiNumber;
            this.maxValuesController.Bounds = new Rectangle(0, 2 * (LocalPopOutControllerHeight + VerticalMargin), LocalStatusControllerWidth, LocalPopOutControllerHeight);
            //this.maxValuesController.BackColor = Color.DeepPink;
            this.maxValuesController.Click += new EventHandler(Null_Click);
            /***** maxLabel *****/
            this.maxLabel.Name = "maxLabel_" + roiNumber;
            this.maxLabel.Bounds = new Rectangle(0, 0, 50, LocalPopOutControllerHeight);
            //this.maxLabel.BackColor = Color.DeepPink;
            this.maxLabel.Text = "Max:";
            this.maxLabel.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.maxLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.maxLabel.Click += new EventHandler(Null_Click);
            /***** max415Label *****/
            this.max415Label.Name = "maxLabel_415_" + roiNumber;
            this.max415Label.Bounds = new Rectangle(50, 0, 50, LocalPopOutControllerHeight);
            //this.max415Label.BackColor = Color.DeepPink;
            this.max415Label.Text = "1.0";
            this.max415Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.max415Label.ForeColor = Color.Purple;
            this.max415Label.TextAlign = ContentAlignment.MiddleCenter;
            this.max415Label.Click += new EventHandler(maxAxis_Click);
            /***** max470Label *****/
            this.max470Label.Name = "maxLabel_470_" + roiNumber;
            this.max470Label.Bounds = new Rectangle(100, 0, 50, LocalPopOutControllerHeight);
            //this.max470Label.BackColor = Color.DeepPink;
            this.max470Label.Text = "1.0";
            this.max470Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.max470Label.ForeColor = Color.Green;
            this.max470Label.TextAlign = ContentAlignment.MiddleCenter;
            this.max470Label.Click += new EventHandler(maxAxis_Click);
            /***** max560Label *****/
            this.max560Label.Name = "maxLabel_560_" + roiNumber;
            this.max560Label.Bounds = new Rectangle(150, 0, 50, LocalPopOutControllerHeight);
            //this.max560Label.BackColor = Color.DeepPink;\
            this.max560Label.ForeColor = Color.Red;
            this.max560Label.Text = "1.0";
            this.max560Label.Font = new Font("Century Gothic", 8, System.Drawing.FontStyle.Regular);
            this.max560Label.TextAlign = ContentAlignment.MiddleCenter;
            this.max560Label.Click += new EventHandler(maxAxis_Click);
            /***** minMaxLabelMultiValue2DController *****/
            this.minMaxLabelMultiValue2DController.Name = "minMaxLabelMultiValue2DController_" + roiNumber;
            this.minMaxLabelMultiValue2DController.Size = new Size(LocalStatusControllerWidth, 3 * LocalPopOutControllerHeight + 2 * VerticalMargin);
            //this.minMaxLabelMultiValue2DController.BackColor = Color.DeepPink;
            this.minMaxLabelMultiValue2DController.Click += new EventHandler(Null_Click);
            /***** state *****/
            this.state.Name = "state_" + roiNumber;
            this.state.Bounds = new Rectangle(0, 0, LocalStatusControllerWidth, 5 * LocalPopOutControllerHeight + 4 * VerticalMargin);
            //this.state.BackColor = Color.Green;
            this.state.Click += new EventHandler(Null_Click);
            this.state.Controls.Add(plotVisibleController);
            this.state.Controls.Add(deinterleaveController);
            this.state.Controls.Add(autoScaleController);
            this.state.Controls.Add(minController);
            this.state.Controls.Add(maxController);
            this.state.Visible = true;
            /***** state2 *****/

            /***** localConfigController *****/
            this.localConfigController.Name = "localConfigController_" + roiNumber;
            this.localConfigController.Location = new Point(0, LocalPopOutControllerHeight);
            this.localConfigController.Size = new Size(this.localPopOutController.Width, 5 * LocalPopOutControllerHeight + 4 * VerticalMargin);
            //this.localConfigController.BackColor = Color.BlueViolet;
            this.localConfigController.Click += new EventHandler(Null_Click);
            this.localConfigController.Controls.Add(state);
            this.localConfigController.Visible = true;


            this.Controls.Add(localPopOutController);
            this.Controls.Add(localConfigController);
            this.BackColor = Color.White;
            this.Name = "localStatusController_" + roiNumber;
        }

        #endregion
        private UserControl localPopOutController;
        private Label localPopOutControllerLabel;
        private Label localPopOutControllerLabel1;
        private Label localPopOutControllerLabel2;
        private UserControl localConfigController;
        private LabelButtonController plotVisibleController;
        private LabelButtonController deinterleaveController;
        private LabelButtonController autoScaleController;
        private LabelValueController minController;
        private LabelValueController maxController;
        private UserControl axisLabelMultiButtonController;
        private Label axisLabel;
        private LabelButtonController axis415Controller;
        private LabelButtonController axis470Controller;
        private LabelButtonController axis560Controller;
        private UserControl autoScaleLabelMultiButtonController;
        private Label autoScaleLabel;
        private LabelButtonController autoScale415Controller;
        private LabelButtonController autoScale470Controller;
        private LabelButtonController autoScale560Controller;
        private UserControl minMaxLabelMultiValue2DController;
        private UserControl minMaxLabels;
        private Label Label0;
        private Label Label1;
        private Label Label2;
        private UserControl minValuesController;
        private Label minLabel;
        private Label min415Label;
        private Label min470Label;
        private Label min560Label;
        private UserControl maxValuesController;
        private Label maxLabel;
        private Label max415Label;
        private Label max470Label;
        private Label max560Label;
        private UserControl state;
        private TextBox textBox;
    }
}