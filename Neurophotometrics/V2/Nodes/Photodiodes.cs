using Bonsai;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Neurophotometrics.V2
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Reads the value from each of the built-in photodiodes in FP3002 devices, in the sequence P415, P470, P560.")]
    public class Photodiodes : Transform<HarpMessage, PhotodiodeDataFrame>
    {
        private const int NumPhotodiodes = 3;

        public override IObservable<PhotodiodeDataFrame> Process(IObservable<HarpMessage> source)
        {
            return source.Event(Registers.Photodiodes).Select(input =>
            {
                var payload = input.GetTimestampedPayloadArray<ushort>();

                var photodiodeDataFrame = new PhotodiodeDataFrame()
                {
                    PD415 = (ushort)AverageSamplesOfPhotodiode(payload.Value, 0),
                    PD470 = (ushort)AverageSamplesOfPhotodiode(payload.Value, 1),
                    PD560 = (ushort)AverageSamplesOfPhotodiode(payload.Value, 2),
                    SystemTimestamp = payload.Seconds,
                    ComputerTimestamp = DateTime.Now.TimeOfDay.TotalMilliseconds
                };
                return photodiodeDataFrame;
            });
        }

        private double AverageSamplesOfPhotodiode(ushort[] interleavedSamples, int photodiodeIndex)
        {
            var sum = 0.0;
            var count = 0;
            for (var i = 0; i < interleavedSamples.Length; i++)
            {
                if (i % NumPhotodiodes == photodiodeIndex)
                {
                    sum += interleavedSamples[i];
                    count++;
                }
            }
            return sum / count;
        }
    }
}