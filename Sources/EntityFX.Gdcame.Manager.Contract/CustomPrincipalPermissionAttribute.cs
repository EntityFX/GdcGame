using System;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer
{
    public class CustomPrincipalPermissionAttribute : Attribute
    {
        public UserRole[] AllowedRoles { get; set; }
    }
}