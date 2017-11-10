namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    

    public class TopRatingStatistics
    {
        
        public TopStatisticsAggregate ManualStepsCount { get; set; }
        
        public TopStatisticsAggregate TotalEarned { get; set; }
        
        public TopStatisticsAggregate RootCounter { get; set; }
    }
}