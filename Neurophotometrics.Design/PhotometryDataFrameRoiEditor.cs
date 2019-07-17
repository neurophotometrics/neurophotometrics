using Bonsai.Vision.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCV.Net;
using System.Reactive.Linq;

namespace Neurophotometrics.Design
{
    public class PhotometryDataFrameRoiEditor : IplImageEllipseRoiEditor
    {
        public PhotometryDataFrameRoiEditor()
            : base(DataSource.Output)
        {
        }

        protected override IObservable<IplImage> GetImageSource(IObservable<IObservable<object>> dataSource)
        {
            return dataSource.Merge().Select(input => ((PhotometryDataFrame)input).Image);
        }
    }
}
