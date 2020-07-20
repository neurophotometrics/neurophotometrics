using Bonsai.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Bonsai;
using System.ComponentModel;
using OpenCV.Net;

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

        [Description("Indicates whether to generate an image file with labelled photometry region boundaries.")]
        public bool IncludeRegions { get; set; }

        [Description("Indicates whether to generate an image file with a summary chart of logged photometry data.")]
        public bool IncludeChart { get; set; }

        FileSink ChartWriter<TSource>(ref IObservable<TSource> source)
        {
            if (IncludeChart)
            {
                var type = Type.GetType("Neurophotometrics.Design.ChartWriter, Neurophotometrics.Design", true);
                var chartWriter = (FileSink)Activator.CreateInstance(type);
                var processMethod = type.GetMethod(nameof(Process), new[] { typeof(IObservable<TSource>) });
                source = (IObservable<TSource>)processMethod.Invoke(chartWriter, new[] { source });
                return chartWriter;
            }
            else return null;
        }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            var sink = new PhotometrySink(this)
            {
                FileName = FileName,
                Suffix = Suffix,
                Buffered = Buffered,
                Overwrite = Overwrite,
                ChartWriter = ChartWriter(ref source)
            };
            return sink.Process(source);
        }

        public IObservable<GroupedPhotometryDataFrame> Process(IObservable<GroupedPhotometryDataFrame> source)
        {
            var sink = new GroupedPhotometrySink(this)
            {
                FileName = FileName,
                Suffix = Suffix,
                Buffered = Buffered,
                Overwrite = Overwrite,
                ChartWriter = ChartWriter(ref source)
            };
            return sink.Process(source);
        }

        static IplImage GetColorCopy(IplImage image)
        {
            if (image.Depth != IplDepth.U8)
            {
                var temp = new IplImage(image.Size, IplDepth.U8, image.Channels);
                CV.ConvertScale(image, temp, (double)byte.MaxValue / ushort.MaxValue);
                image = temp;
            }

            var output = new IplImage(image.Size, IplDepth.U8, 3);
            if (image.Channels == 1) CV.CvtColor(image, output, ColorConversion.Gray2Bgr);
            else CV.Copy(image, output);
            return output;
        }

        static string GetModeLabel(RegionMode mode)
        {
            switch (mode)
            {
                case RegionMode.Red: return "R";
                case RegionMode.Green: return "G";
                default: return string.Empty;
            }
        }

        static void SaveImage(string fileName, IplImage image)
        {
            fileName = Path.ChangeExtension(fileName, ".jpg");
            CV.SaveImage(fileName, image);
        }

        class PhotometrySink : FileSink<PhotometryDataFrame, StreamWriter>
        {
            internal PhotometrySink(PhotometryWriter writer)
            {
                Writer = writer;
            }

            internal PhotometryWriter Writer { get; private set; }

            internal FileSink ChartWriter { get; set; }

            protected override StreamWriter CreateWriter(string fileName, PhotometryDataFrame input)
            {
                if (ChartWriter != null)
                {
                    ChartWriter.FileName = fileName;
                }

                if (Writer.IncludeRegions)
                {
                    var image = GetColorCopy(input.Image);
                    GraphicsHelper.LabelBitmap(image, input.Activity);
                    SaveImage(fileName, image);
                }

                var activity = input.Activity;
                var writer = new StreamWriter(fileName, false, Encoding.ASCII);
                var columns = new List<string>(activity.Length + MetadataOffset);
                columns.Add(nameof(input.FrameCounter));
                columns.Add(nameof(input.Timestamp));
                columns.Add(nameof(input.Flags));
                for (int i = 0; i < activity.Length; i++)
                {
                    var modeLabel = GetModeLabel(activity[i].Region.Mode);
                    columns.Add(RegionLabel + activity[i].Region.Index + modeLabel);
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
            internal GroupedPhotometrySink(PhotometryWriter writer)
            {
                Writer = writer;
            }

            internal PhotometryWriter Writer { get; private set; }

            internal FileSink ChartWriter { get; set; }

            protected override StreamWriter CreateWriter(string fileName, GroupedPhotometryDataFrame input)
            {
                if (ChartWriter != null)
                {
                    ChartWriter.FileName = fileName;
                }

                var groups = input.Groups;
                var image = Writer.IncludeRegions ? GetColorCopy(input.Image) : null;
                var writer = new StreamWriter(fileName, false, Encoding.ASCII);
                var columns = new List<string>(groups.Length + MetadataOffset);
                columns.Add(nameof(input.FrameCounter));
                columns.Add(nameof(input.Timestamp));
                columns.Add(nameof(input.Flags));
                for (int i = 0; i < groups.Length; i++)
                {
                    var group = groups[i];
                    if (image != null) GraphicsHelper.LabelBitmap(image, group.Activity, group.Name);
                    for (int j = 0; j < group.Activity.Length; j++)
                    {
                        var modeLabel = GetModeLabel(group.Activity[j].Region.Mode);
                        columns.Add(group.Name + group.Activity[j].Region.Index + modeLabel);
                    }
                }

                if (image != null) SaveImage(fileName, image);
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
