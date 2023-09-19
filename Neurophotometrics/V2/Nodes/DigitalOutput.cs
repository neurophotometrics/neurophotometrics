using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Neurophotometrics.V2
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
        public DigitalOutputFlag Mask { get; set; } = DigitalOutputFlag.Output0;

        string INamedElement.Name
        {
            get
            {
                return $"{nameof(DigitalOutput)}.{Command}";
            }
        }

        [Browsable(false)]
        public string Description
        {
            get
            {
                switch (Command)
                {
                    case DigitalOutputCommand.Set: return "Sets the specified digital outputs to HIGH.";
                    case DigitalOutputCommand.Clear: return "Sets the specified digital outputs to LOW";
                    case DigitalOutputCommand.Toggle: return "Toggles the specified digital outputs.";
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
            return HarpCommand.WriteByte(Registers.OutSet + (byte)Command, (byte)Mask);
        }
    }

    public enum DigitalOutputCommand
    {
        Set,
        Clear,
        Toggle
    }

    [Flags]
    public enum DigitalOutputFlag : byte
    {
        Output1 = 1 << 3,
        Output0 = 1 << 4
    }
}