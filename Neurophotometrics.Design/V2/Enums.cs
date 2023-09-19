using System.ComponentModel;

namespace Neurophotometrics.Design.V2
{
    public enum ClockSynchronizerConfiguration : byte
    {
        [Description("This Device")]
        ThisDevice = 0,

        [Description("External Device")]
        ExternalDevice = 1,
    }

    public enum DigitalOutputRouting : byte
    {
        [Description("BNC Port")]
        Bnc = 0,

        [Description("Internal Laser")]
        InternalLaser = 1,

        [Description("Both")]
        Both = 2
    }

    public enum DigitalOutputConfiguration : byte
    {
        [Description("Software")]
        Software = 0,

        [Description("Strobe")]
        Strobe = 1,

        [Description("Trigger State")]
        TriggerState = 2
    }

    public enum DigitalInputConfiguration : byte
    {
        [Description("None")]
        None = 0,

        [Description("Event: Rising")]
        EventRising = 1,

        [Description("Event: Falling")]
        EventFalling = 2,

        [Description("Event: Change")]
        EventChange = 3,

        [Description("Control: Trigger")]
        ControlTrigger = 4,

        [Description("Control: External Camera")]
        ControlExternalCamera = 5,

        [Description("Control: External Camera with Events")]
        ControlExternalCameraEvents = 6,

        [Description("Control: Trigger and External Camera")]
        ControlTriggerAndExternalCamera = 7,

        [Description("Control: Trigger and External Camera with Events")]
        ControlTriggerAndExternalCameraEvents = 8,

        [Description("Start Stimulation: Finite")]
        StartStimulationFinite = 9,

        [Description("Start Stimulation: Continuous")]
        StartStimulationContinuous = 10
    }
}