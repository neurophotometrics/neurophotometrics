using Bonsai;
using Bonsai.Design;
using Bonsai.Expressions;
using Neurophotometrics.Design.V1.Visualizers;
using Neurophotometrics.V1;
using Neurophotometrics.V1.Definitions;
using System;

[assembly: TypeVisualizer(typeof(ActivityVisualizer), Target = typeof(PhotometryData))]

namespace Neurophotometrics.Design.V1.Visualizers
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
            if (value is PhotometryDataFrame dataFrame)
            {
                View.TryUpdateNewFrame(dataFrame);
            }
        }

        public override void Unload()
        {
            Node.VisualizerSettings = View.VisualizerSettings;
            View.Dispose();
        }
    }
}