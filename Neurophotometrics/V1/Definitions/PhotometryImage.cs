using System;
using System.Drawing;

namespace Neurophotometrics.V1.Definitions
{
    public sealed class PhotometryImage : IDisposable
    {
        public ulong FrameID { get; set; }
        public ulong Timestamp { get; set; }
        public ushort OffsetX { get; set; }
        public ushort OffsetY { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort BitsPerPixel { get; set; }
        public Bitmap Bitmap { get; set; }

        public bool IsValid()
        {
            return Bitmap != null;
        }

        public void Dispose()
        {
            if (Bitmap == null) return;

            Bitmap.Dispose();
            Bitmap = null;
        }
    }
}