namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Runtime.Serialization;

    public class TopRatingStatistics
    {
        [DataMember]
        public TopStatisticsAggregate ManualStepsCount { get; set; }
        [DataMember]
        public TopStatisticsAggregate TotalEarned { get; set; }
        [DataMember]
        public TopStatisticsAggregate RootCounter { get; set; }
    }
}