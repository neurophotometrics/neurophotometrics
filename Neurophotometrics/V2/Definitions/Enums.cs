using System;
using System.ComponentModel;

namespace Neurophotometrics.V2.Definitions
{
    [Flags]
    public enum AcquisitionModes : byte
    {
        None = 0,
        StartPhotometry = 1 << 0,
        StartExternalCamera = 1 << 2,
        StopPhotometry = 1 << 3,
        StopExternalCamera = 1 << 4
    }

    [Flags]
    public enum AcquisitionStreams : byte
    {
        None = 0,
        Photometry = 1 << 0,
        ExternalCamera = 1 << 1
    }

    public enum RegionChannel : byte
    {
        Unspecified,
        Red,
        Green
    }

    public enum CommandMode : byte
    {
        Stop = 0,
        Start = 1,
        Control = 2
    }

    [Flags]
    public enum FrameFlags : ushort
    {
        [Description("None")]
        None = 0,

        [Description("415 nm")]
        L415 = 1 << 0,

        [Description("470 nm")]
        L470 = 1 << 1,

        [Description("560 nm")]
        L560 = 1 << 2,

        [Description("Output 1")]
        Output1 = 1 << 3,

        [Description("Output 0")]
        Output0 = 1 << 4,

        [Description("Stimulation")]
        Stimulation = 1 << 5,

        [Description("Line 2")]
        Line2 = 1 << 6,

        [Description("Line 3")]
        Line3 = 1 << 7,

        [Description("Input 1")]
        Input1 = 1 << 8,

        [Description("Input 0")]
        Input0 = 1 << 9
    }

    public enum StimulationCommand
    {
        Stop,
        StartFinite,
        StartContinuous
    }

    public enum DigitalIOStreams
    {
        Input1 = (1 << 0),
        Input0 = (1 << 1),
        Output1 = (1 << 2),
        Output0 = (1 << 3),
        All = 0x0F
    }
}