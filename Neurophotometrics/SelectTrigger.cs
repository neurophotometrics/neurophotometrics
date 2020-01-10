using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    [Description("Selects photometry data acquired with the specified trigger.")]
    public class SelectTrigger : INamedElement
    {
        public SelectTrigger()
        {
            Trigger = TriggerEvents.L410;
        }

        [Description("The trigger event used to select photometry data.")]
        public TriggerEvents Trigger { get; set; }

        string INamedElement.Name
        {
            get { return Trigger != 0 ? Trigger.ToString() : typeof(SelectTrigger).Name; }
        }

        public IObservable<PhotometryDataFrame> Process(IObservable<PhotometryDataFrame> source)
        {
            return source.Where(frame => (frame.TriggerEvents & Trigger) != 0);
        }

        public IObservable<GroupedPhotometryDataFrame> Process(IObservable<GroupedPhotometryDataFrame> source)
        {
            return source.Where(frame => (frame.TriggerEvents & Trigger) != 0);
        }
    }
}
