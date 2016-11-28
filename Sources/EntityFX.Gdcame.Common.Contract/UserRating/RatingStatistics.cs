using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
    [DataContract]
    public class RatingStatistics
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public CountValues MunualStepsCount  { get; set; }

        [DataMember]
        public CountValues TotalEarned { get; set; }

        [DataMember]
        public CountValues RootCounter { get; set; }
    }
}