using Bonsai;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Neurophotometrics.Design
{
    public class FP3002Configuration
    {
        const string ConfigCategory = "Config";
        const string PhotometryCategory = "Photometry";
        const string StimulationCategory = "Stimulation";
        const string DIOCategory = "Digital IO";
        const string PowerCategory = "Power";
        const int ExposureSafetyMargin = 1000;

        internal int Config { get; set; }

        [Category(ConfigCategory)]
        [Description("Specifies whether the device outputs its own clock line, or synchronizes to an external clock.")]
        public ClockSynchronizerConfiguration ClockSynchronizer
        {
            get { return (ClockSynchronizerConfiguration)((Config & 0x3) >> 1); }
            set { Config = (Config & 0xFFFC) | (1 << (int)value) & 0x3; }
        }

        [Category(ConfigCategory)]
        [Description("Specifies whether digital output pin 0 state is routed to the BNC, internal laser, or both.")]
        public DigitalOutputRouting Output0Routing
        {
            get { return (DigitalOutputRouting)((Config & 0x1C) >> 3); }
            set { Config = (Config & 0xFFE3) | (1 << ((int)value + 2)) & 0x1C; }
        }

        [Range(0, 15)]
        [Category(ConfigCategory)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The brightness level of the LCD screen. Zero turns off the screen entirely.")]
        public int ScreenBrightness { get; set; }

        [Range(15, 200)]
        [Category(PhotometryCategory)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The frame rate of photometry acquisition, in frames per second.")]
        public int FrameRate { get; set; }

        [XmlArrayItem("Trigger")]
        [Category(PhotometryCategory)]
        [Description("The trigger sequence to use for each of the 410, 470, and 560nm LEDs.")]
        public FrameFlags[] TriggerState { get; set; }

        [Category(PhotometryCategory)]
        [Description("The duration of an individual exposure, in microseconds.")]
        public int ExposureTime { get; set; }

        [Category(PhotometryCategory)]
        [Description("The time from exposure start that each of the 410, 470, and 560nm LEDs will stay in the state, before switching.")]
        public int DwellTime { get; set; }

        internal int TriggerPeriod
        {
            get { return 1000000 / FrameRate; }
            set { FrameRate = 1000000 / value; }
        }

        [Category(PowerCategory)]
        [Range(0, ushort.MaxValue)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 410nm excitation LED, in percent of total power.")]
        public int L410 { get; set; }

        [Category(PowerCategory)]
        [Range(0, ushort.MaxValue)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 470nm excitation LED, in percent of total power.")]
        public int L470 { get; set; }

        [Category(PowerCategory)]
        [Range(0, ushort.MaxValue)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 560nm excitation LED, in percent of total power.")]
        public int L560 { get; set; }

        [Range(0, ushort.MaxValue)]
        [Category(StimulationCategory)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the stimulation laser, in percent of total power.")]
        public int LaserPower { get; set; }

        [Category(StimulationCategory)]
        [Description("The wavelength of the selected laser.")]
        public int LaserWavelength { get; set; }

        [Category(StimulationCategory)]
        [Description("The frequency to use for optogenetics stimulation, in Hz.")]
        public int PulseFrequency { get; set; }

        [Category(StimulationCategory)]
        [Description("The duration of each optogenetics pulse, in milliseconds.")]
        public int PulseWidth { get; set; }

        [Category(StimulationCategory)]
        [Description("The number of pulses in the stimulation train.")]
        public int PulseCount { get; set; }

        internal int PulsePeriod
        {
            get { return 1000 / PulseFrequency; }
            set { PulseFrequency = 1000 / value; }
        }

        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 0.")]
        public DigitalOutputConfiguration DigitalOutput0 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 1.")]
        public DigitalOutputConfiguration DigitalOutput1 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the events which will trigger the digital input line 0.")]
        public DigitalInputConfiguration DigitalInput0 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the events which will trigger the digital input line 1.")]
        public DigitalInputConfiguration DigitalInput1 { get; set; }

        public void Validate()
        {
            var maxExposure = TriggerPeriod - ExposureSafetyMargin;
            ExposureTime = Math.Min(ExposureTime, maxExposure);
            DwellTime = Math.Min(maxExposure, Math.Max(DwellTime, ExposureTime));
        }
    }

    class ConfigurationRegisters
    {
        public const byte Reset = 11;
        public const byte Config = 32;
        public const byte DacL410 = 34;
        public const byte DacL470 = 35;
        public const byte DacL560 = 36;
        public const byte DacAllLeds = 37;
        public const byte DacLaser = 38;

        public const byte ScreenBrightness = 39;
        public const byte ScreenImageIndex = 40;

        public const byte GainL410 = 41;
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

        public const byte Out0Conf = 52;
        public const byte Out1Conf = 53;
        public const byte In0Conf = 54;
        public const byte In1Conf = 55;
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

        public const byte CalibrationL410 = 77;
        public const byte CalibrationL470 = 78;
        public const byte CalibrationL560 = 79;
        public const byte CalibrationLaser = 80;
        public const byte CalibrationPhotodiode410 = 81;
        public const byte CalibrationPhotodiode470 = 82;
        public const byte CalibrationPhotodiode560 = 83;
    }

    enum ResetDeviceConfiguration : byte
    {
        ResetDefault = 1 << 0,
        ResetEeprom = 1 << 1,
        Save = 1 << 2
    }

    public enum ClockSynchronizerConfiguration : byte
    {
        SyncToMaster = 0,
        SyncToSlave = 1,
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
        StopTrigger = 4,
        StopExternalCamera = 5,
        StartTriggerExternalCamera = 6,
        ControlTrigger = 7,
        ControlExternalCamera = 8,
        ControlTriggerExternalCamera = 9,
        StartStimulation = 10,
        ControlStimulation = 11
    }
}
