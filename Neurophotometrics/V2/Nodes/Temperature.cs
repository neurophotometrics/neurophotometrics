using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Neurophotometrics.V2
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Reads the value from the temperature sensor on the FP3002, in degrees Celsius.")]
    public class Temperature : SingleArgumentExpressionBuilder
    {
        [Description("Specifies whether to include the Harp timestamp in the result.")]
        public bool IncludeTimestamp { get; set; }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var expression = arguments.First();
            var methodName = IncludeTimestamp ? nameof(GetTimestampedTemperature) : nameof(GetTemperature);
            return Expression.Call(typeof(Temperature), methodName, null, expression);
        }

        private static double FromRawTemperature(ushort value)
        {
            return 55 + (value - 16384) / 160.0;
        }

        private static IObservable<double> GetTemperature(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Temperature).Select(input =>
            {
                var payload = input.GetPayloadUInt16();
                return FromRawTemperature(payload);
            });
        }

        private static IObservable<Timestamped<double>> GetTimestampedTemperature(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Temperature).Select(input =>
            {
                var payload = input.GetTimestampedPayloadUInt16();
                return Timestamped.Create(FromRawTemperature(payload.Value), payload.Seconds);
            });
        }
    }
}