using Bonsai;
using Bonsai.Expressions;
using System;
using System.Linq.Expressions;
using Bonsai.Harp;
using System.ComponentModel;

namespace Neurophotometrics
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Creates standard command messages for FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<Command>))]
    public class Command : SelectBuilder, INamedElement
    {
        [Description("Specifies which command to send to the device.")]
        public CommandType Type { get; set; } = CommandType.Stimulation;

        [Description("Specifies whether to start or stop the command.")]
        public CommandMode Mode { get; set; } = CommandMode.Start;

        string INamedElement.Name => $"{Type}.{Mode}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case CommandType.Stimulation: return "Controls the start and stop of stimulation.";
                    case CommandType.ExternalCamera: return "Starts or stops the camera triggers on DO1.";
                    case CommandType.Photodiodes: return "Starts or stops the streaming of photodiode values.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            int address;
            var commandType = Type;
            switch (commandType)
            {
                case CommandType.Stimulation: address = Registers.StimStart; break;
                case CommandType.ExternalCamera: address = Registers.ExtCameraStart; break;
                case CommandType.Photodiodes: address = Registers.PhotodiodeStart; break;
                default:
                    throw new InvalidOperationException("Invalid or unsupported command type.");
            }

            var instance = Expression.Constant(this);
            return Expression.Call(instance, nameof(CreateCommand), null, Expression.Constant(address));
        }

        HarpMessage CreateCommand(int address)
        {
            return HarpCommand.WriteByte(address, (byte)Mode);
        }
    }

    public enum CommandType : byte
    {
        Stimulation,
        ExternalCamera,
        Photodiodes
    }

    public enum CommandMode : byte
    {
        Stop = 0,
        Start = 1
    }
}
