using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
    public class TopStatisticsCounter
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }

}