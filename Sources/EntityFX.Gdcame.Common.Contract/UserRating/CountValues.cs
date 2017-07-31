namespace EntityFX.Gdcame.Contract.Common.UserRating
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CountValues
    {
        [DataMember]
        public decimal Day { get; set; }
        [DataMember]
        public decimal Week { get; set; }
        [DataMember]
        public decimal Total { get; set; }
    }
}
