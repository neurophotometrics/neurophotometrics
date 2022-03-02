using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Neurophotometrics
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Returns the sequence of state transitions for the digital IO ports of FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<DigitalIOs>))]
    public class DigitalIOs : SingleArgumentExpressionBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which digital IO state to read.")]
        public DigitalIOEvent Type { get; set; } = DigitalIOEvent.Input0;

        [Description("Specifies whether to include the Harp timestamp in the result.")]
        public bool IncludeTimestamp { get; set; }

        string INamedElement.Name => $"{nameof(DigitalIOs)}.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case DigitalIOEvent.State: return "Reads the compound state of all digital inputs, as a bitmask.";
                    case DigitalIOEvent.Input0: return "Reads the state of digital input 0.";
                    case DigitalIOEvent.Input1: return "Reads the state of digital input 1.";
                    case DigitalIOEvent.Output0: return "Reads the state of the digital output 0.";
                    case DigitalIOEvent.Output1: return "Reads the state of the digital output 1.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            string methodName;
            ConstantExpression bitmask;
            ConstantExpression eventMask;
            var expression = arguments.First();
            switch (Type)
            {
                case DigitalIOEvent.State:
                    methodName = IncludeTimestamp ? nameof(GetTimestampedState) : nameof(GetState);
                    return Expression.Call(typeof(DigitalIOs), methodName, null, expression);
                case DigitalIOEvent.Input0:
                case DigitalIOEvent.Input1:
                    bitmask = Expression.Constant((byte)Type);
                    eventMask = Expression.Constant((byte)((byte)Type << 4));
                    methodName = IncludeTimestamp ? nameof(GetTimestampedDigitalIO) : nameof(GetDigitalIO);
                    return Expression.Call(typeof(DigitalIOs), methodName, null, expression, eventMask, bitmask);
                case DigitalIOEvent.Output0:
                    bitmask = Expression.Constant((byte)8);
                    eventMask = Expression.Constant((byte)((byte)8 << 4));
                    methodName = IncludeTimestamp ? nameof(GetTimestampedDigitalIO) : nameof(GetDigitalIO);
                    return Expression.Call(typeof(DigitalIOs), methodName, null, expression, eventMask, bitmask);
                case DigitalIOEvent.Output1:
                    bitmask = Expression.Constant((byte)4);
                    eventMask = Expression.Constant((byte)((byte)4 << 4));
                    methodName = IncludeTimestamp ? nameof(GetTimestampedDigitalIO) : nameof(GetDigitalIO);
                    return Expression.Call(typeof(DigitalIOs), methodName, null, expression, eventMask, bitmask);
                default:
                    throw new InvalidOperationException("Invalid or unsupported event type.");
            }
        }

        static IObservable<byte> GetState(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.InRead).Select(input => input.GetPayloadByte());
        }

        static IObservable<bool> GetDigitalIO(IObservable<HarpMessage> source, byte eventMask, byte bitmask)
        {
            return GetState(source).Where(state => (state & eventMask) == eventMask).Select(value => (value & bitmask) == bitmask);
        }

        static IObservable<Timestamped<byte>> GetTimestampedState(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.InRead).Select(input => input.GetTimestampedPayloadByte());
        }

        static IObservable<Timestamped<bool>> GetTimestampedDigitalIO(IObservable<HarpMessage> source, byte eventMask, byte bitmask)
        {
            return GetTimestampedState(source).Where(state => (state.Value & eventMask) == eventMask).Select(state =>
            {
                return Timestamped.Create((state.Value & bitmask) == bitmask, state.Seconds);
            });
        }
    }

    public enum DigitalIOEvent
    {
        State,
        Input1,
        Input0,
        Output0,
        Output1
    }
}
