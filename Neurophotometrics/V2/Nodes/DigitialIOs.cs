using Bonsai;
using Bonsai.Harp;

using Neurophotometrics.V2.Definitions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Neurophotometrics.V2
{
    [WorkflowElementIcon(typeof(ElementCategory), "ElementIcon.Daq")]
    [Description("Returns the sequence of state transitions for the digital IO ports of FP3002 devices.")]
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<DigitalIOs>))]
    public class DigitalIOs : Transform<HarpMessage, DigitalIODataFrame>
    {

        private static Dictionary<byte, byte> ConvertFWByteToDigitalIOFlag = new Dictionary<byte, byte>()
        {
            { (byte)DigitalIOStreams.Input0, 0 },
            { (byte)DigitalIOStreams.Input1, 1 },
            { (byte)DigitalIOStreams.Output0, 2 },
            { (byte)DigitalIOStreams.Output1, 3 },
        };
        public bool Input0 { get; set; } = true;
        public bool Input1 { get; set; } = true;
        public bool Output0 { get; set; } = true;
        public bool Output1 { get; set; } = true;

        private byte EventMask;

        public override IObservable<DigitalIODataFrame> Process(IObservable<HarpMessage> source)
        {
            SetEventMask();
            return source.Event(Registers.InRead)
                .Select(msg => msg.GetTimestampedPayloadByte())
                .Where(IsInEventMask)
                .Select(GetDigitalIODataFrame);
        }

        private bool IsInEventMask(Timestamped<byte> message)
        {
            return (message.Value & EventMask) > 0;
        }

        private DigitalIODataFrame GetDigitalIODataFrame(Timestamped<byte> message)
        {
            var streamFlag = (byte)(message.Value >> 4);
            var streamName = Enum.GetName(typeof(DigitalIOStreams), streamFlag);
            var state = (message.Value & streamFlag) == streamFlag;
            var digitalIODataFrame = new DigitalIODataFrame()
            {
                DigitalIOName = streamName,
                DigitalIOFlag = ConvertFWByteToDigitalIOFlag[streamFlag],
                DigitalIOState = state,
                SystemTimestamp = message.Seconds,
                ComputerTimestamp = DateTime.Now.TimeOfDay.TotalMilliseconds
            };
            return digitalIODataFrame;
        }

        private void SetEventMask()
        {
            var input0Mask = Input0 ? (byte)DigitalIOStreams.Input0 : (byte)0;
            var input1Mask = Input1 ? (byte)DigitalIOStreams.Input1 : (byte)0;
            var output0Mask = Output0 ? (byte)DigitalIOStreams.Output0 : (byte)0;
            var output1Mask = Output1 ? (byte)DigitalIOStreams.Output1 : (byte)0;
            EventMask = (byte)((input0Mask | input1Mask | output0Mask | output1Mask) << 4);
        }

    }
}