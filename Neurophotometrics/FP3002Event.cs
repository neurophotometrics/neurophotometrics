using Bonsai;
using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bonsai.Harp;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Neurophotometrics
{
    [Description(
        "Photometry: photometry data\n" +
        "Trigger: led state at frame trigger (bitmask)")]
    public class FP3002Event : SingleArgumentExpressionBuilder, INamedElement
    {
        public FP3002Event()
        {
            Type = FP3002EventType.Photometry;
        }

        [Description("Specifies which event type to select from the FP3002 data stream.")]
        public FP3002EventType Type { get; set; }

        string INamedElement.Name
        {
            get { return typeof(FP3002Event).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var expression = arguments.First();
            switch (Type)
            {
                case FP3002EventType.Photometry:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessPhotometry), null, expression);
                case FP3002EventType.Trigger:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessTrigger), null, expression);
                case FP3002EventType.Adc:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessAdc), null, expression);
                default:
                    throw new InvalidOperationException("Invalid or unsupported event type.");
            }
        }

        static bool IsPhotometryEvent(HarpMessage input) => IsEventMessage(input, FP3002EventType.Photometry);
        static bool IsTriggerEvent(HarpMessage input) => IsEventMessage(input, FP3002EventType.Trigger);
        static bool IsAdcEvent(HarpMessage input) => IsEventMessage(input, FP3002EventType.Adc);

        static bool IsEventMessage(HarpMessage input, FP3002EventType type)
        {
            return input.Address == (byte)type && input.MessageType == MessageType.Event && input.Error == false;
        }

        static IObservable<byte> ProcessTrigger(IObservable<HarpMessage> source)
        {
            return source.Where(IsTriggerEvent).Select(input => input.GetPayloadByte());
        }

        static IObservable<ushort> ProcessAdc(IObservable<HarpMessage> source)
        {
            return source.Where(IsAdcEvent).Select(input => input.GetPayloadUInt16());
        }

        static IObservable<PhotometryDataFrame> ProcessPhotometry(IObservable<HarpMessage> source)
        {
            return source.Where(IsPhotometryEvent).Select(input => ((PhotometryHarpMessage)input).PhotometryData);
        }
    }

    public enum FP3002EventType : byte
    {
        Photometry = Registers.Photometry,
        Trigger = Registers.Trigger,
        Adc = Registers.Adc
    }
}
