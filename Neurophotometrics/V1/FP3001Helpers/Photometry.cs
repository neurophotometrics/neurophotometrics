using Bonsai;

using Neurophotometrics.V1.Definitions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Neurophotometrics.V1.FP3001Helpers
{
    internal class Photometry : Combinator<PhotometryImage, PhotometryDataFrame>
    {
        public Rectangle RegionOffset { get; set; }
        public List<PhotometryRegion> Regions { get; set; } = new List<PhotometryRegion>();

        public Rectangle GetCrop()
        {
            if (Regions.Count > 0)
            {
                var x = Regions.Select(reg => reg.Rectangle.X).Min();
                var y = Regions.Select(reg => reg.Rectangle.Y).Min();
                var width = Regions.Select(reg => reg.Rectangle.X + reg.Rectangle.Width).Max() - x;
                var height = Regions.Select(reg => reg.Rectangle.Y + reg.Rectangle.Height).Max() - y;
                return new Rectangle(x, y, width, height);
            }
            else
            {
                return new Rectangle(0, 0, int.MaxValue, int.MaxValue);
            }
        }

        public override IObservable<PhotometryDataFrame> Process(IObservable<PhotometryImage> source)
        {
            ulong? frameOffset = null;
            return source.Select(photometryImage =>
            {
                if (frameOffset == null) frameOffset = photometryImage.FrameID;
                var result = new PhotometryDataFrame
                {
                    PhotometryImage = photometryImage,
                    FrameCounter = (ulong)(photometryImage.FrameID - frameOffset),
                    SystemTimestamp = photometryImage.Timestamp * 1e-9,
                    ComputerTimestamp = DateTime.Now.TimeOfDay.TotalMilliseconds,
                    Activities = GetActivity(photometryImage)
                };
                return result;
            });
        }

        private RegionActivity[] GetActivity(PhotometryImage photometryImage)
        {
            if (photometryImage.BitsPerPixel == 16)
                return GetMono16Activity(photometryImage);
            else
                return new RegionActivity[1];
        }

        private RegionActivity[] GetMono16Activity(PhotometryImage photometryImage)
        {
            var cropInPixels = new Rectangle(0, 0, photometryImage.Width, photometryImage.Height);
            var regions = GetRegions(cropInPixels);

            var output = new RegionActivity[regions.Count];
            if (photometryImage.Bitmap == null) return output;

            unsafe
            {
                var img_bitmapData = photometryImage.Bitmap.LockBits(new Rectangle(0, 0, photometryImage.Width, photometryImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppGrayScale);
                var img_ptrFirstPixel = (ushort*)img_bitmapData.Scan0;

                for (var i = 0; i < regions.Count; i++)
                {
                    var region = regions[i];
                    var reg_widthInPixels = region.Rectangle.Width;
                    var reg_heightInPixels = region.Rectangle.Height;
                    var reg_offsetInPixels = Math.Max(0, region.Rectangle.Y - photometryImage.OffsetY) * photometryImage.Width + Math.Max(0, region.Rectangle.X - photometryImage.OffsetX);
                    var rowSums = new double[reg_heightInPixels];

                    var reg_ptrFirstPixel = img_ptrFirstPixel + reg_offsetInPixels;
                    Parallel.For(0, reg_heightInPixels, y =>
                    {
                        var row_offsetInPixels = photometryImage.Width * y;
                        var row_ptrFirstPixel = reg_ptrFirstPixel + row_offsetInPixels;
                        var row_ptrLastPixel = row_ptrFirstPixel + reg_widthInPixels;

                        var rowSum = 0.0;
                        while (row_ptrFirstPixel < row_ptrLastPixel)
                            rowSum += *(row_ptrFirstPixel++);
                        rowSums[y] = rowSum;
                    });

                    var avgPixelValue = 0.0;
                    if (rowSums.Any())
                        avgPixelValue = rowSums.Average() / reg_widthInPixels;
                    var normalPixelValue = avgPixelValue / ushort.MaxValue;
                    output[i] = new RegionActivity()
                    {
                        Region = region,
                        Value = normalPixelValue
                    };
                }

                photometryImage.Bitmap.UnlockBits(img_bitmapData);
            }
            return output;
        }

        private List<PhotometryRegion> GetRegions(Rectangle cropInPixels)
        {
            if (Regions.Count > 0) return Regions;

            var regions = new List<PhotometryRegion>()
            {
                new PhotometryRegion(){ Index = 0, Channel = RegionChannel.Unspecified, Rectangle = cropInPixels}
            };
            return regions;
        }
    }
}