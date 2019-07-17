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
        const string ListSeparator = ",";
        const string FrameCounterLabel = "FrameCounter";
        const string ChannelLabel = "Channel";
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
                var columns = new string[activity.Length + 1];
                columns[0] = FrameCounterLabel;
                for (int i = 0; i < activity.Length; i++)
                {
                    var color = activity[i].Region.Center.X < halfWidth ? RedLabel : GreenLabel;
                    columns[i + 1] = ChannelLabel + i + color;
                }

                var header = string.Join(ListSeparator, columns);
                writer.WriteLine(header);
                return writer;
            }

            protected override void Write(StreamWriter writer, PhotometryDataFrame input)
            {
                var activity = input.Activity;
                var values = new string[activity.Length + 1];
                values[0] = input.FrameCounter.ToString(CultureInfo.InvariantCulture);
                for (int i = 0; i < activity.Length; i++)
                {
                    values[i + 1] = activity[i].Activity.ToString(CultureInfo.InvariantCulture);
                }

                var line = string.Join(ListSeparator, values);
                writer.WriteLine(line);
            }
        }

        class GroupedPhotometrySink : FileSink<GroupedPhotometryDataFrame, StreamWriter>
        {
            protected override StreamWriter CreateWriter(string fileName, GroupedPhotometryDataFrame input)
            {
                var activity = input.Activity;
                var halfWidth = input.Image.Width / 2f;
                var writer = new StreamWriter(fileName, false, Encoding.ASCII);
                var columns = new List<string>(activity.Length + 1);
                columns.Add(FrameCounterLabel);
                for (int i = 0; i < activity.Length; i++)
                {
                    var group = activity[i];
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
                var activity = input.Activity;
                var values = new List<string>(activity.Length + 1);
                values.Add(input.FrameCounter.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < activity.Length; i++)
                {
                    var group = activity[i];
                    for (int j = 0; j < group.Activity.Length; j++)
                    {
                        values.Add(group.Activity[j].Activity.ToString(CultureInfo.InvariantCulture));
                    }
                }

                var line = string.Join(ListSeparator, values);
                writer.WriteLine(line);
            }
        }
    }
}
