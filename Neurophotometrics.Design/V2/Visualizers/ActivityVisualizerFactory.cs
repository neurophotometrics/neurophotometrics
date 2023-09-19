using Neurophotometrics.V2.Definitions;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Neurophotometrics.Design.V2.Visualizers
{
    public sealed class ActivityVisualizerFactory : IDisposable
    {
        private const double TargetElapsedTime = 1.0 / 40.0;

        private readonly ConcurrentQueue<PhotometryDataFrame> FrameQueue;
        private readonly ConcurrentQueue<VisualizerData> OutputQueue;
        private readonly object Observer;

        private double PreviousUpdateTime;
        private bool Generating = true;
        private bool IsNewFrame;

        internal ActivityVisualizerFactory(object observer)
        {
            PreviousUpdateTime = 0;
            Observer = observer;
            FrameQueue = new ConcurrentQueue<PhotometryDataFrame>();
            OutputQueue = new ConcurrentQueue<VisualizerData>();
        }

        public void Open()
        {
            Task.Factory.StartNew(new Action<object>(DataGenerator), Observer);
        }

        public void Write(PhotometryDataFrame newFrame)
        {
            FrameQueue.Enqueue(newFrame);
            IsNewFrame = true;
        }

        public void Dispose()
        {
            Generating = false;
        }

        private void DataGenerator(object observer)
        {
            var processObserver = (IObserver<ConcurrentQueue<VisualizerData>>)observer;
            while (Generating)
            {
                while (IsNewFrame)
                {
                    var isReadFrame = FrameQueue.TryDequeue(out var currentFrame);
                    if (!isReadFrame) continue;

                    var newVisualizerData = new VisualizerData()
                    {
                        FrameCounter = currentFrame.FrameCounter,
                        SystemTimestamp = currentFrame.SystemTimestamp,
                        Flags = currentFrame.Flags,
                        Activities = currentFrame.Activities
                    };
                    OutputQueue.Enqueue(newVisualizerData);

                    if (currentFrame.SystemTimestamp - PreviousUpdateTime > TargetElapsedTime)
                    {
                        PreviousUpdateTime = currentFrame.SystemTimestamp;
                        processObserver.OnNext(OutputQueue);
                    }
                }

            }
        }
    }
}