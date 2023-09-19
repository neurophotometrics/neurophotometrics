using Neurophotometrics.V2.Definitions;

namespace Neurophotometrics.V2.PhotometryWriterHelpers
{
    public class TiffWriterHelper
    {
        private const uint MaxTiffSizeInBytes = uint.MaxValue / 2;

        private const byte HeaderLength = 8;
        private const byte IFDOffsetLength = 4;

        private const byte IFDLength = 182;
        private const byte TagPrefix = 0x01;
        private const byte FieldPrefix = 0x00;

        private byte[] HeaderBytes;
        private byte[] NextIFDOffsetBytes;
        private byte[] IFDBytes;
        private byte[] EndOfDir;

        private ushort Width;
        private ushort Height;
        private ushort BitsPerSample;
        private uint BytesPerImage;
        private uint NextIFDOffset;
        private uint StripOffset;

        private uint FramesInFile;
        private uint FileNumber;

        public TiffWriterHelper()
        {
            InitializeByteArrays();
        }

        private void InitializeByteArrays()
        {
            InitializeHeaderBytes();
            NextIFDOffsetBytes = new byte[IFDOffsetLength];
            InitializeIFDBytes();
            EndOfDir = new byte[IFDOffsetLength];
        }

        private void InitializeHeaderBytes()
        {
            HeaderBytes = new byte[]
            {
                // Declare Byte Order = Little-Endian and that the file is a Tiff (42)
                0x49, 0x49, 0x2a, 0x00,
                // Initialize offset to first IFD, depends on size and bit depth
                0x00, 0x00, 0x00, 0x00
            };
        }

        private void InitializeIFDBytes()
        {
            IFDBytes = new byte[]
            {
                // IDF (Number of Entries)
                0x0f, 0x00,                                                                 // Bytes 000-001: Number of Entries = 15
                // Width Entry
                (byte)Tags.Width, TagPrefix, (byte)Fields.Short, FieldPrefix,               // Bytes 002-005: Tag and Field Type = Width, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 006-009: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 010-013: Width Value = 0 ****(Uninitialized)****
                // Height Entry
                (byte)Tags.Height, TagPrefix, (byte)Fields.Short, FieldPrefix,              // Bytes 014-017: Tag and Field Type = Height, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 018-021: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 022-025: Height Value = 0 ****(Uninitialized)****
                // Bits Per Sample Entry
                (byte)Tags.BPS, TagPrefix, (byte)Fields.Short, FieldPrefix,                 // Bytes 026-029: Tag and Field Type = Bits Per Sample, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 030-033: Number of values = 1 (Grayscale)
                0x10, 0x00, 0x00, 0x00,                                                     // Bytes 034-037: Bits Per Sample Value = 16 (Mono16)
                // Compression Entry
                (byte)Tags.Compression, TagPrefix, (byte)Fields.Short, FieldPrefix,         // Bytes 038-041: Tag and Field Type = Compression, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 042-045: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 046-049: Compression Value = 1 (No Compression)
                // Photometric Interpolation Entry
                (byte)Tags.PhotoInt, TagPrefix, (byte)Fields.Short, FieldPrefix,            // Bytes 050-053: Tag and Field Type = Photometric Interpolation, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 054-057: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 058-061: Photometric Interpolation Value = 1 (Grayscale)
                // Strip Offset Entry
                (byte)Tags.StripOffset, TagPrefix, (byte)Fields.Long, FieldPrefix,         // Bytes 062-065: Tag and Field Type = Strip Offset, Long8
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 066-069: Number of values = 1
                0x10, 0x00, 0x00, 0x00,                                                     // Bytes 070-073: Strip Offset Value = 16 ****(Variable)****
                // Orientation Entry
                (byte)Tags.Orientation, TagPrefix, (byte)Fields.Short, FieldPrefix,         // Bytes 074-077: Tag and Field Type = Orientation, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 078-081: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 082-085: Orientation Value = 1
                // Samples Per Pixel Entry
                (byte)Tags.SamplesPerPixel, TagPrefix, (byte)Fields.Short, FieldPrefix,     // Bytes 086-089: Tag and Field Type = Samples Per Pixel, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 090-093: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 094-097: Samples Per Pixel Value = 1 (Grayscale)
                // Rows Per Strip Entry
                (byte)Tags.RowsPerStrip, TagPrefix, (byte)Fields.Short, FieldPrefix,        // Bytes 098-101: Tag and Field Type = Rows Per Strip, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 102-105: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 106-109: Rows Per Strip Value = 0 ****(Uninitialized)****
                // Strip Bytes Entry
                (byte)Tags.StripBytesCount, TagPrefix, (byte)Fields.Short, FieldPrefix,      // Bytes 110-113: Tag and Field Type = Strip Bytes, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 114-117: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 118-121: Strip Bytes Value = 0 ****(Uninitialized)****
                // Minimum Sample Value Entry
                (byte)Tags.MinSampleVal, TagPrefix, (byte)Fields.Short, FieldPrefix,        // Bytes 122-125: Tag and Field Type = Minimum Sample Value, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 126-129: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 130-133: Minimum Sample Value = 0
                // Maximum Sample Value Entry
                (byte)Tags.MaxSampleVal, TagPrefix, (byte)Fields.Short, FieldPrefix,        // Bytes 134-137: Tag and Field Type = Maximum Sample Value, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 138-141: Number of values = 1
                0xff, 0xff, 0x00, 0x00,                                                     // Bytes 142-145: Maximum Sample Value = 65535 (Mono16)
                // Planar Configuration Entry
                (byte)Tags.PlanarConfig, TagPrefix, (byte)Fields.Short, FieldPrefix,        // Bytes 146-149: Tag and Field Type = Planar Configuration, ushort
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 150-153: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 154-157: Planar Configuration Value = 1
                // Page Number Entry
                (byte)Tags.PageNumber, TagPrefix, (byte)Fields.Short, FieldPrefix,          // Bytes 158-161: Tag and Field Type = Page Number, ASCII
                0x02, 0x00, 0x00, 0x00,                                                     // Bytes 162-165: Number of values = 1
                0x00, 0x00, 0x00, 0x00,                                                     // Bytes 166-169: Page Number Values = (0, short.Max) ****(Variable)****
                // Sample Format Entry
                (byte)Tags.SampleFormat, TagPrefix, (byte)Fields.Short, FieldPrefix,        // Bytes 170-173: Tag and Field Type = Sample Format, long
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 174-177: Number of values = 1
                0x01, 0x00, 0x00, 0x00,                                                     // Bytes 178-181: Sample Format Value = 1, unsigned integer
            };
        }

