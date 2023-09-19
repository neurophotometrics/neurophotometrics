using Neurophotometrics.V1.Definitions;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Neurophotometrics.V1.PhotometryWriterHelpers
{
    public class TiffWriter
    {
        private const string VideoFolderName = "VideoFiles";
        private const string VideoFileName = "PhotometryFootage";

        private readonly TiffWriterHelper TiffWriterHelper;
        private readonly string VideoDirectory;
        private readonly string Suffix;

        private FileStream StreamEven;
        private BinaryWriter WriterEven;
        private FileStream StreamOdd;
        private BinaryWriter WriterOdd;
        private FileStream CurrStream;
        private BinaryWriter CurrWriter;

        public TiffWriter(string parentDirectory, string suffix)
        {
            VideoDirectory = GetVideoDirectory(parentDirectory);
            Suffix = suffix;
            TiffWriterHelper = new TiffWriterHelper();
            CreateTiffWriters();
        }

        private string GetVideoDirectory(string parentDirectory)
        {
            var directoryInfo = new DirectoryInfo(parentDirectory);
            var folderAbsPath = $@"{directoryInfo.FullName}\{VideoFolderName}";

            if (!Directory.Exists(folderAbsPath))
                Directory.CreateDirectory(folderAbsPath);
            else
                throw new InvalidOperationException(string.Format("The path '{0}' already exists.", folderAbsPath));

            var videoDirectory = folderAbsPath + @"\";

            return videoDirectory;
        }

        private void CreateTiffWriters()
        {
            CreateNewTiffFile(0);
            CreateNewTiffFile(1);
            CurrStream = StreamEven;
            CurrWriter = WriterEven;
        }

        internal void TryInitializeTiff(PhotometryDataFrame dataFrame)
        {
            try
            {
                InitializeTiff(dataFrame);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        internal void TryWriteNewFrame(PhotometryDataFrame dataFrame)
        {
            try
            {
                WriteNewFrame(dataFrame);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void InitializeTiff(PhotometryDataFrame dataFrame)
        {
            TiffWriterHelper.SetImageInfo(dataFrame);
        }

        private void WriteNewFrame(PhotometryDataFrame dataFrame)
        {
            WriteHeaderOrIFDOffset();
            WriteImage(dataFrame);
            WriteIFD(dataFrame.FrameCounter);
            WriteNewFile();
        }

        private void WriteHeaderOrIFDOffset()
        {
            var isFirstFrameInFile = TiffWriterHelper.GetIsFirstFrameInFile();
            if (isFirstFrameInFile)
                CurrWriter.Write(TiffWriterHelper.GetHeaderBytes());
            else
                CurrWriter.Write(TiffWriterHelper.GetNextIFDOffsetBytes());
        }

        private void WriteImage(PhotometryDataFrame dataFrame)
        {
            unsafe
            {
                var img_bitmapData = dataFrame.PhotometryImage.Bitmap.LockBits(new Rectangle(0, 0, dataFrame.PhotometryImage.Bitmap.Width, dataFrame.PhotometryImage.Bitmap.Height), ImageLockMode.ReadOnly, dataFrame.PhotometryImage.Bitmap.PixelFormat);
                var img_ptrFirstPixel = (byte*)img_bitmapData.Scan0;
                using (var unmanagedMemStream = new UnmanagedMemoryStream(img_ptrFirstPixel, TiffWriterHelper.GetImageSize()))
                {
                    unmanagedMemStream.CopyTo(CurrStream);
                }
                dataFrame.PhotometryImage.Bitmap.UnlockBits(img_bitmapData);
            }
        }

        private void WriteIFD(ulong frameCounter)
        {
            TiffWriterHelper.UpdateFields(frameCounter);
            CurrWriter.Write(TiffWriterHelper.GetIFDBytes());
            TiffWriterHelper.UpdateValues();
        }

        private void WriteNewFile()
        {
            var isFirstFrameInFile = TiffWriterHelper.GetIsFirstFrameInFile();
            if (isFirstFrameInFile)
            {
                var nextFileNumber = TiffWriterHelper.GetFileNumber();
                var isNextFileEven = nextFileNumber % 2 == 0;
                CurrWriter = isNextFileEven ? WriterEven : WriterOdd;
                CurrStream = isNextFileEven ? StreamEven : StreamOdd;

                Task.Run(() => UpdatePrevTiffWriter(nextFileNumber + 1));
            }
        }

        private void UpdatePrevTiffWriter(uint updatedFileNumber)
        {
            if (updatedFileNumber % 2 == 0)
            {
                WriterEven.Close();
                StreamEven.Close();
            }
            else
            {
                WriterOdd.Close();
                StreamOdd.Close();
            }
            CreateNewTiffFile(updatedFileNumber);
        }

        private void CreateNewTiffFile(uint fileNumber)
        {
            var filePath = VideoDirectory + VideoFileName;
            filePath = SuffixHelper.AppendSuffix(filePath, Suffix) + "_" + fileNumber.ToString() + ".tif";

            if (fileNumber % 2 == 0)
            {
                StreamEven = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                WriterEven = new BinaryWriter(StreamEven);
            }
            else
            {
                StreamOdd = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                WriterOdd = new BinaryWriter(StreamOdd);
            }
        }

        internal void TryClose()
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void Close()
        {
            var isFirstFrameInFile = TiffWriterHelper.GetIsFirstFrameInFile();
            if (!isFirstFrameInFile)
            {
                var fileIsEven = TiffWriterHelper.GetFileNumber() % 2 == 0;

                if (fileIsEven)
                    WriterEven.Write(TiffWriterHelper.GetEndOfDirBytes());
                else
                    WriterOdd.Write(TiffWriterHelper.GetEndOfDirBytes());
            }

            var lastEvenFile = StreamEven.Name;
            var lastOddFile = StreamOdd.Name;
            WriterEven.Dispose();
            WriterOdd.Dispose();
            StreamEven.Dispose();
            StreamOdd.Dispose();

            // Delete file if empty
            var fileInfo = new FileInfo(lastEvenFile);
            if (fileInfo.Exists && fileInfo.Length <= TiffWriterHelper.GetHeaderBytes().Length)
                File.Delete(lastEvenFile);
            fileInfo = new FileInfo(lastOddFile);
            if (fileInfo.Exists && fileInfo.Length <= TiffWriterHelper.GetHeaderBytes().Length)
                File.Delete(lastOddFile);
        }
    }
}