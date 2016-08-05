using System;
using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.AdminManager;

namespace EntityFX.Gdcame.Manager
{
    public class AdminManager : IAdminManager
    {
        private readonly GameSessions _gameSessions;
        private IOperationContextHelper _operationContextHelper;
        public AdminManager(IOperationContextHelper operationContextHelper, GameSessions gameSessions)
        {
            _gameSessions = gameSessions;
            _operationContextHelper = operationContextHelper;
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            return _gameSessions.Sessions.GroupBy(_ => _.Login).Select(sessionsOfUser => new UserSessionsInfo
            {
                UserName = sessionsOfUser.Key,
                UserSessions = sessionsOfUser.ToArray()
            }).ToArray();
        }

        public void CloseSessionByGuid(Guid guid)
        {
            _gameSessions.RemoveSession(guid);
        }

        public void CloseAllUserSessions(string username)
        {
            _gameSessions.Sessions.Where(_ => _.Login == username).AsParallel()
                .ForAll(_ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void CloseAllSessions()
        {
            var sessionId = _operationContextHelper.Instance.SessionId;
            _gameSessions.RemoveAllSessions();
        }

        public void CloseAllSessionsExcludeThis(Guid guid)
        {
            _gameSessions.Sessions.Where(_ => _.SessionIdentifier != guid).AsParallel()
                .ForAll(_ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void WipeUser(string username)
        {
            _gameSessions.GetGame(username).Reset();
        }

        public void ReloadGame(string username)
        {
            throw new NotImplementedException();
        }
    }
}
