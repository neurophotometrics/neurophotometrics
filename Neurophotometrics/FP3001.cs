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
        readonly Photometry photometry = new Photometry();

        class FP3001SpinnakerCapture : SpinnakerCapture
        {
            protected override void Configure(IManagedCamera camera)
            {
                base.Configure(camera);
                camera.PixelFormat.Value = PixelFormatEnums.Mono16.ToString();
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

        [Description("The regions of interest used to specify independent photometry data.")]
        [Editor("Neurophotometrics.Design.PhotometryDataFrameRoiEditor, Neurophotometrics.Design", typeof(UITypeEditor))]
        public RotatedRect[] Regions
        {
            get { return photometry.Regions; }
            set { photometry.Regions = value; }
        }

        [TypeConverter(typeof(TriggerModeConverter))]
        [Description("The trigger mode used to drive each of the 410, 470, and 560nm LEDs.")]
        public TriggerMode TriggerMode { get; set; }

        public override IObservable<PhotometryDataFrame> Generate()
        {
            return Observable.Defer(() =>
            {
                var output = photometry.Process(capture.Generate());
                switch (TriggerMode)
                {
                    case TriggerMode.Trigger1:
                        return output.Do(frame => frame.Flags =
                            frame.FrameCounter % 2 == 0 ? FrameFlags.L410 : FrameFlags.L470 | FrameFlags.L560);
                    case TriggerMode.Trigger2:
                        return output.Do(frame => frame.Flags =
                            frame.FrameCounter % 2 == 0 ? FrameFlags.L470 : FrameFlags.L560);
                    case TriggerMode.Trigger3:
                        return output.Do(frame =>
                        {
                            switch (frame.FrameCounter % 3)
                            {
                                case 0: frame.Flags = FrameFlags.L410; break;
                                case 1: frame.Flags = FrameFlags.L470; break;
                                case 2: frame.Flags = FrameFlags.L560; break;
                            }
                        });
                    default:
                        return output.Do(frame => frame.Flags = FrameFlags.L410 | FrameFlags.L470 | FrameFlags.L560);
                }
            });
        }

        class TriggerModeConverter : EnumConverter
        {
            public TriggerModeConverter(Type type)
                : base(type)
            {
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new[]
                {
                    TriggerMode.Constant,
                    TriggerMode.Trigger1,
                    TriggerMode.Trigger2,
                    TriggerMode.Trigger3
                });
            }
        }
    }
}
