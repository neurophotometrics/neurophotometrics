
namespace Neurophotometrics.Design.V2.Visualizers
{
    partial class SignalTimingVisualizer
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
            this.components = new System.ComponentModel.Container();
            this.Plot_ZedGraphControl = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // Plot_ZedGraphControl
            // 
            this.Plot_ZedGraphControl.AutoSize = true;
            this.Plot_ZedGraphControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Plot_ZedGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Plot_ZedGraphControl.IsEnableHPan = false;
            this.Plot_ZedGraphControl.IsEnableHZoom = false;
            this.Plot_ZedGraphControl.IsEnableVPan = false;
            this.Plot_ZedGraphControl.IsEnableVZoom = false;
            this.Plot_ZedGraphControl.IsEnableWheelZoom = false;
            this.Plot_ZedGraphControl.IsShowContextMenu = false;
            this.Plot_ZedGraphControl.IsShowCopyMessage = false;
            this.Plot_ZedGraphControl.Location = new System.Drawing.Point(0, 0);
            this.Plot_ZedGraphControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Plot_ZedGraphControl.Name = "Plot_ZedGraphControl";
            this.Plot_ZedGraphControl.ScrollGrace = 0D;
            this.Plot_ZedGraphControl.ScrollMaxX = 0D;
            this.Plot_ZedGraphControl.ScrollMaxY = 0D;
            this.Plot_ZedGraphControl.ScrollMaxY2 = 0D;
            this.Plot_ZedGraphControl.ScrollMinX = 0D;
            this.Plot_ZedGraphControl.ScrollMinY = 0D;
            this.Plot_ZedGraphControl.ScrollMinY2 = 0D;
            this.Plot_ZedGraphControl.Size = new System.Drawing.Size(648, 405);
            this.Plot_ZedGraphControl.TabIndex = 0;
            this.Plot_ZedGraphControl.UseExtendedPrintDialog = true;
            this.Plot_ZedGraphControl.Resize += new System.EventHandler(this.Plot_ZedGraphControl_Resize);
            // 
            // SignalTimingVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Plot_ZedGraphControl);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SignalTimingVisualizer";
            this.Size = new System.Drawing.Size(648, 405);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl Plot_ZedGraphControl;
    }
}
