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
    [Description("Deinterleaves a photometry data stream using the specified event trigger.")]
    public class PhotometryData : Combinator<PhotometryDataFrame, PhotometryDataFrame>, INamedElement
    {
        [Description("The optional event trigger used to deinterleave the photometry data. If no event is specified, all photometry data frames are transmitted.")]
        public TriggerEvents? Filter { get; set; }

        string INamedElement.Name
        {
            get { return Filter.HasValue ? Filter.ToString() : null; }
        }

        public override IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return source.Where(input => (input.TriggerEvents & Filter.GetValueOrDefault((TriggerEvents)0xFF)) != 0);
        }

        public IObservable<PhotometryDataFrame> Process(IObservable<HarpMessage> source)
        {
            return Process(source.Where(evt => evt.Address == Registers.Photometry).Select(input => ((PhotometryHarpMessage)input).PhotometryData));
        }
    }
}
