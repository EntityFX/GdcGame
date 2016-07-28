using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.UserManager
{
    [DataContract]
    public class UserData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
    }
}