using SpinnakerNET;
using SpinnakerNET.GenApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Neurophotometrics.V1.FP3001Helpers
{
    internal class SerialNumberConverter : StringConverter
    {
        private static readonly object Lock = new object();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var serialNumbers = new List<string>();
            TryGetSerialNumbers(ref serialNumbers);

            return new StandardValuesCollection(serialNumbers);
        }

        private void TryGetSerialNumbers(ref List<string> serialNumbers)
        {
            try
            {
                GetSerialNumbers(ref serialNumbers);
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
        }

        private void GetSerialNumbers(ref List<string> serialNumbers)
        {
            lock (Lock)
            {
                using (var system = new ManagedSystem())
                {
                    var cameras = system.GetCameras();
                    foreach (var camera in cameras)
                    {
                        var nodeMap = camera.GetTLDeviceNodeMap();
                        var deviceSerialNumber = nodeMap.GetNode<IString>("DeviceSerialNumber");
                        serialNumbers.Add(deviceSerialNumber.Value);
                    }
                }
            }
        }
    }
}