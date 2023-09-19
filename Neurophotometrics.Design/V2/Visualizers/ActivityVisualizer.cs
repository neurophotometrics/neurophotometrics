using Bonsai;
using Bonsai.Design;
using Bonsai.Expressions;
using Neurophotometrics.Design.V2.Visualizers;
using Neurophotometrics.V2;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

[assembly: TypeVisualizer(typeof(ActivityVisualizer), Target = typeof(PhotometryData))]

namespace Neurophotometrics.Design.V2.Visualizers
{
    public class ActivityVisualizer : DialogTypeVisualizer
    {
        private PhotometryData Node;
        private ActivityView View;

        public override void Load(IServiceProvider provider)
        {
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            Node = (PhotometryData)ExpressionBuilder.GetWorkflowElement(context.Source);
            var settings = Node.VisualizerSettings;
            if (settings == null)
                settings = new VisualizerSettings();

            View = new ActivityView
            {
                StoredVisualizerSettings = settings
            };

            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            visualizerService?.AddControl(View);
        }

        public override void Show(object value)
        {
            if (value is ConcurrentQueue<VisualizerData> dataFrames)
            {
                View.TryUpdateNewFrame(dataFrames);
            }
        }

        public override void Unload()
        {
            Node.VisualizerSettings = View.VisualizerSettings;
            View.Dispose();
        }

        public override IObservable<object> Visualize(IObservable<IObservable<object>> source, IServiceProvider provider)
        {
            var visualizerControl = provider.GetService(typeof(IDialogTypeVisualizerService)) as Control;
            if (visualizerControl != null)
            {
                return Observable.Create<ConcurrentQueue<VisualizerData>>(observer =>
                {
                    var transport = CreateTransport(observer);
                    var sourceDisposable = new SingleAssignmentDisposable
                    {
                        Disposable = source.SelectMany(xs => xs.Select(input => (PhotometryDataFrame)input))
                                                        .Subscribe(transport.Write)
                    };

                    return new CompositeDisposable(sourceDisposable, transport);
                })
                .ObserveOn(visualizerControl)
                .Select(visualizerData => (object)visualizerData)
                .Do(Show, SequenceCompleted);
            }

            return source;
        }

        private ActivityVisualizerFactory CreateTransport(IObserver<ConcurrentQueue<VisualizerData>> observer)
        {
            var transport = new ActivityVisualizerFactory(observer);
            transport.Open();
            return transport;
        }
    }
}