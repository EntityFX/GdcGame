using System;

namespace EntityFX.Gdcame.Utils.Common
{
    public abstract class TaskTimerBase
    {
        protected readonly TimeSpan interval;
        protected readonly bool runOnce;
        protected readonly Action tick;

        public bool IsRunning { get; protected set; }


        protected TaskTimerBase(TimeSpan interval, Action tick, bool runOnce = false)
        {
            this.interval = interval;
            this.tick = tick;
            this.runOnce = runOnce;
        }
    }
}