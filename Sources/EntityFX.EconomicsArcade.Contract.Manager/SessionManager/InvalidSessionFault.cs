using System;
using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    [DataContract]
    public class InvalidSessionFault
    {
        [DataMember]
        public Guid SessionGuid { get; set; }
    }
}