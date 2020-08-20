namespace Neurophotometrics.Design
{
    static class TriggerHelper
    {
        public static FrameFlags[] ToFrameFlags(byte[] triggerState)
        {
            var index = triggerState.Length - 1;
            for (; index >= 0; index--)
            {
                if (triggerState[index] != 0) break;
            }

            var frameFlags = new FrameFlags[index + 1];
            for (int i = 0; i < frameFlags.Length; i++)
            {
                frameFlags[i] = (FrameFlags)triggerState[i];
            }
            return frameFlags;
        }

        public static byte[] FromFrameFlags(params FrameFlags[] pattern)
        {
            var triggerState = new byte[32];
            for (int i = 0; i < pattern.Length; i++)
            {
                triggerState[i] = (byte)pattern[i];
            }
            return triggerState;
        }
    }
}
