namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    using EntityFX.Gdcame.Common.Application.Model;

    public class MainServerStatisticsInfoModel  : ServerStatisticsInfoModel
    {

        public PerformanceInfoModel PerformanceInfo { get; set; }

        public int ActiveGamesCount { get; set; }

        public int RegistredUsersCount { get; set; }
    }
}