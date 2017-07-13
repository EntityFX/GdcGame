using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using ServerStatisticsInfoModel = EntityFX.Gdcame.Application.Contract.Model.MainServer.ServerStatisticsInfoModel;

namespace EntityFX.Gdcame.Application.Api.MainServer.Mappers
{
    public class StatisticsInfoMapper : IMapper<StatisticsInfo, ServerStatisticsInfoModel>
    {
        public ServerStatisticsInfoModel Map(StatisticsInfo source, ServerStatisticsInfoModel destination = null)
        {
            return new ServerStatisticsInfoModel
            {
                ActiveGamesCount = source.ActiveGamesCount,
                ActiveSessionsCount = source.ActiveSessionsCount,
                RegistredUsersCount = source.RegistredUsersCount,
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