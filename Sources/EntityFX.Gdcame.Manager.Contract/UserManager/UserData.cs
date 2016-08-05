using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.UserManager
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