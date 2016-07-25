using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Contract.Manager.AdminManager
{
    [ServiceContract]
    public interface IAdminManager
    {
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.GenericUser, UserRole.Admin })]
        UserSessionsInfo[] GetActiveSessions();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new []{ UserRole.Admin } )]
        void CloseSessionByGuid(Guid guid);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void CloseAllUserSessions(string username);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void CloseAllSessions();
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void CloseAllSessionsExcludeThis(Guid guid);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void WipeUser(string username);
        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void ReloadGame(string username);
    }
}
