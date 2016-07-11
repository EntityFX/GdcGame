using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
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

            if (user != null)
            {
                _logger.Info("EntityFX.EconomicsArcade.Manager.SessionManager.AddSession: Session added for login: {0}", login);
                return _gameSessions.AddSession(user);
            }
            var message = string.Format("User with login {0} not found", login);
            _logger.Warning(message);
            throw new FaultException(new FaultReason(message), new FaultCode("AddSession"), "AddSession");
        }

        public Session GetSession()
        {
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            var session = _gameSessions.GetSession(sessionId);
            if (session == null)
            {
                throw new InvalidOperationException("No user session");
            }
            return session;
        }
    }
}
