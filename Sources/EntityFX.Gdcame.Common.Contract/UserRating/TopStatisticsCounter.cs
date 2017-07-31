namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Runtime.Serialization;

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