using Bonsai;
using Bonsai.Design;
using Neurophotometrics;
using Neurophotometrics.Design;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

[assembly: TypeVisualizer(typeof(ActivityVisualizer), Target = typeof(PhotometryDataFrame))]
[assembly: TypeVisualizer(typeof(ActivityVisualizer), Target = typeof(GroupedPhotometryDataFrame))]

namespace Neurophotometrics.Design
{
    public class ActivityVisualizer : DialogTypeVisualizer
    {
        static readonly TimeSpan TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 30);
        internal RollingGraphView view;
        DateTimeOffset updateTime;
        bool? showDefaultRegion;

        internal void UpdateView(DateTime time)
        {
            if ((time - updateTime) > TargetElapsedTime)
            {
                view.Graph.Invalidate();
                updateTime = time;
            }
        }

        public override void Load(IServiceProvider provider)
        {
            view = new RollingGraphView();
            view.Dock = DockStyle.Fill;
            GraphHelper.FormatDateAxis(view.Graph.GraphPane.XAxis);
            GraphHelper.SetAxisLabel(view.Graph.GraphPane.XAxis, "Time");

            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            if (visualizerService != null)
            {
                visualizerService.AddControl(view);
            }
        }

        static string GetRegionLabel(ref PhotometryRegion region, float halfWidth)
        {
            const string DefaultLabel = "0";
            if (region.Index < 0) return DefaultLabel;
            return region.Index + (region.Center.X < halfWidth ? " R" : " G");
        }

        private void Show(XDate time, PhotometryDataFrame frame)
        {
            var activity = frame.Activity;
            view.Graph.PaneCount = activity.Length;
            if (activity.Length == 1)
            {
                var defaultRegion = activity[0].Region.Index < 0;
                if (showDefaultRegion != defaultRegion)
                {
                    view.Graph.ResetColorCycle();
                    view.Graph.MasterPane.PaneList[0].CurveList.Clear();
                    showDefaultRegion = defaultRegion;
                }
            }

            var halfWidth = frame.Image.Width / 2f;
            var paneList = view.Graph.MasterPane.PaneList;
            for (int i = 0; i < activity.Length; i++)
            {
                CurveItem curve;
                var pane = paneList[i];
                if (pane.CurveList.Count == 0)
                {
                    var points = new RollingPointPairList(view.Capacity);
                    var series = pane.AddCurve(string.Empty, points, view.Graph.GetNextColor(), SymbolType.None);
                    pane.YAxis.Title.Text = GetRegionLabel(ref activity[i].Region, halfWidth);
                    pane.YAxis.Title.FontSpec.Angle = 90;
                    pane.YAxis.Title.IsVisible = true;
                    series.Line.IsAntiAlias = false;
                    series.Line.IsOptimizedDraw = true;
                    series.Label.IsVisible = false;
                    curve = series;
                }
                else curve = pane.CurveList[0];
                curve.AddPoint(time, activity[i].Value);
            }
        }

        private void Show(XDate time, GroupedPhotometryDataFrame frame)
        {
            var groups = frame.Groups;
            view.Graph.PaneCount = groups.Length;

            var halfWidth = frame.Image.Width / 2f;
            var paneList = view.Graph.MasterPane.PaneList;
            for (int i = 0; i < groups.Length; i++)
            {
                var pane = paneList[i];
                var group = groups[i];
                if (pane.CurveList.Count != group.Activity.Length) pane.CurveList.Clear();
                if (pane.CurveList.Count == 0)
                {
                    for (int j = 0; j < group.Activity.Length; j++)
                    {
                        var points = new RollingPointPairList(view.Capacity);
                        var label = GetRegionLabel(ref group.Activity[j].Region, halfWidth);
                        var series = pane.AddCurve(label, points, view.Graph.GetNextColor(), SymbolType.None);
                        series.Line.IsAntiAlias = false;
                        series.Line.IsOptimizedDraw = true;
                    }

                    pane.YAxis.Title.Text = group.Name;
                    pane.YAxis.Title.IsVisible = true;
                }

                for (int j = 0; j < group.Activity.Length; j++)
                {
                    pane.CurveList[j].AddPoint(time, group.Activity[j].Value);
                }
            }
        }

        public override void Show(object value)
        {
            XDate time = DateTime.Now;
            if (value is PhotometryDataFrame frame) Show(time, frame);
            else Show(time, (GroupedPhotometryDataFrame)value);
            UpdateView(DateTime.Now);
        }

        public override void Unload()
        {
            view.Dispose();
            view = null;
        }
    }
}
