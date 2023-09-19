using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class ROIControl : UserControl
    {
        private const float MinFontSize = 8.0f;
        private const int FillOpacity = 85;
        private const float LabelFontScale = 0.8f;
        private const byte MaxImageScale = 50;
        private const byte MinImageScale = 1;
        private const int DisplayBPP = 3;

        // Displayed Image
        private byte ImageScale = 11;

        private const byte ScaleIncrement = 5;

        // Acquired Image
        private int ImgWidth = -1;

        private int ImgHeight = -1;

        private int SelectedRegion;
        private Point MouseDownStartPosition;
        private bool IsMouseDown;
        private bool IsAddingRegion;
        private bool IsMovingRegion;
        private bool IsScalingRegion;
        private bool IsUIEnabled;

        private bool FirstFrame;

        public event RegSettingChangedEventHandler ROIsChanged;

        public event RegSettingChangedEventHandler LEDPowerFP3002Changed;

        private List<PhotometryRegion> _Regions;

        public List<PhotometryRegion> Regions
        {
            get { return _Regions; }
            set
            {
                _Regions = value;
                if (_Regions != null)
                    ROICountVal_Label.Text = _Regions.Count.ToString();
            }
        }

        public ROIControl()
        {
            InitializeComponent();
            FirstFrame = true;
            L470Power_Slider.LEDName = nameof(FP3002Settings.LedPowers.L470);
            L470Power_Slider.SetLEDPower(LedPowerConverter.Default);
            L470Power_Slider.LEDPowerChanged += L470Power_Slider_LEDPowerChanged;
            IsUIEnabled = true;
        }

        internal void EnableUI()
        {
            IsUIEnabled = true;
        }

        internal void DisableUI()
        {
            IsUIEnabled = false;
        }

        internal void ResetTab()
        {
            L470Power_Slider.SetLEDPower(LedPowerConverter.Default);
            LEDPowerFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L470), LedPowerConverter.Default));
        }

        internal void SetLEDToPD()
        {
            var registerValueAtPDPercent = LedPowerConverter.GetRegisterValueAtPDPercent();
            L470Power_Slider.SetLEDPower(registerValueAtPDPercent);
            LEDPowerFP3002Changed?.Invoke(this, new RegSettingChangedEventArgs(nameof(FP3002Settings.LedPowers.L470), registerValueAtPDPercent));
        }

        internal void TryUpdateNewFrame(PhotometryImage photometryImage)
        {
            try
            {
                UpdateNewFrame(photometryImage);
            }
            catch (ObjectDisposedException ex)
            {
                ConsoleLogger.SuppressError(ex);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateNewFrame(PhotometryImage photometryImage)
        {
            if (photometryImage.Bitmap == null) return;

            InitFirstFrame(photometryImage);
            var displayImage = GetImage(photometryImage);
            DrawRegions(ref displayImage);
            SetImage(displayImage);
        }

        internal void TryUpdateKeyDown(KeyEventArgs e)
        {
            try
            {
                UpdateKeyDown(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void UpdateKeyDown(KeyEventArgs e)
        {
            // Do nothing if IsMouseDown
            if (IsMouseDown || L470Power_Slider.GetIsTypingPower() || !IsUIEnabled)
                return;

            if (e.KeyCode == Keys.PageUp && ImageScale < MaxImageScale) ImageScale = (byte)Math.Min(ImageScale + ScaleIncrement, MaxImageScale);
            else if (e.KeyCode == Keys.PageDown && ImageScale > MinImageScale) ImageScale = (byte)Math.Max(ImageScale - ScaleIncrement, MinImageScale);
            else if (e.KeyCode == Keys.Delete) DeleteROI();
        }

        internal void TabSelectROI()
        {
            if (Regions.Count == 0) return;

            SelectedRegion = (SelectedRegion + 1) % Regions.Count;
        }

        private void DeleteROI()
        {
            if (SelectedRegion < 0 || Regions == null || Regions.Count == 0) return;

            Regions.RemoveAt(SelectedRegion);
            for (var i = 0; i < Regions.Count; i++)
                Regions[i] = new PhotometryRegion() { Channel = Regions[i].Channel, Index = i, Rectangle = Regions[i].Rectangle };
            ROICountVal_Label.Text = Regions.Count.ToString();
            SelectedRegion = Math.Max(SelectedRegion - 1, 0);
        }

        private void SetImage(Bitmap displayImage)
        {
            Invoke((MethodInvoker)delegate {
                // Update the displayed image
                if (Image_PictureBox.Image != null)
                    Image_PictureBox.Image.Dispose();
                Image_PictureBox.Image = displayImage;
            });
        }

        private Bitmap GetImage(PhotometryImage photometryImage)
        {
            var widthInPixels = photometryImage.Bitmap.Width;
            var heightInPixels = photometryImage.Bitmap.Height;

            var inImg_widthInBytes = widthInPixels * photometryImage.BitsPerPixel / 8;
            var outImg_widthInBytes = widthInPixels * DisplayBPP;

            var outImg = new Bitmap(widthInPixels, heightInPixels, PixelFormat.Format24bppRgb);

            var inImg_bitmapData = photometryImage.Bitmap.LockBits(new Rectangle(0, 0, widthInPixels, heightInPixels), ImageLockMode.ReadOnly, photometryImage.Bitmap.PixelFormat);
            var outImg_bitmapData = outImg.LockBits(new Rectangle(0, 0, widthInPixels, heightInPixels), ImageLockMode.WriteOnly, outImg.PixelFormat);
            unsafe
            {
                var inImg_ptrFirstPixel = (byte*)inImg_bitmapData.Scan0;
                var outImg_ptrFirstPixel = (byte*)outImg_bitmapData.Scan0;
                Parallel.For(0, heightInPixels, y =>
                {
                    var inImg_rowOffset = inImg_widthInBytes * y;
                    var outImg_rowOffset = outImg_widthInBytes * y;

                    var inImg_ptrCurrentPixel = inImg_ptrFirstPixel + inImg_rowOffset;
                    var outImg_ptrCurrentPixel = outImg_ptrFirstPixel + outImg_rowOffset;

                    var inImg_ptrLastPixel = inImg_ptrCurrentPixel + inImg_widthInBytes;

                    while (inImg_ptrCurrentPixel < inImg_ptrLastPixel)
                    {
                        var pixelValue = *inImg_ptrCurrentPixel++;
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

        private void InitFirstFrame(PhotometryImage photometryImage)
        {
            if (!FirstFrame) return;

            ImgWidth = photometryImage.Width;
            ImgHeight = photometryImage.Height;

            FirstFrame = false;
        }

        private void DrawRegions(ref Bitmap displayImage)
        {
            if (Regions.Count <= 0) return;

            // Using graphics, draw rectangles and labels over the image where the regions are located. Use a different pen for the selected region if one exists
            using (var graphics = Graphics.FromImage(displayImage))
            using (var redBrush = new SolidBrush(Color.FromArgb(FillOpacity, Color.Red)))
            using (var greenBrush = new SolidBrush(Color.FromArgb(FillOpacity, Color.LimeGreen)))
            using (var transparentBrush = new SolidBrush(Color.FromArgb(0, Color.White)))
            using (var format = new StringFormat())
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                // For each region
                for (var i = 0; i < Regions.Count; i++)
                {
                    var region = Regions[i];
                    var digits = region.Index.ToString().Length;
                    var brush = region.Channel == RegionChannel.Red ? redBrush : greenBrush;
                    brush = IsUIEnabled ? brush : transparentBrush;
                    var fontSize = Math.Max(Math.Min(region.Rectangle.Height * LabelFontScale / digits,
                                            region.Rectangle.Width * LabelFontScale / digits), MinFontSize);

                    var labelFont = new Font(Font.Name, fontSize, Font.Style, Font.Unit);
                    if (i == SelectedRegion) brush = new SolidBrush(Color.FromArgb(FillOpacity * 2, brush.Color));
                    if (!IsUIEnabled) brush = transparentBrush;
                    // Draw the rectangle and label it with the region index
                    graphics.FillEllipse(brush, region.Rectangle);
                    graphics.DrawString(i.ToString(), labelFont, Brushes.White, region.Rectangle, format);
                }
            }
        }

        private void L470Power_Slider_LEDPowerChanged(object sender, RegSettingChangedEventArgs args)
        {
            if (LEDPowerFP3002Changed != null)
                LEDPowerFP3002Changed.Invoke(this, args);
        }

        private void Image_PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                HandleMouseDown(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void Image_PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            IsAddingRegion = false;
            IsMovingRegion = false;
            IsScalingRegion = false;
            IsMouseDown = false;
        }

        private void Image_PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                HandleMouseMove(e);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void HandleMouseDown(MouseEventArgs e)
        {
            Image_PictureBox.Focus();
            if (Image_PictureBox.Image == null || !IsUIEnabled)
                return;

            MouseDownStartPosition = GetLocationInFrame(e.Location);
            SelectedRegion = GetSelectedRegion();
            IsMouseDown = true;
            IsAddingRegion = SelectedRegion < 0 && e.Button == MouseButtons.Left;
            IsMovingRegion = SelectedRegion >= 0 && e.Button == MouseButtons.Left;
            IsScalingRegion = SelectedRegion >= 0 && e.Button == MouseButtons.Right;
        }

        private void HandleMouseMove(MouseEventArgs e)
        {
            if (!IsUIEnabled)
                return;

            if (IsAddingRegion)
                AddRegion(e.Location);
            else if (IsMovingRegion)
                MoveRegion(e.Location);
            else if (IsScalingRegion)
                ScaleRegion(e.Location);
        }

        private void AddRegion(Point location)
        {
            var region = CreateRegion(location);
            region.Index = (byte)(Regions.Count - 1);
            Regions.Add(region);
            ROICountVal_Label.Text = Regions.Count.ToString();
            SelectedRegion = Regions.Count - 1;
            IsAddingRegion = false;
            IsScalingRegion = true;

            if (ROIsChanged != null)
                ROIsChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(Regions), Regions));
        }

        private void MoveRegion(Point location)
        {
            var currentRect = Regions[SelectedRegion].Rectangle;
            var prevMousePos = MouseDownStartPosition;
            var currMousePos = GetLocationInFrame(location);
            var tx = currMousePos.X - prevMousePos.X;
            var ty = currMousePos.Y - prevMousePos.Y;
            var xPos = Math.Max(0, Math.Min(currentRect.X + tx, ImgWidth - 1 - currentRect.Width));
            var yPos = Math.Max(0, Math.Min(currentRect.Y + ty, ImgHeight - 1 - currentRect.Height));
            var newPos = new Point(xPos, yPos);
            var rect = new Rectangle(newPos, currentRect.Size);
            var channel = newPos.X * 2 + currentRect.Width < ImgWidth ? RegionChannel.Red : RegionChannel.Green;
            var index = (byte)SelectedRegion;
            var region = new PhotometryRegion() { Channel = channel, Index = index, Rectangle = rect };
            Regions[SelectedRegion] = region;
            MouseDownStartPosition = currMousePos;
            if (ROIsChanged != null)
                ROIsChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(Regions), Regions));
        }

        private void ScaleRegion(Point location)
        {
            var region = CreateRegion(location);
            region.Index = SelectedRegion;
            Regions[SelectedRegion] = region;
            if (ROIsChanged != null)
                ROIsChanged.Invoke(this, new RegSettingChangedEventArgs(nameof(Regions), Regions));
        }

        private PhotometryRegion CreateRegion(Point location)
        {
            var centerPos = MouseDownStartPosition;
            var edgePos = GetLocationInFrame(location);
            var halfWidth = Math.Abs(edgePos.X - centerPos.X);
            var halfHeight = Math.Abs(edgePos.Y - centerPos.Y);

            // Ensure region within bounds of image
            var minXDistToImgEdge = Math.Min(ImgWidth - centerPos.X, centerPos.X);
            var minYDistToImgEdge = Math.Min(ImgHeight - centerPos.Y, centerPos.Y);
            halfWidth = Math.Min(halfWidth, minXDistToImgEdge);
            halfHeight = Math.Min(halfHeight, minYDistToImgEdge);

            // Enable Control modifier key
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                var min = Math.Min(halfWidth, halfHeight);
                halfWidth = min;
                halfHeight = min;
            }
            var xPos = centerPos.X - halfWidth;
            var yPos = centerPos.Y - halfHeight;
            var width = 2 * halfWidth;
            var height = 2 * halfHeight;
            var rect = new Rectangle(xPos, yPos, width, height);
            var channel = 2 * centerPos.X < ImgWidth ? RegionChannel.Red : RegionChannel.Green;
            var region = new PhotometryRegion() { Channel = channel, Rectangle = rect };
            return region;
        }

        private int GetSelectedRegion()
        {
            for (var i = 0; i < Regions.Count; i++)
                if (TestIntersection(Regions[i].Rectangle, MouseDownStartPosition))
                    return i;
            return -1;
        }

        private bool TestIntersection(Rectangle region, Point locInFrame)
        {
            var regionPoints = GetPoints(region);

            // Check each edge of the rectangle to see if point lies of the edge, all edges must pass
            for (var i = 0; i < regionPoints.Count; i++)
            {
                var point2 = regionPoints[(i + 1) % regionPoints.Count];
                var point1 = regionPoints[i % regionPoints.Count];

                var D = (point2.X - point1.X) * (locInFrame.Y - point1.Y) - (locInFrame.X - point1.X) * (point2.Y - point1.Y);

                if (D < 0)
                    return false;
            }
            return true;
        }

        private List<Point> GetPoints(Rectangle region)
        {
            var points = new List<Point>
            {
                region.Location,
                new Point(region.Location.X + region.Width, region.Location.Y),
                new Point(region.Location.X + region.Width, region.Location.Y + region.Height),
                new Point(region.Location.X, region.Location.Y + region.Height)
            };
            return points;
        }

        private Point GetLocationInFrame(Point location)
        {
            var x = location.X;
            var y = location.Y;
            var frame_width = ImgWidth;
            var frame_height = ImgHeight;
            var image_width = Image_PictureBox.Width;
            var image_height = Image_PictureBox.Height;

            return new Point(
                Math.Max(0, Math.Min((int)(x * frame_width / (float)image_width), frame_width - 1)),
                Math.Max(0, Math.Min((int)(y * frame_height / (float)image_height), frame_height - 1)));
        }

        internal void SetImageScale(byte imageScale)
        {
            ImageScale = imageScale;
        }

        internal void UpdateRegionChannels()
        {
            foreach (var region in Regions)
                region.Channel = 2 * region.Rectangle.X + region.Rectangle.Width < ImgWidth ? RegionChannel.Red : RegionChannel.Green;
        }
    }
}