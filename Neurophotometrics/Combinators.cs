using Bonsai.Harp;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Neurophotometrics
{
    static class Combinators
    {
        public static IObservable<PhotometryDataFrame> FillMissing(this IObservable<PhotometryDataFrame> source, PhotometryDataFrame defaultValue)
        {
            return Observable.Create<PhotometryDataFrame>(observer =>
            {
                var previousIndex = 0L;
                var sourceObserver = Observer.Create<PhotometryDataFrame>(
                    value =>
                    {
                        var currentIndex = value.FrameCounter;
                        var missingValues = currentIndex - previousIndex - 1;
                        while (missingValues-- > 0)
                        {
                            observer.OnNext(defaultValue);
                        }

                        observer.OnNext(value);
                        previousIndex = currentIndex;
                    },
                    observer.OnError,
                    observer.OnCompleted);
                return source.SubscribeSafe(sourceObserver);
            });
        }

        public static IObservable<HarpMessage> FillMissing(this IObservable<HarpMessage> source, HarpMessage defaultValue)
        {
            return Observable.Create<HarpMessage>(observer =>
            {
                bool first = true;
                long multiplier = 0;
                ushort previousIndex = 0;
                var sourceObserver = Observer.Create<HarpMessage>(
                    value =>
                    {
                        var currentIndex = value.GetPayloadUInt16(1);
                        if (first) first = false;
                        else
                        {
                            var totalPreviousIndex = multiplier * ushort.MaxValue + previousIndex;
                            var delta = currentIndex - previousIndex;
                            if (delta < 0)
                            {
                                multiplier++;
                            }

                            var totalIndex = multiplier * ushort.MaxValue + currentIndex;
                            var missingValues = totalIndex - totalPreviousIndex - 1;
                            while (missingValues-- > 0)
                            {
                                observer.OnNext(defaultValue);
                            }
                        }
                        observer.OnNext(value);
                        previousIndex = currentIndex;
                    },
                    observer.OnError,
                    observer.OnCompleted);
                return source.SubscribeSafe(sourceObserver);
            });
        }
    }
}
