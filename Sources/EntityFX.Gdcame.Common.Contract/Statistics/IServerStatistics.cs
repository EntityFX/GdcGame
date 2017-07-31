namespace EntityFX.Gdcame.Contract.Common.Statistics
{
    using System.ServiceModel;

    public interface IServerStatistics<out TStatistics>
    where TStatistics: StatisticsInfo
    {
        [OperationContract]
        TStatistics GetStatisticsInfo();
    }
}