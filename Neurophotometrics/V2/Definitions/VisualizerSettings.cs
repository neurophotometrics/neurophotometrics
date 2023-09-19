using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Neurophotometrics.V2.Definitions
{
    [Serializable()]
    public class VisualizerSettings : ISerializable
    {
        public List<PlotSetting> PlotSettings { get; set; }
        public int Capacity { get; set; }

        public VisualizerSettings()
        {
            PlotSettings = new List<PlotSetting>();
            Capacity = 1000;
        }

        protected VisualizerSettings(SerializationInfo info, StreamingContext context)
        {
            PlotSettings = (List<PlotSetting>)info.GetValue("PlotSettings", typeof(List<PlotSetting>));
            Capacity = (int)info.GetValue("Capacity", typeof(int));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PlotSettings", PlotSettings);
            info.AddValue("Capacity", Capacity);
        }
    }

    [Serializable()]
    public class PlotSetting : ISerializable
    {
        public PhotometryRegion PhotometryRegion { get; set; }
        public bool IsVisible { get; set; }
        public List<SignalSetting> SignalSettings { get; set; }

        public PlotSetting()
        {
            IsVisible = true;
            SignalSettings = new List<SignalSetting>();
        }

        public PlotSetting(PhotometryRegion photometryRegion, bool isVisible)
        {
            IsVisible = isVisible;
            PhotometryRegion = photometryRegion;
            SignalSettings = new List<SignalSetting>();
        }

        protected PlotSetting(SerializationInfo info, StreamingContext context)
        {
            PhotometryRegion = (PhotometryRegion)info.GetValue("PhotometryRegion", typeof(PhotometryRegion));
            IsVisible = (bool)info.GetValue("IsVisible", typeof(bool));
            SignalSettings = (List<SignalSetting>)info.GetValue("SignalSettings", typeof(List<SignalSetting>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PhotometryRegion", PhotometryRegion);
            info.AddValue("IsVisible", IsVisible);
            info.AddValue("SignalSettings", SignalSettings);
        }
    }

    [Serializable()]
    public class SignalSetting : ISerializable
    {
        public string Name { get; set; }
        public FrameFlags LEDFlag { get; set; }
        public bool IsVisible { get; set; }
        public Scaling Scaling { get; set; }

        public SignalSetting()
        {
            LEDFlag = FrameFlags.None;
            Scaling = new Scaling();
        }

        public SignalSetting(FrameFlags ledFlag, RegionChannel channel)
        {
            Name = ledFlag.ToString();
            LEDFlag = ledFlag;
            IsVisible = GetDefaultVisibility(ledFlag, channel);
            Scaling = new Scaling();
        }

        protected SignalSetting(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            LEDFlag = (FrameFlags)(ushort)info.GetValue("LEDFlag", typeof(ushort));
            IsVisible = (bool)info.GetValue("IsVisible", typeof(bool));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("LEDFlag", (ushort)LEDFlag);
            info.AddValue("IsVisible", IsVisible);
        }

        private bool GetDefaultVisibility(FrameFlags ledFlag, RegionChannel channel)
        {
            if (channel == RegionChannel.Green)
                return ledFlag == FrameFlags.L415 || ledFlag == FrameFlags.L470;
            else if (channel == RegionChannel.Red)
                return ledFlag == FrameFlags.L415 || ledFlag == FrameFlags.L560;
            else
                return true;
        }
    }

    [Serializable()]
    public class Scaling : ISerializable
    {
        public AutoScalingMode Mode { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public Scaling()
        {
            Mode = AutoScalingMode.Auto;
            Min = 0.0;
            Max = 1.0;
        }

        protected Scaling(SerializationInfo info, StreamingContext context)
        {
            Mode = (AutoScalingMode)(byte)info.GetValue("Mode", typeof(byte));
            Min = (double)info.GetValue("Min", typeof(double));
            Max = (double)info.GetValue("Max", typeof(double));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Mode", (byte)Mode);
            info.AddValue("Min", Min);
            info.AddValue("Max", Max);
        }
    }

    public enum AutoScalingMode : byte
    {
        Auto,
        Manual
    }
}