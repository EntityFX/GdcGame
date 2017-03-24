using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.User
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public uint Role { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
    }
}