using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    public class TopRatingStatisticsModel
    {
        [DataMember]
        public TopStatisticsAggregateModel ManualStepsCount { get; set; }
        [DataMember]
        public TopStatisticsAggregateModel TotalEarned { get; set; }
        [DataMember]
        public TopStatisticsAggregateModel RootCounter { get; set; }
    }
}