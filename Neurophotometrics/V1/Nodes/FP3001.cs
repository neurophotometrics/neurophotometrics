using Bonsai;

using Neurophotometrics.V1.Definitions;
using Neurophotometrics.V1.FP3001Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Neurophotometrics.V1
{
    [Description("Generates a sequence of photometry data from an FP3001 device.")]
    [Editor("Neurophotometrics.Design.V1.Forms.FP3001FormEditor, Neurophotometrics.Design", typeof(ComponentEditor))]
    public class FP3001 : Source<PhotometryDataFrame>
    {
        private readonly Photometry Photometry;
        private readonly CameraCapture Capture;

        [Description("The Serial Number of the internal camera on the FP3001 system.")]
        [TypeConverter(typeof(SerialNumberConverter))]
        public string SerialNumber
        {
            get { return Capture.SerialNumber; }
            set { Capture.SerialNumber = value; }
        }

        [Browsable(false)]
        public List<PhotometryRegion> Regions
        {
            get { return Photometry.Regions; }
            set { Photometry.Regions = value; }
        }

        [Browsable(false)]
        public TriggerMode TriggerMode { get; set; } = TriggerMode.Constant;

        [Browsable(false)]
        public int FPS
        {
            get { return Capture.FPS; }
            set { Capture.FPS = value; }
        }

        public FP3001()
        {
            Photometry = new Photometry();
            Capture = new CameraCapture(Photometry);
        }

        public override IObservable<PhotometryDataFrame> Generate()
        {
            return Observable.Defer(() =>
            {
                var output = Photometry.Process(Capture.Generate());
                Capture.EnsureCameraSerialNumber();
                switch (TriggerMode)
                {
                    case TriggerMode.Trigger1:
                        return output.Do(frame => frame.Flags =
                            frame.FrameCounter % 2 == 0 ? FrameFlags.L470 | FrameFlags.L560 : FrameFlags.L415);

                    case TriggerMode.Trigger2:
                        return output.Do(frame => frame.Flags =
                            frame.FrameCounter % 2 == 0 ? FrameFlags.L470 : FrameFlags.L560);

                    case TriggerMode.Trigger3:
                        return output.Do(frame =>
                        {
                            switch (frame.FrameCounter % 3)
                            {
                                case 0: frame.Flags = FrameFlags.L470; break;
                                case 1: frame.Flags = FrameFlags.L560; break;
                                case 2: frame.Flags = FrameFlags.L415; break;
                            }
                        });
                    default:
                        return output.Do(frame => frame.Flags = FrameFlags.L415 | FrameFlags.L470 | FrameFlags.L560);
                }
            });
        }

        public void SetAutoCrop(bool autoCrop)
        {
            Capture.SetAutoCrop(autoCrop);
        }

        public void EnsureCameraSerialNumber()
        {
            Capture.EnsureCameraSerialNumber();
        }
    }
}