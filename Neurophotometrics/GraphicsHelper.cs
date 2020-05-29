using OpenCV.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Neurophotometrics
{
    static class GraphicsHelper
    {
        static RectangleF RegionRectangle(PhotometryRegion region)
        {
            var x = region.Center.X - region.Size.Width / 2f;
            var y = region.Center.Y - region.Size.Height / 2f;
            var width = region.Size.Width;
            var height = region.Size.Height;
            return new RectangleF(x, y, width, height);
        }

        internal static void LabelBitmap(IplImage image, RegionActivity[] activity)
        {
            LabelBitmap(image, activity, string.Empty);
        }

        internal static void LabelBitmap(IplImage image, RegionActivity[] activity, string prefix)
        {
            const float RegionWidth = 2;
            var halfWidth = image.Width / 2f;
            using (var labelBitmap = new Bitmap(image.Width, image.Height, image.WidthStep, PixelFormat.Format24bppRgb, image.ImageData))
            using (var graphics = Graphics.FromImage(labelBitmap))
            using (var redPen = new Pen(Color.Red, RegionWidth))
            using (var greenPen = new Pen(Color.LimeGreen, RegionWidth))
            using (var format = new StringFormat())
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                for (int i = 0; i < activity.Length; i++)
                {
                    var region = activity[i].Region;
                    var rect = RegionRectangle(region);
                    var pen = region.Center.X < halfWidth ? redPen : greenPen;
                    var labelSuffix = region.Center.X < halfWidth ? "R" : "G";
                    graphics.DrawEllipse(pen, rect);
                    graphics.DrawString(prefix + region.Index + labelSuffix, SystemFonts.DefaultFont, Brushes.White, rect, format);
                }
            }
        }
    }
}
