namespace EntityFX.Gdcame.Manager.Contract.Common.WorkerManager
{
    using System.Collections.Generic;

    public class WorkerStatus
    {
        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public long Ticks { get; set; }

        public IDictionary<string, double> PerfomanceCounters { get; set; }
    }
}