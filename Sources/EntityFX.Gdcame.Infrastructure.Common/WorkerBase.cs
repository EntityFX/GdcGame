using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public abstract class WorkerBase : IWorker
    {
        public abstract void Run<TData>(TData data = null) where TData : class;
        public string Name { get; protected set; }
        public abstract bool IsRunning { get; }

        public virtual bool IsRunOnStart
        {
            get
            {
                return true;
            }
        }

        public long Ticks
        {
            get { return _ticks; }
        }

        private long _ticks;

        private readonly IDictionary<string, double> perfomanceCounters = new Dictionary<string, double>();

        public IDictionary<string, double> PerfomanceCounters
        {
            get
            {
                return this.perfomanceCounters;
            }
        }

        public void IncrementTick()
        {
            Interlocked.Add(ref _ticks, 1);
        }
    }
}
