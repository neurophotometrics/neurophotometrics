using Bonsai.Design.Visualizers;
using System;
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
        const float TileMasterPaneHorizontalMargin = 1;
        const float TilePaneVerticalMargin = 2;
        const float TilePaneInnerGap = 1;

        public RollingGraph()
        {
            autoScale = true;
            IsEnableZoom = false;
            IsShowContextMenu = false;
            capacity = DefaultCapacity;
            MasterPane = new RollingMultiPane(MasterPane);
            MasterPane.Title.IsVisible = false;
            MasterPane.InnerPaneGap = TilePaneInnerGap;
            MasterPane.Margin.Left = TileMasterPaneHorizontalMargin;
            MasterPane.Margin.Right = TileMasterPaneHorizontalMargin;
            GraphPane.Margin.Top = TilePaneVerticalMargin;
            GraphPane.Margin.Bottom = TilePaneVerticalMargin;
            GraphPane.YAxis.MinSpace = YAxisMinSpace;
            GraphPane.XAxis.IsVisible = false;
            GraphPane.IsFontsScaled = false;
            EnsurePaneMargin();

            GraphPane.YAxis.Scale.IsVisible = false;
            GraphPane.YAxis.MajorTic.IsAllTics = false;
            GraphPane.YAxis.IsAxisSegmentVisible = false;
            GraphPane.YAxis.Title.FontSpec.Size = 12 * scaleFactor;
            GraphPane.AxisChangeEvent += GraphPane_AxisChangeEvent;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            EnsurePaneMargin();
            base.OnSizeChanged(e);
        }

        private void GraphPane_AxisChangeEvent(GraphPane sender)
        {
            if (autoScale && sender == GraphPane)
            {
                var max = double.MinValue;
                var min = double.MaxValue;
                foreach (var pane in MasterPane.PaneList)
                {
                    foreach (var curve in pane.CurveList)
                    {
                        double xMin, xMax, yMin, yMax;
                        curve.GetRange(out xMin, out xMax, out yMin, out yMax, pane.IsIgnoreInitial, pane.IsBoundedRanges, pane);
                        max = Math.Max(max, yMax);
                        min = Math.Min(min, yMin);
                    }
                }

                var updateSender = max != sender.YAxis.Scale.Max || min != sender.YAxis.Scale.Min;
                foreach (var pane in MasterPane.PaneList)
                {
                    if (pane == sender && !updateSender) continue;
                    pane.YAxis.Scale.Max = max;
                    pane.YAxis.Scale.Min = min;
                }
            }
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
                    GraphPane.YAxis.Scale.MaxAuto = value;
                    GraphPane.YAxis.Scale.MinAuto = value;
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
            ResetColorCycle();
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
            EnsurePaneMargin();
        }

        private void EnsurePaneMargin()
        {
            if (MasterPane != null)
            {
                var rect = MasterPane.Rect;
                var aspectRatio = rect.Width / rect.Height;
                var length = aspectRatio > 1.5f ? rect.Height : 0.7f * rect.Width;
                var marginScaleFactor = scaleFactor * 72 * MasterPane.BaseDimension / length;
                MasterPane.Margin.Bottom = 32 * marginScaleFactor;
            }
        }

        class RollingMultiPane : MasterPane
        {
            public RollingMultiPane(MasterPane template)
                : base(template)
            {
            }

            public override void Draw(Graphics g)
            {
                base.Draw(g);
                var paneList = PaneList;
                var timePane = paneList[paneList.Count - 1];
                if (timePane.Rect.Width > 0 && timePane.Rect.Height > 0)
                {
                    timePane.XAxis.IsVisible = true;
                    timePane.XAxis.Draw(g, timePane, timePane.CalcScaleFactor(), 10);
                    timePane.XAxis.IsVisible = false;
                }
            }
        }
    }
}
