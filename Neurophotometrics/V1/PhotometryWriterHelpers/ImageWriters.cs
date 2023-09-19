using Neurophotometrics.V1.Definitions;

using System;
using System.Reactive.Linq;

namespace Neurophotometrics.V1.PhotometryWriterHelpers
{
    public sealed class ImageWriters
    {
        public string ParentDirectory { get; set; }
        public string SuffixInstance { get; set; }
        public bool IncludeRegions { get; internal set; }
        public bool IncludeVideo { get; internal set; }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                RegionImageWriter regionImageWriter = null;
                TiffWriter videoWriter = null;

                return source.Do(input =>
                {
                    if (videoWriter == null && IncludeVideo)
                    {
                        videoWriter = new TiffWriter(ParentDirectory, SuffixInstance);
                        videoWriter.TryInitializeTiff(input);
                    }
                    if (regionImageWriter == null && IncludeRegions)
                    {
                        regionImageWriter = new RegionImageWriter(ParentDirectory, SuffixInstance);
                        regionImageWriter.TryStoreFirstRegionImage(input);
                    }

                    if (videoWriter != null)
                        videoWriter.TryWriteNewFrame(input);
                    if (regionImageWriter != null)
                        regionImageWriter.TryStoreCurrentRegionImage(input);
                }).Finally(() =>
                {

                    if (videoWriter != null)
                        videoWriter.TryClose();

                    if (regionImageWriter != null)
                        regionImageWriter.TryClose();
                });
            });
        }
    }
}