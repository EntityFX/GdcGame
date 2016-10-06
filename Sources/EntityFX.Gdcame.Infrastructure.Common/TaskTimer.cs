using System;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class TaskTimer
    {
        private readonly TimeSpan interval;
        private readonly bool runOnce;
        private readonly Action tick;
        private bool timerRunning;

        public TaskTimer(TimeSpan interval, Action tick, bool runOnce = false)
        {
            this.interval = interval;
            this.tick = tick;
            this.runOnce = runOnce;
        }

        public async void Start()
        {
            if (!timerRunning)
            {
                timerRunning = true;
                await RunTimer();
            }
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