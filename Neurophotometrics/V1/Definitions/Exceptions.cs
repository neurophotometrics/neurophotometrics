using System;

namespace Neurophotometrics.V1.Definitions
{
    public class SerialNumberNotFoundException : Exception
    {
        public SerialNumberNotFoundException()
        {
        }

        public SerialNumberNotFoundException(string message)
            : base(message)
        {
        }

        public SerialNumberNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}