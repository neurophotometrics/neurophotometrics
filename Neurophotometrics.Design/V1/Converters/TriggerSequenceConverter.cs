using Neurophotometrics.V1.Definitions;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V1.Converters
{
    public static class TriggerSequenceConverter
    {
        private static readonly Dictionary<string, object> TextToFrameFlag = new Dictionary<string, object>()
        {
            {"None", FrameFlags.None},
            {"415 nm", FrameFlags.L415},
            {"470 nm", FrameFlags.L470},
            {"560 nm", FrameFlags.L560}
        };

        public static FrameFlags[] ConvertByteArrToFrameFlagsArr(byte[] byteArr, byte triggerSequenceLength)
        {
            var frameFlags = new FrameFlags[triggerSequenceLength];
            for (var i = 0; i < frameFlags.Length; i++)
            {
                frameFlags[i] = (FrameFlags)byteArr[i];
            }
            return frameFlags;
        }

        public static byte[] ConvertFrameFlagsArrToByteArr(FrameFlags[] frameFlagsArr)
        {
            var triggerSequence = new byte[32];

            for (var i = 0; i < frameFlagsArr.Length; i++)
            {
                triggerSequence[i] = (byte)frameFlagsArr[i];
            }

            return triggerSequence;
        }

        public static FrameFlags ConvertStringToFrameFlag(string ledStr)
        {
            return (FrameFlags)TextToFrameFlag[ledStr];
        }

        public static string ConvertFrameFlagToString(FrameFlags ledFlag)
        {
            return TextToFrameFlag.FirstOrDefault(x => (FrameFlags)x.Value == ledFlag).Key;
        }

        public static Color GetColor(FrameFlags ledFlag)
        {
            switch (ledFlag)
            {
                case FrameFlags.None:
                    return Color.LightGray;

                case FrameFlags.L415:
                    return Color.FromArgb(118, 0, 237);

                case FrameFlags.L470:
                    return Color.FromArgb(0, 169, 255);

                case FrameFlags.L560:
                    return Color.FromArgb(129, 255, 0);

                case FrameFlags.L470 | FrameFlags.L560:
                    return Color.FromArgb(64, 212, 128);

                case FrameFlags.All:
                    return Color.Gray;

                default:
                    return Color.White;
            }
        }

        public static Color GetLightColor(FrameFlags ledFlag)
        {
            var color = GetColor(ledFlag);
            color = ControlPaint.LightLight(color);
            color = ControlPaint.LightLight(color);
            return color;
        }
    }
}