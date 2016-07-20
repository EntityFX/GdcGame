using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;

namespace EntityFX.EconomicsArcade.Manager
{
    public class AdminManager : IAdminManager
    {
        private readonly GameSessions _gameSessions;

        public AdminManager(GameSessions gameSessions)
        {
            _gameSessions = gameSessions;
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            return _gameSessions.Sessions.GroupBy(_ => _.Login).Select(sessionsOfUser => new UserSessionsInfo
            {
                UserName = sessionsOfUser.Key, UserSessions = sessionsOfUser.ToArray()
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
    }
}
