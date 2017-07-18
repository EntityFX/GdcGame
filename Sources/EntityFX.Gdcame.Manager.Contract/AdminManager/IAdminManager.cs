using System;
using System.ServiceModel;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    [ServiceContract]
    public interface IAdminManager
    {
        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.GenericUser, UserRole.Admin})]
        UserSessionsInfo[] GetActiveSessions();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseSessionByGuid(Guid guid);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllUserSessions(string username);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessions();

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessionsExcludeThis(Guid guid);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void WipeUser(string username);

        [OperationContract]
        [FaultContract(typeof (InvalidSessionFault))]
        [FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void ReloadGame(string username);

        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void AddServer(string address);

        [OperationContract]
        [FaultContract(typeof(InvalidSessionFault))]
        [FaultContract(typeof(InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] { UserRole.Admin })]
        void RemoveServer(string address);

        [OperationContract]
        StatisticsInfo GetStatisticsInfo();
        [OperationContract]
        void StopGame(string login);
        [OperationContract]
        void StopAllGames();

        [OperationContract]
        bool UpdateNodeData();
    }
}