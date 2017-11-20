using System.Diagnostics;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.Manager.Contract.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class PerformanceHelper : IPerformanceHelper
    {
        private readonly IRuntimeHelper _runtimeHelper;

        public float CpuUsage
        {
            get { return _runtimeHelper.GetCpuUsage(); }
        }

        public float MemoryUsage
        {
            get { return _runtimeHelper.GetAvailablememoryInMb(); }
        }

        public float MemoryUsageByProcess
        {
            get { return _runtimeHelper.GetMemoryUsageInMb(); }
        }

        public PerformanceHelper(IRuntimeHelper runtimeHelper)
        {
            _runtimeHelper = runtimeHelper;
        }


    }
}
