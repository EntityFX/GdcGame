using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.UserRating;

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