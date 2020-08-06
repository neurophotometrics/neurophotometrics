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
    [Description("Filters and selects specific event messages from the FP3002.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<FP3002Event>))]
    public class FP3002Event : SingleArgumentExpressionBuilder, INamedElement
    {
        [Description("Specifies which event to select from the FP3002 data stream.")]
        public FP3002EventType Type { get; set; } = FP3002EventType.Photometry;

        string INamedElement.Name => $"{nameof(FP3002)}.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case FP3002EventType.Photometry: return "The photometry data extracted from the device.";
                    case FP3002EventType.FrameEvent: return "The raw trigger event used to synchronize photometry data.";
                    case FP3002EventType.Photodiodes: return "The photodiodes calibration data.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var expression = arguments.First();
            switch (Type)
            {
                case FP3002EventType.Photometry:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessPhotometry), null, expression);
                case FP3002EventType.FrameEvent:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessTrigger), null, expression);
                case FP3002EventType.Photodiodes:
                    return Expression.Call(typeof(FP3002Event), nameof(ProcessAdc), null, expression);
                default:
                    throw new InvalidOperationException("Invalid or unsupported event type.");
            }
        }

        static IObservable<byte> ProcessTrigger(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.FrameEvent).Select(input => input.GetPayloadByte());
        }

        static IObservable<ushort> ProcessAdc(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photodiodes).Select(input => input.GetPayloadUInt16());
        }

        static IObservable<PhotometryDataFrame> ProcessPhotometry(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photometry).Select(input => ((PhotometryHarpMessage)input).PhotometryData);
        }
    }

    public enum FP3002EventType : byte
    {
        Photometry = Registers.Photometry,
        FrameEvent = Registers.FrameEvent,
        Photodiodes = Registers.Photodiodes
    }
}
