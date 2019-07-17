using Bonsai.Design.Visualizers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Drawing;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    class RollingGraph : GraphControl
    {
        int capacity;
        bool autoScale;
        int paneCount;
        float scaleFactor;
        const int DefaultCapacity = 300;
        const float YAxisMinSpace = 50;
        const float DefaultPaneMargin = 10;
        const float DefaultPaneTitleGap = 0.5f;
        const float TileMasterPaneHorizontalMargin = 1;
        const float TilePaneVerticalMargin = 2;
        const float TilePaneHorizontalMargin = 2;
        const float TilePaneInnerGap = 1;

        public RollingGraph()
        {
            autoScale = true;
            IsShowContextMenu = false;
            capacity = DefaultCapacity;
            MasterPane.Title.IsVisible = false;
            MasterPane.InnerPaneGap = TilePaneInnerGap;
            MasterPane.Margin.Left = TileMasterPaneHorizontalMargin;
            MasterPane.Margin.Right = TileMasterPaneHorizontalMargin;
            MasterPane.Margin.Bottom = 50;
            GraphPane.Margin.Top = TilePaneVerticalMargin;
            GraphPane.Margin.Bottom = TilePaneVerticalMargin;
            //GraphPane.Margin.Bottom = overlaySeries ? DefaultPaneMargin : TilePaneVerticalMargin;
            GraphPane.YAxis.MinSpace = YAxisMinSpace;
            GraphPane.XAxis.IsVisible = false;
            GraphPane.IsFontsScaled = false;

            GraphPane.YAxis.Scale.IsVisible = false;
            GraphPane.YAxis.MajorTic.IsAllTics = false;
            GraphPane.YAxis.IsAxisSegmentVisible = false;
            GraphPane.YAxis.Title.FontSpec.Size = 12 * scaleFactor;
        }

        public int PaneCount
        {
            get { return paneCount; }
            set
            {
                var changed = paneCount != value;
                paneCount = value;
                if (changed)
                {
                    ResetPaneLayout();
                    Invalidate();
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                EnsureCapacity();
                Invalidate();
            }
        }

        public double Min
        {
            get { return GraphPane.YAxis.Scale.Min; }
            set
            {
                foreach (var pane in MasterPane.PaneList)
                {
                    pane.YAxis.Scale.Min = value;
                    pane.AxisChange();
                }
                Invalidate();
            }
        }

        public double Max
        {
            get { return GraphPane.YAxis.Scale.Max; }
            set
            {
                foreach (var pane in MasterPane.PaneList)
                {
                    pane.YAxis.Scale.Max = value;
                    pane.AxisChange();
                }
                Invalidate();
            }
        }

        public bool AutoScale
        {
            get { return autoScale; }
            set
            {
                var changed = autoScale != value;
                autoScale = value;
                if (changed)
                {
                    foreach (var pane in MasterPane.PaneList)
                    {
                        pane.YAxis.Scale.MaxAuto = value;
                        pane.YAxis.Scale.MinAuto = value;
                    }
                    if (autoScale) Invalidate();
                }
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            scaleFactor = factor.Height;
            base.ScaleControl(factor, specified);
        }

        public void EnsureCapacity()
        {
            foreach (var pane in MasterPane.PaneList)
            {
                foreach (var curve in pane.CurveList)
                {
                    var points = new RollingPointPairList(capacity);
                    points.Add(curve.Points);
                    curve.Points = points;
                }
            }
        }

        private void ResetPaneLayout()
        {
            var paneList = MasterPane.PaneList;
            paneList.RemoveRange(1, paneList.Count - 1);

            var paneTemplate = paneList[0];
            paneTemplate.CurveList.Clear();
            for (int i = 1; i < paneCount; i++)
            {
                var pane = new GraphPane(paneTemplate);
                paneList.Add(pane);
            }

            SetLayout(PaneLayout.SingleColumn);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var paneList = MasterPane.PaneList;
            var timePane = paneList[paneList.Count - 1];
            timePane.XAxis.IsVisible = true;
            timePane.XAxis.Draw(e.Graphics, timePane, timePane.CalcScaleFactor(), 10);
            timePane.XAxis.IsVisible = false;
        }
    }
}
