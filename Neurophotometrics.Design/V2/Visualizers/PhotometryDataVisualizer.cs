using Neurophotometrics.V2.Definitions;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace Neurophotometrics.Design.V2.Visualizers
{
    public partial class PhotometryDataVisualizer : UserControl
    {
        private const int DefaultCapacity = 300;

        public PhotometryDataVisualizer()
        {
            InitializeComponent();

            Plot_ZedGraphControl.IsShowContextMenu = false;
            Plot_ZedGraphControl.MasterPane.Tag = "Master";
            Plot_ZedGraphControl.MasterPane.Title.IsVisible = true;
            Plot_ZedGraphControl.MasterPane.Title.Text = "Photometry Data";
            Plot_ZedGraphControl.MasterPane.Title.FontSpec.Size = 18;
            Plot_ZedGraphControl.MasterPane.TitleGap = 0.0f;
            Plot_ZedGraphControl.MasterPane.InnerPaneGap = 0;
            Plot_ZedGraphControl.MasterPane.Margin.All = 2.5f;
            Plot_ZedGraphControl.MasterPane.Border.IsVisible = true;
            Plot_ZedGraphControl.MasterPane.IsFontsScaled = true;
            Plot_ZedGraphControl.MasterPane.PaneList.Clear();
        }

        internal void TryUpdatePlots(RegionActivity[] activities, double timestamp)
        {
            try
            {
                UpdatePlots(activities, timestamp);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdatePlots(RegionActivity[] activities, double timestamp)
        {
            if (Plot_ZedGraphControl.MasterPane == null) return;

            EnsureGraphPanes(activities);
            UpdatePoints(activities, timestamp);
        }

        internal void TryRefreshPlot()
        {
            try
            {
                RefreshPlot();
            }
            catch (ObjectDisposedException ex)
            {
                ConsoleLogger.SuppressError(ex);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void RefreshPlot()
        {
            Invoke((MethodInvoker)delegate {
                Plot_ZedGraphControl.Refresh();
            });
        }

        private void EnsureGraphPanes(RegionActivity[] activity)
        {
            // If no Graph Panes, initialize them and return
            if (Plot_ZedGraphControl.MasterPane.PaneList.Count == 0)
            {
                for (var i = 0; i < activity.Length; i++)
                    Plot_ZedGraphControl.MasterPane.PaneList.Add(new SignalPane(activity[i].Region));
                return;
            }

            // Handle Region Added/Deleted

            var numGraphPanes = Plot_ZedGraphControl.MasterPane.PaneList.Count;
            if (activity.Length < numGraphPanes)
            {
                // Region Deleted: Remove Graph Panes with region rectangle tag not in activity
                Plot_ZedGraphControl.MasterPane.PaneList.RemoveAll(graphPane => !activity.Select(act => act.Region.Rectangle).Contains(((PhotometryRegion)graphPane.Tag).Rectangle));
            }
            else if (activity.Length > numGraphPanes)
            {
                // Region Added: Add Graph Pane to end of PaneList
                Plot_ZedGraphControl.MasterPane.PaneList.Add(new SignalPane(activity[activity.Length - 1].Region));
            }
            else
            {
                // Region Translated, Scaled or No Change
                // Region Channel and thus Region color and label could have changed, assume only one can be changed
                var mislabeledPanes = Plot_ZedGraphControl.MasterPane.PaneList.Where(graphPane => !activity.Select(act => act.Region).Contains((PhotometryRegion)graphPane.Tag)).Cast<SignalPane>();
                var updatedRegions = activity.Select(act => act.Region).Where(region => !Plot_ZedGraphControl.MasterPane.PaneList.Select(graphPane => ((PhotometryRegion)graphPane.Tag)).Contains(region));
                if (mislabeledPanes.Any() && updatedRegions.Any())
                {
                    mislabeledPanes.First().UpdateRegion(updatedRegions.First());
                }
            }

            using (var g = CreateGraphics())
                Plot_ZedGraphControl.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
        }

        private void UpdatePoints(RegionActivity[] activity, double? timestamp)
        {
            var globalXMin = double.MaxValue;
            var globalXMax = double.MinValue;
            Plot_ZedGraphControl.MasterPane.PaneList.ForEach(graphPane =>
            {
                var newActivity = activity.Where(act => act.Region.Equals((PhotometryRegion)graphPane.Tag));

                if (newActivity.Any())
                {
                    graphPane.CurveList[0].AddPoint(new PointPair(timestamp.Value, newActivity.First().Value));
                    while (graphPane.CurveList[0].Points.Count > DefaultCapacity)
                        graphPane.CurveList[0].RemovePoint(0);
                    graphPane.CurveList[0].GetRange(out var xMin, out var xMax, out var yMin, out var yMax, graphPane.IsIgnoreInitial, graphPane.IsBoundedRanges, graphPane);
                    if (yMin == yMax)
                    {
                        yMin = 0.0;
                        yMax = 1.0;
                    }
                    else
                    {
                        var offset = (yMax - yMin) * 0.1;
                        yMin -= offset;
                        yMax += yMax + offset;
                    }
                    graphPane.YAxis.Scale.Min = yMin;
                    graphPane.YAxis.Scale.Max = yMax;
                    if (xMin < globalXMin) globalXMin = xMin;
                    if (xMax > globalXMax) globalXMax = xMax;
                }
            });

            var numPlots = Plot_ZedGraphControl.MasterPane.PaneList.Count;

            var scaleFactor = Plot_ZedGraphControl.MasterPane.CalcScaleFactor();
            var margin = Plot_ZedGraphControl.MasterPane.Margin.Left;
            var titleHeight = Plot_ZedGraphControl.MasterPane.Title.FontSpec.Size * scaleFactor + 5.0f;
            var height = Plot_ZedGraphControl.Height - titleHeight - 2.0f * margin;
            var width = (float)Plot_ZedGraphControl.Width - 2.0f * margin;
            var yAxisWidth = width * 0.05f;

            var chartSize = new SizeF(width - yAxisWidth, height / numPlots);
            var chartPosX = yAxisWidth + margin;

            for (var i = 0; i < numPlots; i++)
            {
                var graph = Plot_ZedGraphControl.MasterPane.PaneList[i];

                var chartPoint = new PointF(chartPosX, chartSize.Height * i + margin + titleHeight);
                graph.Chart.Rect = new RectangleF(chartPoint, chartSize);

                var rectPoint = new PointF(margin, chartSize.Height * i + margin + titleHeight);
                var rectSize = new SizeF(width, chartSize.Height);
                var rect = new RectangleF(rectPoint, rectSize);
                graph.Rect = rect;

                var paneScaleFactor = graph.CalcScaleFactor();
                var fontSizeFactor = Math.Min(yAxisWidth, graph.Rect.Height);
                graph.YAxis.Title.FontSpec.Size = fontSizeFactor * 0.4f / paneScaleFactor;

                graph.XAxis.Scale.Min = globalXMin;
                graph.XAxis.Scale.Max = globalXMax;

                if (graph.CurveList[0].Points.Count == 2)
                {
                    var triggerPeriod = graph.CurveList[0].Points[1].X - graph.CurveList[0].Points[0].X;
                    graph.XAxis.Scale.MajorStep = Math.Round(DefaultCapacity / 6 * triggerPeriod, 3);
                }
            }
        }

        private sealed class SignalPane : GraphPane
        {
            public SignalPane(PhotometryRegion region)
            {
                var label = region.Name;
                Tag = region;
                Title.IsVisible = false;
                YAxisList.Clear();
                AddYAxis(label);
                Legend.IsVisible = false;
                Border.IsVisible = false;
                Chart.Border.IsVisible = false;
                Margin.All = 0;

                // X-Axis
                XAxis.IsVisible = false;
                XAxis.Title.IsVisible = false;
                XAxis.Title.Gap = 0;

                XAxis.MinorTic.IsAllTics = false;

                XAxis.MajorTic.IsAllTics = false;
                XAxis.MajorTic.Size = 10;

                XAxis.Scale.MajorUnit = DateUnit.Second;
                XAxis.Scale.LabelGap = 0;
                XAxis.Scale.BaseTic = 0;

                // Y-Axis
                var color = region.GetColor();
                YAxis.MinorTic.IsAllTics = false;
                YAxis.MajorTic.IsAllTics = false;
                YAxis.Scale.IsVisible = false;
                YAxis.Title.FontSpec.FontColor = color;
                YAxis.Title.FontSpec.Angle = 90;
                YAxis.Title.Gap = 0;
                YAxis.MajorGrid.IsZeroLine = false;

                AddCurve(label, new PointPairList(), color, SymbolType.None);
                Chart.IsRectAuto = true;
                IsFontsScaled = true;
            }

            internal void UpdateRegion(PhotometryRegion region)
            {
                var label = region.Name;
                var color = region.GetColor();
                var curve = (LineItem)CurveList[0];
                Tag = region;
                YAxis.Title.Text = label;
                YAxis.Title.FontSpec.FontColor = color;
                curve.Color = color;
            }
        }

        internal void Clear()
        {
            Plot_ZedGraphControl.MasterPane.PaneList.Clear();
        }
    }
}