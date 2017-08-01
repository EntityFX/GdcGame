using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Application.Api.Common.Mappers
{
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class StatisticsInfoMapper<TStatisticsDomain, TStatisticsInfoModel> : IMapper<TStatisticsDomain, TStatisticsInfoModel>
        where TStatisticsDomain : StatisticsInfo
        where TStatisticsInfoModel : ServerStatisticsInfoModel, new()
    {
        public virtual TStatisticsInfoModel Map(TStatisticsDomain source, TStatisticsInfoModel destination = default(TStatisticsInfoModel))
        {
            return new TStatisticsInfoModel
            {
                ActiveSessionsCount = source.ActiveSessionsCount,
                ServerStartDateTime = source.ServerStartDateTime,
                ServerUptime = source.ServerUptime,
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
