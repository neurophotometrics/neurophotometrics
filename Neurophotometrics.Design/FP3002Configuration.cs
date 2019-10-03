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

        [Range(16, 100)]
        [Category(PhotometryCategory)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The frame rate of photometry acquisition, in frames per second.")]
        public int SampleFrequency { get; set; }

        [Category(PhotometryCategory)]
        [Description("The trigger sequence to use for each of the 410, 470, and 560nm LEDs.")]
        public TriggerMode TriggerMode { get; set; }

        [Category(PhotometryCategory)]
        [Description("The duration of each individual trigger, in microseconds.")]
        public int TriggerWidth { get; set; }

        [Category(StimulationCategory)]
        [Description("The frequency to use for optogenetics stimulation, in Hz.")]
        public int PulseFrequency { get; set; }

        [Category(StimulationCategory)]
        [Description("The duration of each optogenetics pulse, in milliseconds.")]
        public int PulseWidth { get; set; }

        [Category(StimulationCategory)]
        [Description("The number of pulses in the stimulation train.")]
        public int PulseCount { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 0.")]
        public DigitalOutputConfiguration DigitalOutput0 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the action for the digital output line 1.")]
        public DigitalOutputConfiguration DigitalOutput1 { get; set; }

        [Category(DIOCategory)]
        [Description("Configures the events which will trigger the digital input line.")]
        public DigitalInputConfiguration DigitalInput0 { get; set; }
    }

    class ConfigurationRegisters
    {
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
