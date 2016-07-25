using System;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager
{
    public class CustomPrincipalPermissionAttribute : Attribute
    {
        public UserRole[] AllowedRoles { get; set; }
    }
}
