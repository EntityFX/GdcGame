//

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;

    public class MainServerStatisticsInfo : StatisticsInfo
    {
        //
        public int ActiveGamesCount { get; set; }
        //
        public int RegistredUsersCount { get; set; }

        //
        public GamePerformanceInfo PerformanceInfo { get; set; }
    }
}