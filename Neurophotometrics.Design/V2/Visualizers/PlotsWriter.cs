using Neurophotometrics.V2.Definitions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ZedGraph;

namespace Neurophotometrics.V2.PhotometryWriterHelpers
{
    public sealed class PlotsWriter : IDisposable
    {
        private const float Width = 1920;
        private const float Height = 1080;
        private const float xAxisHeightPercent = 0.1f;
        private const float yAxisWidthPercent = 0.1f;
        private const string PlotsFolderName = "Plots";
        private const string PlotsBaseFileName = "_Plot.jpg";
        private readonly string PlotsDirectory;
        private readonly string Suffix;
        private readonly List<ZedGraphControl> RegionPlots;
        private readonly List<ushort> LEDFlags;

        private PhotometryDataFrame DataFrame;

        private bool Saving = true;
        private bool IsNewDataFrame = false;
        private bool IsFirstFrame = true;

        public PlotsWriter(string parentDirectory, string suffix)
        {
            PlotsDirectory = GetParentDirectory(parentDirectory);
            Suffix = suffix;
            RegionPlots = new List<ZedGraphControl>();
            LEDFlags = new List<ushort>();
        }

        private string GetParentDirectory(string parentDirectory)
        {
            var directoryInfo = new DirectoryInfo(parentDirectory);
            var folderAbsPath = $@"{directoryInfo.FullName}\{PlotsFolderName}";

            if (!Directory.Exists(folderAbsPath))
                Directory.CreateDirectory(folderAbsPath);
            else
                throw new InvalidOperationException(string.Format("The path '{0}' already exists.", folderAbsPath));

            parentDirectory = folderAbsPath + @"\";

            return parentDirectory;
        }

        internal void Open()
        {
            Task.Factory.StartNew(new Action(DataSaver));
        }

        internal void Write(PhotometryDataFrame dataFrame)
        {
            DataFrame = dataFrame;
            IsNewDataFrame = true;
        }

        private void DataSaver()
        {
            while (Saving)
            {
                if (IsNewDataFrame)
                {
                    IsNewDataFrame = false;
                    SaveDataFrame();
                }
            }
            TrySavePlots();
        }

        private void SaveDataFrame()
        {
            if (IsFirstFrame)
            {
                TryWriteFirstFrame();
                IsFirstFrame = false;
            }

            TryWriteNewFrame();
        }

