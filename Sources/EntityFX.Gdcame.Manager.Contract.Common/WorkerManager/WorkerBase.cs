﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Contract.Common.WorkerManager
{
    public abstract class WorkerBase : IWorker
    {
        public abstract void Run();
        public string Name { get; protected set; }
        public abstract bool IsRunning { get; }

        public long Ticks
        {
            get { return _ticks; }
        }

        private long _ticks;

        public void IncrementTick()
        {
            Interlocked.Add(ref _ticks, 1);
        }
    }
}
