namespace Neurophotometrics.V1.Definitions
{
    public class PhotometryDataFrame
    {
        public PhotometryImage PhotometryImage { get; set; }
        public ulong FrameCounter { get; set; }
        public double SystemTimestamp { get; set; }
        public double ComputerTimestamp { get; set; }
        public FrameFlags Flags { get; set; }
        public RegionActivity[] Activities { get; set; }
    }
}