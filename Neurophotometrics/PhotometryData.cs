using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics
{
    [Description("Selects the photometry data stream from an FP3002 device.")]
    public class PhotometryData : Combinator<HarpMessage, PhotometryDataFrame>
    {
        public override IObservable<PhotometryDataFrame> Process(IObservable<HarpMessage> source)
        {
            return source.Where(message => message.Address == Registers.PhotometryEvent)
                         .Select(message => ((PhotometryHarpMessage)message).PhotometryData);
        }
    }
}
