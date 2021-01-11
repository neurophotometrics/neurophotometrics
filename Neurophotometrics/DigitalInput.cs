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
    [Description("Returns the sequence of state transitions for the digital inputs of FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<DigitalInput>))]
    public class DigitalInput : SingleArgumentExpressionBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which digital input state to read.")]
        public DigitalInputEvent Type { get; set; } = DigitalInputEvent.Input0;

        [Description("Specifies whether to include the Harp timestamp in the result.")]
        public bool IncludeTimestamp { get; set; }

        string INamedElement.Name => $"{nameof(DigitalInput)}.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case DigitalInputEvent.State: return "Reads the compound state of all digital inputs, as a bitmask.";
                    case DigitalInputEvent.Input0: return "Reads the state of digital input 0.";
                    case DigitalInputEvent.Input1: return "Reads the state of digital input 1.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            string methodName;
            var expression = arguments.First();
            switch (Type)
            {
                case DigitalInputEvent.State:
                    methodName = IncludeTimestamp ? nameof(GetTimestampedState) : nameof(GetState);
                    return Expression.Call(typeof(DigitalInput), methodName, null, expression);
                case DigitalInputEvent.Input0:
                case DigitalInputEvent.Input1:
                    var bitmask = Expression.Constant((byte)Type);
                    var eventMask = Expression.Constant((byte)((byte)Type << 4));
                    methodName = IncludeTimestamp ? nameof(GetTimestampedDigitalInput) : nameof(GetDigitalInput);
                    return Expression.Call(typeof(DigitalInput), methodName, null, expression, eventMask, bitmask);
                default:
                    throw new InvalidOperationException("Invalid or unsupported event type.");
            }
        }

        static IObservable<byte> GetState(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.InRead).Select(input => input.GetPayloadByte());
        }

        static IObservable<bool> GetDigitalInput(IObservable<HarpMessage> source, byte eventMask, byte bitmask)
        {
            return GetState(source).Where(state => (state & eventMask) == eventMask).Select(value => (value & bitmask) == bitmask);
        }

        static IObservable<Timestamped<byte>> GetTimestampedState(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.InRead).Select(input => input.GetTimestampedPayloadByte());
        }

        static IObservable<Timestamped<bool>> GetTimestampedDigitalInput(IObservable<HarpMessage> source, byte eventMask, byte bitmask)
        {
            return GetTimestampedState(source).Where(state => (state.Value & eventMask) == eventMask).Select(state =>
            {
                return Timestamped.Create((state.Value & bitmask) == bitmask, state.Seconds);
            });
        }
    }

    public enum DigitalInputEvent
    {
        State,
        Input1,
        Input0
    }
}
