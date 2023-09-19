using Bonsai.Harp;

using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2;
using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Generic;

namespace Neurophotometrics.Design.V2.Forms
{
    internal class SettingsConfigurator
    {
        public FP3002Settings Settings;
        //Configurator
        public SettingsConfigurator()
        {
            Settings = new FP3002Settings();
        }

        public event RegSettingChangedEventHandler SettingChanged;

        internal void TryReadRegister(HarpMessage message)
        {
            try
            {
                ReadRegister(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: ReadRegister({message.Address})\nMessage = {ex.Message}");
            }
        }

        private void ReadRegister(HarpMessage message)
        {
            switch (message.Address)
            {
                case Registers.WhoAmI:
                    Settings.WhoAmI = message.GetPayloadUInt16();
                    break;

                case Registers.HardwareVersionHigh:
                    Settings.HardwareVersionHigh = message.GetPayloadByte();
                    break;

                case Registers.HardwareVersionLow:
                    Settings.HardwareVersionLow = message.GetPayloadByte();
                    break;

                case Registers.FirmwareVersionHigh:
                    Settings.FirmwareVersionHigh = message.GetPayloadByte();
                    break;

                case Registers.FirmwareVersionLow:
                    Settings.FirmwareVersionLow = message.GetPayloadByte();
                    break;

                case Registers.Config:
                    Settings.Config = message.GetPayloadUInt16();
                    break;

                case Registers.DacL415:
                    Settings.LedPowers.L415 = message.GetPayloadUInt16();
                    break;

                case Registers.DacL470:
                    Settings.LedPowers.L470 = message.GetPayloadUInt16();
                    break;

                case Registers.DacL560:
                    Settings.LedPowers.L560 = message.GetPayloadUInt16();
                    break;

                case Registers.DacLaser:
                    Settings.PulseTrain.LaserAmplitude = message.GetPayloadUInt16();
                    break;

                case Registers.ScreenBrightness:
                    Settings.ScreenBrightness = message.GetPayloadByte();
                    break;

                case Registers.StimWavelength:
                    Settings.PulseTrain.LaserWavelength = message.GetPayloadUInt16();
                    break;

                case Registers.StimPeriod:
                    Settings.PulseTrain.StimPeriod = message.GetPayloadUInt16();
                    break;

                case Registers.StimOn:
                    Settings.PulseTrain.StimOn = message.GetPayloadUInt16();
                    break;

                case Registers.StimReps:
                    Settings.PulseTrain.StimReps = message.GetPayloadUInt16();
                    break;

                case Registers.Out1Config:
                    Settings.DigitalOutput1 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;

                case Registers.Out0Config:
                    Settings.DigitalOutput0 = (DigitalOutputConfiguration)message.GetPayloadByte();
                    break;

                case Registers.In1Config:
                    Settings.DigitalInput1 = (DigitalInputConfiguration)message.GetPayloadByte();
                    break;

                case Registers.In0Config:
                    Settings.DigitalInput0 = (DigitalInputConfiguration)message.GetPayloadByte();
                    break;

                case Registers.TriggerState:
                    Settings.TriggerSequence = message.GetPayloadArray<byte>();
                    break;

                case Registers.TriggerStateLength:
                    Settings.TriggerSequenceLength = message.GetPayloadByte();
                    break;

                case Registers.TriggerPeriod:
                    Settings.TriggerPeriod = message.GetPayloadUInt16();
                    break;

                case Registers.TriggerTime:
                    Settings.TriggerTime = message.GetPayloadUInt16();
                    break;

                case Registers.TriggerTimeUpdateOutputs:
                    Settings.TriggerTimeUpdateOutputs = message.GetPayloadUInt16();
                    break;

                case Registers.SerialNumber:
                    Settings.SerialNumber = message.GetTimestampedPayloadUInt16().Value;
                    break;

                case Registers.StimKeySwitchState:
                    Settings.StimKeySwitchState = message.GetPayloadByte();
                    break;
                default:
                    break;
            }
        }

        internal FirmwareMetadata TryGetFirmwareMetadata()
        {
            try
            {
                return GetFirmwareMetadata();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
            return null;
        }

        private FirmwareMetadata GetFirmwareMetadata()
        {
            var hardwareVersion = new HarpVersion(Settings.HardwareVersionHigh, Settings.HardwareVersionLow);
            var firmwareVersion = new HarpVersion(Settings.FirmwareVersionHigh, Settings.FirmwareVersionLow);
            var protocolVersion = new HarpVersion(1, null);
            return new FirmwareMetadata(nameof(FP3002), firmwareVersion, protocolVersion, hardwareVersion);
        }

        internal void NotifyChangeInReg(HarpMessage message)
        {
            try
            {
                switch (message.Address)
                {
                    case Registers.DacL415:
                        SafeInvokeSettingChanged(nameof(Settings.LedPowers.L415), Settings.LedPowers.L415);
                        break;

                    case Registers.DacL470:
                        SafeInvokeSettingChanged(nameof(Settings.LedPowers.L470), Settings.LedPowers.L470);
                        break;

                    case Registers.DacL560:
                        SafeInvokeSettingChanged(nameof(Settings.LedPowers.L560), Settings.LedPowers.L560);
                        break;

                    case Registers.DacLaser:
                        SafeInvokeSettingChanged(nameof(Settings.PulseTrain.LaserAmplitude), Settings.PulseTrain.LaserAmplitude);
                        break;

                    case Registers.StimWavelength:
                        SafeInvokeSettingChanged(nameof(Settings.PulseTrain.LaserWavelength), Settings.PulseTrain.LaserWavelength);
                        break;

                    case Registers.StimPeriod:
                        SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimPeriod), Settings.PulseTrain.StimPeriod);
                        break;

                    case Registers.StimOn:
                        SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimOn), Settings.PulseTrain.StimOn);
                        break;

                    case Registers.StimReps:
                        SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimReps), Settings.PulseTrain.StimReps);
                        break;

