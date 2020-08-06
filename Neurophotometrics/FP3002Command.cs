using Bonsai;
using Bonsai.Expressions;
using System;
using System.Linq.Expressions;
using Bonsai.Harp;
using System.ComponentModel;

namespace Neurophotometrics
{
    [WorkflowElementIcon("Bonsai:ElementIcon.Daq")]
    [Description("Creates standard command messages for FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<FP3002Command>))]
    public class FP3002Command : SelectBuilder, INamedElement
    {
        [Description("Specifies which command to send to the FP3002.")]
        public FP3002CommandType Type { get; set; } = FP3002CommandType.StimulationStart;

        [Description("The set of flags used to control the FP3002 command.")]
        public FP3002CommandFlags Flags { get; set; } = FP3002CommandFlags.Start;

        string INamedElement.Name => $"{nameof(FP3002)}.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case FP3002CommandType.StimulationStart: return "Controls the start and stop of stimulation.";
                    case FP3002CommandType.ExternalCameraStart: return "Starts or stops the camera triggers on DO1.";
                    case FP3002CommandType.OutputSet: return "Sets the specified digital outputs to HIGH.";
                    case FP3002CommandType.OutputClear: return "Sets the specified digital outputs to LOW";
                    case FP3002CommandType.OutputToggle: return "Toggles the specified digital outputs.";
                    case FP3002CommandType.OutputWrite: return "Sets the specified digital outputs to the specified state.";
                    case FP3002CommandType.PhotodiodeStart: return "Starts or stops the streaming of photodiode values.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            var commandType = Type;
            switch (commandType)
            {
                case FP3002CommandType.StimulationStart:
                case FP3002CommandType.ExternalCameraStart:
                case FP3002CommandType.OutputSet:
                case FP3002CommandType.OutputClear:
                case FP3002CommandType.OutputToggle:
                case FP3002CommandType.OutputWrite:
                case FP3002CommandType.PhotodiodeStart:
                    var address = Expression.Constant((int)commandType);
                    var messageType = Expression.Constant(MessageType.Write);
                    var payload = Expression.Constant((byte)Flags);
                    return Expression.Call(typeof(HarpMessage), nameof(HarpMessage.FromByte), null, address, messageType, payload);
                default:
                    throw new InvalidOperationException("Invalid or unsupported command type.");
            }
        }
    }

    public enum FP3002CommandType : byte
    {
        StimulationStart = Registers.StimStart,
        ExternalCameraStart = Registers.ExtCameraStart,

        OutputSet = Registers.OutSet,
        OutputClear = Registers.OutClear,
        OutputToggle = Registers.OutToggle,
        OutputWrite = Registers.OutWrite,

        PhotodiodeStart = Registers.PhotodiodeStart
    }

    [Flags]
    public enum FP3002CommandFlags : byte
    {
        None = 0 << 0,
        Start = 1 << 0,
        L410 = 1 << 0,
        L470 = 1 << 1,
        L560 = 1 << 2,
        Output0 = 1 << 3,
        Output1 = 1 << 4,
        CameraTrigger = 1 << 5,
        CameraGpio0 = 1 << 6,
        CameraGpio1 = 1 << 7
    }
}
