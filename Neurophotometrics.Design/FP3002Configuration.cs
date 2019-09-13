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

        [Category(PhotometryCategory)]
        [Description("The frame rate of photometry acquisition.")]
        public int TriggerFrequency { get; set; }

        [Category(PhotometryCategory)]
        public TriggerMode TriggerMode { get; set; }

        [Category(PhotometryCategory)]
        public int TriggerTime { get; set; }

        [Category(PhotometryCategory)]
        public int PreTriggerTime { get; set; }

        [Category(StimulationCategory)]
        public int StimFrequency { get; set; }

        [Category(StimulationCategory)]
        public int StimTime { get; set; }

        [Category(StimulationCategory)]
        public int StimRepetitions { get; set; }

        [Category(DIOCategory)]
        public DigitalOutputConfiguration DigitalOutput0 { get; set; }

        [Category(DIOCategory)]
        public DigitalOutputConfiguration DigitalOutput1 { get; set; }

        [Category(DIOCategory)]
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
