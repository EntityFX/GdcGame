namespace EntityFX.Gdcame.Common.Application.Model
{
    using System.Collections.Generic;

    public class WorkerStatusModel
    {
        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public long Ticks { get; set; }

        public IDictionary<string, double> PerfomanceCounters { get; set; }
    }
}