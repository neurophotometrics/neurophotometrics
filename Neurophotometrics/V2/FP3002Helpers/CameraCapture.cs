using Neurophotometrics.V2.Definitions;

using SpinnakerNET;
using SpinnakerNET.GenApi;

using System;
using System.Drawing;
using System.Reactive.Linq;

namespace Neurophotometrics.V2.FP3002Helpers
{
    internal class CameraCapture
    {
        private const ushort ExposureSafetyMargin = 1000;

        private ushort TriggerPeriod;
        private ushort ExposureTime;
        private bool AutoCrop = true;

        private readonly Photometry Photometry;
        private FrameFactory FrameFactory;
        internal IManagedCamera Camera;

        public event EventHandler ReadyToAcquireFrames;

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
            SetCrop();
            InitializeFrameFactory();
        }

        private void ResetStoredProps()
        {
            TriggerPeriod = 0;
            ExposureTime = 0;
        }

        private void InitializeFrameFactory()
        {
            FrameFactory = new FrameFactory(this)
            {
                Timeout = 10
            };
            FrameFactory.AllocateBuffer();
            FrameFactory.ReadyToAcquireFrames += FrameFactory_ReadyToAcquireFrames;
        }

        private void FrameFactory_ReadyToAcquireFrames(object sender, EventArgs e)
        {
            ReadyToAcquireFrames?.Invoke(this, EventArgs.Empty);
        }

