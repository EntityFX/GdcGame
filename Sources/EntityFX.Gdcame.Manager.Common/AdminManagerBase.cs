namespace EntityFX.Gdcame.Manager.Common
{
    using System;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Manager.Contract.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;

    public class AdminManagerBase<TStatistics> : IAdminManager<TStatistics>
        where TStatistics : StatisticsInfo, new()
    {
        private static readonly DateTime ServerStartTime = DateTime.Now;

        private readonly ISessions _gameSessions;

        private readonly IPerformanceHelper _performanceHelper;

        private readonly SystemInfo _systemInfo;

        public AdminManagerBase(ISessions gameSessions, IPerformanceHelper performanceHelper, SystemInfo systemInfo)
        {
            this._gameSessions = gameSessions;
            this._performanceHelper = performanceHelper;
            this._systemInfo = systemInfo;
        }

        public virtual TStatistics GetStatisticsInfo()
        {
            return new TStatistics()
            {
                ActiveSessionsCount = _gameSessions.Sessions.Count,
                ServerStartDateTime = ServerStartTime,
                ServerUptime = DateTime.Now - ServerStartTime,
                ResourcesUsageInfo = new ResourcesUsageInfo()
                {
                    CpuUsed = _performanceHelper.CpuUsage,
                    MemoryAvailable = _performanceHelper.MemoryUsage,
                    MemoryUsedByProcess = _performanceHelper.MemoryUsageByProcess
                },
                SystemInfo = _systemInfo
            };
        }
    }
}