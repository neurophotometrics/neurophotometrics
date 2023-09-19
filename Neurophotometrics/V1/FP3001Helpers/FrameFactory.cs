using Neurophotometrics.V1.Definitions;

using SpinnakerNET;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Neurophotometrics.V1.FP3001Helpers
{
    public sealed class FrameFactory : IDisposable
    {
        public ulong Timeout { get; set; }

        private readonly CameraCapture Capture;
        private IObserver<PhotometryImage> Observer;
        private const byte BufferSize = 10;
        private const byte Mono16BPP = 2;

        private IntPtr ImageBuffer;
        private bool Generating;
        private int FPS;

        internal FrameFactory(CameraCapture capture)
        {
            Capture = capture;
            FPS = capture.FPS;
            Timeout = (ulong)(1000 / FPS);
        }
        internal void SetObserver(IObserver<PhotometryImage> observer)
        {
            Observer = observer;
        }

        public void Open()
        {
            Generating = true;
            Task.Factory.StartNew(new Action(AcquireFrames));
        }

        private void Close()
        {
            TryCloseCamera();
            DeallocateBuffer();
        }

        private void TryCloseCamera()
        {
            try
            {
                CloseCamera();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void CloseCamera()
        {
            if (Capture == null || Capture.Camera == null)
                return;

            Capture.Camera.EndAcquisition();
            Capture.Camera.DeInit();
            Capture.Camera.Dispose();
            Capture.Camera = null;
        }

        public void Dispose()
        {
            Generating = false;
            Task.Run(() => Task.Delay(TimeSpan.FromMilliseconds(Timeout)));
            Close();
        }

        private void AcquireFrames()
        {
            TryEnsureExposureTime();
            AllocateBuffer();
            while (Generating)
                AcquireFrame();
        }

        private void TryEnsureExposureTime()
        {
            try
            {
                EnsureExposureTime();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void EnsureExposureTime()
        {
            if (FPS != Capture.FPS)
            {
                FPS = Capture.FPS;
                Timeout = (ulong)(1000 / FPS);
                Capture.UpdateExposureTime();
            }
        }

        private void AcquireFrame()
        {
            using (var managedImage = Capture.Camera.GetNextImage())
            {
                var newImageIndex = managedImage.FrameID % BufferSize;
                CloneImageData(managedImage, newImageIndex);
                var photometryImage = new PhotometryImage()
                {
                    FrameID = managedImage.FrameID,
                    Timestamp = managedImage.TimeStamp,
                    OffsetX = (ushort)managedImage.OffsetX,
                    OffsetY = (ushort)managedImage.OffsetY,
                    Width = (ushort)managedImage.Width,
                    Height = (ushort)managedImage.Height,
                    BitsPerPixel = (ushort)managedImage.BitsPerPixel,
                    Bitmap = GetBitmap(managedImage, newImageIndex)
                };
                Observer.OnNext(photometryImage);
            }
            TryEnsureExposureTime();
        }

        internal void AllocateBuffer()
        {
            var width = Capture.Camera.Width.Value;
            var height = Capture.Camera.Height.Value;
            // For FP3002, Images are Mono8, so this is correct
            // But for FP3001, Images are Mono16, so buffer size is actually halved
            var bytesPerImage = width * height * BufferSize * Mono16BPP;

            ImageBuffer = Marshal.AllocHGlobal((int)bytesPerImage);
        }

        private void DeallocateBuffer()
        {
            if (ImageBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(ImageBuffer);
        }

        private void CloneImageData(IManagedImage managedImage, ulong newImageIndex)
        {
            unsafe
            {
                var heightInPixels = (int)managedImage.Height;
                var widthInPixels = (int)managedImage.Width;

                var widthInBytes = widthInPixels * managedImage.BitsPerPixel / 8;

                var in_ptrFirstPixel = managedImage.NativeData;
                var bufImg_ptrFirstPixel = (byte*)ImageBuffer + (int)newImageIndex * widthInBytes * heightInPixels;

                Parallel.For(0, heightInPixels, y =>
                {
                    var offset = y * widthInBytes;

                    var in_ptrCurrentPixel = in_ptrFirstPixel + offset;
                    var out_ptrCurrentPixel = bufImg_ptrFirstPixel + offset;

                    var in_ptrLastPixel = in_ptrCurrentPixel + widthInBytes;
                    while (in_ptrCurrentPixel < in_ptrLastPixel)
                        *out_ptrCurrentPixel++ = *in_ptrCurrentPixel++;
                });
            }
        }

        private Bitmap GetBitmap(IManagedImage managedImage, ulong newImageIndex)
        {
            var widthInBytes = managedImage.Width * managedImage.BitsPerPixel / 8;
            var bytesPerImage = widthInBytes * managedImage.Height;
            var pointer = ImageBuffer + (int)(newImageIndex * bytesPerImage);

            return new Bitmap((int)managedImage.Width, (int)managedImage.Height, (int)widthInBytes, PixelFormat.Format16bppGrayScale, pointer);
        }
    }
}