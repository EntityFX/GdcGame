using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.NotifyConsumerService
{
    [DataContract]
    public class UserContext
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}