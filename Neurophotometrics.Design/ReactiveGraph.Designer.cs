using System.Drawing;

namespace Neurophotometrics.Design
{
    partial class ReactiveGraph
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
            this.master = this.MasterPane;
            this.graph = new ZedGraph.GraphPane();

            this.components = new System.ComponentModel.Container();

            this.SuspendLayout();


            //
            // masterPane
            //
            this.master.Tag = "Master";
            this.master.Title.IsVisible = true;
            this.master.Title.Text = "ROI " + roiNumber;
            this.master.Title.FontSpec.Size = 18;
            this.master.TitleGap = 0;
            this.master.InnerPaneGap = 0;
            this.master.Margin.All = 0;
            this.master.Border.IsVisible = false;
            //this.master.Fill.Color = Color.Red;
            this.master.IsFontsScaled = true;
            this.master.PaneList.Clear();
            //
            // graph
            //
            this.graph.Tag = "a_Interleaved";
            this.graph.Title.IsVisible = false;
            this.graph.YAxisList.Clear();
            this.graph.AddYAxis("Fluorescence (A.U.)");
            this.graph.Border.IsVisible = false;
            this.graph.YAxis.MinSpace = YAxisMinSpace;
            this.graph.Margin.Top = 0;
            this.graph.Margin.Left = 0;
            this.graph.Margin.Right = 0;
            this.graph.Margin.Bottom = 0;
            this.graph.YAxis.Color = Color.Black;
            this.graph.YAxis.Scale.FontSpec.FontColor = Color.Black;
            this.graph.YAxis.Scale.FontSpec.Size = 30;
            this.graph.YAxis.Title.FontSpec.FontColor = Color.Black;
            this.graph.YAxis.Title.FontSpec.Size = 30;
            this.graph.YAxis.Title.Gap = 0;
            this.graph.XAxis.IsVisible = true;
            this.graph.XAxis.Scale.IsVisible = false;
            this.graph.XAxis.Scale.MajorUnit = ZedGraph.DateUnit.Second;
            this.graph.XAxis.MajorTic.IsAllTics = true;
            this.graph.XAxis.MajorTic.IsInside = true;
            this.graph.XAxis.MajorTic.IsOutside = false;
            this.graph.XAxis.MajorTic.IsCrossOutside = false;
            this.graph.XAxis.MajorTic.Size = 10;
            this.graph.XAxis.Scale.LabelGap = 0;
            this.graph.XAxis.AxisGap = 0;
            this.graph.XAxis.MinorTic.IsAllTics = false;
            this.graph.XAxis.IsAxisSegmentVisible = true;
            this.graph.XAxis.Title.Text = "Time (seconds)";
            this.graph.XAxis.Title.FontSpec.Size = 36;
            this.graph.XAxis.Title.Gap = 0;
            this.graph.Chart.IsRectAuto = false;
            this.graph.IsFontsScaled = false;
            //this.graph.Fill.Color = Color.Beige;
            this.master.Add(graph);

            using (Graphics g = this.CreateGraphics())
            {
                master.SetLayout(g, ZedGraph.PaneLayout.SingleColumn);
            }

            this.Name = "ReactiveGraph_" + roiNumber;
        }

        private ZedGraph.MasterPane master;
        private ZedGraph.GraphPane graph;

        #endregion
    }
}
