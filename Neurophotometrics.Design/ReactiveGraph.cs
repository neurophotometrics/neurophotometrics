using System;
using ZedGraph;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace Neurophotometrics.Design
{
    partial class ReactiveGraph : ZedGraphControl
    {
        /***** DEBUG VARIABLES *****/
        private int layoutCount = 0;
        /****** PRIVATE GLOBAL VARIABLES TO BE PUSHED TO REACTIVE GRAPHS *****/
        private int roiNumber;
        private bool[] foundLEDs;
        private int capacity;
        private bool deinterleave;
        private bool[] roiVisible;
        private bool[] autoScales;
        private double[] mins;
        private double[] maxes;
        private GraphPane[] graphPanes;

        /***** PRIVATE CONSTANTS FOR FORMATTING *****/
        private const float YAxisMinSpace = 50;

        /***** DEFAULT CONSTRUCTOR ******/
        public ReactiveGraph()
        { }

        /***** CONSTRUCTOR WITH INITIALIZATION ******/
        public ReactiveGraph(Tuple<Tuple<int, bool[], int, bool>, Tuple<bool[], bool[], double[], double[]>> InitProps)
        {
            // Initialize ReactiveGraph variables
            roiNumber = InitProps.Item1.Item1;
            foundLEDs = InitProps.Item1.Item2;
            capacity = InitProps.Item1.Item3;
            deinterleave = InitProps.Item1.Item4;
            roiVisible = InitProps.Item2.Item1;
            autoScales = InitProps.Item2.Item2;
            mins = InitProps.Item2.Item3;
            maxes = InitProps.Item2.Item4;

            InitializeComponent();

            var points = new RollingPointPairList(Capacity);       // Initiallize points
            var series = graph.AddCurve(string.Empty, points, Color.Black, SymbolType.None);
            series.Label.IsVisible = false;
            series.Line.IsAntiAlias = false;
            series.Line.IsOptimizedDraw = true;

            graphPanes = new GraphPane[1] { graph };

            Refresh();
        }

        /****** PUBLIC GLOBAL VARIABLES *****/
        public virtual int ROINumber
        {
            get { return roiNumber; }
        }

        public virtual bool[] FoundLEDs
        {
            get { return foundLEDs; }
            set
            {
                foundLEDs = value;

                for (int i = 0; i < FoundLEDs.Length; i++)
                {
                    if (FoundLEDs[i])
                    {
                        AddAxis(i);
                    }

                }
            }
        }

        public virtual int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
            }
        }

        public virtual bool Deinterleave
        {
            get { return deinterleave; }
            set
            {
                deinterleave = value;
                EnsurePaneList();
            }
        }

        public virtual bool[] ROIVisible
        {
            get { return roiVisible; }
            set
            {
                roiVisible = value;
                EnsurePaneList();
            }

        }

        public virtual bool[] AutoScales
        {
            get { return autoScales; }
            set
            {
                autoScales = value;
            }
        }

        public virtual double[] Mins
        {
            get { return mins; }
            set
            {
                mins = value;
                foreach (GraphPane graph in GraphPanes)
                {
                    int index = 0;
                    string axis = (string)graph.Tag;
                    if (axis.Equals("a_Interleaved"))
                    {
                        index = 0;
                    }
                    else if (axis.Equals("b_L415"))
                    {
                        index = 1;
                    }
                    else if (axis.Equals("c_L470"))
                    {
                        index = 2;
                    }
                    else if (axis.Equals("d_L560"))
                    {
                        index = 3;
                    }
                    graph.YAxis.Scale.Min = mins[index];
                    Tuple<double, double> ticProps = FindTicProps(mins[index], maxes[index]);
                    graph.YAxis.Scale.BaseTic = ticProps.Item1;
                    graph.YAxis.Scale.MajorStep = ticProps.Item2;
                }
                Refresh();
            }
        }

        public virtual double[] Maxes
        {
            get { return maxes; }
            set
            {
                maxes = value;
                foreach (GraphPane graph in master.PaneList)
                {
                    int index = 0;
                    string axis = (string)graph.Tag;
                    if (axis.Equals("a_Interleaved"))
                    {
                        index = 0;
                    }
                    else if (axis.Equals("b_L415"))
                    {
                        index = 1;
                    }
                    else if (axis.Equals("c_L470"))
                    {
                        index = 2;
                    }
                    else if (axis.Equals("d_L560"))
                    {
                        index = 3;
                    }
                    graph.YAxis.Scale.Max = maxes[index];
                    Tuple<double, double> ticProps = FindTicProps(mins[index], maxes[index]);
                    graph.YAxis.Scale.BaseTic = ticProps.Item1;
                    graph.YAxis.Scale.MajorStep = ticProps.Item2;
                }
                Refresh();
            }
        }

        public virtual GraphPane[] GraphPanes
        {
            get { return graphPanes; }
            set
            {
                graphPanes = value;
            }
        }

        /***** ENSURES THE CAPACITY FOR EACH CURVE *****/
        public void EnsureCapacity()
        {
            foreach (GraphPane graphPane in GraphPanes)
            {
                var points = new RollingPointPairList(Capacity);
                points.Add(graphPane.CurveList[0].Points);
                graphPane.CurveList[0].Points = points;
            }
        }

        private Color GetColor(string LED)
        {
            if (LED.Equals("b_L415"))
            {
                return Color.Purple;
            }
            else if (LED.Equals("c_L470"))
            {
                return Color.Green;
            }
            else if (LED.Equals("d_L560"))
            {
                return Color.Red;
            }
            else
            {
                return Color.Black;
            }
        }

        private void AddAxis(int axisIndex)
        {
            string LED;
            string axisName;
            if (axisIndex == 0)
            {
                axisName = "L415";
                LED = "b_L415";
            }
            else if (axisIndex == 1)
            {
                axisName = "L470";
                LED = "c_L470";
            }
            else if (axisIndex == 2)
            {
                axisName = "L560";
                LED = "d_L560";
            }
            else
            {
                axisName = "";
                LED = "";
            }

            bool newLED = true;
            foreach (GraphPane graphPane in GraphPanes)
            {
                if (graphPane.Tag.Equals(LED))
                {
                    newLED = false;
                }
            }
            if (newLED && !LED.Equals(""))
            {
                GraphPane ledGraph = new GraphPane();
                ledGraph.Tag = LED;
                ledGraph.YAxisList.Clear();
                ledGraph.AddYAxis(axisName);
                Color color = GetColor(LED);
                ledGraph.YAxis.Color = color;
                ledGraph.YAxis.MinSpace = YAxisMinSpace;
                ledGraph.YAxis.Scale.FontSpec.FontColor = color;
                ledGraph.YAxis.Scale.FontSpec.Size = 80;
                ledGraph.YAxis.Title.FontSpec.FontColor = color;
                ledGraph.YAxis.Title.FontSpec.Size = 100;
                ledGraph.YAxis.Title.Gap = 0;
                ledGraph.YAxis.MinorTic.Color = color;
                ledGraph.YAxis.MajorTic.Color = color;
                ledGraph.YAxis.MajorGrid.IsZeroLine = false;
                ledGraph.YAxis.Scale.FontSpec.Size = 12;
                ledGraph.YAxis.Scale.LabelGap = 0;
                ledGraph.XAxis.IsVisible = true;
                ledGraph.XAxis.Scale.IsVisible = false;
                ledGraph.XAxis.Scale.FontSpec.Size = 36;
                ledGraph.XAxis.Scale.MajorUnit = DateUnit.Second;
                ledGraph.XAxis.Scale.LabelGap = 0;
                ledGraph.XAxis.MinorTic.IsAllTics = false;
                ledGraph.XAxis.MajorTic.IsAllTics = true;
                ledGraph.XAxis.MajorTic.IsInside = true;
                ledGraph.XAxis.MajorTic.IsOutside = false;
                ledGraph.XAxis.MajorTic.IsCrossOutside = false;
                ledGraph.XAxis.MajorTic.Size = 20;
                ledGraph.XAxis.AxisGap = 0;
                ledGraph.XAxis.IsAxisSegmentVisible = false;
                ledGraph.XAxis.Title.IsVisible = false;
                ledGraph.XAxis.Title.Gap = 0;
                ledGraph.XAxis.Title.Text = "Time (seconds)";
                ledGraph.XAxis.Title.FontSpec.Size = 36;
                ledGraph.Margin.Top = 0;
                ledGraph.Margin.Left = 0;
                ledGraph.Margin.Right = 10;
                ledGraph.Margin.Bottom = 0;
                ledGraph.TitleGap = 0;
                ledGraph.Title.IsVisible = false;
                ledGraph.Border.IsVisible = false;
                ledGraph.Chart.IsRectAuto = false;
                ledGraph.IsFontsScaled = false;

                var points = new RollingPointPairList(Capacity);
                var series = ledGraph.AddCurve(LED, points, color, SymbolType.None);

                series.Label.IsVisible = false;
                series.Line.IsAntiAlias = false;
                series.Line.IsOptimizedDraw = true;

                if (GraphPanes.Length == 0)
                {
                    GraphPanes = new GraphPane[1] { ledGraph };
                }
                else
                {

                    GraphPane[] tempGraphPanes = new GraphPane[GraphPanes.Length + 1];
                    int i = 0;
                    foreach (GraphPane graphPane in GraphPanes)
                    {
                        tempGraphPanes[i] = graphPane;
                        i++;
                    }
                    tempGraphPanes[GraphPanes.Length] = ledGraph;
                    GraphPanes = tempGraphPanes;
                }
            }
        }

        /*********** AXIS CHANGE EVENT CALLED BY ACTIVITY VISUALIZER AT 30HZ ***********/
        public void GraphPane_AxisChange()
        {
            double[] xMins = new double[Mins.Length];
            double[] xMaxes = new double[Mins.Length];
            double[] yMins = new double[Mins.Length];
            double[] yMaxes = new double[Mins.Length];

            bool minsChanged = false;
            bool maxesChanged = false;
            // Find the x and y mins and maxes of each graph
            foreach (GraphPane graphPane in GraphPanes)
            {
                string axis = (string)graphPane.Tag;
                int index = GetIndex(axis);
                graphPane.CurveList[0].GetRange(out xMins[index], out xMaxes[index], out yMins[index], out yMaxes[index], graphPane.IsIgnoreInitial, graphPane.IsBoundedRanges, graphPane);
            }

            double newXMin = double.MaxValue;
            double newXMax = double.MinValue;
            if (Deinterleave)
            {
                for (int i = 1; i < xMins.Length; i++)
                {
                    if (FoundLEDs[i - 1])
                    {
                        if (xMins[i] < newXMin)
                        {
                            newXMin = xMins[i];
                        }
                        if (xMaxes[i] > newXMax)
                        {
                            newXMax = xMaxes[i];
                        }
                    }
                }
            }
            else
            {
                newXMin = xMins[0];
                newXMax = xMaxes[0];
            }

            int j = 0;
            double[] newMins = Mins;
            double[] newMaxes = Maxes;
            foreach (GraphPane graphPane in GraphPanes)
            {
                if (graphPane.XAxis.Scale.Min != newXMin || graphPane.XAxis.Scale.Max != newXMax)
                {
                    graphPane.XAxis.Scale.Min = newXMin;
                    graphPane.XAxis.Scale.Max = newXMax;
                    graphPane.XAxis.Scale.MajorStep = Math.Round(4.0 * (newXMax - newXMin) / 5.0, MidpointRounding.ToEven) / 4.0;
                    graphPane.XAxis.Scale.BaseTic = 0;
                }

                string axis = (string)graphPane.Tag;
                int index = GetIndex(axis);

                if ((graphPane.YAxis.Scale.Min != yMins[index] || graphPane.YAxis.Scale.Max != yMaxes[index]) && AutoScales[index])
                {
                    graphPane.YAxis.Scale.Min = yMins[index];
                    graphPane.YAxis.Scale.Max = yMaxes[index];
                    Tuple<double, double> ticProps = FindTicProps(yMins[index], yMaxes[index]);
                    graphPane.YAxis.Scale.BaseTic = ticProps.Item1;
                    graphPane.YAxis.Scale.MajorStep = ticProps.Item2;
                    minsChanged = true;
                    maxesChanged = true;
                    newMins[index] = yMins[index];
                    newMaxes[index] = yMaxes[index];
                }

                j++;
            }




            if (minsChanged)
            {
                mins = newMins;
                Mins_Changed(this, EventArgs.Empty);
            }
            if (maxesChanged)
            {
                maxes = newMaxes;
                Maxes_Changed(this, EventArgs.Empty);
            }
        }

        private int GetIndex(string axis)
        {
            if (axis.Equals("a_Interleaved"))
            {
                return 0;
            }
            else if (axis.Equals("b_L415"))
            {
                return 1;
            }
            else if (axis.Equals("c_L470"))
            {
                return 2;
            }
            else if (axis.Equals("d_L560"))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        private Tuple<double, double> FindTicProps(double min, double max)
        {
            double diff = max - min;

            double power = 1.0;
            double val;
            bool lookingForPower = true;
            if (diff > 0)
            {
                while (lookingForPower)
                {
                    val = diff * Math.Pow(10.0, power);
                    if (val >= 1.0)
                    {
                        lookingForPower = false;
                    }
                    else
                    {
                        power += 1.0;
                    }
                }
            }

            double step = (int)(diff * Math.Pow(10.0, power)) / (Math.Pow(10.0, power) * 5.0);
            double baseTic = (int)(min * Math.Pow(10.0, power)) / (Math.Pow(10.0, power) * 5.0);

            return Tuple.Create(baseTic, step);
        }

        public void ColorBlindMode(bool inColorBlindMode)
        {
            if (inColorBlindMode)
            {
                foreach (GraphPane graphPane in GraphPanes)
                {
                    if (graphPane.Tag.Equals("b_L415"))
                    {
                        graphPane.YAxis.Color = Color.FromArgb(30, 136, 229);
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(30, 136, 229);
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.FromArgb(30, 136, 229);
                        graphPane.YAxis.MinorTic.Color = Color.FromArgb(30, 136, 229);
                        graphPane.YAxis.MajorTic.Color = Color.FromArgb(30, 136, 229);
                        graphPane.CurveList[0].Color = Color.FromArgb(30, 136, 229);
                    }
                    else if (graphPane.Tag.Equals("c_L470"))
                    {
                        graphPane.YAxis.Color = Color.FromArgb(255, 193, 7);
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(255, 193, 7);
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.FromArgb(255, 193, 7);
                        graphPane.YAxis.MinorTic.Color = Color.FromArgb(255, 193, 7);
                        graphPane.YAxis.MajorTic.Color = Color.FromArgb(255, 193, 7);
                        graphPane.CurveList[0].Color = Color.FromArgb(255, 193, 7);
                    }
                    else if (graphPane.Tag.Equals("d_L560"))
                    {
                        graphPane.YAxis.Color = Color.FromArgb(216, 27, 96);
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(216, 27, 96);
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.FromArgb(216, 27, 96);
                        graphPane.YAxis.MinorTic.Color = Color.FromArgb(216, 27, 96);
                        graphPane.YAxis.MajorTic.Color = Color.FromArgb(216, 27, 96);
                        graphPane.CurveList[0].Color = Color.FromArgb(216, 27, 96);
                    }
                }
            }
            else
            {
                foreach (GraphPane graphPane in GraphPanes)
                {
                    if (graphPane.Tag.Equals("b_L415"))
                    {
                        graphPane.YAxis.Color = Color.Purple;
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.Purple;
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.Purple;
                        graphPane.YAxis.MinorTic.Color = Color.Purple;
                        graphPane.YAxis.MajorTic.Color = Color.Purple;
                        graphPane.CurveList[0].Color = Color.Purple;
                    }
                    else if (graphPane.Tag.Equals("c_L470"))
                    {
                        graphPane.YAxis.Color = Color.Green;
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.Green;
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.Green;
                        graphPane.YAxis.MinorTic.Color = Color.Green;
                        graphPane.YAxis.MajorTic.Color = Color.Green;
                        graphPane.CurveList[0].Color = Color.Green;
                    }
                    else if (graphPane.Tag.Equals("d_L560"))
                    {
                        graphPane.YAxis.Color = Color.Red;
                        graphPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
                        graphPane.YAxis.Title.FontSpec.FontColor = Color.Red;
                        graphPane.YAxis.MinorTic.Color = Color.Red;
                        graphPane.YAxis.MajorTic.Color = Color.Red;
                        graphPane.CurveList[0].Color = Color.Red;
                    }
                }
            }
        }

        /***** EVENT HANDLERS FOR WHEN THE MINS/MAXES ARE CHANGED *****/
        public event EventHandler MinsChanged;

        public event EventHandler MaxesChanged;

        private void Mins_Changed(object sender, EventArgs e)
        {
            OnMinsChanged(e);
        }

        private void Maxes_Changed(object sender, EventArgs e)
        {
            OnMaxesChanged(e);
        }

        protected virtual void OnMinsChanged(EventArgs e)
        {
            var handler = MinsChanged;
            if (handler != null)
            {
                MinsChanged.Invoke(this, e);
            }

        }

        protected virtual void OnMaxesChanged(EventArgs e)
        {
            var handler = MaxesChanged;
            if (handler != null)
            {
                MaxesChanged.Invoke(this, e);
            }

        }

        /***** HANDLES RESIZING *****/
        private void EnsurePaneList()
        {
            master.PaneList.Clear();
            if (deinterleave)
            {
                for (int i = 1; i < ROIVisible.Length; i++)
                {
                    if (ROIVisible[i])
                    {
                        string tempTag = "";
                        if (i == 1)
                        {
                            tempTag = "b_L415";
                        }
                        else if (i == 2)
                        {
                            tempTag = "c_L470";
                        }
                        else if (i == 3)
                        {
                            tempTag = "d_L560";
                        }

                        foreach (GraphPane graphPane in GraphPanes)
                        {
                            if (graphPane.Tag.Equals(tempTag))
                            {
                                master.Add(graphPane);

                            }
                        }
                    }
                }
            }
            else
            {
                if (ROIVisible[0])
                {
                    foreach (GraphPane graphPane in GraphPanes)
                    {
                        if (graphPane.Tag.Equals("a_Interleaved"))
                        {
                            master.Add(graphPane);
                        }
                    }
                }
            }

            if (master.PaneList.Count > 0)
            {
                using (Graphics g = this.CreateGraphics())
                {
                    master.PaneList.Sort(new GraphPaneTagComparer());
                    master.SetLayout(g, PaneLayout.SingleColumn);

                }
            }
            EnsureLayout();
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (master != null)
            {
                EnsureLayout();
                Refresh();
            }
        }

        private void EnsureLayout()
        {

            double xAxisHeight = (double)Height * 0.1;
            int chartWidth = (int)((double)Width * 0.85);
            double chartGap = (double)Width * 0.01;
            if (master.PaneList.Count == 1)
            {
                master.PaneList[master.PaneList.Count - 1].XAxis.Scale.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].XAxis.Title.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].Margin.All = 0;
                master.PaneList[0].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[0].Rect.Y + (float)chartGap, chartWidth - (float)chartGap, master.PaneList[0].Rect.Height - (float)(xAxisHeight + chartGap));
                master.PaneList[0].Rect = new RectangleF(0, master.PaneList[0].Chart.Rect.Y, Width, master.PaneList[0].Chart.Rect.Height + (float)xAxisHeight);
                master.PaneList[0].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[0].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[0].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[0].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);

            }
            else if (master.PaneList.Count == 2)
            {
                master.PaneList[master.PaneList.Count - 1].XAxis.Scale.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].XAxis.Title.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].Margin.All = 0;
                master.PaneList[0].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[0].Rect.Y + (float)chartGap, chartWidth - (float)chartGap, master.PaneList[0].Rect.Height - (float)((xAxisHeight + chartGap) / 2.0));
                master.PaneList[1].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[1].Rect.Y - (float)(xAxisHeight / 2.0) + (float)(chartGap / 2.0), chartWidth - (float)chartGap, master.PaneList[1].Rect.Height - (float)((xAxisHeight + chartGap) / 2.0));
                master.PaneList[0].Rect = new RectangleF(0, master.PaneList[0].Chart.Rect.Y, Width, master.PaneList[0].Chart.Rect.Height);
                master.PaneList[1].Rect = new RectangleF(0, master.PaneList[1].Chart.Rect.Y, Width, master.PaneList[1].Chart.Rect.Height + (float)xAxisHeight);
                float smallScaleFactor = master.PaneList[0].CalcScaleFactor();
                float largeScaleFactor = master.PaneList[1].CalcScaleFactor();
                master.PaneList[0].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[1].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[1].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[1].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[1].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0) * (smallScaleFactor / (largeScaleFactor));
            }
            else if (master.PaneList.Count == 3)
            {
                master.PaneList[master.PaneList.Count - 1].XAxis.Scale.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].XAxis.Title.IsVisible = true;
                master.PaneList[master.PaneList.Count - 1].Margin.All = 0;
                master.PaneList[0].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[0].Rect.Y + (float)chartGap, chartWidth - (float)chartGap, master.PaneList[0].Rect.Height - (int)((xAxisHeight + chartGap) / 3.0));
                master.PaneList[1].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[1].Rect.Y - (float)(xAxisHeight / 3.0) + (float)(2.0 * chartGap / 3.0), chartWidth - (float)chartGap, master.PaneList[1].Rect.Height - (float)((xAxisHeight + chartGap) / 3.0));
                master.PaneList[2].Chart.Rect = new RectangleF(Width - chartWidth, master.PaneList[2].Rect.Y - (float)(2.0 * xAxisHeight / 3.0) + (float)(chartGap / 3.0), chartWidth - (float)chartGap, master.PaneList[2].Rect.Height - (float)((xAxisHeight + chartGap) / 3.0));
                master.PaneList[0].Rect = new RectangleF(0, master.PaneList[0].Chart.Rect.Y, Width, master.PaneList[0].Chart.Rect.Height);
                master.PaneList[1].Rect = new RectangleF(0, master.PaneList[1].Chart.Rect.Y, Width, master.PaneList[1].Chart.Rect.Height);
                master.PaneList[2].Rect = new RectangleF(0, master.PaneList[2].Chart.Rect.Y, Width, master.PaneList[2].Chart.Rect.Height + (float)xAxisHeight);
                float smallScaleFactor = master.PaneList[0].CalcScaleFactor();
                float largeScaleFactor = master.PaneList[2].CalcScaleFactor();
                master.PaneList[0].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[1].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[2].YAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[1].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[2].YAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[1].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0);
                master.PaneList[2].XAxis.Title.FontSpec.Size = (int)((float)(Width - chartWidth) / 4.0) * (smallScaleFactor / (largeScaleFactor));
                master.PaneList[0].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[1].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0);
                master.PaneList[2].XAxis.Scale.FontSpec.Size = (int)((float)(Width - chartWidth) / 8.0) * (smallScaleFactor / (largeScaleFactor));
            }
        }

    }

    class GraphPaneTagComparer : IComparer<GraphPane>
    {
        public int Compare(GraphPane A, GraphPane B)
        {
            string strA = (string)A.Tag;
            string strB = (string)B.Tag;
            char[] charsA = strA.ToCharArray();
            char[] charsB = strB.ToCharArray();
            char charA = charsA[0];
            char charB = charsB[0];

            if (charA < charB)
            {
                return 1;
            }
            else if (charA == charB)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}


