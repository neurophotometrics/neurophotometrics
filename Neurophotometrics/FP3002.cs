using Bonsai;
using System;
using System.Linq;
using SpinnakerNET;
using System.Reactive.Linq;
using Bonsai.Harp;
using System.ComponentModel;
using System.IO.Ports;
using OpenCV.Net;

namespace Neurophotometrics
{
    [Description("Generates a sequence of photometry and auxiliary data from an FP3002 device.")]
    [Editor("Neurophotometrics.Design.FP3002CalibrationEditor, Neurophotometrics.Design", typeof(ComponentEditor))]
    public class FP3002 : Source<HarpMessage>
    {
        readonly Device board;
        readonly FP3002SpinnakerCapture capture;
        readonly Photometry photometry;

        public FP3002()
        {
            photometry = new Photometry();
            capture = new FP3002SpinnakerCapture(photometry);
            board = new Device { DeviceState = DeviceState.Standby, DumpRegisters = false };
        }

        class FP3002SpinnakerCapture : AutoCropCapture
        {
            public FP3002SpinnakerCapture(Photometry photometry)
                : base(photometry)
            {
            }

            public double ExposureTime { get; set; }

            protected override void Configure(IManagedCamera camera)
            {
                camera.BinningSelector.Value = BinningSelectorEnums.All.ToString();
                camera.BinningHorizontalMode.Value = BinningHorizontalModeEnums.Average.ToString();
                camera.BinningVerticalMode.Value = BinningVerticalModeEnums.Average.ToString();
                camera.BinningHorizontal.Value = 2;
                camera.BinningVertical.Value = 2;
                base.Configure(camera);
                camera.V3_3Enable.Value = true;
                camera.AcquisitionFrameRateEnable.Value = false;
                camera.PixelFormat.Value = PixelFormatEnums.Mono8.ToString();
                camera.TriggerSelector.Value = TriggerSelectorEnums.FrameStart.ToString();
                camera.TriggerSource.Value = TriggerSourceEnums.Line0.ToString();
                camera.TriggerMode.Value = TriggerModeEnums.On.ToString();
                camera.TriggerOverlap.Value = TriggerOverlapEnums.ReadOut.ToString();
                camera.TriggerActivation.Value = TriggerActivationEnums.RisingEdge.ToString();
                camera.LineSelector.Value = LineSelectorEnums.Line1.ToString();
                camera.LineMode.Value = LineModeEnums.Output.ToString();
                camera.LineSource.Value = LineSourceEnums.ExposureActive.ToString();
                camera.ExposureAuto.Value = ExposureAutoEnums.Off.ToString();
                camera.ExposureMode.Value = ExposureModeEnums.Timed.ToString();
                camera.ExposureTime.Value = ExposureTime;
                camera.GainAuto.Value = GainAutoEnums.Off.ToString();
                camera.Gain.Value = 0;

                var effectiveTriggerPeriod = 1.0 / camera.AcquisitionFrameRate.Value;
                if (effectiveTriggerPeriod > ExposureTime)
                {
                    throw new InvalidOperationException("The camera acquisition rate cannot match the trigger frequency. Make sure ROIs are defined and AutoCrop is enabled.");
                }
            }
        }

        [Description("The name of the serial port used to communicate with the Harp device.")]
        [TypeConverter(typeof(PortNameConverter))]
        public string PortName
        {
            get { return board.PortName; }
            set { board.PortName = value; }
        }

        [Description("The regions of interest used to specify independent photometry data.")]
        [Editor("Neurophotometrics.Design.PhotometryRoiEditor, Neurophotometrics.Design", DesignTypes.UITypeEditor)]
        public RotatedRect[] Regions
        {
            get { return photometry.Regions; }
            set { photometry.Regions = value; }
        }

        [Description("Specifies whether to crop the imaging sensor around photometry regions to minimize data transfer bandwidth.")]
        public bool AutoCrop
        {
            get { return capture.AutoCrop; }
            set { capture.AutoCrop = value; }
        }

        public override IObservable<HarpMessage> Generate()
        {
            return Generate(Observable.Empty<HarpMessage>());
        }

        public IObservable<HarpMessage> Generate(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var readFps = Observable.Return(HarpCommand.ReadUInt16(Registers.TriggerPeriod));
                var activate = Observable.Return(HarpCommand.OperationControl(
                    DeviceState.Active,
                    board.LedState,
                    board.VisualIndicators,
                    board.Heartbeat,
                    replies: EnableType.Enable,
                    dumpRegisters: true)).Publish();
                var start = Observable.Return(HarpCommand.WriteByte(Registers.Start, 1)).Publish();
                var stop = Observable.Return(HarpCommand.WriteByte(Registers.Start, 8)).Publish();
                var deviceControl = Observable.Concat(readFps, activate, start, stop);
                var messages = board.Generate(deviceControl.Merge(source));
                var frames = capture.Generate(start.RefCount());

                return messages.Publish(ps => ps.Where(Registers.TriggerPeriod).FirstAsync().SelectMany(message =>
                {
                    const int ExposureSafetyMargin = 1000;
                    var triggerPeriod = message.GetPayloadUInt16();
                    capture.ExposureTime = triggerPeriod - ExposureSafetyMargin;
                    activate.Connect();
                    return ps.Merge(photometry.Process(frames).Zip(
                        ps.Event(Registers.FrameEvent),
                        (f, m) => new PhotometryHarpMessage(f, m))
                        .Finally(() => stop.Connect()));
                }));
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
            var payload = message.GetTimestampedPayloadUInt16();
            frame.Flags = (FrameFlags)payload.Value;
            frame.Timestamp = payload.Seconds;
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
