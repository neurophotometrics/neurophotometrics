using Bonsai;
using Bonsai.Harp;
using Bonsai.Spinnaker;
using OpenCV.Net;
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
    class Photometry : Combinator<SpinnakerDataFrame, PhotometryDataFrame>
    {
        public Rect RegionOfInterest { get; set; }

        public RotatedRect[] Regions { get; set; }

        struct ActiveRegion
        {
            public RotatedRect Region;
            public IplImage Mask;
            public Rect Rect;
        }

        public override IObservable<PhotometryDataFrame> Process(IObservable<SpinnakerDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                RotatedRect[] currentRegions = null;
                ActiveRegion[] activeRegions = null;
                return source.Select(input =>
                {
                    var result = new PhotometryDataFrame();
                    var rect = RegionOfInterest;
                    result.Image = input.Image;
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        result.Image = result.Image.GetSubRect(rect);
                    }

                    result.FrameCounter = input.ChunkData.FrameID;
                    result.Timestamp = input.ChunkData.Timestamp * 1e-9;

                    if (currentRegions != Regions || activeRegions == null)
                    {
                        currentRegions = Regions;
                        if (currentRegions == null || currentRegions.Length == 0)
                        {
                            ActiveRegion activeRegion;
                            activeRegion.Mask = null;
                            activeRegion.Rect = new Rect(0, 0, result.Image.Width, result.Image.Height);
                            activeRegion.Region = new RotatedRect(
                                new Point2f(result.Image.Width / 2f, result.Image.Height / 2f),
                                new Size2f(result.Image.Width, result.Image.Height),
                                0);
                            activeRegions = new ActiveRegion[] { activeRegion };
                        }
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
