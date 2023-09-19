using Bonsai;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Neurophotometrics.V2
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    [Description("Processes Harp Messages from the FP3002 node in order to extract photometry data from the incoming image data.")]
    public class PhotometryData
    {
        [Browsable(false)]
        public VisualizerSettings VisualizerSettings { get; set; }

        public IObservable<PhotometryDataFrame> Process(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photometry).Select(input => ((PhotometryHarpMessage)input).PhotometryData);
        }
    }
}