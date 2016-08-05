using System.Runtime.Serialization;

namespace EntityFX.Gdcame.NotifyConsumer.Contract
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