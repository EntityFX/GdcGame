﻿namespace EntityFX.Gdcame.Contract.Common
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Principal;

    [DataContract]
    public class Session
    {
        [DataMember]
        public Guid SessionIdentifier { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public UserRole[] UserRoles { get; set; }

        public DateTime LastActivity { get; set; }

        public IIdentity Identity { get; set; }
    }
}