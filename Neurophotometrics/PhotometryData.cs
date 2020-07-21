using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Neurophotometrics
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    [Description("Deinterleaves a photometry data stream using the specified frame flags.")]
    public class PhotometryData : INamedElement
    {
        [Description("The optional frame flags used to deinterleave the photometry data. If no flags are specified, all photometry data frames are transmitted.")]
        public FrameFlags? Filter { get; set; }

        string INamedElement.Name
        {
            get { return Filter.HasValue ? Filter.ToString() : null; }
        }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            var filter = Filter.GetValueOrDefault(0);
            return filter != 0 ? source.Where(input => (input.Flags & filter) != 0) : source;
        }

        public IObservable<GroupedPhotometryDataFrame> Process(IObservable<GroupedPhotometryDataFrame> source)
        {
            var filter = Filter.GetValueOrDefault(0);
            return filter != 0 ? source.Where(input => (input.Flags & filter) != 0) : source;
        }
    }
}
