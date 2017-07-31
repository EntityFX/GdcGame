using System.ServiceModel;

namespace EntityFX.Gdcame.Manager.Contract.Common.Statistics
{
    public interface IServerStatistics<out TStatistics>
    where TStatistics: StatisticsInfo
    {
        [OperationContract]
        TStatistics GetStatisticsInfo();
    }
}