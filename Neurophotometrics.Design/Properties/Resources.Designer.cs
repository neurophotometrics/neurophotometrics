﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Neurophotometrics.Design.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Neurophotometrics.Design.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The device ID does not match a valid FP3002 system. Please check if the specified serial port is correct and try again..
        /// </summary>
        internal static string InvalidDeviceID_Error {
            get {
                return ResourceManager.GetString("InvalidDeviceID_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WARNING: To protect the user and the system&apos;s internal laser, the laser amplitude cannot exceed 50% when operating with a duty cycle over 75%. Also, the duration of stimulation cannot exceed 1 minute while operating with a duty cycle over 75%..
        /// </summary>
        internal static string LaserCalibrationRestriction_Warning {
            get {
                return ResourceManager.GetString("LaserCalibrationRestriction_Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The configuration file does not match the device serial number. Do you want to proceed?.
        /// </summary>
        internal static string MatchingSerialNumbers_Warning {
            get {
                return ResourceManager.GetString("MatchingSerialNumbers_Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Neurophotometrics {
            get {
                object obj = ResourceManager.GetObject("Neurophotometrics", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This will reset the device to its default factory settings. Do you want to continue?.
        /// </summary>
        internal static string ResetPersistentRegisters_Question {
            get {
                return ResourceManager.GetString("ResetPersistentRegisters_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to save the register values on persistent device memory?.
        /// </summary>
        internal static string SavePersistentRegisters_Question {
            get {
                return ResourceManager.GetString("SavePersistentRegisters_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Have you attached your second patch cord?.
        /// </summary>
        internal static string SecondaryPatchCord_Question {
            get {
                return ResourceManager.GetString("SecondaryPatchCord_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The firmware on the device is unsupported by the current driver. Please update the firmware version on the device or contact support for assistance..
        /// </summary>
        internal static string UnsupportedFirmwareVersion_Error {
            get {
                return ResourceManager.GetString("UnsupportedFirmwareVersion_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The hardware version of this FP3002 system is incompatible with the current driver. Please contact support for assistance..
        /// </summary>
        internal static string UnsupportedHardwareVersion_Error {
            get {
                return ResourceManager.GetString("UnsupportedHardwareVersion_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no registered imaging sensor in the camera firmware. Please disconnect all other imaging devices from the computer to allow registration..
        /// </summary>
        internal static string UpdateCameraSerialNumber_Warning {
            get {
                return ResourceManager.GetString("UpdateCameraSerialNumber_Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The firmware on the device is older than the current driver firmware. Do you want to update the firmware on the device? (recommended).
        /// </summary>
        internal static string UpdateDeviceFirmware_Question {
            get {
                return ResourceManager.GetString("UpdateDeviceFirmware_Question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The firmware on the device is newer than the current driver firmware. Please update the Bonsai package or contact support for assistance..
        /// </summary>
        internal static string UpdateDriverVersion_Error {
            get {
                return ResourceManager.GetString("UpdateDriverVersion_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The FP3001 system cannot be calibrated while the workflow is running. Please stop the workflow and try again..
        /// </summary>
        internal static string WorkflowRunning_Error {
            get {
                return ResourceManager.GetString("WorkflowRunning_Error", resourceCulture);
            }
        }
    }
}
