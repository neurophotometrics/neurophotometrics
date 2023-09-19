using Bonsai.Harp;

namespace Neurophotometrics.V2.Definitions
{
    public class PhotometryHarpMessage : HarpMessage
    {
        public PhotometryDataFrame PhotometryData { get; private set; }
        public HarpMessage TriggerData { get; private set; }

        internal PhotometryHarpMessage(PhotometryDataFrame frame, HarpMessage message)
            : base(true, CreateMessage(frame, message))
        {
            var payload = message.GetTimestampedPayloadUInt16();
            frame.Flags = (FrameFlags)payload.Value;
            frame.SystemTimestamp = payload.Seconds;
            PhotometryData = frame;
            TriggerData = message;
        }

        private static byte[] CreateMessage(PhotometryDataFrame frame, HarpMessage message)
        {
            var counter = frame.FrameCounter;
            return new byte[]
            {
                (byte)MessageType.Event, // message type
                18, // message length = 3 bytes header + 14 bytes payload + 1 byte checksum
                Registers.Photometry, // header address
                (byte)message.Port, // header port
                (byte)PayloadType.TimestampedS64, // header payload type
                message.MessageBytes[5], // timestamp byte 0
                message.MessageBytes[6], // timestamp byte 1
                message.MessageBytes[7], // timestamp byte 2
                message.MessageBytes[8], // timestamp byte 3
                message.MessageBytes[9], // timestamp byte 4
                message.MessageBytes[10], // timestamp byte 5
                (byte)(counter >> (8 * 0)), // frame counter byte 0
                (byte)(counter >> (8 * 1)), // frame counter byte 1
                (byte)(counter >> (8 * 2)), // frame counter byte 2
                (byte)(counter >> (8 * 3)), // frame counter byte 3
                (byte)(counter >> (8 * 4)), // frame counter byte 4
                (byte)(counter >> (8 * 5)), // frame counter byte 5
                (byte)(counter >> (8 * 6)), // frame counter byte 6
                (byte)(counter >> (8 * 7)), // frame counter byte 7
                0 // checksum, will be updated by constructor
            };
        }
    }
}