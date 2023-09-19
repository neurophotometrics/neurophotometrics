using System;
using System.IO;
using System.Reactive.Concurrency;

namespace Neurophotometrics.V1.PhotometryWriterHelpers
{
    public sealed class ActivityWriterDisposable : IDisposable
    {
        private readonly EventLoopScheduler Scheduler;

        public ActivityWriterDisposable()
        {
            Scheduler = new EventLoopScheduler();
        }

        public StreamWriter Writer { get; set; }

        public void Schedule(Action action)
        {
            if (Scheduler == null) action();
            else Scheduler.Schedule(action);
        }

        void DisposeInternal()
        {
            var writer = Writer;
            if (writer != null)
            {
                writer.Dispose();
            }
        }

        public void Dispose()
        {
            if (Scheduler == null) DisposeInternal();
            else
            {
                Scheduler.Schedule(() =>
                {
                    DisposeInternal();
                    Scheduler.Dispose();
                });
            }
        }
    }
}