using Neurophotometrics.Properties;
using Neurophotometrics.V1.Definitions;

using SpinnakerNET;

using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Neurophotometrics.V1.FP3001Helpers
{
    internal class CameraCapture
    {
        private const ushort ExposureSafetyMargin = 1000;
        private const int FPS_Default = 40;

        private int _FPS = FPS_Default;

        internal int FPS
        {
            get { return _FPS; }
            set
            {
                _FPS = value;
                ExposureTime = 1000.0 * 1000.0 / _FPS - ExposureSafetyMargin;
            }
        }

        internal string SerialNumber;
        private double ExposureTime = 1000.0 * 1000.0 / FPS_Default - ExposureSafetyMargin;
        private bool AutoCrop = true;

        private readonly Photometry Photometry;
        private FrameFactory FrameFactory;
        internal IManagedCamera Camera;

        public CameraCapture(Photometry photometry)
        {
            Photometry = photometry;
        }

        public IObservable<PhotometryImage> Generate()
        {
            TryInitializeCapture();
            return Observable.Create<PhotometryImage>(observer =>
            {
                TryOpenCapture(observer);
                return FrameFactory;
            }).Finally(() => ResetStoredProps());
        }

        private void TryInitializeCapture()
        {
            try
            {
                InitializeCapture();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }
        private void InitializeCapture()
        {
            Camera = GetCameraBySerialNumber();
            Camera.Init();
            EnsureCameraSettings();
            SetCrop();
            InitializeFrameFactory();
        }
        private void InitializeFrameFactory()
        {
            FrameFactory = new FrameFactory(this)
            {
                Timeout = 10
            };
            FrameFactory.AllocateBuffer();
        }
        private void ResetStoredProps()
        {
            //FPS = FPS_Default;
        }
        internal void UpdateExposureTime()
        {
            if (Camera != null)
            {
                Camera.ExposureTime.Value = ExposureTime;
            }
        }


        private void TryOpenCapture(IObserver<PhotometryImage> observer)
        {
            try
            {
                OpenCapture(observer);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void OpenCapture(IObserver<PhotometryImage> observer)
        {
            Camera.BeginAcquisition();
            FrameFactory.SetObserver(observer);
            FrameFactory.Open();
        }

        private IManagedCamera GetCameraBySerialNumber()
        {
            using (var system = new ManagedSystem())
            {
                var cameraList = system.GetCameras();
                var camera = cameraList.GetBySerial(SerialNumber);

                if (camera == null)
                    throw new SerialNumberNotFoundException("Cannot Find Camera By Serial Number");

                cameraList.Clear();
                return camera;
            }
        }

        private void SetCrop()
        {
            // NOTICE: Must still apply a crop even when cropping is turned off (i.e. displaying in the FP3001 Editor)
            // This changing of the crop fixes a bug where the camera would not be triggered to acquire a frame in the FP3001 Editor
            // after running the workflow at rates over the bandwidth maximum for an uncropped image (>40Hz)
            var crop = Photometry.GetCrop();
            var width = Math.Min(Math.Ceiling((double)crop.Width / Camera.Width.Increment) * Camera.Width.Increment, Camera.WidthMax);
            var height = Math.Min(Math.Ceiling((double)crop.Height / Camera.Height.Increment) * Camera.Height.Increment, Camera.HeightMax);
            Camera.Width.Value = (long)width;
            Camera.Height.Value = (long)height;
            Camera.OffsetX.Value = Math.Max(Math.Min(crop.X / Camera.OffsetX.Increment * Camera.OffsetX.Increment, Camera.OffsetX.Max), 0);
            Camera.OffsetY.Value = Math.Max(Math.Min(crop.Y / Camera.OffsetY.Increment * Camera.OffsetY.Increment, Camera.OffsetY.Max), 0);
            if (!AutoCrop)
            {
                Camera.OffsetX.Value = 0;
                Camera.OffsetY.Value = 0;
                Camera.Width.Value = Camera.WidthMax;
                Camera.Height.Value = Camera.HeightMax;
            }

            Photometry.RegionOffset = new Rectangle((int)Camera.OffsetX.Value, (int)Camera.OffsetY.Value, (int)Camera.Width.Value, (int)Camera.Height.Value);
        }

        internal void EnsureCameraSerialNumber()
        {
            using (var system = new ManagedSystem())
            {
                var cameraList = system.GetCameras();
                var camera = cameraList.GetBySerial(SerialNumber);

                if (camera == null)
                    throw new SerialNumberNotFoundException("Cannot Find Camera By Serial Number");

                cameraList.Clear();
            }
        }

        internal void SetAutoCrop(bool autoCrop)
        {
            AutoCrop = autoCrop;
        }

        private void EnsureCameraSettings()
        {
            Camera.PixelFormat.Value = PixelFormatEnums.Mono16.ToString();
            Camera.TriggerSource.Value = TriggerSourceEnums.Line0.ToString();
            Camera.TriggerMode.Value = TriggerModeEnums.On.ToString();
            Camera.TriggerOverlap.Value = TriggerOverlapEnums.ReadOut.ToString();
            Camera.TriggerActivation.Value = TriggerActivationEnums.RisingEdge.ToString();
            Camera.ExposureAuto.Value = ExposureAutoEnums.Off.ToString();
            Camera.ExposureMode.Value = ExposureModeEnums.Timed.ToString();
            Camera.GainAuto.Value = GainAutoEnums.Off.ToString();
            Camera.Gain.Value = 0;
        }
    }
}