namespace EntityFX.Gdcame.Infrastructure.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IWorker
    {
        void Run<TData>(TData data = default(TData)) where TData : class;

        string Name { get; }

        bool IsRunOnStart { get; }

        bool IsRunning { get; }

        long Ticks { get; }

        IDictionary<string, double> PerfomanceCounters { get; }

        void IncrementTick();
    }
}