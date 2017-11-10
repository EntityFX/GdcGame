namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    

    public class TopStatisticsAggregate
    {
        
        public TopStatisticsCounter[] Day { get; set; }
        
        public TopStatisticsCounter[] Week { get; set; }
        
        public TopStatisticsCounter[] Total { get; set; }
    }
}