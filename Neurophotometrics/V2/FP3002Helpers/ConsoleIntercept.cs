using System;
using System.ComponentModel;
using System.IO;

namespace Neurophotometrics.V2.FP3002Helpers
{
    internal class ConsoleIntercept : StringWriter
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void FlushedEventHandler(object sender, EventArgs args);

        public event FlushedEventHandler Flushed;

        public virtual bool AutoFlush { get; set; }

        public ConsoleIntercept()
            : base() { }

        public ConsoleIntercept(bool autoFlush)
            : base() { AutoFlush = autoFlush; }

        protected void OnFlush()
        {
            Flushed?.Invoke(this, EventArgs.Empty);
        }

        public override void Flush()
        {
            base.Flush();
            OnFlush();
        }

        public override void Write(char value)
        {
            base.Write(value);
            if (AutoFlush) Flush();
        }

        public override void Write(string value)
        {
            base.Write(value);
            if (AutoFlush) Flush();
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            if (AutoFlush) Flush();
        }
    }
}