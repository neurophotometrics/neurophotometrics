using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurophotometrics.Design
{
    static class TriggerHelper
    {
        static readonly byte[] Constant = CreateTriggerState(FrameFlags.L410 | FrameFlags.L470 | FrameFlags.L560);
        static readonly byte[] Trigger1 = CreateTriggerState(FrameFlags.L410, FrameFlags.L470 | FrameFlags.L560);
        static readonly byte[] Trigger1A = CreateTriggerState(FrameFlags.L410, FrameFlags.L470);
        static readonly byte[] Trigger2 = CreateTriggerState(FrameFlags.L470, FrameFlags.L560);
        static readonly byte[] Trigger3 = CreateTriggerState(FrameFlags.L410, FrameFlags.L470, FrameFlags.L560);

        public static byte[] ToTriggerState(TriggerMode trigger)
        {
            switch (trigger)
            {
                case TriggerMode.Constant: return Constant;
                case TriggerMode.Trigger1: return Trigger1;
                case TriggerMode.Trigger1A: return Trigger1A;
                case TriggerMode.Trigger2: return Trigger2;
                case TriggerMode.Trigger3: return Trigger3;
                default:
                    throw new InvalidOperationException("Unsupported trigger mode.");
            }
        }

        public static TriggerMode FromTriggerState(byte[] triggerState)
        {
            if (CompareTriggerState(triggerState, Constant)) return TriggerMode.Constant;
            if (CompareTriggerState(triggerState, Trigger1)) return TriggerMode.Trigger1;
            if (CompareTriggerState(triggerState, Trigger1A)) return TriggerMode.Trigger1A;
            if (CompareTriggerState(triggerState, Trigger2)) return TriggerMode.Trigger2;
            if (CompareTriggerState(triggerState, Trigger3)) return TriggerMode.Trigger3;
            return TriggerMode.UserSpecified;
        }

        public static byte GetTriggerStateLength(TriggerMode trigger)
        {
            switch (trigger)
            {
                case TriggerMode.Constant: return 1;
                case TriggerMode.Trigger1: return 2;
                case TriggerMode.Trigger1A: return 2;
                case TriggerMode.Trigger2: return 2;
                case TriggerMode.Trigger3: return 3;
                default:
                    throw new InvalidOperationException("Unsupported trigger mode.");
            }
        }

        static bool CompareTriggerState(byte[] pattern1, byte[] pattern2)
        {
            if (pattern1.Length != pattern2.Length) return false;
            for (int i = 0; i < pattern1.Length; i++)
            {
                if (pattern1[i] != pattern2[i])
                {
                    return false;
                }
            }

            return true;
        }

        static byte[] CreateTriggerState(params FrameFlags[] pattern)
        {
            var triggerState = new byte[32];
            Array.Copy(pattern, triggerState, pattern.Length);
            return triggerState;
        }
    }
}
