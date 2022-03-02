using Bonsai;
using Bonsai.Design;
using Neurophotometrics;
using Neurophotometrics.Design;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

[assembly: TypeVisualizer(typeof(ActivityVisualizer2), Target = typeof(VisualizeROI))]

namespace Neurophotometrics.Design
{
    public class ActivityVisualizer2 : DialogTypeVisualizer
    {
        /* Global Variables
         * 
         * TargetElapsedTime: (double), the period we want the Graph to update (currently 30Hz).
         * view: (MasterGraphController), the Master Graph Controller.
         * updateTime: (double), tracks how much time since the last time the Graph was updated. 
         */
        static readonly double TargetElapsedTime = 1.0 / 30.0;
        internal MasterController view;
        double updateTime;
        private bool[] foundLEDs = new bool[3] { false, false, false };
        private bool firstFrame = true;

        /* Update View at a rate of 30Hz 
         * Invalidates and redraws the graph
         */
        internal void UpdateView(double time)
        {
            if ((time - updateTime) > TargetElapsedTime)
            {
                foreach (ReactiveGraph graph in view.GraphListController.ReactiveGraphList)
                {
                    try
                    {
                        graph.GraphPane_AxisChange();
                        graph.Invalidate();
                    }
                    catch (ArgumentException e)
                    {
                        updateTime = time;
                        return;
                    }

                }
                updateTime = time;
            }
        }

        /* Load the Master Controller
         * 
         * This is run when the visualizer is opened.
         * Creates a new Master Graph Controller, 
         * Fits to the Dock,
         * And the Master Graph Controller to the Visualizer Service.
         */
        public override void Load(IServiceProvider provider)
        {
            view = new MasterController();
            foundLEDs = new bool[3] { false, false, false };
            firstFrame = true;
            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            if (visualizerService != null)
            {
                visualizerService.AddControl(view);
            }
        }

        private void FormatPane(int ROI, RegionActivity activity, string LED, double seconds)
        {
            ReactiveGraph reactiveGraph = view.GraphListController.ReactiveGraphList[ROI];
            string title = activity.Region.Mode.ToString().ToCharArray()[0].ToString() + activity.Region.Index.ToString();
            double data = activity.Value;

            if (LED.Equals("L415"))
            {
                LED = "b_L415";
            }
            else if (LED.Equals("L470"))
            {
                LED = "c_L470";
            }
            else if (LED.Equals("L560"))
            {
                LED = "d_L560";
            }

            reactiveGraph.MasterPane.Title.Text = title;
            foreach (GraphPane graphPane in reactiveGraph.GraphPanes)
            {
                if (graphPane.Tag.Equals("a_Interleaved"))
                {
                    CurveItem interleavedCurve = graphPane.CurveList[0];
                    interleavedCurve.AddPoint(seconds, data);   // Add new point to the curve
                }
                else if (graphPane.Tag.Equals(LED))
                {
                    CurveItem deinterleavedCurve = graphPane.CurveList[0];
                    deinterleavedCurve.AddPoint(seconds, data);
                }
            }

            reactiveGraph.EnsureCapacity();
        }

        private void Show(double time, PhotometryDataFrame frame)
        {
            double seconds = time;
            RegionActivity[] activity = frame.Activity;
            int numROIs = activity.Length;


            if (firstFrame)
            {
                if (numROIs == 1)
                {
                    view.GlobalStatusController.ROICount = 1;
                    view.GraphListController.ROICount = 1;
                    firstFrame = false;
                }
                else
                {
                    view.ROICount = numROIs;
                    firstFrame = false;
                }
            }


            string CurrentLED = frame.Flags.ToString().Split(',')[0];
            if (CurrentLED.Equals("L415"))
            {
                foundLEDs[0] = true;
            }
            else if (CurrentLED.Equals("L470"))
            {
                foundLEDs[1] = true;
            }
            else if (CurrentLED.Equals("L560"))
            {
                foundLEDs[2] = true;
            }

            // Find the amount of found LEDs
            int totFoundLEDs = 0;
            for (int i = 0; i < 3; i++)
            {
                if (foundLEDs[i])
                {
                    totFoundLEDs += 1;
                }
            }

            view.FoundLEDs = foundLEDs;

            for (int i = 0; i < numROIs; i++)
            {
                FormatPane(i, activity[i], CurrentLED, seconds);
            }
        }

        /* Initial Show function. This is where data from the "VisualizeROI" node first enters this visualizer
         * 
         * Find the format of the incoming data,
         * Change the formatting of the data and send it to the proper Show() function
         * Update the viewer
         */
        public override void Show(object value)
        {
            if (value is Tuple<PhotometryDataFrame, double> inputs0)
            {
                PhotometryDataFrame fpData = inputs0.Item1;
                double time = inputs0.Item2;
                Show(time, fpData);
                UpdateView(time);
            }
        }

        /* Unload the Master Controller.
         * 
         * Dispose() the Master Controller that was loaded,
         * And set the global variable to null.
         */
        public override void Unload()
        {
            view.Dispose();
            view = null;
            firstFrame = true;
        }
    }
}
