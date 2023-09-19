using Bonsai.Harp;
using System;
using System.IO;

namespace Neurophotometrics.V2.FP3002Helpers
{
    public class DeviceInformation
    {
        private const ushort DeviceFirmwarePageSize = 512;
        public const string TargetWhoAmI = "2064";
        public const string TargetDeviceName = "FP3002";
        public string WhoAmI { get; set; }
        public string DeviceName { get; set; }
        public string DeviceSerialNumber { get; set; }

        public void Clear()
        {
            WhoAmI = "";
            DeviceName = "";
            DeviceSerialNumber = "";
        }

        public string GetTargetDeviceUserID()
        {
            return DeviceName + "-" + WhoAmI + "-" + DeviceSerialNumber;
        }

        public bool IsDeviceFound()
        {
            return WhoAmI.Equals(TargetWhoAmI) && DeviceName.Equals(TargetDeviceName);
        }
        public FirmwareMetadata TryGetTargetFirmwareMetaData()
        {
            try
            {
                return GetTargetFirmwareMetadata();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
            return null;
        }

        public DeviceFirmware TryGetTargetFirmware()
        {
            try
            {
                return GetTargetFirmware();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
            return null;
        }

        private FirmwareMetadata GetTargetFirmwareMetadata()
        {
            var firmwareLocation = GetTargetFirmwareLocation();
            if (firmwareLocation == null) return null;
            return FirmwareMetadata.Parse(Path.GetFileNameWithoutExtension(firmwareLocation));
        }

        private DeviceFirmware GetTargetFirmware()
        {
            var firmwareLocation = GetTargetFirmwareLocation();
            if (firmwareLocation == null) return null;
            return DeviceFirmware.FromFile(firmwareLocation, DeviceFirmwarePageSize);
        }

        private string GetTargetFirmwareLocation()
        {
            var assemblyLocation = typeof(FP3002).Assembly.Location;
            var firmwarePath = Path.Combine(Path.GetDirectoryName(assemblyLocation), @"..\..\content\Resources\");
            firmwarePath = Path.GetFullPath(firmwarePath);
            try
            {
                var firmwareFiles = Directory.GetFiles(Path.GetFullPath(firmwarePath), "*.hex");
                if (firmwareFiles.Length != 1) return null;
                return firmwareFiles[0];
            }
            catch { return null; }
        }
    }
}