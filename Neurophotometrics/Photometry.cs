using Bonsai;
using Bonsai.Spinnaker;
using OpenCV.Net;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Neurophotometrics
{
    class Photometry : Combinator<SpinnakerDataFrame, PhotometryDataFrame>
    {
        public Rect RegionOffset { get; set; }

        public RotatedRect[] Regions { get; set; }

        struct ActiveRegion
        {
            public PhotometryRegion Region;
            public IplImage Mask;
            public Rect Rect;
        }

        static void GetRegionRectangle(ref RotatedRect region, out Rect rect)
        {
            rect.Width = (int)region.Size.Width;
            rect.Height = (int)region.Size.Height;
            rect.X = (int)region.Center.X - rect.Width / 2;
            rect.Y = (int)region.Center.Y - rect.Height / 2;
        }

        internal Rect GetBoundingRegion(int widthMax, int heightMax)
        {
            var regions = Regions;
            if (regions == null || regions.Length == 0) return new Rect(0, 0, widthMax, heightMax);

            var bounds = new Rect(int.MaxValue, int.MaxValue, 0, 0);
            for (int i = 0; i < regions.Length; i++)
            {
                GetRegionRectangle(ref regions[i], out Rect rect);
                bounds.X = Math.Min(bounds.X, rect.X);
                bounds.Y = Math.Min(bounds.Y, rect.Y);
                bounds.Width = Math.Max(bounds.Width, rect.X + rect.Width);
                bounds.Height = Math.Max(bounds.Height, rect.Y + rect.Height);
            }
            bounds.Width -= bounds.X;
            bounds.Height -= bounds.Y;

            const int SafetyMargin = 4;
            bounds.X = Math.Max(0, bounds.X - SafetyMargin);
            bounds.Y = Math.Max(0, bounds.Y - SafetyMargin);
            bounds.Width = Math.Min(widthMax, bounds.Width + 2 * SafetyMargin);
            bounds.Height = Math.Min(heightMax, bounds.Height + 2 * SafetyMargin);
            return bounds;
        }

        public override IObservable<PhotometryDataFrame> Process(IObservable<SpinnakerDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                long frameOffset = -1;
                RotatedRect[] currentRegions = null;
                ActiveRegion[] activeRegions = null;
                return source.Select(input =>
                {
                    var result = new PhotometryDataFrame();
                    result.Image = input.Image;
                    if (frameOffset < 0) frameOffset = input.ChunkData.FrameID;
                    result.FrameCounter = input.ChunkData.FrameID - frameOffset;
                    result.Timestamp = input.ChunkData.Timestamp * 1e-9;

                    if (currentRegions != Regions || activeRegions == null)
                    {
                        var index = 0;
                        currentRegions = Regions;
                        var regionOffset = RegionOffset;
                        var halfWidth = regionOffset.Width > 0 ? regionOffset.Width / 2f : result.Image.Width / 2f;
                        if (currentRegions == null || currentRegions.Length == 0)
                        {
                            ActiveRegion activeRegion;
                            activeRegion.Mask = null;
                            activeRegion.Rect = new Rect(0, 0, result.Image.Width, result.Image.Height);
                            activeRegion.Region.Center = new Point2f(halfWidth, result.Image.Height / 2f);
                            activeRegion.Region.Size = new Size2f(result.Image.Width, result.Image.Height);
                            activeRegion.Region.Index = index;
                            activeRegion.Region.Mode = RegionMode.Unspecified;
                            activeRegions = new ActiveRegion[] { activeRegion };
                        }
                        else activeRegions = Array.ConvertAll(currentRegions, region =>
                        {
                            ActiveRegion activeRegion;
                            activeRegion.Region.Index = index++;
                            activeRegion.Region.Mode = region.Center.X < halfWidth ? RegionMode.Red : RegionMode.Green;
                            region.Center -= new Point2f(regionOffset.X, regionOffset.Y);
                            GetRegionRectangle(ref region, out activeRegion.Rect);
                            activeRegion.Region.Center = region.Center;
                            activeRegion.Region.Size = region.Size;
                            activeRegion.Mask = new IplImage(new Size(activeRegion.Rect.Width, activeRegion.Rect.Height), IplDepth.U8, 1);
                            activeRegion.Mask.SetZero();
                            region.Center = new Point2f(region.Size.Width / 2, region.Size.Height / 2);
                            CV.EllipseBox(activeRegion.Mask, region, Scalar.All(255), -1);
                            return activeRegion;
                        });
                    }

                    const double U8Scale = 1.0 / byte.MaxValue;
                    const double U16Scale = 1.0 / ushort.MaxValue;
                    var scale = input.Image.Depth == IplDepth.U8 ? U8Scale : U16Scale;
                    result.Activity = Array.ConvertAll(activeRegions, region =>
                    {
                        using (var image = input.Image.GetSubRect(region.Rect))
                        {
                            RegionActivity activity;
                            activity.Region = region.Region;
                            activity.Value = CV.Avg(image, region.Mask).Val0 * scale;
                            return activity;
                        }
                    });
                    return result;
                });
            });
        }
    }
}
