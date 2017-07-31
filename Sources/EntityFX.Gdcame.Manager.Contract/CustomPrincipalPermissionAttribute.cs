using System;

namespace EntityFX.Gdcame.Manager.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    public class CustomPrincipalPermissionAttribute : Attribute
    {
        public UserRole[] AllowedRoles { get; set; }
    }
}