using Bonsai.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Bonsai;
using System.ComponentModel;

namespace Neurophotometrics
{
    [Combinator]
    [DefaultProperty(nameof(FileName))]
    [Description("Writes photometry data frames into a CSV text file.")]
    public class PhotometryWriter : FileSink
    {
        const int MetadataOffset = 3;
        const string ListSeparator = ",";
        const string RegionLabel = "Region";
        const string RedLabel = "R";
        const string GreenLabel = "G";

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            var sink = new PhotometrySink
            {
                FileName = FileName,
                Suffix = Suffix,
                Buffered = Buffered,
                Overwrite = Overwrite
            };
            return sink.Process(source);
        }

        public IObservable<GroupedPhotometryDataFrame> Process(IObservable<GroupedPhotometryDataFrame> source)
        {
            var sink = new GroupedPhotometrySink
            {
                FileName = FileName,
                Suffix = Suffix,
                Buffered = Buffered,
                Overwrite = Overwrite
            };
            return sink.Process(source);
        }

        class PhotometrySink : FileSink<PhotometryDataFrame, StreamWriter>
        {
            protected override StreamWriter CreateWriter(string fileName, PhotometryDataFrame input)
            {
                var activity = input.Activity;
                var halfWidth = input.Image.Width / 2f;
                var writer = new StreamWriter(fileName, false, Encoding.ASCII);
                var columns = new List<string>(activity.Length + MetadataOffset);
                columns.Add(nameof(input.FrameCounter));
                columns.Add(nameof(input.Timestamp));
                columns.Add(nameof(input.Flags));
                for (int i = 0; i < activity.Length; i++)
                {
                    var color = activity[i].Region.Center.X < halfWidth ? RedLabel : GreenLabel;
                    columns.Add(RegionLabel + i + color);
                }

                var header = string.Join(ListSeparator, columns);
                writer.WriteLine(header);
                return writer;
            }

            protected override void Write(StreamWriter writer, PhotometryDataFrame input)
            {
                var activity = input.Activity;
                var values = new List<string>(activity.Length + MetadataOffset);
                values.Add(input.FrameCounter.ToString(CultureInfo.InvariantCulture));
                values.Add(input.Timestamp.ToString(CultureInfo.InvariantCulture));
                values.Add(((int)input.Flags).ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < activity.Length; i++)
                {
                    values.Add(activity[i].Value.ToString(CultureInfo.InvariantCulture));
                }

                var line = string.Join(ListSeparator, values);
                writer.WriteLine(line);
            }
        }

        class GroupedPhotometrySink : FileSink<GroupedPhotometryDataFrame, StreamWriter>
        {
            protected override StreamWriter CreateWriter(string fileName, GroupedPhotometryDataFrame input)
            {
                var groups = input.Groups;
                var halfWidth = input.Image.Width / 2f;
                var writer = new StreamWriter(fileName, false, Encoding.ASCII);
                var columns = new List<string>(groups.Length + MetadataOffset);
                columns.Add(nameof(input.FrameCounter));
                columns.Add(nameof(input.Timestamp));
                columns.Add(nameof(input.Flags));
                for (int i = 0; i < groups.Length; i++)
                {
                    var group = groups[i];
                    for (int j = 0; j < group.Activity.Length; j++)
                    {
                        var color = group.Activity[j].Region.Center.X < halfWidth ? RedLabel : GreenLabel;
                        columns.Add(group.Name + j + color);
                    }
                }

                var header = string.Join(ListSeparator, columns);
                writer.WriteLine(header);
                return writer;
            }

            protected override void Write(StreamWriter writer, GroupedPhotometryDataFrame input)
            {
                var groups = input.Groups;
                var values = new List<string>(groups.Length + MetadataOffset);
                values.Add(input.FrameCounter.ToString(CultureInfo.InvariantCulture));
                values.Add(input.Timestamp.ToString(CultureInfo.InvariantCulture));
                values.Add(((int)input.Flags).ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < groups.Length; i++)
                {
                    var group = groups[i];
                    for (int j = 0; j < group.Activity.Length; j++)
                    {
                        values.Add(group.Activity[j].Value.ToString(CultureInfo.InvariantCulture));
                    }
                }

                var line = string.Join(ListSeparator, values);
                writer.WriteLine(line);
            }
        }
    }
}
