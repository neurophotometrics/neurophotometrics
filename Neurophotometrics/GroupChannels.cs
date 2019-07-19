using Bonsai;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Neurophotometrics
{
    [DefaultProperty(nameof(Groups))]
    [Description("Groups photometry data channels into labeled groups.")]
    public class GroupChannels : Transform<PhotometryDataFrame, GroupedPhotometryDataFrame>
    {
        readonly Collection<ChannelGroup> groups = new Collection<ChannelGroup>();

        [Description("Specifies the mapping between raw photometry data channels and group labels.")]
        public Collection<ChannelGroup> Groups
        {
            get { return groups; }
        }

        public override IObservable<GroupedPhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return source.Select(input =>
            {
                var groupActivity = new GroupActivity[groups.Count];
                for (int i = 0; i < groupActivity.Length; i++)
                {
                    var group = groups[i];
                    groupActivity[i].Name = group.Name;
                    groupActivity[i].Activity = Array.ConvertAll(group.Channels, channel =>
                    {
                        if (channel >= input.Activity.Length)
                        {
                            throw new InvalidOperationException("Channel not found in raw activity data. Please make sure all ROIs have been correctly defined.");
                        }

                        return input.Activity[channel];
                    });
                }

                var result = new GroupedPhotometryDataFrame();
                result.Image = input.Image;
                result.FrameCounter = input.FrameCounter;
                result.Activity = groupActivity;
                return result;
            });
        }
    }

    public class ChannelGroup
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [TypeConverter(typeof(UnidimensionalArrayConverter))]
        public int[] Channels { get; set; }
    }

    public class GroupedPhotometryDataFrame
    {
        public IplImage Image;
        public long FrameCounter;
        public GroupActivity[] Activity;
    }

    public struct GroupActivity
    {
        public string Name { get; set; }

        public RegionActivity[] Activity { get; set; }
    }
}
