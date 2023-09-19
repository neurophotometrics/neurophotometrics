using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System.ComponentModel;
using System.Linq.Expressions;

namespace Neurophotometrics.V2
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Controls the state of laser pulse stimulation in FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<Stimulation>))]
    public class Stimulation : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies the command used to control laser pulse stimulation.")]
        public StimulationCommand Command { get; set; } = StimulationCommand.Stop;

        string INamedElement.Name
        {
            get
            {
                return $"{nameof(Stimulation)}.{Command}";
            }
        }

        [Browsable(false)]
        public string Description
        {
            get
            {
                switch (Command)
                {
                    case StimulationCommand.Stop: return "Stops laser pulse stimulation.";
                    case StimulationCommand.StartFinite: return "Starts a finite laser pulse stimulation.";
                    case StimulationCommand.StartContinuous: return "Starts continuous laser pulse stimulation.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            var instance = Expression.Constant(this);
            return Expression.Call(instance, nameof(CreateCommand), null);
        }

        private HarpMessage CreateCommand()
        {
            return HarpCommand.WriteByte(Registers.StimStart, (byte)Command);
        }
    }
}