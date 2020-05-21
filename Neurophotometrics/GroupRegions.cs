using Bonsai;
using OpenCV.Net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Neurophotometrics
{
    [DefaultProperty(nameof(Groups))]
    [Description("Groups photometry data into labeled groups.")]
    public class GroupRegions : Transform<PhotometryDataFrame, GroupedPhotometryDataFrame>
    {
        readonly Collection<GroupedRegions> groups = new Collection<GroupedRegions>();

        [Editor("Neurophotometrics.Design.GroupRegionsEditor, Neurophotometrics.Design", DesignTypes.UITypeEditor)]
        [Description("Specifies the mapping between raw photometry regions of interest and group labels.")]
        public Collection<GroupedRegions> Groups
        {
            get { return groups; }
        }

        public override IObservable<GroupedPhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return source.Select(input =>
            {
                var groupActivity = new GroupedActivity[groups.Count];
                for (int i = 0; i < groupActivity.Length; i++)
                {
                    var group = groups[i];
                    groupActivity[i].Name = group.Name;
                    groupActivity[i].Activity = Array.ConvertAll(group.Regions, index =>
                    {
                        if (index >= input.Activity.Length)
                        {
                            throw new InvalidOperationException("Region not found in raw activity data. Please make sure all regions have been correctly defined.");
                        }

                        return input.Activity[index];
                    });
                }

                var result = new GroupedPhotometryDataFrame();
                result.Image = input.Image;
                result.FrameCounter = input.FrameCounter;
                result.Timestamp = input.Timestamp;
                result.Flags = input.Flags;
                result.Groups = groupActivity;
                return result;
            });
        }
    }

    public class GroupedRegions
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [TypeConverter(typeof(UnidimensionalArrayConverter))]
        public int[] Regions { get; set; }
    }

    public class GroupedPhotometryDataFrame
    {
        public IplImage Image;
        public long FrameCounter;
        public double Timestamp;
        public FrameFlags Flags;
        public GroupedActivity[] Groups;
    }

    public struct GroupedActivity
    {
        public string Name { get; set; }

        public RegionActivity[] Activity { get; set; }
    }
}
