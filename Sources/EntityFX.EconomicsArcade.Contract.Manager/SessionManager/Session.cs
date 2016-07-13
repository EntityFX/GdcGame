using System;
using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    [DataContract]
    public class Session
    {
        [DataMember]
        public Guid SessionIdentifier { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public int UserId { get; set; }

        public DateTime LastActivity { get; set; }
    }
}
