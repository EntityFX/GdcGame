using System;

namespace EntityFX.Gdcame.Manager.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.Common;

    public class CustomPrincipalPermissionAttribute : Attribute
    {
        public UserRole[] AllowedRoles { get; set; }
    }
}