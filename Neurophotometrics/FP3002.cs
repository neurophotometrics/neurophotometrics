using Bonsai;
using Bonsai.Spinnaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpinnakerNET;
using System.Reactive.Linq;
using Bonsai.Harp;
using System.Reactive.Subjects;
using System.ComponentModel;
using System.IO.Ports;
using System.Drawing.Design;
using OpenCV.Net;
using System.Reactive;

namespace Neurophotometrics
{
    [Description("Generates a sequence of photometry and auxiliary data from an FP3002 device.")]
    [Editor("Neurophotometrics.Design.FP3002CalibrationEditor, Neurophotometrics.Design", typeof(ComponentEditor))]
    public class FP3002 : Source<HarpMessage>
    {
        readonly CreateHarpMessage start = new CreateHarpMessage { Address = Registers.Start, Payload = 1 };
        readonly CreateHarpMessage stop = new CreateHarpMessage { Address = Registers.Start, Payload = 4 };
        readonly Device board = new Device();
        readonly SpinnakerCapture capture = new FP3002SpinnakerCapture();
        readonly Photometry photometry = new Photometry();

        [Description("The name of the serial port used to communicate with the Harp device.")]
        [TypeConverter(typeof(PortNameConverter))]
        public string PortName
        {
            get { return board.PortName; }
            set { board.PortName = value; }
        }

        [Description("The regions of interest used to configure independent photometry data channels.")]
        [Editor("Neurophotometrics.Design.PhotometryDataFrameRoiEditor, Neurophotometrics.Design", typeof(UITypeEditor))]
        public RotatedRect[] Regions
        {
            get { return photometry.Regions; }
            set { photometry.Regions = value; }
        }

        class FP3002SpinnakerCapture : SpinnakerCapture
        {
            protected override void Configure(IManagedCamera camera)
            {
                base.Configure(camera);
                camera.GainAuto.Value = GainAutoEnums.Off.ToString();
                camera.ExposureAuto.Value = ExposureAutoEnums.Off.ToString();
                camera.ExposureMode.Value = ExposureModeEnums.TriggerWidth.ToString();
                camera.BalanceWhiteAuto.Value = BalanceWhiteAutoEnums.Off.ToString();
                camera.TriggerSource.Value = TriggerSourceEnums.Line0.ToString();
                camera.Gain.Value = 0;
            }
        }

        public override IObservable<HarpMessage> Generate()
        {
            return Generate(Observable.Empty<HarpMessage>());
        }

        public IObservable<HarpMessage> Generate(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var startCommand = start.Generate().Publish();
                var stopCommand = stop.Generate().Publish();
                var messages = board.Generate(source.Merge(startCommand.Concat(stopCommand)));
                var frames = capture.Generate(startCommand.RefCount());

                return messages.Publish(ps => ps.Merge(
                    photometry.Process(frames).Zip(
                        ps.Where(m => m.Address == Registers.Trigger && m.MessageType == MessageType.Event),
                        (f, m) => new PhotometryHarpMessage(f, m))
                        .Finally(() => stopCommand.Connect())));
            });
        }

        class PortNameConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(SerialPort.GetPortNames());
            }
        }
    }

    public class PhotometryHarpMessage : HarpMessage
    {
        internal PhotometryHarpMessage(PhotometryDataFrame frame, HarpMessage message)
            : base(true, CreateMessage(frame, message))
        {
            frame.Timestamp = message.GetTimestamp();
            frame.TriggerEvents = (TriggerEvents)message.MessageBytes[11];
            PhotometryData = frame;
        }

        public PhotometryDataFrame PhotometryData { get; private set; }

        static byte[] CreateMessage(PhotometryDataFrame frame, HarpMessage message)
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
