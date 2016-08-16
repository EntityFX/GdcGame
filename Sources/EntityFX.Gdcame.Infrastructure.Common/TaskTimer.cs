using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Common
{

    public class TaskTimer
    {
        private bool timerRunning;
        private TimeSpan interval;
        private Action tick;
        private bool runOnce;

        public TaskTimer(TimeSpan interval, Action tick, bool runOnce = false)
        {
            this.interval = interval;
            this.tick = tick;
            this.runOnce = runOnce;
        }

        public TaskTimer Start()
        {
            if (!timerRunning)
            {
                timerRunning = true;
                RunTimer();
            }

            return this;
        }

        public void Stop()
        {
            timerRunning = false;
        }

        private async Task RunTimer()
        {
            while (timerRunning)
            {
                await Task.Delay(interval);

                if (timerRunning)
                {
                    tick();

                    if (runOnce)
                    {
                        Stop();
                    }
                }
            }
        }
    }
}