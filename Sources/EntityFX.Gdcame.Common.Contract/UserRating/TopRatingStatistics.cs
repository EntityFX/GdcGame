using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
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