﻿using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.ServiceModel;
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
                var result = _gameSessions.AddSession(user);
                _logger.Info("EntityFX.EconomicsArcade.Manager.SessionManager.AddSession: Session {0} added for login: {1}", result, login);
                return result;
            }
            var message = string.Format("User with login {0} not found", login);
            _logger.Warning(message);
            throw new FaultException(new FaultReason(message), new FaultCode("AddSession"), "AddSession");
        }

        public bool CloseSession()
        {
            var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
            var session = _gameSessions.GetSession(sessionId);
            if (session == null)
            {
                return false;
            }
            var result =_gameSessions.RemoveSession(sessionId);
            _logger.Info("EntityFX.EconomicsArcade.Manager.SessionManager.CloseSession: Session {0} closed", sessionId);
            return result;
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
