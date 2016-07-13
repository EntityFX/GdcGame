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
            var sessionsOfUsersTemp = new Dictionary<string, List<Session>>();
            foreach (var session in _gameSessions.Sessions)
            {
                if (!sessionsOfUsersTemp.ContainsKey(session.Value.Login))
                    sessionsOfUsersTemp.Add(session.Value.Login, new List<Session>());

                sessionsOfUsersTemp[session.Value.Login].Add(session.Value);
            }


            return sessionsOfUsersTemp.Select(sessionsOfUser => new UserSessionsInfo
            {
                UserName = sessionsOfUser.Key, UserSessions = sessionsOfUser.Value.ToArray()
            }).ToArray();
        }

        public void CloseSessionByGuid(Guid guid)
        {
            _gameSessions.RemoveSession(guid);
        }

        public void CloseAllUserSessions(string username)
        {
            var userSessions = _gameSessions.Sessions.Where(_ => _.Value.Login == username);
            foreach (var userSession in userSessions)
            {
                _gameSessions.RemoveSession(userSession.Key);
            }
        }

        public void WipeUser(string username)
        {
            _gameSessions.GetGame(username).Reset();
        }
    }
}
