namespace EntityFX.Gdcame.Contract.Common.Statistics
{
    

    public interface IServerStatistics<out TStatistics>
    where TStatistics: StatisticsInfo
    {
        
        TStatistics GetStatisticsInfo();
    }
}