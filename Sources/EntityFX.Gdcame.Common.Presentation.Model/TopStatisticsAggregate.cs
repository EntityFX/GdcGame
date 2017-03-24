using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    public class TopStatisticsAggregateModel
    {
        [DataMember]
        public TopStatisticsCounterModel[] Day { get; set; }
        [DataMember]
        public TopStatisticsCounterModel[] Week { get; set; }
        [DataMember]
        public TopStatisticsCounterModel[] Total { get; set; }
    }
}