        internal byte[] GetEndOfDirBytes()
        {
            return EndOfDir;
        }

        internal uint GetImageSize()
        {
            return BytesPerImage;
        }

        internal void SetImageInfo(PhotometryDataFrame dataFrame)
        {
            Width = dataFrame.PhotometryImage.Width;
            Height = dataFrame.PhotometryImage.Height;
            BitsPerSample = dataFrame.PhotometryImage.BitsPerPixel;
            var bytesPerPixel = (ushort)(dataFrame.PhotometryImage.BitsPerPixel / 8);
            BytesPerImage = (uint)(Width * Height * bytesPerPixel);
            NextIFDOffset = HeaderLength + BytesPerImage;
            StripOffset = HeaderLength;

            UpdateHeader();
            UpdateNextIFDOffset();
            UpdateIFD();
        }

        internal bool GetIsFirstFrameInFile()
        {
            return FramesInFile == 0;
        }

        internal uint GetFileNumber()
        {
            return FileNumber;
        }

        internal byte[] GetHeaderBytes()
        {
            return HeaderBytes;
        }

        internal byte[] GetNextIFDOffsetBytes()
        {
            return NextIFDOffsetBytes;
        }

        internal byte[] GetIFDBytes()
        {
            return IFDBytes;
        }

        internal void UpdateFields(ulong frameCounter)
        {
            // Update ushort Page Number
            var pageNumber = (ushort)(frameCounter % ushort.MaxValue);
            IFDBytes[(int)FieldValueIndex.PageNumber + 1] = (byte)((pageNumber & 0xff00L) / 0x0100L);
            IFDBytes[(int)FieldValueIndex.PageNumber + 0] = (byte)((pageNumber & 0x00ffL) / 0x0001L);

            // Update uint Strip Offset
            IFDBytes[(int)FieldValueIndex.StripOffset + 3] = (byte)((StripOffset & 0xff000000L) / 0x01000000L);
            IFDBytes[(int)FieldValueIndex.StripOffset + 2] = (byte)((StripOffset & 0x00ff0000L) / 0x00010000L);
            IFDBytes[(int)FieldValueIndex.StripOffset + 1] = (byte)((StripOffset & 0x0000ff00L) / 0x00000100L);
            IFDBytes[(int)FieldValueIndex.StripOffset + 0] = (byte)((StripOffset & 0x000000ffL) / 0x00000001L);
        }

