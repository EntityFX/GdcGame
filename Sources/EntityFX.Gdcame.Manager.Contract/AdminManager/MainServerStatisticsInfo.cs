using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;

    public class MainServerStatisticsInfo : StatisticsInfo
    {
        [DataMember]
        public int ActiveGamesCount { get; set; }
        [DataMember]
        public int RegistredUsersCount { get; set; }

        [DataMember]
        public GamePerformanceInfo PerformanceInfo { get; set; }
    }
}