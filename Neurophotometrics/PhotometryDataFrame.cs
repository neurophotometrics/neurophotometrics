using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics
{
    public class PhotometryDataFrame
    {
        public IplImage Image;
        public long FrameCounter;
        public double Timestamp;
        public RegionActivity[] Activity;
    }

    public struct RegionActivity
    {
        public RotatedRect Region;
        public double Activity;
    }
}
