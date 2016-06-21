using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.User
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}
