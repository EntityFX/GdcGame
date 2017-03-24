using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Application.Model
{
    public class TopStatisticsCounterModel
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }

}