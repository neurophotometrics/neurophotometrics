namespace Neurophotometrics
{
    class Registers
    {
        public const byte Config = 32;
        public const byte Reserved0 = 33;
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
        public const byte CalibrationL415 = 77;
        public const byte CalibrationL470 = 78;
        public const byte CalibrationL560 = 79;
        public const byte CalibrationLaser = 80;
        public const byte CalibrationPhotodiode410 = 81;
        public const byte CalibrationPhotodiode470 = 82;
        public const byte CalibrationPhotodiode560 = 83;
        public const byte Photometry = 0xF0;
    }
}
