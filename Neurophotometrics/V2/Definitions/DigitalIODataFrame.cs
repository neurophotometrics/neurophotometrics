namespace Neurophotometrics.V2.Definitions
{
    public class DigitalIODataFrame
    {
        public string DigitalIOName { get; set; }
        public byte DigitalIOFlag { get; set; }
        public bool DigitalIOState { get; set; }
        public double SystemTimestamp { get; set; }
        public double ComputerTimestamp { get; set; }
    }
}