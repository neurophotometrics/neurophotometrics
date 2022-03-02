using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Diagnostics;

namespace Neurophotometrics
{
    [Description("Accepts the FP data from the 'PhotometryData' node and creates visualizer for viewing the data of each ROI.")]
    [Combinator(MethodName = nameof(Process))]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class VisualizeROI
    {
        Stopwatch stopwatch = new Stopwatch();
        double workflowStartTime = 0;
        long frame;
        double sysTime;
        double workflowTime;
        double compTime;
        double samplingPeriod;
        long stopwatchFreq;
        XAxisScale timeAxisUnits;

        public VisualizeROI()
        {
            stopwatchFreq = DisplayTimerProperties();
            TimeAxisUnits = XAxisScale.SysWorkflowStart;
        }

        [Description("Specify the clock/units to be used for the X-Axis of the visualizer. 'SystemON' uses the system's clock with units of seconds since the system turned on. 'SysWorkflowStart' uses the system's clock with units of seconds since the workflow started. 'CompWorkflowStart' uses the computer's  clock with units of seconds since the workflow started.")]
        public XAxisScale TimeAxisUnits
        {
            get { return timeAxisUnits; }
            set
            {
                double currentCompTime = (double)stopwatch.ElapsedTicks / (double)stopwatchFreq;
                if (currentCompTime > compTime + 5.0 * samplingPeriod || currentCompTime == 0)
                {
                    timeAxisUnits = value;
                }
            }
        }

        /* CASE: PhotometryData */
        public IObservable<Tuple<PhotometryDataFrame, double>>
            Process(IObservable<PhotometryDataFrame> inputs)
        {

            return inputs.Select(value =>
            {
                frame = value.FrameCounter;
                sysTime = value.Timestamp;
                if (frame == 0)
                {
                    stopwatch.Restart();
                    workflowStartTime = sysTime;
                    workflowTime = 0;
                    compTime = 0;
                }
                else if (frame == 1)
                {
                    compTime = (double)stopwatch.ElapsedTicks / (double)stopwatchFreq;
                    workflowTime = sysTime - workflowStartTime;
                    samplingPeriod = compTime;
                }
                else
                {
                    compTime = (double)stopwatch.ElapsedTicks / (double)stopwatchFreq;
                    workflowTime = sysTime - workflowStartTime;
                }

                Tuple<PhotometryDataFrame, double> output;
                if (TimeAxisUnits.Equals(XAxisScale.SystemON))
                {
                    output = Tuple.Create(value, sysTime);
                }
                else if (TimeAxisUnits.Equals(XAxisScale.SysWorkflowStart))
                {
                    output = Tuple.Create(value, workflowTime);
                }
                else
                {
                    output = Tuple.Create(value, compTime);
                }

                return output;
            });
        }

        public static long DisplayTimerProperties()
        {
            long frequency = Stopwatch.Frequency;
            return frequency;
        }
    }


    public enum XAxisScale
    {
        SystemON,
        SysWorkflowStart,
        CompWorkflowStart
    }
}


