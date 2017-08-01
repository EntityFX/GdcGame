using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using ServerStatisticsInfoModel = EntityFX.Gdcame.Common.Application.Model.ServerStatisticsInfoModel;

namespace EntityFX.Gdcame.Application.Api.MainServer.Mappers
{
    using EntityFX.Gdcame.Application.Api.Common.Mappers;
    using EntityFX.Gdcame.Application.Contract.Model.MainServer;

    public class StatisticsInfoMapper : StatisticsInfoMapper<MainServerStatisticsInfo, MainServerStatisticsInfoModel>
    {
        public override MainServerStatisticsInfoModel Map(MainServerStatisticsInfo source, MainServerStatisticsInfoModel destination = null)
        {
            var model = base.Map(source, destination);
            model.RegistredUsersCount = source.RegistredUsersCount;
            model.ActiveGamesCount = source.ActiveGamesCount;
            model.PerformanceInfo = new PerformanceInfoModel()
                                        {
                                            CalculationsPerCycle =
                                                source.PerformanceInfo.CalculationsPerCycle,
                                            PersistencePerCycle =
                                                source.PerformanceInfo.PersistencePerCycle
                                        };
            return model;
        }
    }
}