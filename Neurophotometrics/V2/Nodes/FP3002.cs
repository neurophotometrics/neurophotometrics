using Bonsai;
using Bonsai.Harp;
using Neurophotometrics.Properties;
using Neurophotometrics.V2.Definitions;
using Neurophotometrics.V2.FP3002Helpers;

using SpinnakerNET;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Neurophotometrics.V2
{
    [Description("Generates a sequence of photometry and auxiliary data from an FP3002 device.")]
    [Editor("Neurophotometrics.Design.V2.Forms.FP3002FormEditor, Neurophotometrics.Design", typeof(ComponentEditor))]
    public class FP3002 : Source<HarpMessage>
    {
        private const int WAIT_TIME = 10;
        private const int TIMEOUT = 1000;
        private const int NUM_REPEATS_FINDDEVICE = 15;

        private static readonly HarpMessage NullTrigger = HarpMessage.FromUInt16(Registers.FrameEvent, 0.0, MessageType.Event, 0, 0);
        private const AcquisitionModes StopModes = AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera;

        private readonly Photometry Photometry;
        private readonly CameraCapture Capture;
        private Device Board = new Device();
        public readonly DeviceInformation DeviceInfo;

        [Description("Specifies the initial photometry acquisition mode.")]
        public AcquisitionModes AcquisitionMode { get; set; } = AcquisitionModes.StartPhotometry;

        [Description("The name of the serial port used to communicate with the FP3002 device.")]
        [TypeConverter(typeof(PortNameConverter))]
        public string PortName
        {
            get { return Board.PortName; }
            set { Board.PortName = value; }
        }

        [Browsable(false)]
        public List<PhotometryRegion> Regions
        {
            get { return Photometry.Regions; }
            set { Photometry.Regions = value; }
        }

        private event EventHandler SequenceDisposed;

        private event EventHandler RegistersReadEvent;

        public FP3002()
        {
            DeviceInfo = new DeviceInformation();
            Photometry = new Photometry();
            Capture = new CameraCapture(Photometry);
        }

        public void SetExposureTime(ushort triggerTimeUpdateOutputs)
        {
            Capture.SetExposureTime(triggerTimeUpdateOutputs);
        }

        public void SetTriggerPeriod(ushort triggerPeriod)
        {
            Capture.SetTriggerPeriod(triggerPeriod);
        }

        public void SetAutoCrop(bool autoCrop)
        {
            Capture.SetAutoCrop(autoCrop);
        }

        public override IObservable<HarpMessage> Generate()
        {
            return Generate(Observable.Empty<HarpMessage>());
        }

        public IObservable<HarpMessage> Generate(IObservable<HarpMessage> source)
        {
            TryGetHarpVersion();
            TryFindSystem();
            return Observable.Defer(() => CreateObservableFactory(source));
        }

        private IObservable<HarpMessage> CreateObservableFactory(IObservable<HarpMessage> source)
        {
            var messagesToBoard = GetHarpMessagesToBoard(source).Replay().RefCount();
            var messagesFromBoard = Board.Generate(messagesToBoard);
            var messagesFromCapture = Capture.Generate();

            var outputMessages = GenerateOutputMessages(messagesFromBoard, messagesFromCapture)
                .Finally(SendLastCommand).Publish().RefCount();
            return outputMessages;
        }

        private void SendFirstCommand(HarpMessage lastReadMessage)
        {
            RegistersReadEvent?.Invoke(this, EventArgs.Empty);
        }

        private void SendLastCommand()
        {
            SequenceDisposed?.Invoke(this, EventArgs.Empty);
        }

        private IObservable<HarpMessage> GetHarpMessagesToBoard(IObservable<HarpMessage> source)
        {
            var firstAndLastCommands = GetFirstAndLastCommands();
            var harpMessagesToBoard = source.Merge(firstAndLastCommands);
            return harpMessagesToBoard;
        }

        private IObservable<HarpMessage> GetFirstAndLastCommands()
        {
            var registersRead = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => RegistersReadEvent += handler,
                handler => RegistersReadEvent -= handler)
                .Select(evt => true);
            var captureReady = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => Capture.ReadyToAcquireFrames += handler,
                handler => Capture.ReadyToAcquireFrames -= handler)
                .Select(evt => true);
            var firstCommand = registersRead.Zip(captureReady, (reg, capt) => reg && capt)
                .Where(input => input)
                .Do(input => Capture.VerifyThroughput())
                .Select(input => HarpCommand.WriteByte(Registers.Start, (byte)AcquisitionMode))
                .FirstAsync();
            var lastCommand = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => SequenceDisposed += handler,
                handler => SequenceDisposed -= handler)
                .Select(evt => HarpCommand.WriteByte(Registers.Start, (byte)StopModes));
            var firstAndLastCommands = Observable.Merge(firstCommand, lastCommand);
            return firstAndLastCommands;
        }

        private IObservable<HarpMessage> GenerateOutputMessages(IObservable<HarpMessage> messagesFromBoard, IObservable<PhotometryImage> messagesFromCapture)
        {
            var outputMessages = messagesFromBoard.Publish(boardMessages =>
            {
                var boardAndPhotometryMessages = boardMessages.Merge(ReadRegistersSequence(boardMessages).Do(SendFirstCommand)
                    .SelectMany(ProcessPhotometryMessages(boardMessages, messagesFromCapture)));
                return boardAndPhotometryMessages;
            });

            return outputMessages;
        }

        private IObservable<HarpMessage> ReadRegistersSequence(IObservable<HarpMessage> boardMessages)
        {
            var readRegistersSequence = boardMessages.Do(message => ReadRegister(message))
                .Where(IsLastReadRegister).FirstAsync();
            return readRegistersSequence;
        }

        private IObservable<HarpMessage> ProcessPhotometryMessages(IObservable<HarpMessage> messagesFromBoard, IObservable<PhotometryImage> messagesFromCapture)
        {
            var photometryMessages = Photometry.Process(messagesFromCapture)
                .Zip(GetFrameEvents(messagesFromBoard), (photometryDataFrame, frameEvent) => new PhotometryHarpMessage(photometryDataFrame, frameEvent))
                .Where(IsNotNullFrame);
            return photometryMessages;
        }

        private IObservable<HarpMessage> GetFrameEvents(IObservable<HarpMessage> messagesFromBoard)
        {
            var frameEvents = messagesFromBoard.Event(Registers.FrameEvent).FillMissing(NullTrigger);
            return frameEvents;
        }

        private void ReadRegister(HarpMessage message)
        {
            switch (message.Address)
            {
                case Registers.TriggerPeriod:
                    var triggerPeriodMicros = message.GetPayloadUInt16();
                    Capture.SetTriggerPeriod(triggerPeriodMicros);
                    break;

                case Registers.TriggerTimeUpdateOutputs:
                    var triggerTimeUpdateOutputs = message.GetPayloadUInt16();
                    Capture.SetExposureTime(triggerTimeUpdateOutputs);
                    break;

                default:
                    break;
            }
        }

        private bool IsLastReadRegister(HarpMessage message)
        {
            return message.Address == Registers.CalibrationPhotodiode560;
        }

        private bool IsNotNullFrame(PhotometryHarpMessage photometryHarpMessage)
        {
            if (photometryHarpMessage.PhotometryData.PhotometryImage == null)
                return false;

            return photometryHarpMessage.PhotometryData.PhotometryImage.IsValid() && photometryHarpMessage.TriggerData != NullTrigger;
        }


        private void TryGetHarpVersion()
        {
            var assemblyLocation = typeof(FP3002).Assembly.Location;
            var configPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), @"..\..\..\..\Bonsai.config");
            configPath = Path.GetFullPath(configPath);
            using (var reader = XmlReader.Create(configPath))
            {
                if (reader.ReadToDescendant("Package"))
                {
                    do
                    {
                        var id = reader[0];
                        var version = reader[1];

                        if (!id.Contains("Bonsai.Harp")) continue;
                        if (version.Contains("3.4."))
                            return;
                        else
                            throw new NotSupportedException(Resources.MsgBox_Error_HarpVersion);
                    }
                    while (reader.ReadToNextSibling("Package"));
                }
            }
        }

        public void TryFindSystem()
        {
            try
            {
                FindSystem();
            }
            catch (SpinnakerException ex)
            {
                throw new TimeoutException(ex.Message);
            }
            catch(TimeoutException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void FindSystem()
        {
            DeviceInfo.Clear();
            FindDevice();
            var targetDeviceID = DeviceInfo.GetTargetDeviceUserID();
            Capture.EnsureCamera(targetDeviceID);
        }

        private void FindDevice()
        {
            if (PortName == "COMx")
                throw new TimeoutException("Please Specify the PortName.");
            for(var i = 0; i < NUM_REPEATS_FINDDEVICE; i++)
            {
                using (var writer = new ConsoleIntercept(true))
                {
                    writer.Flushed += WriterFlush_CheckDeviceInfo;

                    var stdOut = Console.Out;
                    Console.SetOut(writer);
                    var harpDevice = new Device()
                    {
                        PortName = PortName,
                        Heartbeat = EnableType.Disable,
                        IgnoreErrors = true
                    };

                    var totalWaitTime = 0;

                    while (!DeviceInfo.IsDeviceFound())
                    {
                        Thread.Sleep(WAIT_TIME);
                        totalWaitTime += WAIT_TIME;
                        if (totalWaitTime >= TIMEOUT)
                            break;
                    }
                    Console.SetOut(stdOut);

                    if (!DeviceInfo.IsDeviceFound())
                        continue;

                    Board = harpDevice;
                    return;
                }
            }
        }

        private void WriterFlush_CheckDeviceInfo(object sender, EventArgs args)
        {
            var deviceStr = sender.ToString();

            if (deviceStr.Contains("WhoAmI: ") && deviceStr.Contains("DeviceName: "))
            {
                var deviceInfoArr = deviceStr.Split('\n');

                // Line 2: "WhoAmI: {WhoAmI}-{SerialNumber}"
                var whoAmIFields = deviceInfoArr[1].Split(' ')[1].Split('-');
                DeviceInfo.WhoAmI = whoAmIFields[0];
                DeviceInfo.DeviceSerialNumber = whoAmIFields[1];

                // Line 6: "DeviceName: {DeviceName}
                DeviceInfo.DeviceName = deviceInfoArr[5].Split(' ')[1].Replace("\r", string.Empty).Replace("\0", string.Empty);
            }
        }
    }
}