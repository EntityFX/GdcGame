namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System;
    

    
    public class RatingHistory
    {
        
        public string UserId { get; set; }

        
        public DateTime Data { get; set; }

        
        public int ManualStepsCount { get; set; }

        
        public decimal TotalEarned { get; set; }

        
        public int RootCounter { get; set; }
    }
}
