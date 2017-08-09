namespace EntityFX.Gdcame.Manager.Common
{
    using System;
    using System.ServiceModel;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Engine.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

    using User = EntityFX.Gdcame.DataAccess.Contract.Common.User.User;

    public class SessionManager : ISessionManager
    {
        protected readonly ISessions GameSessions;
        private readonly ILogger _logger;
        private readonly IOperationContextHelper _operationContextHelper;
        private readonly IUserDataAccessService _userDataAccessService;

        public SessionManager(ILogger logger, IOperationContextHelper operationContextHelper, ISessions gameSessions,
            IUserDataAccessService userDataAccessService)
        {
            this._logger = logger;
            this._operationContextHelper = operationContextHelper;
            this.GameSessions = gameSessions;
            this._userDataAccessService = userDataAccessService;
        }


        public Guid OpenSession(string login)
        {
            User user;
            try
            {
                user = this._userDataAccessService.FindByName(login);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex);
                throw;
            }

            if (user != null)
            {
                var result = this.GameSessions.AddSession(new Engine.Contract.Common.User() { Id = user.Id, Login = user.Login, Role = user.Role});
                this._logger.Info(
                    "{2}: Session {0} added for login: {1}",
                    result, login, this.GetType());
                return result;
            }
            var message = string.Format("User with login {0} not found", login);
            this._logger.Warning(message);
            throw new FaultException(new FaultReason(message), new FaultCode("OpenSession"), "OpenSession");
        }

        public bool CloseSession()
        {
            var sessionId = this._operationContextHelper.Instance.SessionId ?? default(Guid);
            var session = this.GameSessions.GetSession(sessionId);
            if (session == null)
            {
                return false;
            }
            this.GameSessions.RemoveSession(sessionId);
            this._logger.Info("EntityFX.EconomicsArcade.Manager.SessionManager.CloseSession: Session {0} closed", sessionId);
            return true;
        }

        public Session GetSession()
        {
            var sessionId = this._operationContextHelper.Instance.SessionId ?? default(Guid);
            var session = this.GameSessions.GetSession(sessionId);
            if (session == null)
            {
                throw new InvalidOperationException("No user session");
            }
            return session;
        }
    }
}