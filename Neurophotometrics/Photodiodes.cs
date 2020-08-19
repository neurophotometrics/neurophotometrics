using Bonsai;
using Bonsai.Expressions;
using Bonsai.Harp;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Neurophotometrics
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Reads the value from each of the built-in photodiodes in FP3002 devices, in the sequence P410, P470, P560.")]
    public class Photodiodes : SingleArgumentExpressionBuilder
    {
        [Description("Specifies whether to include the Harp timestamp in the result.")]
        public bool IncludeTimestamp { get; set; }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var expression = arguments.First();
            var methodName = IncludeTimestamp ? nameof(GetTimestampedPhotodiodes) : nameof(GetPhotodiodes);
            return Expression.Call(typeof(Photodiodes), methodName, null, expression);
        }

        static Mat FromPayloadArray(ushort[] payload)
        {
            var result = new Mat(3, 4, Depth.U16, 1);
            using (var payloadHeader = Mat.CreateMatHeader(payload, 4, 3, Depth.U16, 1))
            {
                CV.Transpose(payloadHeader, result);
            }
            return result;
        }

        static IObservable<Mat> GetPhotodiodes(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photodiodes).Select(input =>
            {
                var payload = input.GetPayloadArray<ushort>();
                return FromPayloadArray(payload);
            });
        }

        static IObservable<Timestamped<Mat>> GetTimestampedPhotodiodes(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photodiodes).Select(input =>
            {
                var payload = input.GetTimestampedPayloadArray<ushort>();
                return Timestamped.Create(FromPayloadArray(payload.Value), payload.Seconds);
            });
        }
    }
}
