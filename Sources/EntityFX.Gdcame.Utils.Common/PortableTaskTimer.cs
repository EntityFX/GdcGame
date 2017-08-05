using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class PortableTaskTimer : TaskTimerBase, ITaskTimer
    {
        public PortableTaskTimer(ILogger logger, TimeSpan interval, Action tick, bool runOnce = false) : base(logger, interval, tick, runOnce)
        {
        }

        public async Task Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                await RunTimer();
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }



        private async Task RunTimer()
        {
            while (IsRunning)
            {
                var task = Task.Delay(interval);
                await task;
                if (IsRunning)
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