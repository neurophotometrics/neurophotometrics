using System.Drawing;
using ZedGraph;

namespace Neurophotometrics.Design
{
    static class GraphHelper
    {
        internal static void SetAxisLabel(Axis axis, string label)
        {
            axis.Title.Text = label;
            axis.Title.IsVisible = !string.IsNullOrEmpty(label);
        }

        internal static void FormatDateAxis(Axis axis)
        {
            axis.Type = AxisType.DateAsOrdinal;
            axis.Scale.Format = "HH:mm:ss";
            axis.Scale.MajorUnit = DateUnit.Second;
            axis.Scale.MinorUnit = DateUnit.Millisecond;
            axis.MinorTic.IsAllTics = false;
        }

        internal static string GetRegionLabel(ref PhotometryRegion region, float halfWidth)
        {
            const string DefaultLabel = "0";
            if (region.Index < 0) return DefaultLabel;
            return region.Index + (region.Center.X < halfWidth ? " R" : " G");
        }

        internal static LineItem AddActivity(this GraphPane pane, string label, IPointList points, Color color)
        {
            var series = pane.AddCurve(string.Empty, points, color, SymbolType.None);
            pane.YAxis.Title.Text = label;
            pane.YAxis.Title.FontSpec.Angle = 90;
            pane.YAxis.Title.IsVisible = true;
            series.Label.IsVisible = false;
            return series;
        }
    }
}
