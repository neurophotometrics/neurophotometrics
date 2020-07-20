using Bonsai.IO;
using System;
using System.Drawing;
using System.IO;
using System.Reactive.Linq;
using ZedGraph;

namespace Neurophotometrics.Design
{
    class ChartWriter : FileSink
    {
        const int TitleSize = 32;
        const int ScaleSize = 20;
        const int ChartWidth = 1920;
        const int ChartHeight = 1080;
        const string ChartSuffix = "-Chart.jpg";

        struct GroupedData
        {
            public string Name;
            public string[] Labels;
            public IPointListEdit[] Data;
        }

        static void InitializeGraphPane(RollingGraph graph)
        {
            graph.GraphPane.XAxis.Title.Text = "Time (s)";
            graph.GraphPane.XAxis.Title.IsVisible = true;
            graph.GraphPane.XAxis.Title.FontSpec.Size = TitleSize;
            graph.GraphPane.XAxis.Scale.FontSpec.Size = ScaleSize;
            graph.GraphPane.YAxis.Title.FontSpec.Size = TitleSize;
        }

        void SaveGraph(RollingGraph graph)
        {
            var fileName = Path.ChangeExtension(FileName, null) + ChartSuffix;
            using (var graphics = graph.CreateGraphics())
            {
                graph.MasterPane.Title.Text = string.Format("Session {0}", DateTime.Now);
                graph.MasterPane.Title.IsVisible = true;
                graph.MasterPane.Margin.Right = TitleSize;
                graph.MasterPane.Margin.Bottom = TitleSize;
                graph.MasterPane.ReSize(graphics, new RectangleF(0, 0, ChartWidth, ChartHeight));
                graph.MasterPane.SetLayout(graphics, PaneLayout.SingleColumn);
                graph.MasterPane.AxisChange(graphics);
                var image = graph.GetImage();
                image.Save(fileName);
            }
        }

        public IObservable<GroupedPhotometryDataFrame> Process(IObservable<GroupedPhotometryDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                GroupedData[] groups = null;
                return source.Do(input =>
                {
                    if (groups == null || groups.Length != input.Groups.Length)
                    {
                        groups = new GroupedData[input.Groups.Length];
                        for (int i = 0; i < groups.Length; i++)
                        {
                            groups[i].Name = input.Groups[i].Name;
                            var labels = groups[i].Labels = new string[input.Groups[i].Activity.Length];
                            var data = groups[i].Data = new IPointListEdit[input.Groups[i].Activity.Length];
                            for (int j = 0; j < data.Length; j++)
                            {
                                data[j] = new PointPairList();
                                labels[j] = GraphHelper.GetRegionLabel(ref input.Groups[i].Activity[j].Region);
                            }
                        }
                    }

                    for (int i = 0; i < groups.Length; i++)
                    {
                        for (int j = 0; j < groups[i].Data.Length; j++)
                        {
                            groups[i].Data[j].Add(input.Timestamp, input.Groups[i].Activity[j].Value);
                        }
                    }
                }).Finally(() =>
                {
                    if (groups == null) return;
                    var graph = new RollingGraph();
                    InitializeGraphPane(graph);
                    graph.PaneCount = groups.Length;
                    for (int i = 0; i < groups.Length; i++)
                    {
                        var group = groups[i];
                        var pane = graph.MasterPane.PaneList[i];
                        for (int j = 0; j < group.Data.Length; j++)
                        {
                            pane.AddCurve(group.Labels[j], group.Data[j], graph.GetNextColor(), SymbolType.None);
                        }

                        pane.Legend.FontSpec.Size = ScaleSize;
                        pane.YAxis.Title.Text = group.Name;
                        pane.YAxis.Title.IsVisible = true;
                    }

                    SaveGraph(graph);
                });
            });
        }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                string[] labels = null;
                IPointListEdit[] data = null;
                return source.Do(input =>
                {
                    if (data == null || data.Length != input.Activity.Length)
                    {
                        labels = new string[input.Activity.Length];
                        data = new IPointListEdit[input.Activity.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = new PointPairList();
                            labels[i] = GraphHelper.GetRegionLabel(ref input.Activity[i].Region);
                        }
                    }

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i].Add(input.Timestamp, input.Activity[i].Value);
                    }
                }).Finally(() =>
                {
                    if (data == null) return;
                    var graph = new RollingGraph();
                    InitializeGraphPane(graph);
                    graph.PaneCount = data.Length;
                    for (int i = 0; i < data.Length; i++)
                    {
                        var pane = graph.MasterPane.PaneList[i];
                        pane.AddActivity(labels[i], data[i], graph.GetNextColor());
                    }

                    SaveGraph(graph);
                });
            });
        }
    }
}
