using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Diagnostics;

namespace EntityFX.EconomicsArcade.Manager
{
    public class SessionManager : ISessionManager
    {
        private readonly GameSessions _gameSessions;

        public SessionManager(GameSessions gameSessions)
        {
            _gameSessions = gameSessions;
        }


        public Guid AddSession(string login)
        {
            Debug.WriteLine("Login {0}", login);
            return _gameSessions.AddSession(login);
        }

        public Session GetSession()
        {
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            var session = _gameSessions.GetSession(sessionId);
            if (session == null)
            {
                throw new Exception("No user session");
            }
            return session;
        }
    }
}
