using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Neurophotometrics
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Controls the state of laser pulse stimulation in FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<Stimulation>))]
    public class Stimulation : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies the command used to control laser pulse stimulation.")]
        public StimulationCommand Command { get; set; } = StimulationCommand.Stop;

        string INamedElement.Name => $"{nameof(Stimulation)}.{Command}";

        string Description
        {
            get
            {
                switch (Command)
                {
                    case StimulationCommand.Stop: return "Stops laser pulse stimulation.";
                    case StimulationCommand.StartFinite: return "Starts a finite laser pulse stimulation.";
                    case StimulationCommand.StartContinuous: return "Starts continuous laser pulse stimulation.";
                    case StimulationCommand.StartInterleave: return "Starts laser pulse stimulation during photometry dead time.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            var instance = Expression.Constant(this);
            return Expression.Call(instance, nameof(CreateCommand), null);
        }

        HarpMessage CreateCommand()
        {
            return HarpCommand.WriteByte(Registers.StimStart, (byte)Command);
        }
    }

    public enum StimulationCommand
    {
        Stop,
        StartFinite,
        StartContinuous,
        StartInterleave
    }
}
