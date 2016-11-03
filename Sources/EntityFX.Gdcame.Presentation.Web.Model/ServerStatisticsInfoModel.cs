using System;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class ServerStatisticsInfoModel
    {
        public int ActiveSessionsCount { get; set; }

        public int ActiveGamesCount { get; set; }

        public int RegistredUsersCount { get; set; }

        public TimeSpan ServerUptime { get; set; }

        public DateTime ServerStartDateTime { get; set; }

        public PerformanceInfoModel PerformanceInfo { get; set; }

        public ResourcesUsageInfoModel ResourcesUsageInfo { get; set; }

        public SystemInfoModel SystemInfo { get; set; }

        public string[] ActiveWorkers { get; set; }
    }
}