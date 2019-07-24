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
        readonly SpinnakerCapture capture = new SpinnakerCapture();

        [Description("The index of the imaging sensor used to acquire the raw data frame.")]
        public int Index
        {
            get { return capture.Index; }
            set { capture.Index = value; }
        }

        [Description("The regions of interest used to configure independent photometry data channels.")]
        [Editor("Neurophotometrics.Design.PhotometryDataFrameRoiEditor, Neurophotometrics.Design", typeof(UITypeEditor))]
        public RotatedRect[] Regions { get; set; }

        struct ActiveRegion
        {
            public RotatedRect Region;
            public IplImage Mask;
            public Rect Rect;
        }

        public override IObservable<PhotometryDataFrame> Generate()
        {
            return Observable.Defer(() =>
            {
                RotatedRect[] currentRegions = null;
                var activeRegions = new ActiveRegion[0];
                return capture.Generate().Select(input =>
                {
                    var result = new PhotometryDataFrame();
                    result.Image = input.Image;
                    result.FrameCounter = input.ChunkData.FrameID;
                    result.Timestamp = input.ChunkData.Timestamp * 1e-9;

                    if (currentRegions != Regions)
                    {
                        currentRegions = Regions;
                        if (currentRegions == null) activeRegions = new ActiveRegion[0];
                        else activeRegions = Array.ConvertAll(currentRegions, region =>
                        {
                            ActiveRegion activeRegion;
                            var size = new Size((int)region.Size.Width, (int)region.Size.Height);
                            var offset = new Point((int)region.Center.X - size.Width / 2, (int)region.Center.Y - size.Height / 2);
                            activeRegion.Region = region;
                            activeRegion.Mask = new IplImage(size, IplDepth.U8, 1);
                            activeRegion.Mask.SetZero();
                            region.Center = new Point2f(region.Size.Width / 2, region.Size.Height / 2);
                            CV.EllipseBox(activeRegion.Mask, region, Scalar.All(255), -1);
                            activeRegion.Rect = new Rect(offset.X, offset.Y, size.Width, size.Height);
                            return activeRegion;
                        });
                    }

                    result.Activity = Array.ConvertAll(activeRegions, region =>
                    {
                        using (var image = input.Image.GetSubRect(region.Rect))
                        {
                            RegionActivity activity;
                            activity.Region = region.Region;
                            activity.Activity = CV.Avg(image, region.Mask).Val0;
                            return activity;
                        }
                    });
                    return result;
                });
            });
        }
    }
}
