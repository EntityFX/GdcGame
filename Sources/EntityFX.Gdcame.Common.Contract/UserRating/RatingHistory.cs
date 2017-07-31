namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class RatingHistory
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public DateTime Data { get; set; }

        [DataMember]
        public int ManualStepsCount { get; set; }

        [DataMember]
        public decimal TotalEarned { get; set; }

        [DataMember]
        public int RootCounter { get; set; }
    }
}