                    case Registers.TriggerState:
                    case Registers.TriggerStateLength:
                        var frameFlags = TriggerSequenceConverter.ConvertByteArrToFrameFlagsArr(Settings.TriggerSequence, Settings.TriggerSequenceLength);
                        SafeInvokeSettingChanged(nameof(Settings.TriggerSequence), frameFlags);
                        break;

                    case Registers.TriggerPeriod:
                        SafeInvokeSettingChanged(nameof(Settings.TriggerPeriod), Settings.TriggerPeriod);
                        break;

                    case Registers.StimKeySwitchState:
                        SafeInvokeSettingChanged("StimKeySwitchState", Settings.StimKeySwitchState);
                        break;

                    case Registers.Config:
                        SafeInvokeSettingChanged(nameof(Settings.ClockSynchronizer), Settings.ClockSynchronizer);
                        SafeInvokeSettingChanged(nameof(Settings.Output1Routing), Settings.Output1Routing);
                        break;

                    case Registers.ScreenBrightness:
                        SafeInvokeSettingChanged(nameof(Settings.ScreenBrightness), Settings.ScreenBrightness);
                        break;

                    case Registers.In0Config:
                        SafeInvokeSettingChanged(nameof(Settings.DigitalInput0), Settings.DigitalInput0);
                        break;

                    case Registers.In1Config:
                        SafeInvokeSettingChanged(nameof(Settings.DigitalInput1), Settings.DigitalInput1);
                        break;

