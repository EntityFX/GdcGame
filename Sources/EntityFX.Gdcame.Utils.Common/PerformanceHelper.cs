using System.Diagnostics;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class PerformanceHelper : IPerformanceHelper
    {
        private readonly PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        public float CpuUsage
        {
            get { return RuntimeHelper.GetCpuUsage(); }
        }

        public float MemoryUsage
        {
            get { return RuntimeHelper.GetAvailablememoryInMb(); }
        }

        public float MemoryUsageByProcess
        {
            get { return (float)Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024; }
        }


    }
}
