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
        private readonly ILogger _logger;

        public SessionManager(ILogger logger, GameSessions gameSessions, IUserDataAccessService userDataAccessService)
        {
            _logger = logger;

            _gameSessions = gameSessions;
            _userDataAccessService = userDataAccessService;
           
        }


        public Guid AddSession(string login)
        {
            Debug.WriteLine("Login {0}", login);

            User user;
            try
            {
                user = _userDataAccessService.FindByName(login);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            if (user != null) return _gameSessions.AddSession(user);
            _userDataAccessService.Create(new User() {Email = login});
            user = _userDataAccessService.FindByName(login);
            _logger.Info("EntityFX.EconomicsArcade.Manager.SessionManager.AddSession: Session added for login: {0}", login);
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
