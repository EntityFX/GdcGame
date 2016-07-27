using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Common.UserRating
{
    [DataContract]
    public class UserRating
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public decimal TotalFunds { get; set; }
        [DataMember]
        public decimal GdcPoints { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public UserGameSessionStatus Status { get; set; }
    }
}