                    case Registers.Out0Config:
                        SafeInvokeSettingChanged(nameof(Settings.Output1Routing), Settings.Output1Routing);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: NotifyChangeInReg({message.Address})\nMessage = {ex.Message}");
            }
        }

        private void SafeInvokeSettingChanged(string name, object value)
        {
            try
            {
                if (SettingChanged != null)
                    SettingChanged.Invoke(this, new RegSettingChangedEventArgs(name, value));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: SafeInvokeFPSSettingChanged\nMessage: {ex.Message}");
            }
        }

        internal void NotifyVisualizers()
        {
            try
            {
                SafeInvokeSettingChanged(nameof(Settings.LedPowers.L415), Settings.LedPowers.L415);
                SafeInvokeSettingChanged(nameof(Settings.LedPowers.L470), Settings.LedPowers.L470);
                SafeInvokeSettingChanged(nameof(Settings.LedPowers.L560), Settings.LedPowers.L560);
                SafeInvokeSettingChanged(nameof(Settings.PulseTrain.LaserAmplitude), Settings.PulseTrain.LaserAmplitude);
                SafeInvokeSettingChanged(nameof(Settings.PulseTrain.LaserWavelength), Settings.PulseTrain.LaserWavelength);
                SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimPeriod), Settings.PulseTrain.StimPeriod);
                SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimOn), Settings.PulseTrain.StimOn);
                SafeInvokeSettingChanged(nameof(Settings.PulseTrain.StimReps), Settings.PulseTrain.StimReps);
                var frameFlags = TriggerSequenceConverter.ConvertByteArrToFrameFlagsArr(Settings.TriggerSequence, Settings.TriggerSequenceLength);
                SafeInvokeSettingChanged(nameof(Settings.TriggerSequence), frameFlags);
                SafeInvokeSettingChanged(nameof(Settings.TriggerPeriod), Settings.TriggerPeriod);
                SafeInvokeSettingChanged("StimKeySwitchState", Settings.StimKeySwitchState);
                SafeInvokeSettingChanged(nameof(Settings.ClockSynchronizer), Settings.ClockSynchronizer);
                SafeInvokeSettingChanged(nameof(Settings.ScreenBrightness), Settings.ScreenBrightness);
                SafeInvokeSettingChanged(nameof(Settings.DigitalInput0), Settings.DigitalInput0);
                SafeInvokeSettingChanged(nameof(Settings.DigitalInput1), Settings.DigitalInput1);
                SafeInvokeSettingChanged(nameof(Settings.DigitalOutput0), Settings.DigitalOutput0);
                SafeInvokeSettingChanged(nameof(Settings.Output1Routing), Settings.Output1Routing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: NotifyVisualizers\nMessage: {ex.Message}");
            }
        }

        private static readonly FrameFlags[] DefaultTrigSeq = new FrameFlags[] { FrameFlags.L470 | FrameFlags.L560 | FrameFlags.L415 };
        private const ushort DefaultTriggerPeriod = 25000;
        private const ushort DefaultTriggerPeriodUpdateOutputs = 24500;
        private const ushort DefaultTriggerTime = 24000;

        internal IEnumerable<HarpMessage> StoreSettings()
        {
            yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
            yield return HarpCommand.WriteByte(Registers.Start, (byte)(AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera));

            yield return HarpCommand.WriteUInt16(Registers.Config, Settings.Config);
            yield return HarpCommand.WriteByte(Registers.ScreenBrightness, Settings.ScreenBrightness);
            yield return HarpCommand.WriteByte(Registers.TriggerState, Settings.TriggerSequence);
            yield return HarpCommand.WriteByte(Registers.TriggerStateLength, Settings.TriggerSequenceLength);
            yield return HarpCommand.WriteUInt16(Registers.TriggerPeriod, Settings.TriggerPeriod);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTimeUpdateOutputs, Settings.TriggerTimeUpdateOutputs);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTime, Settings.TriggerTime);
            yield return HarpCommand.WriteUInt16(Registers.DacL415, Settings.LedPowers.L415);
            yield return HarpCommand.WriteUInt16(Registers.DacL470, Settings.LedPowers.L470);
            yield return HarpCommand.WriteUInt16(Registers.DacL560, Settings.LedPowers.L560);
            yield return HarpCommand.WriteByte(Registers.Out0Config, (byte)Settings.DigitalOutput0);
            yield return HarpCommand.WriteByte(Registers.Out1Config, (byte)Settings.DigitalOutput1);
            yield return HarpCommand.WriteByte(Registers.In0Config, (byte)Settings.DigitalInput0);
            yield return HarpCommand.WriteByte(Registers.In1Config, (byte)Settings.DigitalInput1);

            yield return HarpCommand.WriteUInt16(Registers.StimWavelength, Settings.PulseTrain.LaserWavelength);
            yield return HarpCommand.WriteUInt16(Registers.DacLaser, Settings.PulseTrain.LaserAmplitude);
            yield return HarpCommand.WriteUInt16(Registers.StimPeriod, Settings.PulseTrain.StimPeriod);
            yield return HarpCommand.WriteUInt16(Registers.StimOn, Settings.PulseTrain.StimOn);
            yield return HarpCommand.WriteUInt16(Registers.StimReps, Settings.PulseTrain.StimReps);
        }

        internal HarpMessage GenResetPersistentRegisters()
        {
            return HarpCommand.ResetDevice(ResetMode.RestoreDefault);
        }

        internal IEnumerable<HarpMessage> WriteToPersistentRegisters()
        {
            yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
            yield return HarpCommand.WriteByte(Registers.Start, (byte)(AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera));

            yield return HarpCommand.WriteUInt16(Registers.Config, Settings.Config);
            yield return HarpCommand.WriteByte(Registers.ScreenBrightness, Settings.ScreenBrightness);
            yield return HarpCommand.WriteByte(Registers.TriggerState, Settings.TriggerSequence);
            yield return HarpCommand.WriteByte(Registers.TriggerStateLength, Settings.TriggerSequenceLength);
            yield return HarpCommand.WriteUInt16(Registers.TriggerPeriod, Settings.TriggerPeriod);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTimeUpdateOutputs, Settings.TriggerTimeUpdateOutputs);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTime, Settings.TriggerTime);
            yield return HarpCommand.WriteUInt16(Registers.DacL415, Settings.LedPowers.L415);
            yield return HarpCommand.WriteUInt16(Registers.DacL470, Settings.LedPowers.L470);
            yield return HarpCommand.WriteUInt16(Registers.DacL560, Settings.LedPowers.L560);
            yield return HarpCommand.WriteByte(Registers.Out0Config, (byte)Settings.DigitalOutput0);
            yield return HarpCommand.WriteByte(Registers.Out1Config, (byte)Settings.DigitalOutput1);
            yield return HarpCommand.WriteByte(Registers.In0Config, (byte)Settings.DigitalInput0);
            yield return HarpCommand.WriteByte(Registers.In1Config, (byte)Settings.DigitalInput1);

            yield return HarpCommand.WriteUInt16(Registers.StimWavelength, 0);
            yield return HarpCommand.WriteUInt16(Registers.DacLaser, Settings.PulseTrain.LaserAmplitude);
            yield return HarpCommand.WriteUInt16(Registers.StimPeriod, Settings.PulseTrain.StimPeriod);
            yield return HarpCommand.WriteUInt16(Registers.StimOn, Settings.PulseTrain.StimOn);
            yield return HarpCommand.WriteUInt16(Registers.StimReps, Settings.PulseTrain.StimReps);

            yield return HarpCommand.ResetDevice(ResetMode.Save);
        }

        internal IEnumerable<HarpMessage> GenCaliSettings()
        {
            yield return HarpCommand.WriteUInt16(Registers.DacL415, 0);
            yield return HarpCommand.WriteUInt16(Registers.DacL470, 0);
            yield return HarpCommand.WriteUInt16(Registers.DacL560, 0);
            yield return HarpCommand.WriteByte(Registers.Out0Config, (byte)DigitalOutputConfiguration.Software);
            yield return HarpCommand.WriteByte(Registers.Out1Config, (byte)DigitalOutputConfiguration.Software);
            yield return HarpCommand.WriteByte(Registers.In0Config, (byte)DigitalInputConfiguration.None);
            yield return HarpCommand.WriteByte(Registers.In1Config, (byte)DigitalInputConfiguration.None);
            yield return HarpCommand.WriteByte(Registers.TriggerState, TriggerSequenceConverter.ConvertFrameFlagsArrToByteArr(DefaultTrigSeq));
            yield return HarpCommand.WriteByte(Registers.TriggerStateLength, (byte)DefaultTrigSeq.Length);
            yield return HarpCommand.WriteUInt16(Registers.TriggerPeriod, DefaultTriggerPeriod);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTime, DefaultTriggerTime);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTimeUpdateOutputs, DefaultTriggerPeriodUpdateOutputs);

            yield return HarpCommand.WriteByte(Registers.Start, (byte)AcquisitionModes.StartPhotometry);
        }

        internal IEnumerable<HarpMessage> WritePropToReg(RegSettingChangedEventArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(FP3002Settings.ScreenBrightness):
                    yield return HarpCommand.WriteByte(Registers.ScreenBrightness, Settings.ScreenBrightness);
                    break;

                case nameof(Settings.LedPowers.L415):
                    yield return HarpCommand.WriteUInt16(Registers.DacL415, (ushort)arg.Value);
                    break;

                case nameof(Settings.LedPowers.L470):
                    yield return HarpCommand.WriteUInt16(Registers.DacL470, (ushort)arg.Value);
                    break;

                case nameof(Settings.LedPowers.L560):
                    yield return HarpCommand.WriteUInt16(Registers.DacL560, (ushort)arg.Value);
                    break;

                case nameof(Settings.PulseTrain.LaserAmplitude):
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, (ushort)arg.Value);
                    break;

                case nameof(Settings.PulseTrain.StimPeriod):
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, (ushort)arg.Value);
                    break;

                case "TabPage":
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
                    yield return HarpCommand.WriteUInt16(Registers.DacL415, 0);
                    yield return HarpCommand.WriteUInt16(Registers.DacL470, 0);
                    yield return HarpCommand.WriteUInt16(Registers.DacL560, 0);
                    break;

                case "StartCalLaser":
                    var pulseTrain = (PulseTrain)arg.Value;
                    yield return HarpCommand.WriteUInt16(Registers.StimWavelength, pulseTrain.LaserWavelength);
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, pulseTrain.LaserAmplitude);
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, pulseTrain.StimPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.StimOn, pulseTrain.StimOn);
                    yield return HarpCommand.WriteUInt16(Registers.StimReps, pulseTrain.StimReps);
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.StartContinuous);
                    break;

                case "StopCalLaser":
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
                    yield return HarpCommand.WriteUInt16(Registers.StimWavelength, Settings.PulseTrain.LaserWavelength);
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, Settings.PulseTrain.LaserAmplitude);
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, Settings.PulseTrain.StimPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.StimOn, Settings.PulseTrain.StimOn);
                    yield return HarpCommand.WriteUInt16(Registers.StimReps, Settings.PulseTrain.StimReps);
                    break;

                default: yield break;
            }
        }

        internal void UpdateProp(RegSettingChangedEventArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(FP3002Settings.ScreenBrightness):
                    Settings.ScreenBrightness = (byte)arg.Value;
                    break;

                case nameof(FP3002Settings.TriggerSequence):
                    var frameFlags = (FrameFlags[])arg.Value;
                    Settings.TriggerSequence = TriggerSequenceConverter.ConvertFrameFlagsArrToByteArr(frameFlags);
                    Settings.TriggerSequenceLength = (byte)frameFlags.Length;
                    break;

                case nameof(FP3002Settings.LedPowers.L415):
                    Settings.LedPowers.L415 = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.LedPowers.L470):
                    Settings.LedPowers.L470 = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.LedPowers.L560):
                    Settings.LedPowers.L560 = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.LaserWavelength):
                    Settings.PulseTrain.LaserWavelength = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.LaserAmplitude):
                    Settings.PulseTrain.LaserAmplitude = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.StimPeriod):
                    Settings.PulseTrain.StimPeriod = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.StimOn):
                    Settings.PulseTrain.StimOn = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.PulseTrain.StimReps):
                    Settings.PulseTrain.StimReps = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.Regions):
                    Settings.Regions = (List<PhotometryRegion>)arg.Value;
                    break;

                case nameof(FP3002Settings.TriggerPeriod):
                    Settings.TriggerPeriod = (ushort)arg.Value;
                    break;

                case nameof(FP3002Settings.ClockSynchronizer):
                    Settings.ClockSynchronizer = (ClockSynchronizerConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalInput0):
                    Settings.DigitalInput0 = (DigitalInputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalInput1):
                    Settings.DigitalInput1 = (DigitalInputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.DigitalOutput0):
                    Settings.DigitalOutput0 = (DigitalOutputConfiguration)arg.Value;
                    break;

                case nameof(FP3002Settings.Output1Routing):
                    Settings.Output1Routing = (DigitalOutputRouting)arg.Value;
                    break;
            }
        }
    }
}