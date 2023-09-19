using Bonsai;
using Bonsai.IO;

using Neurophotometrics.Properties;
using Neurophotometrics.V1.Definitions;
using Neurophotometrics.V1.PhotometryWriterHelpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace Neurophotometrics.V1
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Sink)]
    [Description("Writes photometry data frames into a CSV text file.")]
    public class PhotometryWriter
    {
        private bool _IncludeVideo;
        private bool _IncludePlots;

        [Description("Indicates whether to generate an image file with labeled photometry region boundaries.")]
        public bool IncludeRegions { get; set; }

        [Description("Indicates whether to generate an image file with a summary chart of logged photometry data.")]
        public bool IncludePlots
        {
            get { return _IncludePlots; }
            set
            {
                if (value)
                {
                    var res = MessageBox.Show(Resources.MsgBox_Question_IncludePlots, "Include Plots", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.No)
                        return;
                }
                _IncludePlots = value;
            }
        }

        [Description("Indicates whether to generate multi-page Tiff files to contain the raw footage coming from the photometry system.")]
        public bool IncludeVideo
        {
            get { return _IncludeVideo; }
            set
            {
                if (value)
                {
                    var res = MessageBox.Show(Resources.MsgBox_Question_IncludeVideo, "Include Video", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.No)
                        return;
                }
                _IncludeVideo = value;
            }
        }

        [Description("The name of the output photometry data CSV file.")]
        [FileNameFilter("CSV (Comma delimited)|*.csv|All Files|*.*")]
        [Editor("Bonsai.Design.SaveFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string FileName { get; set; }

        [Description("The suffix used to generate file names.")]
        public PathSuffix Suffix { get; set; }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            var suffixInstance = SuffixHelper.GetSuffix(FileName, Suffix);
            var parentDirectory = GetParentDirectory(suffixInstance);
            var sink = new PhotometrySink()
            {
                FileName = FileName,
                ParentDirectory = parentDirectory,
                SuffixInstance = suffixInstance,
                ImageWriters = CreateImageWriters(ref source, parentDirectory, suffixInstance),
                PlotsWriter = CreatePlotsWriter(ref source)
            };
            return sink.Process(source);
        }
        private string GetParentDirectory(string suffixInstance)
        {

            var filename = FileName;

            if (string.IsNullOrEmpty(filename))
                throw new InvalidOperationException("A valid file path must be specified.");

            var dir_info = new DirectoryInfo(filename);
            var absPath = dir_info.FullName;

            var isMultiFile = GetIsMultipleFiles();
            if (!isMultiFile)
                return absPath.Remove(absPath.Length - dir_info.Name.Length);

            var extension = dir_info.Extension;
            absPath = absPath.Remove(absPath.Length - extension.Length);
            absPath = SuffixHelper.AppendSuffix(absPath, suffixInstance);
            if (!Directory.Exists(absPath))
                Directory.CreateDirectory(absPath);
            else
                throw new InvalidOperationException(string.Format("The path '{0}' already exists.", absPath));

            return absPath;
        }

        private bool GetIsMultipleFiles()
        {
            return IncludePlots || IncludeRegions || IncludeVideo;
        }

        private ImageWriters CreateImageWriters(ref IObservable<PhotometryDataFrame> source, string parentDirectory, string suffixInstance)
        {
            if (IncludeRegions || IncludeVideo)
            {
                var type = Type.GetType("Neurophotometrics.V1.PhotometryWriterHelpers.ImageWriters, Neurophotometrics", true);
                var imageWriters = (ImageWriters)Activator.CreateInstance(type);
                imageWriters.IncludeRegions = IncludeRegions;
                imageWriters.IncludeVideo = IncludeVideo;
                imageWriters.ParentDirectory = parentDirectory;
                imageWriters.SuffixInstance = suffixInstance;
                var processMethod = type.GetMethod(nameof(Process), new[] { typeof(IObservable<PhotometryDataFrame>) });
                source = (IObservable<PhotometryDataFrame>)processMethod.Invoke(imageWriters, new[] { source });
                return imageWriters;
            }
            else return null;
        }
        private PlotsWriter CreatePlotsWriter(ref IObservable<PhotometryDataFrame> source)
        {
            if (IncludePlots)
            {
                var type = Type.GetType("Neurophotometrics.V1.PhotometryWriterHelpers.PlotsWriter, Neurophotometrics", true);
                var plotsWriter = (PlotsWriter)Activator.CreateInstance(type);
                var processMethod = type.GetMethod(nameof(Process), new[] { typeof(IObservable<PhotometryDataFrame>) });
                source = (IObservable<PhotometryDataFrame>)processMethod.Invoke(plotsWriter, new[] { source });
                return plotsWriter;
            }
            else return null;
        }
        private sealed class PhotometrySink
        {
            private const string ListSeparator = ",";
            private const string LedFlagsLabel = "LedState";
            internal string FileName { get; set; }
            internal string ParentDirectory { get; set; }
            internal string SuffixInstance { get; set; }
            internal ImageWriters ImageWriters { get; set; }
            internal PlotsWriter PlotsWriter { get; set; }

            public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
            {
                if (source == null)
                    throw new ArgumentNullException("source");

                return Observable.Create<PhotometryDataFrame>(observer =>
                {
                    var activityWriterDisposable = new ActivityWriterDisposable();
                    var process = source.Do(element =>
                    {
                        Action writeTask = () =>
                        {
                            try
                            {
                                if (activityWriterDisposable.Writer == null)
                                {
                                    activityWriterDisposable.Writer = CreateWriter(element);
                                }
                                Write(activityWriterDisposable.Writer, element);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                            }
                        };

                        activityWriterDisposable.Schedule(writeTask);
                    }).SubscribeSafe(observer);

                    return new CompositeDisposable(process, activityWriterDisposable);
                });
            }


            private StreamWriter CreateWriter(PhotometryDataFrame photometryDataFrame)
            {
                if (PlotsWriter != null)
                {
                    PlotsWriter.ParentDirectory = ParentDirectory;
                    PlotsWriter.SuffixInstance = SuffixInstance;
                }

                var directoryInfo = new DirectoryInfo(FileName);
                var absPath = $@"{ParentDirectory}\{directoryInfo.Name}";
                absPath = SuffixHelper.AppendSuffix(absPath, SuffixInstance);

                if (File.Exists(absPath))
                    throw new InvalidOperationException(string.Format("The path '{0}' already exists.", absPath));


                var writer = new StreamWriter(absPath, false, Encoding.ASCII);
                var columns = new List<string>()
                {
                    nameof(PhotometryDataFrame.FrameCounter),
                    nameof(PhotometryDataFrame.SystemTimestamp),
                    LedFlagsLabel,
                    nameof(PhotometryDataFrame.ComputerTimestamp)
                };
                foreach (var name in photometryDataFrame.Activities.Select(activity => activity.Region.Name))
                    columns.Add(name);

                var header = string.Join(ListSeparator, columns);
                writer.WriteLine(header);
                return writer;
            }
            private void Write(StreamWriter writer, PhotometryDataFrame photometryDataFrame)
            {
                var values = new List<string>()
                {
                    photometryDataFrame.FrameCounter.ToString(CultureInfo.InvariantCulture),
                    photometryDataFrame.SystemTimestamp.ToString(CultureInfo.InvariantCulture),
                    ((ushort)photometryDataFrame.Flags & 0x7).ToString(CultureInfo.InvariantCulture),
                    photometryDataFrame.ComputerTimestamp.ToString(CultureInfo.InvariantCulture)
                };
                foreach (var value in photometryDataFrame.Activities.Select(activity => activity.Value))
                    values.Add(value.ToString(CultureInfo.InvariantCulture));

                var newLine = string.Join(ListSeparator, values);
                writer.WriteLine(newLine);
            }

        }
    }
}