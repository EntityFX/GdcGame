namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    

    
    public class RatingStatistics
    {
        
        public string UserId { get; set; }

        
        public CountValues ManualStepsCount  { get; set; }

        
        public CountValues TotalEarned { get; set; }

        
        public CountValues RootCounter { get; set; }
    }
}