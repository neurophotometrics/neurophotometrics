using OpenCV.Net;

namespace Neurophotometrics
{
    public class PhotometryDataFrame
    {
        public IplImage Image;
        public long FrameCounter;
        public double Timestamp;
        public FrameFlags Flags;
        public RegionActivity[] Activity;
    }

    public struct RegionActivity
    {
        public RotatedRect Region;
        public double Value;
    }
}
