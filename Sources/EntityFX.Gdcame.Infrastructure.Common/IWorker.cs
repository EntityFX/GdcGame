namespace EntityFX.Gdcame.Infrastructure.Common
{
    using System;

    public interface IWorker
    {
        void Run<TData>(TData data = default(TData)) where TData : class;

        string Name { get; }

        bool IsRunOnStart { get; }

        bool IsRunning { get; }

        long Ticks { get; }

        void IncrementTick();
    }
}