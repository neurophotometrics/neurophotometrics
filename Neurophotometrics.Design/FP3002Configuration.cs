using Bonsai;
using Bonsai.Harp;
using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Neurophotometrics.Design
{
    public class FP3002Configuration
    {
        internal const string ConfigCategory = "Config";
        internal const string PhotometryCategory = "Photometry";
        internal const string StimulationPulseCategory = "Stimulation Pulse";
        internal const string StimulationInterleaveCategory = "Stimulation Interleave";
        internal const string DIOCategory = "Digital IO";
        internal const string PowerCategory = "Power";
        internal const string LaserPowerCategory = "Power (Laser)";
        internal const int DeviceWhoAmI = 2064;
        const int DeviceFirmwarePageSize = 512;
        const int ExposureSafetyMargin = 1000;
        const int DefaultScreenBrightness = 7;
        const int MinInterleaveWidth = 500;
        const int MinFrameRate = 16;
        const int MaxFrameRate = 200;

        [Browsable(false)]
        public string Id
        {
            get { return $"{WhoAmI}-{SerialNumber:x4}"; }
            set
            {
                var parts = value?.Split('-');
                if (parts?.Length != 2)
                {
                    throw new ArgumentException("The id string is empty or has an invalid format.", nameof(value));
                }

                WhoAmI = int.Parse(parts[0]);
                SerialNumber = int.Parse(parts[1], NumberStyles.HexNumber);
            }
        }

        internal int WhoAmI { get; set; }

        internal int HardwareVersionHigh { get; set; }

        internal int HardwareVersionLow { get; set; }

        internal int FirmwareVersionHigh { get; set; }

        internal int FirmwareVersionLow { get; set; }

        internal int SerialNumber { get; set; }

        internal int Config { get; set; }

        [Category(ConfigCategory)]
        [Description("Specifies whether the device outputs its own clock line, or synchronizes to an external clock.")]
        public ClockSynchronizerConfiguration ClockSynchronizer
        {
            get { return (ClockSynchronizerConfiguration)((Config & 0x3) >> 1); }
            set { Config = (Config & 0xFFFC) | (1 << (int)value) & 0x3; }
        }

        [Category(ConfigCategory)]
        [Description("Specifies whether digital output pin 1 state is routed to the BNC, internal laser, or both.")]
        public DigitalOutputRouting Output1Routing
        {
            get { return (DigitalOutputRouting)((Config & 0x1C) >> 3); }
            set { Config = (Config & 0xFFE3) | (1 << ((int)value + 2)) & 0x1C; }
        }

        [Range(0, 15)]
        [Category(ConfigCategory)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The brightness level of the LCD screen. Zero turns off the screen entirely.")]
        public int ScreenBrightness { get; set; }

        [Category(PhotometryCategory)]
        [Range(MinFrameRate, MaxFrameRate)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The frame rate of photometry acquisition, in frames per second.")]
        public int FrameRate { get; set; }

        [XmlArrayItem("Trigger")]
        [Category(PhotometryCategory)]
        [Description("The trigger sequence to use for each of the 410, 470, and 560nm LEDs.")]
        public FrameFlags[] TriggerState { get; set; }

        internal int ExposureTime { get; set; }

        internal int DwellTime { get; set; }

        internal int TriggerPeriod
        {
            get { return 1000000 / FrameRate; }
            set { FrameRate = 1000000 / value; }
        }

        [Category(PowerCategory)]
        [TypeConverter(typeof(LedPowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(LedPowerConverter.MinLedPower, LedPowerConverter.MaxLedPower)]
        [Description("The power of the 410nm excitation LED, in percent of total power.")]
        public int L415 { get; set; }

        [Category(PowerCategory)]
        [TypeConverter(typeof(LedPowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(LedPowerConverter.MinLedPower, LedPowerConverter.MaxLedPower)]
        [Description("The power of the 470nm excitation LED, in percent of total power.")]
        public int L470 { get; set; }

        [Category(PowerCategory)]
        [TypeConverter(typeof(LedPowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(LedPowerConverter.MinLedPower, LedPowerConverter.MaxLedPower)]
        [Description("The power of the 560nm excitation LED, in percent of total power.")]
        public int L560 { get; set; }

        [XmlIgnore]
        [Category(LaserPowerCategory)]
        [Description("The wavelength of the selected stimulation laser.")]
        public int LaserWavelength { get; set; }

        [Range(0, ushort.MaxValue)]
        [Category(LaserPowerCategory)]
        [TypeConverter(typeof(LaserPowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The amplitude of the stimulation laser, in percent of total power.")]
        public int LaserAmplitude { get; set; }

        [Category(StimulationPulseCategory)]
        [Description("The frequency to use for optogenetics stimulation, in Hz.")]
        public int PulseFrequency { get; set; }

        [Category(StimulationPulseCategory)]
        [Description("The duration of each stimulation pulse, in milliseconds.")]
        public int PulseWidth { get; set; }

        [Category(StimulationPulseCategory)]
        [Description("The number of pulses in the stimulation train.")]
        public int PulseCount { get; set; }

        [Category(StimulationInterleaveCategory)]
        [Description("The duration of each interleaved stimulation pulse, in microseconds.")]
        public int InterleaveWidth { get; set; }

        internal int TriggerLaserOn
        {
            get { return TriggerPeriod - InterleaveWidth - ExposureSafetyMargin / 2; }
            set { InterleaveWidth = TriggerPeriod - value - ExposureSafetyMargin / 2; }
        }

        internal int TriggerLaserOff { get; set; }

        internal int PulsePeriod
        {
            get { return 1000 / PulseFrequency; }
            set { PulseFrequency = 1000 / value; }
        }

        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 0.")]
        public DigitalOutputConfiguration DigitalOutput0 { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 1.")]
        public DigitalOutputConfiguration DigitalOutput1 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the events which will trigger the digital input line 0.")]
        public DigitalInputConfiguration DigitalInput0 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the events which will trigger the digital input line 1.")]
        public DigitalInputConfiguration DigitalInput1 { get; set; }

        [Browsable(false)]
        public RotatedRect[] Regions { get; set; }

        internal ulong CameraSerialNumber { get; set; }

        public void Validate()
        {
            FrameRate = Math.Max(MinFrameRate, Math.Min(FrameRate, MaxFrameRate));
            InterleaveWidth = InterleaveWidth > 0 ? Math.Max(MinInterleaveWidth, Math.Min(InterleaveWidth, TriggerPeriod / 2)) : 0;
            ExposureTime = TriggerPeriod - InterleaveWidth - ExposureSafetyMargin;
            DwellTime = TriggerPeriod - ExposureSafetyMargin / 2;
            TriggerLaserOff = TriggerPeriod - ExposureSafetyMargin / 2;
            if (LaserWavelength != Design.LaserWavelength.None)
            {
                ScreenBrightness = Math.Max(DefaultScreenBrightness, ScreenBrightness);
            }
        }

        public FirmwareMetadata GetFirmwareMetadata()
        {
            var hardwareVersion = new HarpVersion(HardwareVersionHigh, HardwareVersionLow);
            var firmwareVersion = new HarpVersion(FirmwareVersionHigh, FirmwareVersionLow);
            var protocolVersion = new HarpVersion(1, null);
            return new FirmwareMetadata(nameof(FP3002), firmwareVersion, protocolVersion, hardwareVersion);
        }

        static string GetTargetFirmwareLocation()
        {
            var assemblyLocation = typeof(FP3002).Assembly.Location;
            var firmwarePath = Path.Combine(Path.GetDirectoryName(assemblyLocation), @"..\..\content\Firmware\");
            firmwarePath = Path.GetFullPath(firmwarePath);
            try
            {
                var firmwareFiles = Directory.GetFiles(Path.GetFullPath(firmwarePath), "*.hex");
                if (firmwareFiles.Length != 1) return null;
                return firmwareFiles[0];
            }
            catch { return null; }
        }
        internal static FirmwareMetadata GetTargetFirmwareMetadata()
        {
            var firmwareLocation = GetTargetFirmwareLocation();
            if (firmwareLocation == null) return null;
            return FirmwareMetadata.Parse(Path.GetFileNameWithoutExtension(firmwareLocation));
        }

        internal static DeviceFirmware GetTargetDeviceFirmware()
        {
            var firmwareLocation = GetTargetFirmwareLocation();
            if (firmwareLocation == null) return null;
            return DeviceFirmware.FromFile(firmwareLocation, DeviceFirmwarePageSize);
        }
    }

    static class ConfigurationRegisters
    {
        public const byte WhoAmI = 0;
        public const byte HardwareVersionHigh = 1;
        public const byte HardwareVersionLow = 2;
        public const byte FirmwareVersionHigh = 6;
        public const byte FirmwareVersionLow = 7;
        public const byte Reset = 11;
        public const byte SerialNumber = 13;
        public const byte Config = 32;
        public const byte DacL415 = 34;
        public const byte DacL470 = 35;
        public const byte DacL560 = 36;
        public const byte DacAllLeds = 37;
        public const byte DacLaser = 38;

        public const byte ScreenBrightness = 39;
        public const byte ScreenImageIndex = 40;

        public const byte GainL415 = 41;
        public const byte GainL470 = 42;
        public const byte GainL560 = 43;

        public const byte StimKeySwitchState = 44;
        public const byte StimStart = 45;
        public const byte StimWavelength = 46;
        public const byte StimPeriod = 47;
        public const byte StimOn = 48;
        public const byte StimReps = 49;

        public const byte ExtCameraStart = 50;
        public const byte ExtCameraPeriod = 51;

        public const byte Out1Conf = 52;
        public const byte Out0Conf = 53;
        public const byte In1Conf = 54;
        public const byte In0Conf = 55;
        public const byte OutSet = 56;
        public const byte OutClear = 57;
        public const byte OutToggle = 58;
        public const byte OutWrite = 59;
        public const byte InRead = 60;

        public const byte Start = 61;
        public const byte FrameEvent = 62;

        public const byte TriggerState = 63;
        public const byte TriggerStateLength = 64;
        public const byte TriggerPeriod = 65;
        public const byte TriggerTime = 66;
        public const byte TriggerTimeUpdateOutputs = 67;
        public const byte TriggerStimBehavior = 68;

        public const byte PhotodiodeStart = 69;
        public const byte Photodiodes = 70;
        public const byte Temperature = 71;

        public const byte ScreenHardwareVersionHigh = 72;
        public const byte ScreenHardwareVersionLow = 73;
        public const byte ScreenAssemblyVersion = 74;
        public const byte ScreenFirmwareVersionHigh = 75;
        public const byte ScreenFirmwareVersionLow = 76;

        public const byte CameraSerialNumber = 77;
        public const byte TriggerLaserOn = 78;
        public const byte TriggerLaserOff = 79;
        public const byte CalibrationL415 = 80;
        public const byte CalibrationL470 = 81;
        public const byte CalibrationL560 = 82;
        public const byte CalibrationLaser = 83;
        public const byte CalibrationPhotodiode410 = 84;
        public const byte CalibrationPhotodiode470 = 85;
        public const byte CalibrationPhotodiode560 = 86;
    }

    static class LaserWavelength
    {
        public const int None = 0;
        public const int PatchCord = 635;
        public const int Secondary = 450;
    }

    public enum ClockSynchronizerConfiguration : byte
    {
        ThisDevice = 0,
        ExternalDevice = 1,
    }

    public enum DigitalOutputRouting : byte
    {
        Bnc = 0,
        InternalLaser = 1,
        Both = 2
    }

    public enum DigitalOutputConfiguration : byte
    {
        Software = 0,
        Strobe = 1,
        TriggerState = 2
    }

    public enum DigitalInputConfiguration : byte
    {
        None = 0,
        EventRising = 1,
        EventFalling = 2,
        EventChange = 3,
        ControlTrigger = 4,
        ControlExternalCamera = 5,
        ControlExternalCameraEvents = 6,
        ControlTriggerAndExternalCamera = 7,
        ControlTriggerAndExternalCameraEvents = 8,
        StartStimulationFinite = 9,
        StartStimulationContinuous = 10,
        StartStimulationInterleave = 11
    }
}
