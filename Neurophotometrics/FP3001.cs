using Bonsai;
using Bonsai.Spinnaker;
using OpenCV.Net;
using SpinnakerNET;
using SpinnakerNET.GenApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics
{
    [Description("Generates a sequence of photometry data from an FP3001 device.")]
    [Editor("Neurophotometrics.Design.FP3001CalibrationEditor, Neurophotometrics.Design", typeof(ComponentEditor))]
    public class FP3001 : Source<PhotometryDataFrame>
    {
        readonly SpinnakerCapture capture = new FP3001SpinnakerCapture();
        readonly PhotometryDataProcessor photometry = new PhotometryDataProcessor();

        class FP3001SpinnakerCapture : SpinnakerCapture
        {
            protected override void Configure(IManagedCamera camera)
            {
                base.Configure(camera);
                camera.TriggerSource.Value = TriggerSourceEnums.Line0.ToString();
                camera.TriggerMode.Value = TriggerModeEnums.On.ToString();
            }
        }

        [Description("The index of the imaging sensor used to acquire the raw data frame.")]
        public int Index
        {
            get { return capture.Index; }
            set { capture.Index = value; }
        }

        [Description("The regions of interest used to configure independent photometry data channels.")]
        [Editor("Neurophotometrics.Design.PhotometryDataFrameRoiEditor, Neurophotometrics.Design", typeof(UITypeEditor))]
        public RotatedRect[] Regions
        {
            get { return photometry.Regions; }
            set { photometry.Regions = value; }
        }

        public override IObservable<PhotometryDataFrame> Generate()
        {
            return photometry.Process(capture.Generate());
        }
    }
}
