using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics.Design
{
    public class FP3002Configuration
    {
        const string PhotometryCategory = "Photometry";
        const string StimulationCategory = "Stimulation";
        const string DIOCategory = "Digital IO";
        const string PowerCategory = "Power";
        const int ExposureSafetyMargin = 1000;

        [Range(16, 100)]
        [Category(PhotometryCategory)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The frame rate of photometry acquisition, in frames per second.")]
        public int SampleFrequency { get; set; }

        [Category(PhotometryCategory)]
        [Description("The trigger sequence to use for each of the 410, 470, and 560nm LEDs.")]
        public TriggerMode TriggerMode { get; set; }

        [Category(PhotometryCategory)]
        [Description("The duration of an individual exposure, in microseconds.")]
        public int ExposureTime { get; set; }

        internal int PreTriggerTime { get; set; }

        internal int TriggerPeriod
        {
            get { return 1000000 / SampleFrequency; }
            set { SampleFrequency = 1000000 / value; }
        }

        [Range(0, 255)]
        [Category(PowerCategory)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 410nm excitation LED, in percent of total power.")]
        public int L410 { get; set; }

        [Range(0, 255)]
        [Category(PowerCategory)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 470nm excitation LED, in percent of total power.")]
        public int L470 { get; set; }

        [Range(0, 255)]
        [Category(PowerCategory)]
        [TypeConverter(typeof(PowerConverter))]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The power of the 560nm excitation LED, in percent of total power.")]
        public int L560 { get; set; }

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
        [Description("Configures the events which will trigger the digital input line.")]
        public DigitalInputConfiguration DigitalInput0 { get; set; }

        public void Validate()
        {
            ExposureTime = Math.Min(ExposureTime, TriggerPeriod - PreTriggerTime - ExposureSafetyMargin);
        }
    }

    class ConfigurationRegisters
    {
        public const byte RawPotL410 = 32;
        public const byte RawPotL470 = 33;
        public const byte RawPotL560 = 34;
        public const byte RawPotLExtra = 35;

        public const byte TriggerState = 44;
        public const byte TriggerStateLength = 45;
        public const byte TriggerPeriod = 46;
        public const byte TriggerTime = 47;
        public const byte PreTriggerTime = 48;

        public const byte DigitalOutput0 = 51;
        public const byte DigitalOutput1 = 52;
        public const byte DigitalInput0 = 53;

        public const byte StimPeriod = 62;
        public const byte StimTime = 63;
        public const byte StimRepetitions = 64;
    }

    public enum DigitalOutputConfiguration : byte
    {
        Software = 0,
        FrameTrigger = 1,
        Strobe = 2,
        LedTrigger = 3
    }

    public enum DigitalInputConfiguration : byte
    {
        None = 0,
        EventRising = 1,
        EventFalling = 2,
        EventChange = 3,
        StartTrigger = 4,
        StartExternalCamera = 5,
        StartTriggerExternalCamera = 6,
        ControlTrigger = 7,
        ControlExternalCamera = 8,
        ControlTriggerExternalCamera = 9,
        StartStimulation = 10,
        ControlStimulation = 11
    }
}
