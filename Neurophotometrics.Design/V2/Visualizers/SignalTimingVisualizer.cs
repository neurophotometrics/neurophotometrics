using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace Neurophotometrics.Design.V2.Visualizers
{
    public partial class SignalTimingVisualizer : UserControl
    {
        private const int DefaultTargetFrames = 9;

        private FrameFlags[] TriggerSequence;
        private PulseTrain PulseTrain = new PulseTrain();
        public ushort TriggerPeriod { get; private set; }

        public SignalTimingVisualizer()
        {
            InitializeComponent();

            Plot_ZedGraphControl.MasterPane.Tag = "Master";
            Plot_ZedGraphControl.MasterPane.Title.IsVisible = true;
            Plot_ZedGraphControl.MasterPane.Title.Text = "Excitation Sequence";
            Plot_ZedGraphControl.MasterPane.Title.FontSpec.Size = 18;
            Plot_ZedGraphControl.MasterPane.TitleGap = 0.0f;
            Plot_ZedGraphControl.MasterPane.InnerPaneGap = 0;
            Plot_ZedGraphControl.MasterPane.Margin.All = 2.5f;
            Plot_ZedGraphControl.MasterPane.Border.IsVisible = true;
            Plot_ZedGraphControl.MasterPane.IsFontsScaled = true;
            Plot_ZedGraphControl.MasterPane.PaneList.Clear();
        }

        internal void TryUpdateSignals(RegSettingChangedEventArgs arg)
        {
            try
            {
                UpdateSignals(arg);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateSignals(RegSettingChangedEventArgs arg)
        {
            if (arg.Name == nameof(TriggerSequence))
                TriggerSequence = (FrameFlags[])arg.Value;
            else if (arg.Name == nameof(PulseTrain))
                PulseTrain = (PulseTrain)arg.Value;
            else if (arg.Name == nameof(PulseTrain.LaserWavelength))
                PulseTrain.LaserWavelength = (ushort)arg.Value;
            else if (arg.Name == nameof(PulseTrain.StimPeriod))
                PulseTrain.StimPeriod = (ushort)arg.Value;
            else if (arg.Name == nameof(PulseTrain.StimOn))
                PulseTrain.StimOn = (ushort)arg.Value;
            else if (arg.Name == nameof(PulseTrain.StimReps))
                PulseTrain.StimReps = (ushort)arg.Value;
            else if (arg.Name == nameof(TriggerPeriod))
                TriggerPeriod = (ushort)arg.Value;
            else
                return;
            RefreshPlot();
        }

        private void RefreshPlot()
        {
            if (TriggerSequence == null)
                return;
            var tagColors = GetTagColors();
            EnsureNumSignals(tagColors);
            UpdatePoints(tagColors);
            EnsureSizing();
            Plot_ZedGraphControl.Refresh();
        }

        private List<TagColor> GetTagColors()
        {
            // Get tags and colors
            var uniqueFlags = TriggerSequence.Distinct().OrderBy(flag => (ushort)flag);
            var flags = uniqueFlags.Select(flag => (ushort)flag).ToList();
            var tags = uniqueFlags.Select(flag => flag.ToString()).ToList();
            var colors = uniqueFlags.Select(flag => TriggerSequenceConverter.GetColor(flag)).ToList();
            if (PulseTrain != null && PulseTrain.LaserWavelength != 0)
            {
                tags.Add(LaserWavelengthConverter.ConvertWavelengthToText(PulseTrain.LaserWavelength));
                colors.Add(LaserWavelengthConverter.GetColor(PulseTrain.LaserWavelength));
                flags.Add(PulseTrain.LaserWavelength);
            }

            return tags.Zip(colors, (tag, color) => new TagColor { Tag = tag, Color = color })
                       .Zip(flags, (tagColor, flag) => new TagColor { Tag = tagColor.Tag, Color = tagColor.Color, Flag = flag }).ToList();
        }

        private void EnsureNumSignals(List<TagColor> tagColors)
        {
            // Add signals
            for (var i = 0; i < tagColors.Count; i++)
            {
                if (!Plot_ZedGraphControl.MasterPane.PaneList.Select(graphPane => (string)graphPane.Tag).Contains(tagColors[i].Tag))
                    Plot_ZedGraphControl.MasterPane.PaneList.Insert(i, new SignalPane(tagColors[i].Tag, tagColors[i].Color, i == tagColors.Count - 1));
            }

            // Remove signals
            var tagsToRemove = Plot_ZedGraphControl.MasterPane.PaneList.Select(graphPane => (string)graphPane.Tag)
                                                                       .Except(tagColors.Select(tagColor => tagColor.Tag)).ToList();

            Plot_ZedGraphControl.MasterPane.PaneList.RemoveAll(graphPane => tagsToRemove.Contains((string)graphPane.Tag));

            using (var g = CreateGraphics())
                Plot_ZedGraphControl.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
        }

        private void UpdatePoints(List<TagColor> tagColors)
        {
            // Trigger Period must be known
            if (TriggerPeriod <= 0)
                return;

            // Get number of Trigger Sequence Cycles
            var targetFrame = DefaultTargetFrames;
            var numCycles = targetFrame / TriggerSequence.Length;
            numCycles = numCycles == 0 ? 1 : numCycles;

            // Clear all curves and add a point at the origin
            foreach (var curve in Plot_ZedGraphControl.MasterPane.PaneList.Select(pane => pane.CurveList[0]))
            {
                curve.Clear();
                curve.AddPoint(new PointPair(0.0, 0.0));
            }

            // Add points to LED Curves
            var XAxisMax = 0.0;
            for (var i = 0; i < numCycles; i++)
            {
                for (var j = 0; j < TriggerSequence.Length; j++)
                {
                    var tag = tagColors.Find(tagColor => tagColor.Flag == (ushort)TriggerSequence[j]).Tag;
                    var curve = Plot_ZedGraphControl.MasterPane.PaneList.Find(graphPane => (string)graphPane.Tag == tag).CurveList[0];
                    curve.AddPoint(new PointPair((i * TriggerSequence.Length + j) * TriggerPeriod / 1000.0, 0));
                    curve.AddPoint(new PointPair((i * TriggerSequence.Length + j) * TriggerPeriod / 1000.0, 1));
                    curve.AddPoint(new PointPair((i * TriggerSequence.Length + j + 0.8) * TriggerPeriod / 1000.0, 1));
                    curve.AddPoint(new PointPair((i * TriggerSequence.Length + j + 0.8) * TriggerPeriod / 1000.0, 0));
                    XAxisMax = (i * TriggerSequence.Length + j + 1.5) * TriggerPeriod / 1000.0;
                }
            }

            // If there is a laser curve
            var laserFlags = tagColors.Select(tagColor => tagColor.Flag).Intersect(new List<ushort>() { 450, 635 });
            if (laserFlags.Any())
            {
                // Add a pulse train until the end of the displayed trigger sequence cycles
                var tag = tagColors.Find(tagColor => tagColor.Flag == laserFlags.First()).Tag;
                var curve = Plot_ZedGraphControl.MasterPane.PaneList.Find(graphPane => (string)graphPane.Tag == tag).CurveList[0];
                var startOfPulseTrain = 0.0;

                // Add reps of a single pulse train
                for (var i = 0; i < PulseTrain.StimReps; i++)
                {
                    var xPulseStart = i * PulseTrain.StimPeriod + startOfPulseTrain;
                    curve.AddPoint(new PointPair(xPulseStart, 0));
                    curve.AddPoint(new PointPair(xPulseStart, 1));
                    curve.AddPoint(new PointPair(xPulseStart + PulseTrain.StimOn, 1));
                    curve.AddPoint(new PointPair(xPulseStart + PulseTrain.StimOn, 0));

                    if (xPulseStart + PulseTrain.StimPeriod >= XAxisMax)
                        break;
                }
            }

            // Update the XAxis.Scale settings and add a point at the end of the trigger sequence cycles for each curve
            var majorStep = Math.Round(TriggerPeriod / 1000.0, 1);
            foreach (var graphPane in Plot_ZedGraphControl.MasterPane.PaneList)
            {
                graphPane.XAxis.Scale.MajorStep = majorStep;
                graphPane.XAxis.Scale.Max = XAxisMax;
                graphPane.CurveList[0].AddPoint(new PointPair(XAxisMax, 0.0));
            }
        }

        private void Plot_ZedGraphControl_Resize(object sender, EventArgs e)
        {
            try
            {
                EnsureSizing();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void EnsureSizing()
        {
            if (Plot_ZedGraphControl.Width <= 1 || Plot_ZedGraphControl.Height <= 1) return;

            var numSignals = Plot_ZedGraphControl.MasterPane.PaneList.Count;
            var scaleFactor = Plot_ZedGraphControl.MasterPane.CalcScaleFactor();
            var margin = Plot_ZedGraphControl.MasterPane.Margin.Left;
            var titleHeight = Plot_ZedGraphControl.MasterPane.Title.FontSpec.Size * scaleFactor + 5.0f;
            var height = Plot_ZedGraphControl.Height - titleHeight - 2.0f * margin;
            var width = Plot_ZedGraphControl.Width - 2.0f * margin;
            var xAxisHeight = height * 0.1f;
            var yAxisWidth = width * 0.1f;

            var chartSize = new SizeF(width - yAxisWidth, (height - xAxisHeight) / numSignals);
            var chartPosX = yAxisWidth + margin;

            for (var i = 0; i < numSignals; i++)
            {
                var graph = Plot_ZedGraphControl.MasterPane.PaneList[i];

                var chartPoint = new PointF(chartPosX, chartSize.Height * i + margin + titleHeight);
                graph.Chart.Rect = new RectangleF(chartPoint, chartSize);

                var rectPoint = new PointF(margin, chartSize.Height * i + margin + titleHeight);
                var rectSize = new SizeF(width, chartSize.Height);
                var rect = new RectangleF(rectPoint, rectSize);
                rect.Height += i == numSignals - 1 ? xAxisHeight : 0;
                graph.Rect = rect;

                var paneScaleFactor = graph.CalcScaleFactor();
                graph.YAxis.Title.Gap = 0.1f;
                graph.YAxis.Title.FontSpec.Size = yAxisWidth * 0.2f / paneScaleFactor;

                graph.XAxis.MajorTic.Size = xAxisHeight * 0.1f / paneScaleFactor;
                graph.XAxis.Scale.LabelGap = 0.1f;
                graph.XAxis.Scale.FontSpec.Size = xAxisHeight * 0.3f / paneScaleFactor;
                graph.XAxis.Title.Gap = 0.1f;
                graph.XAxis.Title.FontSpec.Size = xAxisHeight * 0.4f / paneScaleFactor;
            }
        }

        private struct TagColor
        {
            public string Tag { get; set; }
            public Color Color { get; set; }
            public ushort Flag { get; set; }
        }

        private sealed class SignalPane : GraphPane
        {
            public SignalPane(string tag, Color color, bool isLastGraph)
            {
                Tag = tag;
                Title.IsVisible = false;
                YAxisList.Clear();
                AddYAxis(tag);
                Legend.IsVisible = false;
                Border.IsVisible = false;
                Chart.Border.IsVisible = false;
                Margin.All = 0;

                // X-Axis
                XAxis.IsVisible = isLastGraph;
                XAxis.IsAxisSegmentVisible = isLastGraph;

                XAxis.Title.IsVisible = isLastGraph;
                XAxis.Title.Text = "Time (ms)";
                XAxis.Title.Gap = 0;

                XAxis.MinorTic.IsAllTics = false;

                XAxis.MajorTic.IsAllTics = false;
                XAxis.MajorTic.IsOutside = isLastGraph;
                XAxis.MajorTic.Size = 10;

                XAxis.Scale.IsVisible = isLastGraph;
                XAxis.Scale.MajorUnit = DateUnit.Millisecond;
                XAxis.Scale.LabelGap = 0;
                XAxis.Scale.BaseTic = 0;
                XAxis.Scale.Min = -1;

                // Y-Axis
                YAxis.MinorTic.IsAllTics = false;
                YAxis.MajorTic.IsAllTics = false;
                YAxis.Scale.IsVisible = false;
                YAxis.Title.FontSpec.FontColor = color;
                YAxis.Title.FontSpec.Angle = 90;
                YAxis.Title.Gap = 0;
                YAxis.MajorGrid.IsZeroLine = false;
                YAxis.Scale.Min = -0.05;
                YAxis.Scale.Max = 1.05;

                var curve = AddCurve(tag, new PointPairList(), color, SymbolType.None);
                curve.Line.Fill = new Fill(color);
                Chart.IsRectAuto = true;
                IsFontsScaled = true;
            }
        }
    }
}