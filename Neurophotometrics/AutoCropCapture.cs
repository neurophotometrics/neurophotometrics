using Bonsai.Spinnaker;
using OpenCV.Net;
using SpinnakerNET;
using System;

namespace Neurophotometrics
{
    class AutoCropCapture : SpinnakerCapture
    {
        readonly Photometry processor;

        public AutoCropCapture(Photometry photometry)
        {
            processor = photometry;
        }

        public bool AutoCrop { get; set; }

        static long SafeIncrement(long value, long min, long increment)
        {
            var remainder = (value - min) % increment;
            if (remainder == 0) return value;
            return value + increment - remainder;
        }

        protected override void Configure(IManagedCamera camera)
        {
            Rect imageRoi;
            base.Configure(camera);
            var widthMax = (int)camera.WidthMax;
            var heightMax = (int)camera.HeightMax;
            if (AutoCrop)
            {
                imageRoi = processor.GetBoundingRegion(widthMax, heightMax);
                imageRoi.Width = (int)Math.Min(camera.WidthMax, SafeIncrement(imageRoi.Width, camera.Width.Min, camera.Width.Increment));
                imageRoi.Height = (int)Math.Min(camera.HeightMax, SafeIncrement(imageRoi.Height, camera.Height.Min, camera.Height.Increment));
                imageRoi.X = (int)Math.Max(0, imageRoi.X - imageRoi.X % camera.OffsetX.Increment);
                imageRoi.Y = (int)Math.Max(0, imageRoi.Y - imageRoi.Y % camera.OffsetY.Increment);
            }
            else imageRoi = new Rect(0, 0, widthMax, heightMax);
            processor.RegionOffset = new Rect(imageRoi.X, imageRoi.Y, widthMax, heightMax);

            camera.OffsetX.Value = 0;
            camera.OffsetY.Value = 0;
            camera.Width.Value = imageRoi.Width;
            camera.Height.Value = imageRoi.Height;
            camera.OffsetX.Value = imageRoi.X;
            camera.OffsetY.Value = imageRoi.Y;
        }
    }
}
