using Bonsai;
using Bonsai.Expressions;
using System;
using System.Linq.Expressions;
using Bonsai.Harp;
using System.ComponentModel;

namespace Neurophotometrics
{
    [Description(
        "StartAcquisition: Any\n" +
        "StopAcquisition: Any\n" +
        "SetDigitalOutputs: Bitmask\n" +
        "ClearDigitalOutputs: Bitmask\n" +
        "ToggleDigitalOutputs: Bitmask\n" +
        "WriteDigitalOutputs: Bitmask\n" +
        "StartStimulation: Any\n" +
        "StopStimulation: Any\n" +
        "StartExternalCamera: Any\n" +
        "StopExternalCamera: Any")]
    public class FP3002Command : SelectBuilder, INamedElement
    {
        public FP3002Command()
        {
            Type = FP3002CommandType.StartAcquisition;
        }

        [Description("Specifies the FP3002 command type.")]
        public FP3002CommandType Type { get; set; }

        string INamedElement.Name
        {
            get { return typeof(FP3002Command).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            var commandType = Type;
            switch (commandType)
            {
                case FP3002CommandType.SetDigitalOutputs:
                case FP3002CommandType.ClearDigitalOutputs:
                case FP3002CommandType.ToggleDigitalOutputs:
                case FP3002CommandType.WriteDigitalOutputs:
                    return BuildCommand(commandType, typeof(byte), expression);
                case FP3002CommandType.StartAcquisition:
                case FP3002CommandType.StopAcquisition:
                case FP3002CommandType.StartAdc:
                case FP3002CommandType.StopAdc:
                case FP3002CommandType.StartStimulation:
                case FP3002CommandType.StopStimulation:
                case FP3002CommandType.StartExternalCamera:
                case FP3002CommandType.StopExternalCamera:
                    expression = Expression.Constant(((int)commandType >> 8) & 0xFF);
                    return BuildCommand(commandType, typeof(byte), expression);
                default:
                    throw new InvalidOperationException("Invalid or unsupported command type.");
            }
        }

        static Expression BuildCommand(FP3002CommandType commandType, Type type, Expression expression)
        {
            if (expression.Type != type) expression = Expression.Convert(expression, type);
            return Expression.Call(typeof(FP3002Command), nameof(ProcessPayload), null, Expression.Constant(commandType), expression);
        }

        static HarpMessage ProcessPayload(FP3002CommandType commandType, byte input)
        {
            return new HarpMessage(true, (byte)MessageType.Write, 5, (byte)commandType, 255, (byte)PayloadType.U8, input, 0);
        }
    }

    public enum FP3002CommandType : ushort
    {
        StartAcquisition = Registers.Start | 0x100,
        StopAcquisition = Registers.Start | 0x400,

        SetDigitalOutputs = 54,
        ClearDigitalOutputs = 55,
        ToggleDigitalOutputs = 56,
        WriteDigitalOutputs = 57,

        StartAdc = Registers.Adc | 0x100,
        StopAdc = Registers.Adc,

        StartStimulation = 61 | 0x100,
        StopStimulation = 61,

        StartExternalCamera = 65 | 0x100,
        StopExternalCamera = 65
    }
}
