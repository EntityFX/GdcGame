using System;
using System.ServiceModel;
using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    //[ServiceContract]
    public interface IAdminManager : IAdminManager<MainServerStatisticsInfo>
    {
        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.GenericUser, UserRole.Admin})]
        UserSessionsInfo[] GetActiveSessions();

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseSessionByGuid(Guid guid);

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllUserSessions(string username);

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessions();

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessionsExcludeThis(Guid guid);

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void WipeUser(string username);

        //[OperationContract]
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void ReloadGame(string username);

        //[OperationContract]
        void StopGame(string login);
        //[OperationContract]
        void StopAllGames();
        
    }
}