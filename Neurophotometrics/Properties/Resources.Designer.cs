﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Neurophotometrics.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Neurophotometrics.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] FP3002_fw2_6_harp1_9_hw2_0_ass0 {
            get {
                object obj = ResourceManager.GetObject("FP3002_fw2_6_harp1_9_hw2_0_ass0", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot connect to FP3001 System. Please ensure that the system is connected and turned on. Also, please verify that the &quot;Serial Number&quot; property is specified..
        /// </summary>
        internal static string MsgBox_Error_ConnectCamera {
            get {
                return ResourceManager.GetString("MsgBox_Error_ConnectCamera", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported Bonsai.Harp version detected. Please uninstall the &quot;Neurophotometrics.Design&quot; and &quot;Neurophotometrics&quot; packages and reinstall them. This will ensure the correct version of Bonsai.Harp is installed..
        /// </summary>
        internal static string MsgBox_Error_HarpVersion {
            get {
                return ResourceManager.GetString("MsgBox_Error_HarpVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to include the plots of activity data for this recording? This feature is not recommended for recordings longer than 1 hour..
        /// </summary>
        internal static string MsgBox_Question_IncludePlots {
            get {
                return ResourceManager.GetString("MsgBox_Question_IncludePlots", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to include the video recording of the photometry data? Enabling this will result in large amounts of data being saved. .
        /// </summary>
        internal static string MsgBox_Question_IncludeVideo {
            get {
                return ResourceManager.GetString("MsgBox_Question_IncludeVideo", resourceCulture);
            }
        }
    }
}