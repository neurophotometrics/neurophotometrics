using SpinnakerNET;
using System;

namespace Neurophotometrics.Design
{
    static class SerialNumberHelper
    {
        public static ulong GetCameraSerialNumber()
        {
            using (var system = new ManagedSystem())
            {
                var cameraList = system.GetCameras();
                if (cameraList.Count != 1)
                {
                    throw new InvalidOperationException($"Please disconnect all other imaging devices from the computer before configuring the {nameof(FP3002)}.");
                }

                var camera = cameraList.GetByIndex(0);
                var nodeMap = camera.GetTLDeviceNodeMap();
                var serialNumber = nodeMap.GetNode<SpinnakerNET.GenApi.IString>(nameof(camera.DeviceSerialNumber));
                return ulong.Parse(serialNumber.Value);
            }
        }
    }
}
