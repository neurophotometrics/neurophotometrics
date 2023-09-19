using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Converters
{
    public static class LaserWavelengthConverter
    {
        public static Dictionary<string, object> TextToWavelength { get; private set; } =  new Dictionary<string, object>()
        {
            {"None", (ushort)0},
            {"635nm", (ushort)635},
            {"450nm", (ushort)450}
        };

        public static bool IsLaserEnabled(ushort wavelength)
        {
            return wavelength != 0;
        }

        public static bool Is635LaserEnabled(ushort wavelength)
        {
            return wavelength == 635;
        }

        public static ushort GetWavelength(object value)
        {
            if (value is KeyValuePair<string, object> pair)
                return (ushort)pair.Value;
            else if (value is ushort wavelength)
                return wavelength;
            return 0;
        }

        public static string ConvertWavelengthToText(ushort wavelength)
        {
            return TextToWavelength.FirstOrDefault(x => (ushort)x.Value == wavelength).Key;
        }

        public static ushort ConvertTextToWavelength(string text)
        {
            return (ushort)TextToWavelength[text];
        }

        internal static Color GetColor(ushort laserWavelength)
        {
            switch (laserWavelength)
            {
                case 635:
                    return Color.Red;

                case 450:
                    return Color.Blue;

                default:
                    return Color.LightGray;
            }
        }

        public static Color GetLightColor(ushort laserWavelength)
        {
            var color = GetColor(laserWavelength);
            color = ControlPaint.LightLight(color);
            color = ControlPaint.LightLight(color);
            return color;
        }
    }
}