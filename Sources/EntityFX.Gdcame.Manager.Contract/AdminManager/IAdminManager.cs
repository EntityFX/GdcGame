using System;

using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    //
    public interface IAdminManager : IAdminManager<MainServerStatisticsInfo>
    {
        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.GenericUser, UserRole.Admin})]
        UserSessionsInfo[] GetActiveSessions();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseSessionByGuid(Guid guid);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllUserSessions(string username);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessions();

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void CloseAllSessionsExcludeThis(Guid guid);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void WipeUser(string username);

        //
        //[FaultContract(typeof (InvalidSessionFault))]
        //[FaultContract(typeof (InsufficientPermissionsFault))]
        [CustomPrincipalPermission(AllowedRoles = new[] {UserRole.Admin})]
        void ReloadGame(string username);

        //
        void StopGame(string login);
        //
        void StopAllGames();
        
    }
}