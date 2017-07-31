namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Runtime.Serialization;

    [DataContract]
    public class RatingStatistics
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public CountValues ManualStepsCount  { get; set; }

        [DataMember]
        public CountValues TotalEarned { get; set; }

        [DataMember]
        public CountValues RootCounter { get; set; }
    }
}