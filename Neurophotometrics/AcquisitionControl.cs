using Bonsai.Expressions;
using Bonsai.Harp;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Neurophotometrics
{
    [Description("Creates command messages for controlling the acquisition mode of FP3002 devices.")]
    public class AcquisitionControl : SelectBuilder
    {
        [Description("Specifies which acquisition streams to control on the device.")]
        public AcquisitionStreams Streams { get; set; } = AcquisitionStreams.Photometry;

        [Description("Specifies whether to start, stop, or control the stream directly from the input value.")]
        public CommandMode Mode { get; set; } = CommandMode.Start;
        
	
        protected override Expression BuildSelector(Expression expression)
        {
            var instance = Expression.Constant(this);
            switch (Streams)
            {
                case AcquisitionStreams.None: return Expression.Call(instance, nameof(CreateNullCommand), null);
                case AcquisitionStreams.Photometry:
                    if (Mode != CommandMode.Control) return Expression.Call(instance, nameof(CreatePhotometryCommand), null);
                    else return Expression.Call(instance, nameof(CreatePhotometryCommand), null, expression);
                case AcquisitionStreams.ExternalCamera:
                    if (Mode != CommandMode.Control) return Expression.Call(instance, nameof(CreateExternalCameraCommand), null);
                    else return Expression.Call(instance, nameof(CreateExternalCameraCommand), null, expression);
                default:
                    if (Mode != CommandMode.Control) return Expression.Call(instance, nameof(CreateStartCommand), null);
                    else return Expression.Call(instance, nameof(CreateStartCommand), null, expression);
            }
        }

        HarpMessage CreateNullCommand()
        {
            return CreateStartCommand(AcquisitionModes.None);
        }

        HarpMessage CreatePhotometryCommand()
        {
            return CreatePhotometryCommand(Mode == CommandMode.Start);
        }

        HarpMessage CreatePhotometryCommand(bool mode)
        {
            return CreateStartCommand(mode ? AcquisitionModes.StartPhotometry : AcquisitionModes.StopPhotometry);
        }

        HarpMessage CreatePhotometryCommand(byte mode)
        {
            return CreatePhotometryCommand((AcquisitionModes)mode);
        }

        HarpMessage CreatePhotometryCommand(AcquisitionModes mode)
        {
            const AcquisitionModes PhotometryMask = AcquisitionModes.StartPhotometry | AcquisitionModes.StopPhotometry;
            return CreateStartCommand(PhotometryMask & mode);
        }

        HarpMessage CreateExternalCameraCommand()
        {
            return CreateExternalCameraCommand(Mode == CommandMode.Start);
        }

        HarpMessage CreateExternalCameraCommand(bool mode)
        {
            return CreateStartCommand(mode ? AcquisitionModes.StartExternalCamera : AcquisitionModes.StopExternalCamera);
        }

        HarpMessage CreateExternalCameraCommand(byte mode)
        {
            return CreateExternalCameraCommand((AcquisitionModes)mode);
        }

        HarpMessage CreateExternalCameraCommand(AcquisitionModes mode)
        {
            const AcquisitionModes PhotometryMask = AcquisitionModes.StartExternalCamera | AcquisitionModes.StopExternalCamera;
            return CreateStartCommand(PhotometryMask & mode);
        }

        HarpMessage CreateStartCommand()
        {
            return CreateStartCommand(Mode == CommandMode.Start);
        }

        HarpMessage CreateStartCommand(bool mode)
        {
            return CreateStartCommand(mode
                ? AcquisitionModes.StartPhotometry | AcquisitionModes.StartExternalCamera
                : AcquisitionModes.StartPhotometry | AcquisitionModes.StopExternalCamera);
        }

        HarpMessage CreateStartCommand(byte mode)
        {
            return CreateStartCommand((AcquisitionModes)mode);
        }

        HarpMessage CreateStartCommand(AcquisitionModes mode)
        {
            return HarpCommand.WriteByte(address: Registers.Start, (byte)mode);
        }
    }

    [Flags]
    public enum AcquisitionStreams : byte
    {
        None = 0,
        Photometry = 1 << 0,
        ExternalCamera = 1 << 1
    }
    
    [Flags]
    public enum AcquisitionModes : byte
    {
        None = 0,
        StartPhotometry = 1 << 0,
        StartExternalCamera = 1 << 2,
        StopPhotometry = 1 << 3,
        StopExternalCamera = 1 << 4
    }
}
