using System;
using System.Windows.Forms;
using System.Drawing;

namespace Neurophotometrics.Design
{
    partial class MasterController : Panel
    {
        /***** DEBUG VARIABLES *****/
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
        private bool inConfigMode;
        private bool inColorBlindMode;
        private int prevTotLEDsFounds = 0;


        /********** INITIALIZATION VARIABLES FOR VARIABLES PUSHED TO CONTROLLERS **********/

        /***** INITIALIZATION CONSTANTS FOR VARIABLES SHARED BY ALL ROIS *****/
        private static int DefaultROICount = 1;
        private static bool[] DefaultFoundLEDs = new bool[3] { false, false, false };
        private static int DefaultCapacity = 300;

        private Tuple<int, bool[], int> DefaultMasterGlobals = Tuple.Create(DefaultROICount, DefaultFoundLEDs, DefaultCapacity);

        /***** INITIALIZATION CONSTANTS FOR VARIABLES UNIQUE TO EACH ROI *****/
        private static bool[] DefaultDeinterleaves = new bool[] { false };

        private Tuple<bool[]> DefaultROIGlobals = Tuple.Create(DefaultDeinterleaves);

        /***** INITIALIZATION CONSTANTS FOR VARIABLES UNIQUE TO EACH DATASTREAM (I.E. DEINTERLEAVED, L4
         * 15, L415, AND L560) OF EACH ROI ****/
        private static bool[,] DefaultROIsVisible = new bool[,] { { true, true, true, true } };
        private static bool[,] DefaultAutoscales = new bool[,] { { true, true, true, true } };
        private static double[,] DefaultMins = new double[,] { { 0.0, 0.0, 0.0, 0.0 } };
        private static double[,] DefaultMaxes = new double[,] { { 1.0, 1.0, 1.0, 1.0 } };

        private Tuple<bool[,], bool[,], double[,], double[,]> DefaultLocals = Tuple.Create(DefaultROIsVisible, DefaultAutoscales, DefaultMins, DefaultMaxes);

        /****** INITIALIZATION VARIABLES FOR FORMATTING VARIABLES ******/
        private Tuple<int, int> DefaultTitleControllerSize = Tuple.Create(400, 50);
        private Tuple<int, int> DefaultGlobalStatusControllerSize = Tuple.Create(400, 30);
        private Tuple<int, int> DefaultGraphListControllerSize = Tuple.Create(400, 300);

        /***** DEFAULT CONSTRUCTOR ******/
        public MasterController()
        {

            // Set up Initialization Tuples for Controllers
            Tuple<int, int> TitleControllerInitProps = DefaultTitleControllerSize;
            Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> GlobalStatusControllerInitProps = Tuple.Create(DefaultMasterGlobals, DefaultROIGlobals, DefaultLocals);
            Tuple<Tuple<int, bool[], int>, Tuple<bool[]>, Tuple<bool[,], bool[,], double[,], double[,]>> GraphListControllerInitProps = Tuple.Create(DefaultMasterGlobals, DefaultROIGlobals, DefaultLocals);

            // Initialize Master variables
            roiCount = DefaultROICount;
            foundLEDs = DefaultFoundLEDs;
            capacity = DefaultCapacity;
            deinterleaves = DefaultDeinterleaves;
            roisVisible = DefaultROIsVisible;
            autoScales = DefaultAutoscales;
            mins = DefaultMins;
            maxes = DefaultMaxes;
            inConfigMode = false;
            inColorBlindMode = false;

            InitializeComponent(TitleControllerInitProps, GlobalStatusControllerInitProps, GraphListControllerInitProps);

            EnsureLayout();
        }

        /****** PUBLIC GLOBAL VARIABLES TO BE PUSHED TO GRAPH LIST CONTROLLER *****/
        public virtual int ROICount
        {
            get { return roiCount; }
            set
            {
                if (roiCount != value)
                {
                    roiCount = value;

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

                    GlobalStatusController.ROICount = roiCount;
                    GraphListController.ROICount = roiCount;

                }
            }
        }

        public virtual bool[] FoundLEDs
        {
            get { return foundLEDs; }
            set
            {
                foundLEDs = value;

                int totFoundLEDs = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (foundLEDs[i])
                    {
                        totFoundLEDs += 1;
                    }
                }


                if (totFoundLEDs != prevTotLEDsFounds)
                {
                    GlobalStatusController.FoundLEDs = FoundLEDs;
                    GraphListController.FoundLEDs = FoundLEDs;
                }

                prevTotLEDsFounds = totFoundLEDs;



            }
        }

