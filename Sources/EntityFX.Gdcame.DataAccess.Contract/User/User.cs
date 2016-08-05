using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.User
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
    }
}
