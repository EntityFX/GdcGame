using EntityFX.Gdcame.Manager.Contract.Common.Statistics;

namespace EntityFX.Gdcame.Manager.Contract.Common.AdminManager
{
    public interface IAdminManager<out TStatistics> : IServerStatistics<TStatistics>
        where TStatistics: StatisticsInfo
    {
        
    }
}