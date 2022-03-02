using System;
using System.Drawing;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class GraphListController : UserControl
    {
        /****** DEBUG VARIABLES ******/
        private int layoutCount = 0;

        /***** GLOBAL VARIABLES *****/
        private int roiCount;
        private bool[] foundLEDs;
        private int capacity;
        private bool[] deinterleaves;
        private bool[,] roisVisible;
        private bool[,] autoScales;
        private double[,] mins;
        private double[,] maxes;

        /***** PRIVATE CONSTANTS FOR FORMATTING *****/
        private const int MinGap = 10;
        private const float GraphAspect = 400.0F / 240.0F;
        private const int MinGraphWidth = 40;

        /***** PRIVATE VARIABLES *****/
        private ReactiveGraph[] reactiveGraphList;

        /***** DEFAULT CONSTRUCTOR ******/
        public GraphListController()
        {

        }

        /***** CONSTRUCTOR WITH INITIALIZATION ******/
        public GraphListController(Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> InitProps)
        {
            // Initialize GlobalStatusController variables
            roiCount = InitProps.Item1.Item1;
            foundLEDs = InitProps.Item1.Item2;
            capacity = InitProps.Item1.Item3;
            deinterleaves = InitProps.Item2.Item1;
            roisVisible = InitProps.Item3.Item1;
            autoScales = InitProps.Item3.Item2;
            mins = InitProps.Item3.Item3;
            maxes = InitProps.Item3.Item4;

            //WriteCurrentState(1);


            int DefaultROINumber = 0;
            bool[] DefaultROIFoundLEDs = FoundLEDs;
            int DefaultROICapacity = Capacity;
            bool DefaultROIDeinterleave = Deinterleaves[0];
            bool[] DefaultROIVisible = new bool[FoundLEDs.Length + 1];
            bool[] DefaultROIAutoScales = new bool[FoundLEDs.Length + 1];
            double[] DefaultROIMins = new double[FoundLEDs.Length + 1];
            double[] DefaultROIMaxes = new double[FoundLEDs.Length + 1];
            for (int i = 0; i < FoundLEDs.Length + 1; i++)
            {
                DefaultROIVisible[i] = ROIsVisible[0, i];
                DefaultROIAutoScales[i] = AutoScales[0, i];
                DefaultROIMins[i] = Mins[0, i];
                DefaultROIMaxes[i] = Maxes[0, i];
            }

            Tuple<int, bool[], int, bool> DefaultROIGlobals = Tuple.Create(DefaultROINumber, DefaultROIFoundLEDs, DefaultROICapacity, DefaultROIDeinterleave);
            Tuple<bool[], bool[], double[], double[]> DefaultROILocals = Tuple.Create(DefaultROIVisible, DefaultROIAutoScales, DefaultROIMins, DefaultROIMaxes);

            Tuple<Tuple<int, bool[], int, bool>, Tuple<bool[], bool[], double[], double[]>> ReactiveGraphInitProps = Tuple.Create(DefaultROIGlobals, DefaultROILocals);

            InitializeComponent(ReactiveGraphInitProps);
        }

        /****** REACTIVE GRAPHS WITHIN THE GRAPH LIST CONTROLLER *****/
        public ReactiveGraph[] ReactiveGraphList
        {
            get { return reactiveGraphList; }
            set
            {
                reactiveGraphList = value;
                for (int i = 0; i < ROICount; i++)
                {
                    reactiveGraphList[i] = new ReactiveGraph();
                }
            }
        }

        /****** PUBLIC GLOBAL VARIABLES TO BE PUSHED TO REACTIVE GRAPHS *****/
        public virtual int ROICount
        {
            get { return roiCount; }
            set
            {
                roiCount = value;

                // Update Graph List Controller Variables
                bool[] tempDeinterleaves = new bool[ROICount];
                bool[,] tempROIsVisible = new bool[ROICount, FoundLEDs.Length + 1];
                bool[,] tempAutoScales = new bool[ROICount, FoundLEDs.Length + 1];
                double[,] tempMins = new double[ROICount, FoundLEDs.Length + 1];
                double[,] tempMaxes = new double[ROICount, FoundLEDs.Length + 1];
                for (int i = 0; i < ROICount; i++)
                {
                    tempDeinterleaves[i] = false;
                    for (int j = 0; j < FoundLEDs.Length + 1; j++)
                    {

                        tempROIsVisible[i, j] = true;
                        tempAutoScales[i, j] = true;
                        tempMins[i, j] = 0.0;
                        tempMaxes[i, j] = 1.0;
                    }
                }
                deinterleaves = tempDeinterleaves;
                roisVisible = tempROIsVisible;
                autoScales = tempAutoScales;
                mins = tempMins;
                maxes = tempMaxes;

                //WriteCurrentState(1);

                AddReactiveGraphs();
            }
        }

        public virtual bool[] FoundLEDs
        {
            get { return foundLEDs; }
            set
            {
                foundLEDs = value;

                //bool[,] tempROIsVisible = new bool[ROICount, FoundLEDs.Length + 1];
                //bool[,] tempAutoScales = new bool[ROICount, FoundLEDs.Length + 1];
                //double[,] tempMins = new double[ROICount, FoundLEDs.Length + 1];
                //double[,] tempMaxes = new double[ROICount, FoundLEDs.Length + 1];

                //for (int i = 0; i < ROICount; i++)
                //{
                //    for (int j = 0; j < FoundLEDs.Length + 1; j++)
                //    {
                //        tempROIsVisible[i, j] = true;
                //        tempAutoScales[i, j] = true;
                //        tempMins[i, j] = 0.0;
                //        tempMaxes[i, j] = 1.0;
                //    }
                //}
                //roisVisible = tempROIsVisible;
                //autoScales = tempAutoScales;
                //mins = tempMins;
                //maxes = tempMaxes;

                //WriteCurrentState(1);
                for (int i = 0; i < ROICount; i++)
                {
                    ReactiveGraphList[i].FoundLEDs = FoundLEDs;
                }
                //UpdateReactiveGraphs();
            }
        }

        public virtual int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                for (int i = 0; i < ROICount; i++)
                {
                    ReactiveGraphList[i].Capacity = capacity;
                }
            }
        }

        public virtual bool[] Deinterleaves
        {
            get { return deinterleaves; }
            set
            {
                deinterleaves = value;
                for (int i = 0; i < ROICount; i++)
                {
                    ReactiveGraphList[i].Deinterleave = Deinterleaves[i];
                }
            }
        }

        public virtual bool[,] ROIsVisible
        {
            get { return roisVisible; }
            set
            {
                roisVisible = value;
                SuspendLayout();
                for (int i = 0; i < ROICount; i++)
                {
                    if (!ROIsVisible[i, 0])
                    {
                        ReactiveGraphList[i].SuspendLayout();
                        ReactiveGraphList[i].Hide();
                    }
                    else
                    {
                        ReactiveGraphList[i].SuspendLayout();
                        ReactiveGraphList[i].Show();
                    }

                    bool[] tempROIVisible = new bool[ROIsVisible.GetLength(1)];
                    for (int j = 0; j < ROIsVisible.GetLength(1); j++)
                    {
                        tempROIVisible[j] = ROIsVisible[i, j];
                    }

                    ReactiveGraphList[i].ROIVisible = tempROIVisible;
                }
                EnsureLayout();
            }
        }

        public virtual bool[,] AutoScales
        {
            get { return autoScales; }
            set
            {
                autoScales = value;
                for (int i = 0; i < AutoScales.GetLength(0); i++)
                {
                    bool[] tempAutoScales = new bool[AutoScales.GetLength(1)];
                    for (int j = 0; j < AutoScales.GetLength(1); j++)
                    {
                        tempAutoScales[j] = AutoScales[i, j];
                    }

                    ReactiveGraphList[i].AutoScales = tempAutoScales;
                }
            }
        }

        public virtual double[,] Mins
        {
            get { return mins; }
            set
            {
                mins = value;
                for (int i = 0; i < Mins.GetLength(0); i++)
                {
                    double[] tempMins = new double[Mins.GetLength(1)];
                    for (int j = 0; j < Mins.GetLength(1); j++)
                    {
                        tempMins[j] = Mins[i, j];
                    }

                    ReactiveGraphList[i].Mins = tempMins;
                }
            }
        }

        public virtual double[,] Maxes
        {
            get { return maxes; }
            set
            {
                maxes = value;
                for (int i = 0; i < Maxes.GetLength(0); i++)
                {
                    double[] tempMaxes = new double[Maxes.GetLength(1)];
                    for (int j = 0; j < Maxes.GetLength(1); j++)
                    {
                        tempMaxes[j] = Maxes[i, j];
                    }

                    ReactiveGraphList[i].Maxes = tempMaxes;
                }
            }
        }

        /***** REACTIVELY ADD REACTIVE GRAPHS WHEN ROICOUNT CHANGED *****/
        private void AddReactiveGraphs()
        {
            SuspendLayout();
            Controls.Clear();

            ReactiveGraphList = new ReactiveGraph[ROICount];


            int roiNum;
            bool[] roiFoundLEDs = FoundLEDs;
            int roiCap = Capacity;
            bool roiDeinterleave;
            bool[] roiVisible = new bool[FoundLEDs.Length + 1];
            bool[] roiAutoScales = new bool[FoundLEDs.Length + 1];
            double[] roiMins = new double[FoundLEDs.Length + 1];
            double[] roiMaxes = new double[FoundLEDs.Length + 1];
            for (int i = 0; i < ROICount; i++)
            {
                // Construct Initialization Tuple
                roiNum = i;
                roiDeinterleave = Deinterleaves[i];

                for (int j = 0; j < FoundLEDs.Length + 1; j++)
                {
                    roiVisible[j] = ROIsVisible[i, j];
                    roiAutoScales[j] = AutoScales[i, j];
                    roiMins[j] = Mins[i, j];
                    roiMaxes[j] = Maxes[i, j];
                }

                Tuple<int, bool[], int, bool> ROIGlobals = Tuple.Create(roiNum, roiFoundLEDs, roiCap, roiDeinterleave);
                Tuple<bool[], bool[], double[], double[]> ROILocals = Tuple.Create(roiVisible, roiAutoScales, roiMins, roiMaxes);

                Tuple<Tuple<int, bool[], int, bool>, Tuple<bool[], bool[], double[], double[]>> ReactiveGraphInitProps = Tuple.Create(ROIGlobals, ROILocals);

                ReactiveGraph graph = new ReactiveGraph(ReactiveGraphInitProps);
                graph.MinsChanged += ReactiveGraph_MinsChanged;
                graph.MaxesChanged += ReactiveGraph_MaxesChanged;
                graph.SuspendLayout();
                ReactiveGraphList[i] = graph;

            }

            EnsureLayout();
            Controls.AddRange(ReactiveGraphList);
            ResumeLayout();
        }

        /************ UPDATE THE LAYOUT ON RESIZE ***********/

        /***** ON RESIZE EVENT *****/
        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            EnsureLayout();
        }

        /***** FINDS THE OPTIMAL MATRIX TO CONTAIN REACTIVE GRAPHS GIVEN THE CURRENT ASPECT RATIO OF THE GRAPH LIST CONTROLLER *****/
        private Tuple<int, int> FindMatrix(float GraphListAspect)
        {
            int totVisableROIs = 0;
            for (int i = 0; i < ROICount; i++)
            {
                if (ROIsVisible[i, 0] == true)
                {
                    totVisableROIs += 1;
                }
            }

            int bestMatrix = 1;
            int Rows;
            int Cols;
            float minAspectDiff = float.MaxValue;
            int lastMatrix = (int)Math.Ceiling(Math.Sqrt((float)totVisableROIs));

            if (GraphListAspect >= 1.0)
            {
                for (int l = 1; l <= lastMatrix; l++)
                {
                    float actualGraphListAspect = (float)Math.Ceiling((float)totVisableROIs / (float)l) / (float)l * GraphAspect;
                    float aspectDiff = Math.Abs(GraphListAspect - actualGraphListAspect);

                    if (aspectDiff < minAspectDiff)
                    {
                        bestMatrix = l;
                        minAspectDiff = aspectDiff;
                    }
                }

                Cols = (int)Math.Ceiling((float)totVisableROIs / (float)bestMatrix);
                Rows = bestMatrix;
            }
            else
            {
                for (int l = 1; l <= lastMatrix; l++)
                {
                    float actualGraphListAspect = (float)l / (float)Math.Ceiling((float)totVisableROIs / (float)l) * GraphAspect;
                    float aspectDiff = Math.Abs(GraphListAspect - actualGraphListAspect);

                    if (aspectDiff < minAspectDiff)
                    {
                        bestMatrix = l;
                        minAspectDiff = aspectDiff;
                    }
                }
                Rows = (int)Math.Ceiling((float)totVisableROIs / (float)bestMatrix);
                Cols = bestMatrix;
            }

            return Tuple.Create(Rows, Cols);
        }

        /***** FINDS THE OPTIMAL HORIZONTAL AND VERTICAL GAPS AND REACTIVE GRAPH SIZES GIVEN THE REACTIVE GRAPH ASPECT RATIO, THE OPTIMAL MATRIX, AND CURRENT DIMENSIONS OF GRAPH LIST CONTROLLER *****/
        private Tuple<int, int, int, int> FindGraphAndGapDims(Tuple<int, int> Matrix)
        {
            int Gx = (int)((float)(Width - MinGap) / (float)Matrix.Item2 - (float)MinGap); ;
            int Gy = (int)((float)Gx / GraphAspect);
            int dx = MinGap;
            int dy = (int)((float)(Height - Matrix.Item1 * Gy) / (float)(Matrix.Item1 + 1));

            if (dy <= MinGap)
            {
                Gy = (int)((float)(Height - MinGap) / (float)Matrix.Item1 - (float)MinGap);
                Gx = (int)((float)Gy * GraphAspect);
                dy = MinGap;
                dx = (int)((float)(Width - Matrix.Item2 * Gx) / (float)(Matrix.Item2 + 1));
            }

            if (Gx < 54)
            {
                Gx = 54;
                Gy = (int)((float)Gx / GraphAspect);
            }

            return Tuple.Create(Gx, Gy, dx, dy);
        }

        /***** SETS THE POSITIONS OF THE REACTIVE GRAPHS BASED ON THE FOUND OPTIMAL GRAPH AND GAP DIMS *****/
        private void EnsureLayout()
        {

            float GraphListAspect = (float)Width / (float)Height;

            Tuple<int, int> Matrix = FindMatrix(GraphListAspect);
            Tuple<int, int, int, int> Dims = FindGraphAndGapDims(Matrix);

            // Now we have all of the information to put the plots in
            // Set the size and location of each graph controller

            int plots = 0;
            for (int i = 0; i < ROICount; i++)
            {
                if (ROIsVisible[i, 0] == true)
                {
                    int n = plots % Matrix.Item2;
                    int m = plots / Matrix.Item2;

                    int xPoint = n * Dims.Item1 + (n + 1) * Dims.Item3;
                    int yPoint = m * Dims.Item2 + (m + 1) * Dims.Item4;

                    if (ReactiveGraphList != null)
                    {
                        ReactiveGraphList[i].SuspendLayout();
                        ReactiveGraphList[i].Bounds = new Rectangle(xPoint, yPoint, Dims.Item1, Dims.Item2);
                    }

                    plots += 1;
                }
            }
        }

        private void ReactiveGraph_MinsChanged(object sender, EventArgs e)
        {
            ReactiveGraph graph = (ReactiveGraph)sender;
            int roiIndex;
            if (int.TryParse(graph.Name.Split('_')[1], out roiIndex))
            {
                double[,] tempMins = Mins;
                double[] localMins = graph.Mins;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempMins[roiIndex, i] = localMins[i];
                }
                mins = tempMins;
                Mins_Changed(this, EventArgs.Empty);
            }
        }

        private void ReactiveGraph_MaxesChanged(object sender, EventArgs e)
        {
            ReactiveGraph graph = (ReactiveGraph)sender;
            int roiIndex;
            if (int.TryParse(graph.Name.Split('_')[1], out roiIndex))
            {
                double[,] tempMaxes = Maxes;
                double[] localMaxes = graph.Maxes;
                for (int i = 0; i < FoundLEDs.Length + 1; i++)
                {
                    tempMaxes[roiIndex, i] = localMaxes[i];
                }
                maxes = tempMaxes;
            }
            Maxes_Changed(this, EventArgs.Empty);
        }

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

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            //ReactiveGraphList[0].LocalStatusController.DeinterleaveController.Label.Focus();
        }

        public void ColorBlindMode(bool inColorBlindMode)
        {
            foreach (ReactiveGraph reactiveGraph in ReactiveGraphList)
            {
                reactiveGraph.ColorBlindMode(inColorBlindMode);
            }
        }

    }
}
