using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Neurophotometrics.V2.Definitions
{
    public class PhotometryRegion
    {
        public int Index { get; set; }
        public RegionChannel Channel { get; set; }
        public Rectangle Rectangle { get; set; }

        public string Name
        {
            get { return Channel.ToString().Substring(0, 1) + Index.ToString(); }
            set
            {
                Channel = Enum.GetValues(typeof(RegionChannel)).Cast<RegionChannel>()
                            .First(channel => channel.ToString().Substring(0, 1) == value.Substring(0, 1));
                Index = int.Parse(value.Substring(1));
            }
        }

        private readonly Dictionary<RegionChannel, Color> ChannelColors = new Dictionary<RegionChannel, Color>()
        {
            {RegionChannel.Green        , Color.Green },
            {RegionChannel.Red          , Color.Red },
            {RegionChannel.Unspecified  , Color.Black }
        };

        public Color GetColor()
        {
            return ChannelColors[Channel];
        }
    }

    public struct RegionActivity
    {
        public PhotometryRegion Region { get; set; }
        public double Value { get; set; }
    }
}