using Neurophotometrics.V2.Definitions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Neurophotometrics.Design.V2
{
    public class FP3002Settings
    {
        private const ushort ExposureSafetyMargin = 1000;

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

                WhoAmI = ushort.Parse(parts[0]);
                SerialNumber = ushort.Parse(parts[1], NumberStyles.HexNumber);
            }
        }

        internal ushort WhoAmI { get; set; }
        internal ulong SerialNumber { get; set; }
        internal byte HardwareVersionHigh { get; set; }
        internal byte HardwareVersionLow { get; set; }
        internal byte FirmwareVersionHigh { get; set; }
        internal byte FirmwareVersionLow { get; set; }
        internal ushort Config { get; set; }

        public ClockSynchronizerConfiguration ClockSynchronizer
        {
            get { return (ClockSynchronizerConfiguration)((Config & 0x3) >> 1); }
            set { Config = (ushort)((Config & 0xFFFC) | (1 << (int)value) & 0x3); }
        }

        public byte ScreenBrightness { get; set; }

        private ushort _TriggerPeriod;

        public ushort TriggerPeriod
        {
            get { return _TriggerPeriod; }
            set
            {
                _TriggerPeriod = value;
                TriggerTimeUpdateOutputs = (ushort)(_TriggerPeriod - ExposureSafetyMargin / 2);
                TriggerTime = (ushort)(_TriggerPeriod - ExposureSafetyMargin);
            }
        }

        internal ushort TriggerTimeUpdateOutputs { get; set; }
        internal ushort TriggerTime { get; set; }
        public List<PhotometryRegion> Regions { get; set; } = new List<PhotometryRegion>();
        public LedPowers LedPowers { get; set; } = new LedPowers() { L415 = 0, L470 = 0, L560 = 0 };
        public byte[] TriggerSequence { get; set; }

        [Browsable(false)]
        internal byte TriggerSequenceLength { get; set; }

        public PulseTrain PulseTrain { get; set; } = new PulseTrain();

        [Browsable(false)]
        internal byte StimKeySwitchState { get; set; }

        public DigitalOutputConfiguration DigitalOutput0 { get; set; }

        public DigitalOutputRouting Output1Routing
        {
            get { return (DigitalOutputRouting)((Config & 0x1C) >> 3); }
            set { Config = (ushort)((Config & 0xFFE3) | (1 << ((int)value + 2)) & 0x1C); }
        }

        public DigitalOutputConfiguration DigitalOutput1 { get; set; }

        public DigitalInputConfiguration DigitalInput0 { get; set; }

        public DigitalInputConfiguration DigitalInput1 { get; set; }
    }
}