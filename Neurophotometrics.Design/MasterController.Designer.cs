using System;

namespace Neurophotometrics.Design
{
    partial class MasterController
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
        private void InitializeComponent(Tuple<int, int> TitleControllerInitProps, Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> GlobalStatusControllerInitProps, Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> GraphListControllerInitProps)
        {
            this.SuspendLayout();

            this.titleController = new Neurophotometrics.Design.TitleController(TitleControllerInitProps);
            this.globalStatusController = new Neurophotometrics.Design.GlobalStatusController(GlobalStatusControllerInitProps);
            this.graphListController = new Neurophotometrics.Design.GraphListController(GraphListControllerInitProps);


            this.components = new System.ComponentModel.Container();

            this.titleController.SuspendLayout();
            this.graphListController.SuspendLayout();
            this.globalStatusController.SuspendLayout();

            //
            // titleController
            //
            this.titleController.Location = new System.Drawing.Point(0, 0);
            //this.titleController.BackColor = System.Drawing.Color.Red;
            this.titleController.Visible = true;
            //
            // graphListController
            //
            this.graphListController.BackColor = System.Drawing.Color.Black;
            this.graphListController.Visible = true;
            this.graphListController.MinsChanged += GraphListController_MinsChanged;
            this.graphListController.MaxesChanged += GraphListController_MaxesChanged;
            // 
            // globalStatusController
            //
            //this.globalStatusController.BackColor = System.Drawing.Color.Blue;
            this.globalStatusController.Visible = true;
            this.globalStatusController.CapacityChanged += GlobalStatusController_CapacityChanged;
            this.globalStatusController.ROIsVisibleChanged += GlobalStatusController_ROIsVisibleChanged;
            this.globalStatusController.DeinterleavesChanged += GlobalStatusController_DeinterleavesChanged;
            this.globalStatusController.AutoScalesChanged += GlobalStatusController_AutoScalesChanged;
            this.globalStatusController.MinsChanged += GlobalStatusController_MinsChanged;
            this.globalStatusController.MaxesChanged += GlobalStatusController_MaxesChanged;
            this.globalStatusController.ConfigModeChanged += GlobalStatusController_ConfigModeChanged;
            this.globalStatusController.ColorBlindModeChanged += GlobalStatusController_ColorBlindModeChanged;

            //
            // add Controls to Master Controller
            //
            this.Controls.Add(this.titleController);
            this.Controls.Add(this.graphListController);
            this.Controls.Add(this.globalStatusController);

            this.Name = "MasterController";
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            this.titleController.ResumeLayout(false);
            this.graphListController.ResumeLayout(false);
            this.globalStatusController.ResumeLayout(false);
            this.titleController.PerformLayout();
            this.graphListController.PerformLayout();
            this.graphListController.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TitleController titleController;
        private GraphListController graphListController;
        private GlobalStatusController globalStatusController;
    }
}
