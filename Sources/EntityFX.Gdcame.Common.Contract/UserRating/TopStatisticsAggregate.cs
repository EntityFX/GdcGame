namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Runtime.Serialization;

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