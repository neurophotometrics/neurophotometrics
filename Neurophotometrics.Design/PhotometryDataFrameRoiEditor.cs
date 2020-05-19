using Bonsai.Vision.Design;
using System;
using System.Linq;
using OpenCV.Net;
using System.Reactive.Linq;
using Bonsai.Harp;

namespace Neurophotometrics.Design
{
    public class PhotometryDataFrameRoiEditor : IplImageEllipseRoiEditor
    {
        const byte PhotometryEvent = 84;

        public PhotometryDataFrameRoiEditor()
            : base(DataSource.Output)
        {
        }

        protected override IObservable<IplImage> GetImageSource(IObservable<IObservable<object>> dataSource)
        {
            return dataSource.Merge().Select(input =>
            {
                var harpMessage = input as HarpMessage;
                if (harpMessage != null)
                {
                    return harpMessage.Address == PhotometryEvent
                        ? ((PhotometryHarpMessage)harpMessage).PhotometryData.Image : null;
                }

                return ((PhotometryDataFrame)input).Image;
            }).Where(image => image != null);
        }
    }
}
