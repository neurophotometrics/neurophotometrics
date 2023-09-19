using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZedGraph;

namespace Neurophotometrics.Design.V2.Visualizers
{
    public partial class ActivityView : UserControl
    {
        public event PlotSettingsEventHandler ScaleChanged;

        private const double TargetRefreshPeriodMillis = 25.0;
        private const double PlotAspect = 2.0;
        private const int MinCapacity = 10;

        private bool IsPlotQuantitySet;
        private readonly Stopwatch RefreshStopwatch;
        private readonly List<RegionPlotControl> RegionPlotControls;

        public VisualizerSettings StoredVisualizerSettings { get; set; }
        public VisualizerSettings VisualizerSettings { get; set; }

        public ActivityView()
        {
            InitializeComponent();
            RegionPlotControls = new List<RegionPlotControl>();
            VisualizerSettings = new VisualizerSettings();
            Dock = DockStyle.Fill;
            RefreshStopwatch = new Stopwatch();
            RefreshStopwatch.Start();
            Focus();
            CapacityValue_Text.LostFocus += CapacityValue_Text_LostFocus;
        }

        internal void TryUpdateNewFrame(ConcurrentQueue<VisualizerData> dataFrames)
        {
            try
            {
                UpdateNewFrame(dataFrames);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateNewFrame(ConcurrentQueue<VisualizerData> dataFrames)
        {
            while (true)
            {
                var isDataFrame = dataFrames.TryDequeue(out var dataFrame);
                if (!isDataFrame) break;

                EnsurePlotTable(dataFrame);
                AddPoints(dataFrame);
            }
            RefreshPlotTable();
        }

        private void EnsurePlotTable(VisualizerData dataFrame)
        {
            EnsurePlotQuantity(dataFrame);
            EnsureSignals(dataFrame);
        }

        private void AddPoints(VisualizerData dataFrame)
        {
            var ledFlag = (FrameFlags)(ushort)((ushort)dataFrame.Flags & 0x7);
            for (var i = 0; i < dataFrame.Activities.Length; i++)
            {
                var newPoint = new PointPair(dataFrame.SystemTimestamp, dataFrame.Activities[i].Value);
                RegionPlotControls[i].AddPoint(ledFlag, newPoint);
            }
        }

        private void RefreshPlotTable()
        {
            if (RefreshStopwatch.ElapsedMilliseconds >= TargetRefreshPeriodMillis)
            {
                var plots = Plots_TableLayoutPanel.Controls.OfType<RegionPlotControl>();
                if (!plots.Any()) return;

                foreach (var plot in plots)
                    plot.UpdateAxes();

                Plots_TableLayoutPanel.Refresh();
                RefreshStopwatch.Restart();
            }
        }

        private void EnsurePlotQuantity(VisualizerData dataFrame)
        {
            if (IsPlotQuantitySet)
                return;

            EnsurePlotSettingsQuantity(dataFrame.Activities);
            UpdateTableLayout();
            AddPlots();
            IsPlotQuantitySet = true;
        }

        private void EnsurePlotSettingsQuantity(RegionActivity[] activities)
        {
            while (StoredVisualizerSettings.PlotSettings.Count > activities.Length)
                StoredVisualizerSettings.PlotSettings.RemoveAt(StoredVisualizerSettings.PlotSettings.Count - 1);
            while (StoredVisualizerSettings.PlotSettings.Count < activities.Length)
                StoredVisualizerSettings.PlotSettings.Add(new PlotSetting());

            VisualizerSettings.Capacity = StoredVisualizerSettings.Capacity;
            CapacityValue_Label.Text = VisualizerSettings.Capacity.ToString();
            for (var i = 0; i < activities.Length; i++)
                VisualizerSettings.PlotSettings.Add(new PlotSetting(activities[i].Region, StoredVisualizerSettings.PlotSettings[i].IsVisible));
        }

        private void UpdateTableLayout()
        {
            var numVisiblePlots = VisualizerSettings.PlotSettings.Count(plotSetting => plotSetting.IsVisible);
            var tableAspect = (double)Plots_TableLayoutPanel.Width / Plots_TableLayoutPanel.Height;
            var minAspectDiff = double.MaxValue;
            var numRows = 1;
            var numColumns = 1;
            for (var i = 1; i <= numVisiblePlots; i++)
            {
                var rows = i;
                var cols = (int)Math.Ceiling((double)numVisiblePlots / rows);
                var matrixAspect = (double)rows / cols;
                var absAspectDiff = Math.Abs(matrixAspect * tableAspect - PlotAspect);
                if (absAspectDiff < minAspectDiff)
                {
                    numRows = rows;
                    numColumns = cols;
                    minAspectDiff = absAspectDiff;
                }
            }
            Plots_TableLayoutPanel.RowStyles.Clear();
            Plots_TableLayoutPanel.RowCount = numRows;
            for (var i = 0; i < Plots_TableLayoutPanel.RowCount; i++)
                Plots_TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0F / numRows));

            Plots_TableLayoutPanel.ColumnStyles.Clear();
            Plots_TableLayoutPanel.ColumnCount = numColumns;
            for (var i = 0; i < Plots_TableLayoutPanel.ColumnCount; i++)
                Plots_TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0F / numColumns));
        }

        private void AddPlots()
        {
            foreach (var plotSetting in VisualizerSettings.PlotSettings)
            {
                var plotControl = new RegionPlotControl(plotSetting, VisualizerSettings.Capacity);
                plotControl.ScaleChanged += PushEventsOfChildren;
                RegionPlotControls.Add(plotControl);

                if (!plotSetting.IsVisible) continue;

                Plots_TableLayoutPanel.Controls.Add(plotControl);
            }
        }

        private void PushEventsOfChildren(object sender, PlotSettingsEventArgs args)
        {
            if (ScaleChanged != null)
                ScaleChanged.Invoke(sender, args);
        }

        private void EnsureSignals(VisualizerData dataFrame)
        {
            if (dataFrame.Flags == FrameFlags.None || dataFrame.Flags == (FrameFlags.L415 | FrameFlags.L470 | FrameFlags.L560))
                return;
            var ledFlag = (FrameFlags)(ushort)((ushort)dataFrame.Flags & 0x7);

            for (var i = 0; i < RegionPlotControls.Count; i++)
            {
                var signalSettings = VisualizerSettings.PlotSettings[i].SignalSettings;
                if (!signalSettings.Exists(setting => setting.LEDFlag == ledFlag))
                {
                    SignalSetting newSignalSetting;
                    var storedSignalSetting = StoredVisualizerSettings.PlotSettings[i].SignalSettings.Where(setting => setting.LEDFlag == ledFlag);
                    if (storedSignalSetting.Any())
                    {
                        newSignalSetting = storedSignalSetting.First();
                        newSignalSetting.Scaling = new Scaling();
                    }
                    else
                    {
                        var channel = ((PhotometryRegion)RegionPlotControls[i].Tag).Channel;
                        newSignalSetting = new SignalSetting(ledFlag, channel);
                    }
                    signalSettings.Add(newSignalSetting);
                }

                var signalSetting = signalSettings.First(setting => setting.LEDFlag == ledFlag);
                var plot = RegionPlotControls[i];
                plot.EnsureSignal(signalSetting);
            }
        }

        private void ActivityView_Resize(object sender, EventArgs e)
        {
            var plots = Plots_TableLayoutPanel.Controls.OfType<RegionPlotControl>();
            if (!plots.Any()) return;

            UpdateTableLayout();

            foreach (var plot in plots)
            {
                plot.OrderPanes();
            }
        }

        private void StrayClick(object sender, EventArgs e)
        {
            Focus();
        }

        private void CapacityValue_Label_Click(object sender, EventArgs e)
        {
            TryEnableTextEdit();
        }

        private void TryEnableTextEdit()
        {
            try
            {
                EnableTextEdit();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void EnableTextEdit()
        {
            CapacityValue_Text.Text = Regex.Replace(CapacityValue_Label.Text, "[^.0-9]", "");
            CapacityValue_Text.Size = new Size(CapacityValue_Label.Size.Width * 2, CapacityValue_Label.Size.Height);
            Settings_TableLayoutPanel.Controls.Remove(CapacityValue_Label);
            Settings_TableLayoutPanel.Controls.Add(CapacityValue_Text, 1, 0);
            CapacityValue_Text.Enabled = true;
            CapacityValue_Text.Focus();
        }

        private void CapacityValue_Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            var keyChar = e.KeyChar;
            TryApplyKeyFilter(keyChar, e);
        }

        private void TryApplyKeyFilter(char keyChar, KeyPressEventArgs args)
        {
            try
            {
                ApplyKeyFilter(keyChar, args);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void ApplyKeyFilter(char keyChar, KeyPressEventArgs args)
        {
            if (keyChar == (char)Keys.Enter)
            {
                args.Handled = true;
                Focus();
                return;
            }
            var isValid = KeyFilteringHelpers.IsIntegerNumeric(keyChar);
            args.Handled = !isValid;
        }

        private void CapacityValue_Text_LostFocus(object sender, EventArgs e)
        {
            TryAcceptText();
        }

        private void TryAcceptText()
        {
            try
            {
                AcceptText();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void AcceptText()
        {
            UpdateCapacity(CapacityValue_Text.Text);
            ReplaceTextBoxWithLabel();
        }

        private void UpdateCapacity(string capacityText)
        {
            var capacity = Math.Max(MinCapacity, int.Parse(capacityText));

            VisualizerSettings.Capacity = capacity;

            foreach (var plot in RegionPlotControls)
            {
                plot.SetCapacity(capacity);
            }
        }

        private void ReplaceTextBoxWithLabel()
        {
            CapacityValue_Label.Text = VisualizerSettings.Capacity.ToString();
            Settings_TableLayoutPanel.Controls.Remove(CapacityValue_Text);
            Settings_TableLayoutPanel.Controls.Add(CapacityValue_Label, 1, 0);
            CapacityValue_Text.Enabled = false;
        }

        private void ConfigPlots_Button_Click(object sender, EventArgs e)
        {
            Focus();
            using (var plotSettingsForm = new PlotSettingsForm(VisualizerSettings.PlotSettings))
            {
                var plotSettingsToUI = Observable.FromEventPattern<PlotSettingsEventHandler, PlotSettingsEventArgs>(
                    handler => plotSettingsForm.PlotSettingsChanged += handler,
                    handler => plotSettingsForm.PlotSettingsChanged -= handler)
                    .Do(TryUpdateUIFromSettings);
                var uiToPlotSettings = Observable.FromEventPattern<PlotSettingsEventHandler, PlotSettingsEventArgs>(
                    handler => ScaleChanged += handler,
                    handler => ScaleChanged -= handler)
                    .Do(plotSettingsForm.TryUpdateScaling);
                var pipeline = Observable.Merge(plotSettingsToUI, uiToPlotSettings);
                using (var disposable = pipeline.Subscribe())
                {
                    plotSettingsForm.ShowDialog();
                }
            }
        }

        private void TryUpdateUIFromSettings(EventPattern<PlotSettingsEventArgs> obj)
        {
            try
            {
                UpdateUIFromSettings(obj);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateUIFromSettings(EventPattern<PlotSettingsEventArgs> obj)
        {
            var settingName = obj.EventArgs.SettingName;
            var settingValue = obj.EventArgs.SettingValue;

            var settingPath = settingName.Split('.');
            if (settingPath.Length < 3) return;

            if (settingPath[1].Contains("IsVisible"))
                UpdatePlot(settingPath[0], settingValue);
            else
                UpdateSignal(settingPath[0], settingPath[1], settingPath[2], settingValue);
        }

        private void UpdateSignal(string regionName, string signalName, string settingName, object settingValue)
        {
            var plots = RegionPlotControls.Where(plot => ((PhotometryRegion)plot.Tag).Name == regionName);
            if (!plots.Any())
                return;

            var regionPlot = plots.First();
            var plotSetting = VisualizerSettings.PlotSettings.First(setting => setting.PhotometryRegion.Name == regionName);
            var signalSetting = plotSetting.SignalSettings.First(signal => signal.LEDFlag.ToString() == signalName);

            if (settingName.Contains("IsVisible") && settingValue is bool isVisible)
            {
                signalSetting.IsVisible = isVisible;
                regionPlot.EnsureSignal(signalSetting);
                regionPlot.Refresh();
            }
            else if (settingName.Contains("Scaling") && settingValue is string scaling)
            {
                var isAutoScale = scaling.Contains("Auto");
                signalSetting.Scaling.Mode = isAutoScale ? AutoScalingMode.Auto : AutoScalingMode.Manual;
                regionPlot.UpdateAutoScale(signalName, isAutoScale);
            }
            else if (settingName.Contains("Min") && settingValue is string minStr)
            {
                var min = double.Parse(minStr);
                signalSetting.Scaling.Min = min;
                regionPlot.UpdateSignalMin(signalName, min);
            }
            else if (settingName.Contains("Max") && settingValue is string maxStr)
            {
                var max = double.Parse(maxStr);
                signalSetting.Scaling.Max = max;
                regionPlot.UpdateSignalMax(signalName, max);
            }
        }

        private void UpdatePlot(string regionName, object settingValue)
        {
            var plots = RegionPlotControls.Where(plot => ((PhotometryRegion)plot.Tag).Name == regionName);
            if (!plots.Any())
                return;

            var plotSettings = VisualizerSettings.PlotSettings.Where(setting => setting.PhotometryRegion.Name == regionName);
            if (plotSettings.Any() && settingValue is bool isVisible)
            {
                plotSettings.First().IsVisible = isVisible;
                RefreshPlots();
            }
        }

        private void RefreshPlots()
        {
            var displayedPlots = new List<RegionPlotControl>();
            foreach (var plotSetting in VisualizerSettings.PlotSettings)
            {
                if (!plotSetting.IsVisible) continue;

                var regionPlot = RegionPlotControls.Where(plot => (PhotometryRegion)plot.Tag == plotSetting.PhotometryRegion);
                if (!regionPlot.Any()) continue;

                displayedPlots.Add(regionPlot.First());
            }
            Plots_TableLayoutPanel.Controls.Clear();
            UpdateTableLayout();
            Plots_TableLayoutPanel.Controls.AddRange(displayedPlots.ToArray());
            Plots_TableLayoutPanel.Refresh();

            var plots = Plots_TableLayoutPanel.Controls.OfType<RegionPlotControl>();
            if (!plots.Any()) return;

            UpdateTableLayout();

            foreach (var plot in plots)
            {
                plot.OrderPanes();
            }
        }
    }

    public class RegionPlotControl : ZedGraphControl
    {
        public event PlotSettingsEventHandler ScaleChanged;

        public int Capacity { get; private set; }
        private readonly List<SignalPane> SignalPanes;

        public RegionPlotControl(PlotSetting setting, int capacity)
        {
            Capacity = capacity;
            SignalPanes = new List<SignalPane>();

            ConfigureControl(setting.PhotometryRegion);
            ConfigureMasterPane(setting.PhotometryRegion.Name);
        }

        private void ConfigureControl(PhotometryRegion photometryRegion)
        {
            IsShowContextMenu = false;
            IsEnableHPan = false;
            IsEnableHZoom = false;
            IsEnableVPan = false;
            IsEnableVZoom = false;
            IsEnableWheelZoom = false;
            IsEnableZoom = false;
            IsShowCursorValues = false;
            Dock = DockStyle.Fill;
            Margin = new Padding(10);
            Padding = new Padding(0);
            Tag = photometryRegion;
        }

        private void ConfigureMasterPane(string regionName)
        {
            MasterPane.PaneList.Clear();

            MasterPane.Border.IsVisible = true;
            MasterPane.InnerPaneGap = 0;
            MasterPane.IsFontsScaled = true;
            MasterPane.Margin.All = 2.5f;
            MasterPane.Title.IsVisible = true;
            MasterPane.Title.Text = regionName;
            MasterPane.Title.FontSpec.Size = 18;
            MasterPane.IsFontsScaled = false;
            MasterPane.TitleGap = 0.0f;
        }

        internal void EnsureSignal(SignalSetting signalSetting)
        {
            if (!SignalPanes.Exists(signalPane => (FrameFlags)signalPane.Tag == signalSetting.LEDFlag))
            {
                var newPane = new SignalPane((PhotometryRegion)Tag, signalSetting, Capacity);
                newPane.ScaleChanged += PushEventsOfChildren;
                SignalPanes.Add(newPane);
            }

            if (signalSetting.IsVisible)
                AddSignal(signalSetting.LEDFlag);
            else
                RemoveSignal(signalSetting.LEDFlag);
        }

        private void AddSignal(FrameFlags ledFlag)
        {
            if (MasterPane.PaneList.Exists(graphPane => (FrameFlags)graphPane.Tag == ledFlag)) return;

            if (!SignalPanes.Exists(signalPane => (FrameFlags)signalPane.Tag == ledFlag)) return;
            var addingPane = SignalPanes.First(signalPane => (FrameFlags)signalPane.Tag == ledFlag);
            MasterPane.PaneList.Add(addingPane);

            OrderPanes();
        }

        private void RemoveSignal(FrameFlags ledFlag)
        {
            if (!MasterPane.PaneList.Exists(graphPane => (FrameFlags)graphPane.Tag == ledFlag)) return;

            var removingPane = MasterPane.PaneList.First(graphPane => (FrameFlags)graphPane.Tag == ledFlag);
            MasterPane.PaneList.Remove(removingPane);

            OrderPanes();
        }

        internal void OrderPanes()
        {
            var reorderPanes = MasterPane.PaneList.OrderBy(signal => (FrameFlags)signal.Tag).ToList();
            MasterPane.PaneList.Clear();
            MasterPane.PaneList.AddRange(reorderPanes);
            using (var g = CreateGraphics())
                MasterPane.SetLayout(g, PaneLayout.SingleColumn);

            var leftMargin = MasterPane.Margin.Left;
            var topMargin = MasterPane.Margin.Top;
            var titleHeight = MasterPane.Title.FontSpec.Size + 5.0f;
            var height = Height - titleHeight - 2.0f * topMargin;
            var width = Width - 2.0f * leftMargin;
            var xAxisHeight = 40f;
            var yAxisWidth = 100f;

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
            }
        }

        internal void AddPoint(FrameFlags ledFlag, PointPair newPoint)
        {
            if (!SignalPanes.Exists(pane => (FrameFlags)pane.Tag == ledFlag)) return;

            var signalPane = SignalPanes.First(pane => (FrameFlags)pane.Tag == ledFlag);
            var curveList = signalPane.CurveList[0];
            curveList.AddPoint(newPoint);
            foreach (var curve in SignalPanes.Select(pane => pane.CurveList[0]))
            {
                while (curve.Points.Count > Capacity)
                    curve.RemovePoint(0);
            }
        }

        internal void UpdateAxes()
        {
            var plotXMin = double.MaxValue;
            var plotXMax = double.MinValue;
            foreach (var pane in SignalPanes)
            {
                if (!MasterPane.PaneList.Contains(pane)) continue;
                pane.CurveList[0].GetRange(out var xMin, out var xMax, out var yMin, out var yMax, pane.IsIgnoreInitial, pane.IsBoundedRanges, pane);
                if (xMin < plotXMin) plotXMin = xMin;
                if (xMax > plotXMax) plotXMax = xMax;

                if (!pane.IsAutoScale)
                    continue;

                pane.SetScale(yMin, yMax);
            }

            foreach (var scale in SignalPanes.Select(pane => pane.XAxis.Scale))
            {
                scale.Min = plotXMin;
                scale.Max = plotXMax;
                scale.BaseTic = Math.Ceiling(plotXMin);
                scale.MajorStep = Math.Max(1, (int)(plotXMax - plotXMin) / 5);
            }
        }

        internal void SetCapacity(int capacity)
        {
            Capacity = capacity;
            foreach (var signal in SignalPanes)
            {
                signal.SetCapacity(capacity);
            }
        }

        internal void UpdateAutoScale(string led, bool isAutoScale)
        {
            var filteredSignalPanes = SignalPanes.Where(pane => ((FrameFlags)pane.Tag).ToString() == led);
            if (!filteredSignalPanes.Any()) return;
            var signalPane = filteredSignalPanes.First();
            signalPane.UpdateAutoScale(isAutoScale);
        }

        internal void UpdateSignalMin(string led, double min)
        {
            var filteredSignalPanes = SignalPanes.Where(pane => ((FrameFlags)pane.Tag).ToString() == led);
            if (!filteredSignalPanes.Any()) return;
            var signalPane = filteredSignalPanes.First();
            signalPane.UpdateMin(min);
        }

        internal void UpdateSignalMax(string led, double max)
        {
            var filteredSignalPanes = SignalPanes.Where(pane => ((FrameFlags)pane.Tag).ToString() == led);
            if (!filteredSignalPanes.Any()) return;
            var signalPane = filteredSignalPanes.First();
            signalPane.UpdateMax(max);
        }

        private void PushEventsOfChildren(object sender, PlotSettingsEventArgs args)
        {
            if (ScaleChanged != null)
                ScaleChanged.Invoke(sender, args);
        }
    }

    public class SignalPane : GraphPane
    {
        public event PlotSettingsEventHandler ScaleChanged;

        public PhotometryRegion PhotometryRegion { get; private set; }
        public FrameFlags LEDFlag { get; private set; }
        public bool IsAutoScale { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }

        private readonly Color SignalColor;

        public SignalPane(PhotometryRegion photometryRegion, SignalSetting signalSetting, int capacity)
        {
            PhotometryRegion = photometryRegion;
            LEDFlag = signalSetting.LEDFlag;
            Tag = signalSetting.LEDFlag;
            SignalColor = TriggerSequenceConverter.GetColor(signalSetting.LEDFlag);
            IsAutoScale = true;

            Min = signalSetting.Scaling.Min;
            Max = signalSetting.Scaling.Max;

            Border.IsVisible = true;
            Chart.IsRectAuto = false;
            Chart.Border.IsVisible = true;
            IsFontsScaled = false;
            Legend.IsVisible = false;
            Margin.All = 0;
            Title.IsVisible = false;

            XAxis.IsVisible = false;
            XAxis.Scale.IsVisible = false;
            XAxis.Scale.MajorUnit = DateUnit.Second;
            XAxis.Scale.LabelGap = 0;
            XAxis.Scale.BaseTic = 0;
            XAxis.Scale.MajorStep = 1.0;
            XAxis.MajorTic.IsCrossOutside = false;
            XAxis.MajorTic.Size = 5.0f;
            XAxis.MinorTic.Size = 2.5f;
            XAxis.Scale.LabelGap = 0.1f;
            XAxis.Scale.FontSpec.Size = 12f;

            XAxis.AxisGap = 0;
            XAxis.MajorTic.IsAllTics = false;
            XAxis.MajorTic.IsInside = true;
            XAxis.MinorTic.IsAllTics = false;
            XAxis.IsAxisSegmentVisible = true;
            XAxis.Title.Text = "Time (seconds)";
            XAxis.Title.Gap = 0.1f;
            XAxis.Title.FontSpec.Size = 16f;

            YAxisList.Clear();
            AddYAxis(signalSetting.LEDFlag.ToString());
            YAxis.Color = SignalColor;
            YAxis.MajorTic.IsAllTics = false;
            YAxis.MajorTic.IsInside = true;
            YAxis.MajorTic.Size = 5f;
            YAxis.MinorTic.IsAllTics = false;
            YAxis.MinorTic.Size = 2.5f;
            YAxis.Scale.FontSpec.Size = 12f;
            YAxis.Scale.LabelGap = 0.1f;
            YAxis.Title.FontSpec.FontColor = SignalColor;
            YAxis.Title.FontSpec.Size = 16f;
            YAxis.Title.Gap = 0.1f;

            var points = new RollingPointPairList(capacity);
            var series = AddCurve("Data", points, SignalColor, SymbolType.None);
            series.Label.IsVisible = false;
            series.Line.IsAntiAlias = false;
            series.Line.IsOptimizedDraw = true;

            Chart.Fill.Color = Color.FromArgb(64, SignalColor);
        }

        internal void SetCapacity(int capacity)
        {
            var oldPoints = CurveList[0].Points;
            var newPoints = new RollingPointPairList(capacity);

            for (var i = 0; i < oldPoints.Count; i++)
            {
                if (newPoints.Count == capacity)
                    newPoints.RemoveAt(0);
                newPoints.Add(oldPoints[i]);
            }

            CurveList.RemoveAt(0);
            var series = AddCurve("Data", newPoints, SignalColor, SymbolType.None);
            series.Label.IsVisible = false;
            series.Line.IsAntiAlias = false;
            series.Line.IsOptimizedDraw = true;
        }

        internal void SetScale(double yMin, double yMax)
        {
            var range = yMax - yMin;
            var precision = 1;
            while (range > 0 && Math.Pow(10, precision) * range < 1)
                precision++;

            YAxis.Scale.BaseTic = Math.Round(yMin, precision + 1);
            YAxis.Scale.MajorStep = Math.Round(range / 5.0, precision + 1);
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
            Min = yMin;
            Max = yMax;
            YAxis.Scale.Min = yMin;
            YAxis.Scale.Max = yMax;

            if (IsAutoScale)
            {
                ScaleChanged?.Invoke(this, new PlotSettingsEventArgs("Min", Min));
                ScaleChanged?.Invoke(this, new PlotSettingsEventArgs("Max", Max));
            }
        }

        internal void UpdateAutoScale(bool isAutoScale)
        {
            IsAutoScale = isAutoScale;
        }

        internal void UpdateMax(double max)
        {
            SetScale(Min, max);
        }

        internal void UpdateMin(double min)
        {
            SetScale(min, Max);
        }
    }
}