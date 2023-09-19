using Neurophotometrics.V1.Definitions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neurophotometrics.V1.PhotometryWriterHelpers
{
    public class RegionImageWriter
    {
        private const string RegionImagesFolderName = "RegionImages";
        private const string FirstRegionImageStr = "RegionImage_FirstFrame.jpg";
        private const string LastRegionImageStr = "RegionImage_LastFrame.jpg";

        private const int DisplayBPP = 3;
        private const byte ImageScale = 11;
        private const int FillOpacity = 85;
        private const float LabelFontScale = 0.8f;
        private const float MinFontSize = 8.0f;

        private readonly string ImageDirectory;
        private readonly string Suffix;

        private Bitmap FirstRegionImage;
        private Bitmap CurrentRegionImage;

        public RegionImageWriter(string parentDirectory, string suffix)
        {
            ImageDirectory = GetParentDirectory(parentDirectory);
            Suffix = suffix;
        }

        private string GetParentDirectory(string parentDirectory)
        {
            var directoryInfo = new DirectoryInfo(parentDirectory);
            var folderAbsPath = $@"{directoryInfo.FullName}\{RegionImagesFolderName}";

            if (!Directory.Exists(folderAbsPath))
                Directory.CreateDirectory(folderAbsPath);
            else
                throw new InvalidOperationException(string.Format("The path '{0}' already exists.", folderAbsPath));

            parentDirectory = folderAbsPath + @"\";

            return parentDirectory;
        }

        internal void TryStoreFirstRegionImage(PhotometryDataFrame dataFrame)
        {
            try
            {
                StoreFirstRegionImage(dataFrame);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        internal void TryStoreCurrentRegionImage(PhotometryDataFrame dataFrame)
        {
            try
            {
                StoreCurrentRegionImage(dataFrame);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void StoreFirstRegionImage(PhotometryDataFrame dataFrame)
        {
            if (FirstRegionImage != null)
                FirstRegionImage.Dispose();

            FirstRegionImage = GetImage(dataFrame.PhotometryImage);
            DrawRegions(dataFrame, FirstRegionImage);
        }

        private void StoreCurrentRegionImage(PhotometryDataFrame dataFrame)
        {
            if (CurrentRegionImage != null)
                CurrentRegionImage.Dispose();

            CurrentRegionImage = GetImage(dataFrame.PhotometryImage);
            DrawRegions(dataFrame, CurrentRegionImage);
        }

        private Bitmap GetImage(PhotometryImage photometryImage)
        {
            var widthInPixels = photometryImage.Bitmap.Width;
            var heightInPixels = photometryImage.Bitmap.Height;

            var outImg_widthInBytes = widthInPixels * DisplayBPP;

            var outImg = new Bitmap(widthInPixels, heightInPixels, PixelFormat.Format24bppRgb);

            var inImg_bitmapData = photometryImage.Bitmap.LockBits(new Rectangle(0, 0, widthInPixels, heightInPixels), ImageLockMode.ReadOnly, photometryImage.Bitmap.PixelFormat);
            var outImg_bitmapData = outImg.LockBits(new Rectangle(0, 0, widthInPixels, heightInPixels), ImageLockMode.WriteOnly, outImg.PixelFormat);
            unsafe
            {
                var inImg_ptrFirstPixel = (ushort*)inImg_bitmapData.Scan0;
                var outImg_ptrFirstPixel = (byte*)outImg_bitmapData.Scan0;
                Parallel.For(0, heightInPixels, y =>
                {
                    var inImg_rowOffset = widthInPixels * y;
                    var outImg_rowOffset = outImg_widthInBytes * y;

                    var inImg_ptrCurrentPixel = inImg_ptrFirstPixel + inImg_rowOffset;
                    var outImg_ptrCurrentPixel = outImg_ptrFirstPixel + outImg_rowOffset;

                    var outImg_ptrLastPixel = outImg_ptrCurrentPixel + outImg_widthInBytes;

                    while (outImg_ptrCurrentPixel < outImg_ptrLastPixel)
                    {
                        var pixelValue = (byte)((*inImg_ptrCurrentPixel++) >> 8);
                        var scaledValue = (byte)Math.Min(pixelValue * ImageScale, byte.MaxValue);
                        *outImg_ptrCurrentPixel++ = scaledValue;
                        *outImg_ptrCurrentPixel++ = scaledValue;
                        *outImg_ptrCurrentPixel++ = scaledValue;
                    }
                });
            }
            outImg.UnlockBits(outImg_bitmapData);
            photometryImage.Bitmap.UnlockBits(inImg_bitmapData);
            return outImg;
        }

        private void DrawRegions(PhotometryDataFrame dataFrame, Bitmap image)
        {
            var isNoROIs = GetIsNoROIs(dataFrame);
            if (isNoROIs) return;

            using (var graphics = Graphics.FromImage(image))
            using (var redBrush = new SolidBrush(Color.FromArgb(FillOpacity, Color.Red)))
            using (var greenBrush = new SolidBrush(Color.FromArgb(FillOpacity, Color.LimeGreen)))
            using (var format = new StringFormat())
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                foreach (var photometryRegion in dataFrame.Activities.Select(activity => activity.Region))
                {
                    var rect = photometryRegion.Rectangle;
                    var xPos = rect.X - dataFrame.PhotometryImage.OffsetX;
                    var yPos = rect.Y - dataFrame.PhotometryImage.OffsetY;
                    var width = rect.Width - Math.Max(xPos + rect.Width - dataFrame.PhotometryImage.Width, 0);
                    var height = rect.Height - Math.Max(yPos + rect.Height - dataFrame.PhotometryImage.Height, 0);
                    rect = new Rectangle(xPos, yPos, width, height);
                    var digits = photometryRegion.Index.ToString().Length;
                    var brush = photometryRegion.Channel == RegionChannel.Red ? redBrush : greenBrush;
                    var fontSize = Math.Max(Math.Min(rect.Height * LabelFontScale / digits,
                                            rect.Width * LabelFontScale / digits), MinFontSize);
                    var labelFont = new Font("Arial", fontSize, FontStyle.Regular, GraphicsUnit.Point);
                    graphics.FillEllipse(brush, rect);
                    graphics.DrawString(photometryRegion.Index.ToString(), labelFont, Brushes.White, rect, format);
                }
            }
        }

        private bool GetIsNoROIs(PhotometryDataFrame dataFrame)
        {
            return dataFrame.Activities.Length == 1 && dataFrame.Activities[0].Region.Channel == RegionChannel.Unspecified;
        }

        internal void TryClose()
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void Close()
        {
            var firstRegionImagePath = SuffixHelper.AppendSuffix(ImageDirectory + FirstRegionImageStr, Suffix);
            if (FirstRegionImage != null)
                FirstRegionImage.Save(firstRegionImagePath, ImageFormat.Jpeg);

            var lastRegionImagePath = SuffixHelper.AppendSuffix(ImageDirectory + LastRegionImageStr, Suffix);
            if (CurrentRegionImage != null)
                CurrentRegionImage.Save(lastRegionImagePath, ImageFormat.Jpeg);
        }
    }
}