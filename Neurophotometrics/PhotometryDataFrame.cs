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

    public struct PhotometryRegion
    {
        public int Index;
        public Point2f Center;
        public Size2f Size;
    }

    public struct RegionActivity
    {
        public PhotometryRegion Region;
        public double Value;
    }
}
