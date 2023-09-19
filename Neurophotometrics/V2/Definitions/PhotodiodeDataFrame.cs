namespace Neurophotometrics.V2.Definitions
{
    public class PhotodiodeDataFrame
    {
        public ushort PD415 { get; set; }
        public ushort PD470 { get; set; }
        public ushort PD560 { get; set; }
        public double SystemTimestamp { get; set; }
        public double ComputerTimestamp { get; set; }
    }
}