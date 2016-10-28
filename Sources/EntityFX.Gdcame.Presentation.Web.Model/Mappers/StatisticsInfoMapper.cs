using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Application.Contract.Model.Mappers
{
    public class StatisticsInfoMapper : IMapper<StatisticsInfo, ServerStatisticsInfoModel>
    {
        public ServerStatisticsInfoModel Map(StatisticsInfo source, ServerStatisticsInfoModel destination = null)
        {
            return new ServerStatisticsInfoModel
            {
                ActiveGamesCount = source.ActiveGamesCount,
                ActiveSessionsCount = source.ActiveSessionsCount,
                ServerStartDateTime = source.ServerStartDateTime,
                ServerUptime = source.ServerUptime,
                PerformanceInfo = new PerformanceInfoModel()
                {
                    CalculationsPerCycle = source.PerformanceInfo.CalculationsPerCycle,
                    PersistencePerCycle = source.PerformanceInfo.PersistencePerCycle
                },
                ResourcesUsageInfo = new ResourcesUsageInfoModel()
                {
                    CpuUsed = source.ResourcesUsageInfo.CpuUsed,
                    MemoryAvailable = source.ResourcesUsageInfo.MemoryAvailable,
                    MemoryUsedByProcess = source.ResourcesUsageInfo.MemoryUsedByProcess
                },
                SystemInfo = new SystemInfoModel()
                {
                    CpusCount = source.SystemInfo.CpusCount,
                    Os = source.SystemInfo.Os,
                    Runtime = source.SystemInfo.Runtime,
                    MemoryTotal = source.SystemInfo.MemoryTotal,
                }
            };
        }
    }
}