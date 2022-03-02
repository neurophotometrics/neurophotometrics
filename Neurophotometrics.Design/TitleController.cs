using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Neurophotometrics.Design
{
    partial class TitleController : UserControl
    {
        /****** DEBUG VARIABLES ******/
        private int layoutCount = 0;

        /***** PRIVATE CONSTANTS FOR FORMATTING *****/
        private const int TitleVerticalMargin = 10;
        private const int TitleHorizontalMargin = 10;

        /***** DEFAULT CONSTRUCTOR ******/
        public TitleController()
        {
        }

        /***** CONSTRUCTOR WITH INITIALIZATION ******/
        public TitleController(Tuple<int, int> InitProps)
        {
            InitializeComponent(InitProps);
        }

        /****** CONTROLS LOCAL TO TITLE CONTROLLER *****/
        protected Label TitleLabel
        {
            get { return titleLabel; }
            set { titleLabel = value; }
        }

        /****** HANDLES RESIZING *****/
        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            EnsureLayout();
        }

        private void EnsureLayout()
        {
            TitleLabel.SuspendLayout();
            TitleLabel.Bounds = new Rectangle(10 * TitleHorizontalMargin, TitleVerticalMargin, Width - 20 * TitleHorizontalMargin, Height - 2 * TitleVerticalMargin);
            TitleLabel.Font = new Font("Century Gothic", 15, FontStyle.Bold);
        }

        /****** APPLIES FOCUS ON CLICK *****/
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            TitleLabel.Focus();
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {
            TitleLabel.Focus();
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
    }
}
