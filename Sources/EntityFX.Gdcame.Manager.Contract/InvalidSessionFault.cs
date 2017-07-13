using System;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer
{
    [DataContract]
    public class InvalidSessionFault
    {
        [DataMember]
        public Guid SessionGuid { get; set; }
    }
}