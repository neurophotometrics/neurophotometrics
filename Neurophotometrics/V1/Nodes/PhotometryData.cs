using Bonsai;

using Neurophotometrics.V1.Definitions;

using System;
using System.ComponentModel;

namespace Neurophotometrics.V1
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    [Description("Processes Harp Messages from the FP3001 node in order to extract photometry data from the incoming image data.")]
    public class PhotometryData
    {
        [Browsable(false)]
        public VisualizerSettings VisualizerSettings { get; set; }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return source;
        }
    }
}