        public virtual int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                GraphListController.Capacity = capacity;
            }
        }

        public virtual bool[,] ROIsVisible
        {
            get { return roisVisible; }
            set
            {
                roisVisible = value;
                GraphListController.ROIsVisible = roisVisible;
            }
        }

        public virtual bool[] Deinterleaves
        {
            get { return deinterleaves; }
            set
            {
                deinterleaves = value;
                GraphListController.Deinterleaves = deinterleaves;
            }
        }

        public virtual bool[,] AutoScales
        {
            get { return autoScales; }
            set
            {
                autoScales = value;
                GraphListController.AutoScales = autoScales;
            }
        }

        public virtual double[,] Mins
        {
            get { return mins; }
            set
            {
                mins = value;
                GraphListController.Mins = mins;
            }
        }

        public virtual double[,] Maxes
        {
            get { return maxes; }
            set
            {
                maxes = value;
                GraphListController.Maxes = maxes;
            }
        }

        public virtual bool InConfigMode
        {
            get { return inConfigMode; }
            set
            {
                inConfigMode = value;

                SuspendLayout();
                EnsureLayout();
                ResumeLayout();
            }
        }

        public virtual bool InColorBlindMode
        {
            get { return inColorBlindMode; }
            set
            {
                inColorBlindMode = value;

                GraphListController.ColorBlindMode(inColorBlindMode);
            }
        }

        /****** CONTROLS LOCAL TO MASTER CONTROLLER *****/

        protected TitleController TitleController
        {
            get { return titleController; }
            set { titleController = value; }
        }

        public GraphListController GraphListController
        {
            get { return graphListController; }
            set { graphListController = value; }
        }

        public GlobalStatusController GlobalStatusController
        {
            get { return globalStatusController; }
            set { globalStatusController = value; }
        }

        /****** ACCESS POINT FOR EVENTS WITHIN GLOBAL STATUS CONTROLLER *****/

        private void GlobalStatusController_CapacityChanged(object sender, EventArgs e)
        {
            Capacity = GlobalStatusController.Capacity;
        }

        private void GlobalStatusController_ROIsVisibleChanged(object sender, EventArgs e)
        {
            ROIsVisible = GlobalStatusController.ROIsVisible;
        }

        private void GlobalStatusController_DeinterleavesChanged(object sender, EventArgs e)
        {
            Deinterleaves = GlobalStatusController.Deinterleaves;
        }

        private void GlobalStatusController_AutoScalesChanged(object sender, EventArgs e)
        {
            AutoScales = GlobalStatusController.AutoScales;
        }

        private void GlobalStatusController_MinsChanged(object sender, EventArgs e)
        {
            Mins = GlobalStatusController.Mins;
        }

        private void GlobalStatusController_MaxesChanged(object sender, EventArgs e)
        {
            Maxes = GlobalStatusController.Maxes;
        }

        private void GlobalStatusController_ConfigModeChanged(object sender, EventArgs e)
        {
            InConfigMode = GlobalStatusController.InConfigMode;
        }

        private void GlobalStatusController_ColorBlindModeChanged(object sender, EventArgs e)
        {
            InColorBlindMode = GlobalStatusController.InColorBlindMode;
        }

        /****** ACCESS POINT FOR EVENTS WITHIN GLOBAL STATUS CONTROLLER *****/
        private void GraphListController_MinsChanged(object sender, EventArgs e)
        {
            mins = GraphListController.Mins;
            GlobalStatusController.UpdateMins(mins);
        }

        private void GraphListController_MaxesChanged(object sender, EventArgs e)
        {
            maxes = GraphListController.Maxes;
            GlobalStatusController.UpdateMaxes(maxes);
        }
        /****** UPDATES THE LAYOUT ON RESIZE *****/
        private void EnsureLayout()
        {
            TitleController.SuspendLayout();
            GlobalStatusController.SuspendLayout();
            GraphListController.SuspendLayout();

            if (InConfigMode)
            {
                TitleController.Bounds = new Rectangle(0, 0, Width, DefaultTitleControllerSize.Item2);
                GraphListController.Bounds = new Rectangle(0, DefaultTitleControllerSize.Item2, Width, Height - DefaultTitleControllerSize.Item2 - Height / 2);
                GlobalStatusController.Bounds = new Rectangle(0, Height / 2, Width, Height / 2);
            }
            else
            {
                TitleController.Bounds = new Rectangle(0, 0, Width, DefaultTitleControllerSize.Item2);
                GraphListController.Bounds = new Rectangle(0, DefaultTitleControllerSize.Item2, Width, Height - DefaultTitleControllerSize.Item2 - DefaultGlobalStatusControllerSize.Item2);
                GlobalStatusController.Bounds = new Rectangle(0, Height - DefaultGlobalStatusControllerSize.Item2, Width, DefaultGlobalStatusControllerSize.Item2);
            }

            TitleController.ResumeLayout();
            GlobalStatusController.ResumeLayout();
            GraphListController.ResumeLayout();
        }
        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            EnsureLayout();
            ResumeLayout();
        }
    }
}