        internal void UpdateValues()
        {
            FramesInFile++;
            StripOffset += BytesPerImage + IFDLength + IFDOffsetLength;
            NextIFDOffset += BytesPerImage + IFDLength + IFDOffsetLength;

            var nextFrameFileSize = StripOffset + BytesPerImage + IFDOffsetLength;
            if (StripOffset > MaxTiffSizeInBytes)
                ResetValues();
            else if (nextFrameFileSize > MaxTiffSizeInBytes)
                NextIFDOffset = 0;

            UpdateNextIFDOffset();
        }

        private void ResetValues()
        {
            FramesInFile = 0;
            NextIFDOffset = HeaderLength + BytesPerImage;
            StripOffset = HeaderLength;
            FileNumber++;
            UpdateIFD();
        }

        private void UpdateHeader()
        {
            HeaderBytes[7] = (byte)((NextIFDOffset & 0xff000000L) / 0x01000000L);
            HeaderBytes[6] = (byte)((NextIFDOffset & 0x00ff0000L) / 0x00010000L);
            HeaderBytes[5] = (byte)((NextIFDOffset & 0x0000ff00L) / 0x00000100L);
            HeaderBytes[4] = (byte)((NextIFDOffset & 0x000000ffL) / 0x00000001L);
        }

        private void UpdateNextIFDOffset()
        {
            NextIFDOffsetBytes[3] = (byte)((NextIFDOffset & 0xff000000L) / 0x01000000L);
            NextIFDOffsetBytes[2] = (byte)((NextIFDOffset & 0x00ff0000L) / 0x00010000L);
            NextIFDOffsetBytes[1] = (byte)((NextIFDOffset & 0x0000ff00L) / 0x00000100L);
            NextIFDOffsetBytes[0] = (byte)((NextIFDOffset & 0x000000ffL) / 0x00000001L);
        }

        private void UpdateIFD()
        {
            // Initialize UInt16 Width
            IFDBytes[(int)FieldValueIndex.Width + 1] = (byte)((Width & 0xff00) / 0x0100);
            IFDBytes[(int)FieldValueIndex.Width + 0] = (byte)((Width & 0x00ff) / 0x0001);
            // Initialize UInt16 Height
            IFDBytes[(int)FieldValueIndex.Height + 1] = (byte)((Height & 0xff00) / 0x0100);
            IFDBytes[(int)FieldValueIndex.Height + 0] = (byte)((Height & 0x00ff) / 0x0001);
            // Initialize Bits Per Sample
            IFDBytes[(int)FieldValueIndex.BitsPerSample + 1] = (byte)((BitsPerSample & 0xff00) / 0x0100);
            IFDBytes[(int)FieldValueIndex.BitsPerSample + 0] = (byte)((BitsPerSample & 0x00ff) / 0x0001);
            // Initialize UInt16 Rows Per Strip
            IFDBytes[(int)FieldValueIndex.RowsPerStrip + 1] = (byte)((Height & 0xff00) / 0x0100);
            IFDBytes[(int)FieldValueIndex.RowsPerStrip + 0] = (byte)((Height & 0x00ff) / 0x0001);
            // Initialize UInt32 Strip Bytes
            IFDBytes[(int)FieldValueIndex.StripBytes + 3] = (byte)((BytesPerImage & 0xff000000) / 0x01000000);
            IFDBytes[(int)FieldValueIndex.StripBytes + 2] = (byte)((BytesPerImage & 0x00ff0000) / 0x00010000);
            IFDBytes[(int)FieldValueIndex.StripBytes + 1] = (byte)((BytesPerImage & 0x0000ff00) / 0x00000100);
            IFDBytes[(int)FieldValueIndex.StripBytes + 0] = (byte)((BytesPerImage & 0x000000ff) / 0x00000001);
        }

        private enum Tags : byte
        {
            Width = 0x00,
            Height = 0x01,
            BPS = 0x02,
            Compression = 0x03,
            PhotoInt = 0x06,
            StripOffset = 0x11,
            Orientation = 0x12,
            SamplesPerPixel = 0x15,
            RowsPerStrip = 0x16,
            StripBytesCount = 0x17,
            MinSampleVal = 0x18,
            MaxSampleVal = 0x19,
            PlanarConfig = 0x1c,
            PageNumber = 0x29,
            SampleFormat = 0x53
        }

        private enum Fields : byte
        {
            Short = 0x03,
            Long = 0x04
        }

        private enum FieldValueIndex : byte
        {
            Width = 10,
            Height = 22,
            BitsPerSample = 34,
            StripOffset = 70,
            RowsPerStrip = 106,
            StripBytes = 118,
            PageNumber = 166
        }
    }
}