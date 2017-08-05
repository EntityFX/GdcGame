using System;

namespace EntityFX.Gdcame.Utils.Common
{
    using EntityFX.Gdcame.Infrastructure.Common;

    public abstract class TaskTimerBase
    {
        protected readonly TimeSpan interval;
        protected readonly bool runOnce;
        protected readonly Action tick;
        protected ILogger Logger { get; private  set; }

        public bool IsRunning { get; protected set; }


        protected TaskTimerBase(ILogger logger, TimeSpan interval, Action tick, bool runOnce = false)
        {
            this.interval = interval;
            this.tick = tick;
            this.runOnce = runOnce;
            this.Logger = logger;
        }
    }
}