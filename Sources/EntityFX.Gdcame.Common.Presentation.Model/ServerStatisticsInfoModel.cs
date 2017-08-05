using System;

namespace EntityFX.Gdcame.Common.Application.Model
{
    public class ServerStatisticsInfoModel
    {
        public int ActiveSessionsCount { get; set; }

        public TimeSpan ServerUptime { get; set; }

        public DateTime ServerStartDateTime { get; set; }


        public ResourcesUsageInfoModel ResourcesUsageInfo { get; set; }

        public SystemInfoModel SystemInfo { get; set; }

        public WorkerStatusModel[] ActiveWorkers { get; set; }
    }
}