        private void TryWriteFirstFrame()
        {
            try
            {
                WriteFirstFrame();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void WriteFirstFrame()
        {
            CreateRegionPlots(DataFrame);
            EnsureSignals(DataFrame);
            AddPoints(DataFrame);
        }

        private void CreateRegionPlots(PhotometryDataFrame dataFrame)
        {
            foreach (var region in dataFrame.Activities.Select(regionActivity => regionActivity.Region))
            {
                var regionPlot = new ZedGraphControl
                {
                    Tag = region
                };

                regionPlot.MasterPane.PaneList.Clear();

                regionPlot.MasterPane.Border.IsVisible = true;
                regionPlot.MasterPane.InnerPaneGap = 0;
                regionPlot.MasterPane.IsFontsScaled = true;
                regionPlot.MasterPane.Margin.All = 2.5f;
                regionPlot.MasterPane.Title.Text = string.Format("Session {0}, {1}", DateTime.Now, region.Name);
                regionPlot.MasterPane.Title.IsVisible = true;
                regionPlot.MasterPane.Title.FontSpec.Size = 18;
                regionPlot.MasterPane.TitleGap = 0.0f;

                RegionPlots.Add(regionPlot);
            }
        }

        private void EnsureSignals(PhotometryDataFrame dataFrame)
        {
            var ledFlag = (ushort)((ushort)dataFrame.Flags & 0xf);
            if (ledFlag == 0 || ledFlag == 0xf) return;

            if (LEDFlags.Contains(ledFlag)) return;

            LEDFlags.Add(ledFlag);
            AddSignalPane(ledFlag);
        }

        private void AddSignalPane(ushort ledFlag)
        {
            var signalColor = GetColor(ledFlag);
            foreach (var regionPlot in RegionPlots)
            {
                var signalPane = new GraphPane
                {
                    Tag = ledFlag
                };
                signalPane.Border.IsVisible = true;
                signalPane.Chart.IsRectAuto = false;
                signalPane.Chart.Border.IsVisible = true;
                signalPane.IsFontsScaled = false;
                signalPane.Legend.IsVisible = false;
                signalPane.Margin.All = 0;
                signalPane.Title.IsVisible = false;

                signalPane.XAxis.IsVisible = false;
                signalPane.XAxis.Scale.IsVisible = false;
                signalPane.XAxis.Scale.MajorUnit = DateUnit.Second;
                signalPane.XAxis.Scale.LabelGap = 0;
                signalPane.XAxis.Scale.BaseTic = 0;
                signalPane.XAxis.Scale.MajorStep = 1.0;
                signalPane.XAxis.MajorTic.IsCrossOutside = false;
                signalPane.XAxis.MajorTic.Size = 10;
                signalPane.XAxis.Scale.LabelGap = 0;
                signalPane.XAxis.AxisGap = 0;
                signalPane.XAxis.MajorTic.IsAllTics = false;
                signalPane.XAxis.MajorTic.IsInside = true;
                signalPane.XAxis.MinorTic.IsAllTics = false;
                signalPane.XAxis.IsAxisSegmentVisible = true;
                signalPane.XAxis.Title.Text = "Time (seconds)";
                signalPane.XAxis.Title.Gap = 0;

                signalPane.YAxisList.Clear();
                var axisName = GetAxisName(ledFlag);
                signalPane.AddYAxis(axisName);
                signalPane.YAxis.Color = signalColor;
                signalPane.YAxis.MajorTic.Color = signalColor;
                signalPane.YAxis.MajorTic.IsAllTics = false;
                signalPane.YAxis.MajorTic.IsInside = true;
                signalPane.YAxis.MinorTic.IsAllTics = false;
                signalPane.YAxis.MinorTic.Color = signalColor;
                signalPane.YAxis.Scale.FontSpec.FontColor = signalColor;
                signalPane.YAxis.Scale.LabelGap = 0f;
                signalPane.YAxis.Title.FontSpec.FontColor = signalColor;
                signalPane.YAxis.Title.Gap = 0;

                var points = new PointPairList();
                var series = signalPane.AddCurve("Data", points, signalColor, SymbolType.None);
                series.Label.IsVisible = false;
                series.Line.IsAntiAlias = false;
                series.Line.IsOptimizedDraw = true;

                signalPane.Chart.Fill.Color = Color.FromArgb(64, signalColor);

                regionPlot.MasterPane.Add(signalPane);
            }
        }

        private Color GetColor(ushort flag)
        {
            switch (flag)
            {
                case (ushort)FrameFlags.None:
                    return Color.LightGray;

                case (ushort)FrameFlags.L415:
                    return Color.FromArgb(118, 0, 237);

                case (ushort)FrameFlags.L470:
                    return Color.FromArgb(0, 169, 255);

                case (ushort)FrameFlags.L560:
                    return Color.FromArgb(129, 255, 0);

                default:
                    return Color.White;
            }
        }

        private string GetAxisName(ushort ledFlag)
        {
            var name = "";
            if ((ledFlag & (ushort)FrameFlags.L415) > 0)
                name += FrameFlags.L415.ToString() + "/";
            if ((ledFlag & (ushort)FrameFlags.L470) > 0)
                name += FrameFlags.L470.ToString() + "/";
            if ((ledFlag & (ushort)FrameFlags.L560) > 0)
                name += FrameFlags.L560.ToString() + "/";

            name = name.Remove(name.Length - 1);
            return name;
        }

        private void TrySavePlots()
        {
            try
            {
                SavePlots();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void SavePlots()
        {
            foreach (var regionPlot in RegionPlots)
            {
                var photometryRegion = (PhotometryRegion)regionPlot.Tag;
                var fileName = SuffixHelper.AppendSuffix(PlotsDirectory + photometryRegion.Name + PlotsBaseFileName, Suffix);
                using (var g = regionPlot.CreateGraphics())
                {
                    regionPlot.MasterPane.ReSize(g, new RectangleF(0, 0, Width, Height));
                    regionPlot.MasterPane.SetLayout(g, PaneLayout.SingleColumn);

                    OrderPanes(regionPlot);
                    UpdateAxes(regionPlot);
                    using (var image = (Image)regionPlot.GetImage().Clone())
                        image.Save(fileName);
                }
            }
        }

        private void OrderPanes(ZedGraphControl regionPlot)
        {
            var reorderPanes = regionPlot.MasterPane.PaneList.OrderBy(signal => (FrameFlags)signal.Tag).ToList();
            regionPlot.MasterPane.PaneList.Clear();
            regionPlot.MasterPane.PaneList.AddRange(reorderPanes);

            var scaleFactor = regionPlot.MasterPane.CalcScaleFactor();
            var leftMargin = regionPlot.MasterPane.Margin.Left;
            var topMargin = regionPlot.MasterPane.Margin.Top;
            var titleHeight = regionPlot.MasterPane.Title.FontSpec.Size * scaleFactor + 5.0f;
            var height = Height - titleHeight - 2.0f * topMargin;
            var width = Width - 2.0f * leftMargin;
            var xAxisHeight = height * xAxisHeightPercent;
            var yAxisWidth = width * yAxisWidthPercent;

            var chartSize = new SizeF(width - yAxisWidth, (height - xAxisHeight) / reorderPanes.Count);
            var chartPosX = yAxisWidth + leftMargin;

            for (var i = 0; i < reorderPanes.Count; i++)
            {
                var isLastPane = i == reorderPanes.Count - 1;
                var pane = reorderPanes[i];
                pane.XAxis.IsVisible = isLastPane;
                pane.XAxis.IsAxisSegmentVisible = isLastPane;
                pane.XAxis.Title.IsVisible = isLastPane;
                pane.XAxis.Scale.IsVisible = isLastPane;

                var chartPoint = new PointF(chartPosX, chartSize.Height * i + topMargin + titleHeight);
                pane.Chart.Rect = new RectangleF(chartPoint, chartSize);

                var rectPoint = new PointF(leftMargin, chartSize.Height * i + topMargin + titleHeight);
                var rectSize = new SizeF(width, chartSize.Height);
                var rect = new RectangleF(rectPoint, rectSize);
                rect.Height += isLastPane ? xAxisHeight : 0;
                pane.Rect = rect;

                var paneScaleFactor = pane.CalcScaleFactor();
                pane.YAxis.MinorTic.Size = yAxisWidth * 0.05f / paneScaleFactor;
                pane.YAxis.MajorTic.Size = yAxisWidth * 0.1f / paneScaleFactor;
                pane.YAxis.Scale.LabelGap = 0.1f;
                pane.YAxis.Scale.FontSpec.Size = yAxisWidth * 0.5f / (paneScaleFactor * 6.0f);
                pane.YAxis.Title.Gap = 0.1f;
                pane.YAxis.Title.FontSpec.Size = yAxisWidth * 0.2f / paneScaleFactor;

                pane.XAxis.MinorTic.Size = xAxisHeight * 0.05f / paneScaleFactor;
                pane.XAxis.MajorTic.Size = xAxisHeight * 0.1f / paneScaleFactor;
                pane.XAxis.Scale.LabelGap = 0.1f;
                pane.XAxis.Scale.FontSpec.Size = xAxisHeight * 0.3f / paneScaleFactor;
                pane.XAxis.Title.Gap = 0.1f;
                pane.XAxis.Title.FontSpec.Size = xAxisHeight * 0.4f / paneScaleFactor;
            }
        }

        private void UpdateAxes(ZedGraphControl regionPlot)
        {
            var signalPanes = regionPlot.MasterPane.PaneList;
            var plotXMin = double.MaxValue;
            var plotXMax = double.MinValue;
            foreach (var pane in signalPanes)
            {
                pane.CurveList[0].GetRange(out var xMin, out var xMax, out var yMin, out var yMax, pane.IsIgnoreInitial, pane.IsBoundedRanges, pane);
                if (xMin < plotXMin) plotXMin = xMin;
                if (xMax > plotXMax) plotXMax = xMax;

                pane.YAxis.Scale.BaseTic = Math.Round(yMin, 6);
                pane.YAxis.Scale.MajorStep = Math.Round((yMax - yMin) / 5.0, 6);
                if (yMin == yMax)
                {
                    yMin = 0.0;
                    yMax = 1.0;
                }
                else
                {
                    var offset = (yMax - yMin) * 0.1;
                    yMin -= offset;
                    yMax += offset;
                    yMin = Math.Max(0, yMin);
                }
                pane.YAxis.Scale.Min = yMin;
                pane.YAxis.Scale.Max = yMax;
            }

            foreach (var scale in signalPanes.Select(pane => pane.XAxis.Scale))
            {
                scale.Min = plotXMin;
                scale.Max = plotXMax;
                scale.BaseTic = Math.Ceiling(plotXMin);
                scale.MajorStep = Math.Max(1, (int)(plotXMax - plotXMin) / 5);
            }
        }

        internal void TryWriteNewFrame()
        {
            try
            {
                WriteNewFrame();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void WriteNewFrame()
        {
            EnsureSignals(DataFrame);
            AddPoints(DataFrame);
        }

        private void AddPoints(PhotometryDataFrame dataFrame)
        {
            var ledFlag = (ushort)((ushort)dataFrame.Flags & 0x7);
            for (var i = 0; i < RegionPlots.Count; i++)
            {
                var activity = dataFrame.Activities[i];
                var regionPlot = RegionPlots[i];
                var filterSignalPanes = regionPlot.MasterPane.PaneList.Where(pane => (ushort)pane.Tag == ledFlag);
                if (!filterSignalPanes.Any()) continue;

                var signalPane = filterSignalPanes.First();
                var curveList = signalPane.CurveList[0];
                var newPoint = new PointPair(dataFrame.SystemTimestamp, activity.Value);
                curveList.AddPoint(newPoint);
            }
        }

        public void Dispose()
        {
            Saving = false;
        }
    }
}