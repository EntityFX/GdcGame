using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    [DataContract]
    public class InsufficientPermissionsFault
    {
        [DataMember]
        public UserRole[] RequiredRoles { get; set; }

        [DataMember]
        public UserRole[] CurrentRoles { get; set; }
    }
}