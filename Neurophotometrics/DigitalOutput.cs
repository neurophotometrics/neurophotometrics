using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Neurophotometrics
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Controls the digital outputs of FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<DigitalOutput>))]
    public class DigitalOutput : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies the command used to control the digital outputs.")]
        public DigitalOutputCommand Command { get; set; } = DigitalOutputCommand.Set;

        [Description("Specifies which digital outputs to control.")]
        public DigitalOutputFlags Mask { get; set; } = DigitalOutputFlags.Output0;

        string INamedElement.Name => $"{nameof(DigitalOutput)}.{Command}";

        string Description
        {
            get
            {
                switch (Command)
                {
                    case DigitalOutputCommand.Set: return "Sets the specified digital outputs to HIGH.";
                    case DigitalOutputCommand.Clear: return "Sets the specified digital outputs to LOW";
                    case DigitalOutputCommand.Toggle: return "Toggles the specified digital outputs.";
                    case DigitalOutputCommand.Write: return "Sets the specified digital outputs to the specified state.";
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
            return HarpCommand.WriteByte(Registers.OutSet + (byte)Command, (byte)Mask);
        }
    }

    public enum DigitalOutputCommand
    {
        Set,
        Clear,
        Toggle,
        Write
    }

    [Flags]
    public enum DigitalOutputFlags : byte
    {
        None = 0 << 0,
        L415 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        Output1 = 1 << 3,
        Output0 = 1 << 4,
        Trigger = 1 << 5,
        Line2 = 1 << 6,
        Line3 = 1 << 7
    }
}
