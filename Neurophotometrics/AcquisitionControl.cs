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
        [Description("Specifies whether to start or stop the acquisition sequence.")]
        public AcquisitionModes Mode { get; set; } = AcquisitionModes.StartPhotometry;

        protected override Expression BuildSelector(Expression expression)
        {
            return Expression.Call(typeof(AcquisitionControl), nameof(CreateStartCommand), null, Expression.Constant(Mode));
        }

        static HarpMessage CreateStartCommand(AcquisitionModes mode)
        {
            return HarpCommand.WriteByte(address: Registers.Start, (byte)mode);
        }
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
