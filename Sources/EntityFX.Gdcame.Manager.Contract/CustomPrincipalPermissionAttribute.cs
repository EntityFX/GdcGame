using System;
using EntityFX.Gdcame.Manager.Contract.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract
{
    public class CustomPrincipalPermissionAttribute : Attribute
    {
        public UserRole[] AllowedRoles { get; set; }
    }
}
