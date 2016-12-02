using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
    public class TopStatisticsAggregate
    {
        [DataMember]
        public TopStatisticsCounter[] Day { get; set; }
        [DataMember]
        public TopStatisticsCounter[] Week { get; set; }
        [DataMember]
        public TopStatisticsCounter[] Total { get; set; }
    }
}