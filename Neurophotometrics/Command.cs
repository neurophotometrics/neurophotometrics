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

        [Description("Specifies whether to start, stop, or control the command directly from the input value.")]
        public CommandMode Mode { get; set; } = CommandMode.Start;

        string INamedElement.Name => $"{Type}.{Mode}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case CommandType.Stimulation: return "Starts or stops the stimulation.";
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
            var addressValue = Expression.Constant(address);
            if (Mode != CommandMode.Control) return Expression.Call(instance, nameof(CreateCommand), null, addressValue);
            else return Expression.Call(instance, nameof(CreateCommand), null, addressValue, expression);
        }

        HarpMessage CreateCommand(int address)
        {
            return HarpCommand.WriteByte(address, (byte)Mode);
        }

        HarpMessage CreateCommand(int address, bool mode)
        {
            return HarpCommand.WriteByte(address, (byte)(mode ? 1 : 0));
        }

        HarpMessage CreateCommand(int address, byte mode)
        {
            return HarpCommand.WriteByte(address, (byte)mode);
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
        Start = 1,
        Control = 2
    }
}
