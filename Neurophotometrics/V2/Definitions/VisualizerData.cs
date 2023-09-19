namespace Neurophotometrics.V2.Definitions
{
    public class VisualizerData
    {
        public ulong FrameCounter { get; set; }
        public double SystemTimestamp { get; set; }
        public FrameFlags Flags { get; set; }
        public RegionActivity[] Activities { get; set; }
    }
}