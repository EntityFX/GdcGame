﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure
{
    public class GenericTaskTimer: TaskTimerBase, ITaskTimer
    {
        private Timer _timer;

        public GenericTaskTimer(ILogger logger,TimeSpan interval, Action tick, bool runOnce = false) : base(logger, interval, tick, runOnce)
        {
            _timer = new Timer(state => tick(), null, Timeout.Infinite, (int) interval.TotalMilliseconds);
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                _timer = new Timer(
                    state =>
                        {
                            try
                            {
                                tick();
                            }
                            catch (Exception e)
                            {
                                Logger.Error(e);
                            }
                        }, null, TimeSpan.Zero, interval);
                IsRunning = true;
            });
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            IsRunning = false;
        }
    }
}