        private void SetCrop()
        {
            if (AutoCrop)
            {
                var crop = Photometry.GetCrop();

                var width = crop.Width + Camera.Width.Increment - (crop.Width - Camera.Width.Min) % Camera.Width.Increment;
                var height = crop.Height + Camera.Height.Increment - (crop.Height - Camera.Height.Min) % Camera.Height.Increment;
                width = Math.Min(Camera.WidthMax, width);
                height = Math.Min(Camera.HeightMax, height);
                var xpos = crop.X - crop.X % Camera.OffsetX.Increment;
                var ypos = crop.Y - crop.Y % Camera.OffsetY.Increment;
                xpos = Math.Max(0, xpos);
                ypos = Math.Max(0, ypos);

                Camera.OffsetX.Value = 0;
                Camera.OffsetY.Value = 0;
                Camera.Width.Value = width;
                Camera.Height.Value = height;
                Camera.OffsetX.Value = xpos;
                Camera.OffsetY.Value = ypos;
            }
            else
            {
                Camera.OffsetX.Value = 0;
                Camera.OffsetY.Value = 0;
                Camera.Width.Value = Camera.WidthMax;
                Camera.Height.Value = Camera.HeightMax;
            }

            Photometry.RegionOffset = new Rectangle((int)Camera.OffsetX.Value, (int)Camera.OffsetY.Value, (int)Camera.Width.Value, (int)Camera.Height.Value);
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

        internal void SetTriggerPeriod(ushort triggerPeriod)
        {
            if (TriggerPeriod != 0) return;

            TriggerPeriod = triggerPeriod;
        }

        internal void SetExposureTime(ushort triggerTimeUpdateOutputs)
        {
            if (ExposureTime != 0) return;

            ExposureTime = (ushort)(triggerTimeUpdateOutputs - ExposureSafetyMargin / 2);
            if (Camera != null)
                Camera.ExposureTime.Value = ExposureTime;
        }

        internal void SetAutoCrop(bool autoCrop)
        {
            AutoCrop = autoCrop;
        }

        public void EnsureCamera(string targetDeviceUserID)
        {
            using (var system = new ManagedSystem())
            {
                var camera = GetCamera(system, targetDeviceUserID); // 361ms
                camera.Init();  // 341ms
                EnsureCameraSettings(camera); // 62ms
                Camera = camera;
            }
        }

        private IManagedCamera GetCamera(ManagedSystem system, string targetDeviceUserID)
        {
            var cameras = system.GetCameras();
            var camera = GetTargetCamera(cameras, targetDeviceUserID);

            if (camera != null) return camera;

            if (cameras.Count == 0)
                throw new SpinnakerException("No Cameras Found");
            if (cameras.Count > 1)
                throw new SpinnakerException("Multiple Cameras Found");

            SetCameraDeviceID(cameras[0], targetDeviceUserID);
            return cameras[0];
        }
        private IManagedCamera GetTargetCamera(ManagedCameraList cameras, string targetDeviceUserID)
        {
            foreach (var camera in cameras)
            {
                var nodeMap = camera.GetTLDeviceNodeMap();
                var deviceUserID = nodeMap.GetNode<IString>("DeviceUserID");
                var isTargetCamera = deviceUserID.Value == targetDeviceUserID;

                if (isTargetCamera) return camera;
            }
            return null;
        }

        private void SetCameraDeviceID(IManagedCamera camera, string deviceUserID)
        {
            camera.Init();
            camera.DeviceUserID.Value = deviceUserID;
            camera.DeInit();
        }

        private void EnsureCameraSettings(IManagedCamera camera)
        {
            camera.BinningSelector.Value = BinningSelectorEnums.All.ToString();
            camera.BinningHorizontalMode.Value = BinningHorizontalModeEnums.Sum.ToString();
            camera.BinningVerticalMode.Value = BinningVerticalModeEnums.Sum.ToString();
            camera.BinningHorizontal.Value = 2;
            camera.BinningVertical.Value = 2;
            camera.V3_3Enable.Value = true;
            camera.GammaEnable.Value = false;
            camera.AcquisitionFrameRateEnable.Value = false;
            camera.DeviceLinkThroughputLimit.Value = camera.DeviceLinkThroughputLimit.Max;
            camera.PixelFormat.Value = PixelFormatEnums.Mono8.ToString();
            camera.TriggerSelector.Value = TriggerSelectorEnums.FrameStart.ToString();
            camera.TriggerSource.Value = TriggerSourceEnums.Line0.ToString();
            camera.TriggerMode.Value = TriggerModeEnums.On.ToString();
            camera.TriggerOverlap.Value = TriggerOverlapEnums.ReadOut.ToString();
            camera.TriggerActivation.Value = TriggerActivationEnums.RisingEdge.ToString();
            camera.LineSelector.Value = LineSelectorEnums.Line1.ToString();
            camera.LineMode.Value = LineModeEnums.Output.ToString();
            camera.LineSource.Value = LineSourceEnums.ExposureActive.ToString();
            camera.ExposureAuto.Value = ExposureAutoEnums.Off.ToString();
            camera.ExposureMode.Value = ExposureModeEnums.Timed.ToString();
            camera.GainAuto.Value = GainAutoEnums.Off.ToString();
            camera.Gain.Value = 0;
        }

        public void VerifyThroughput()
        {
            if (Camera == null) return;

            const long MinimumAllowableThroughput = 43000000;
            var maxThroughput = Camera.DeviceLinkThroughputLimit.Max;
            if (maxThroughput < MinimumAllowableThroughput)
            {
                var errorMessage = string.Concat(
                    "The current maximum link throughput (", maxThroughput, ") is insufficient for operation of the FP3002. ",
                    "Please make sure the system is connected to a USB 3.0 port using the Neurophotometrics cable.");
                throw new InvalidOperationException(errorMessage);
            }

            var maxFrameRate = Camera.AcquisitionFrameRate.Value;
            var minTriggerPeriod = 1e6 / maxFrameRate;
            if (TriggerPeriod < minTriggerPeriod)
            {
                var errorMessage = string.Concat(
                    "The current maximum acquisition rate (", maxFrameRate.ToString("F2"),
                    " Hz) cannot match the requested trigger frequency (", (1e6 / TriggerPeriod).ToString("F2"),
                    " Hz). Please make sure the system is connected to a USB 3.0 port using the Neurophotometrics cable.",
                    " Otherwise, make sure ROIs are defined as small as possible and AutoCrop is enabled.");
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}