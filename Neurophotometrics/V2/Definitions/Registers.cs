namespace Neurophotometrics.V2.Definitions
{
    public static class Registers
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

        public const byte Out1Config = 52;
        public const byte Out0Config = 53;
        public const byte In1Config = 54;
        public const byte In0Config = 55;
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
        public const byte Photometry = 0xF0;
    }
}