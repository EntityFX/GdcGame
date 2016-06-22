using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Diagnostics;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

namespace EntityFX.EconomicsArcade.Manager
{
    public class SessionManager : ISessionManager
    {
        private readonly GameSessions _gameSessions;
        private readonly IUserDataAccessService _userDataAccessService;

        public SessionManager(GameSessions gameSessions, IUserDataAccessService userDataAccessService)
        {
            _gameSessions = gameSessions;
            _userDataAccessService = userDataAccessService;
        }


        public Guid AddSession(string login)
        {
            Debug.WriteLine("Login {0}", login);
            var user = _userDataAccessService.FindByName(login);
            if (user != null) return _gameSessions.AddSession(user);
            _userDataAccessService.Create(new User() {Email = login});
            user = _userDataAccessService.FindByName(login);
            return _gameSessions.AddSession(user);
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
