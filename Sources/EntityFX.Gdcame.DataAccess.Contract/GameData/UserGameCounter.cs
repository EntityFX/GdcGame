using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class UserGameCounter
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public decimal TotalFunds { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public int AutomaticStepsCount { get; set; }
        [DataMember]
        public decimal CurrentFunds { get; set; }
        [DataMember]
        public decimal DelayedFunds { get; set; }
    }
}