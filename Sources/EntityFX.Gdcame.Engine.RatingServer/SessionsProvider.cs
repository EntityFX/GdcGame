namespace EntityFX.Gdcame.Engine.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class SessionsProvider : IDisposable, ISessions
    {

        private readonly ConcurrentDictionary<Guid, Session> _userSessionsStorage = new ConcurrentDictionary<Guid, Session>();
        private readonly ConcurrentDictionary<string, string> _userIdentities =
            new ConcurrentDictionary<string, string>();

        private readonly ILogger _logger;

        public SessionsProvider(ILogger logger)
        {
            this._logger = logger;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }
                this.disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion

        public IDictionary<string, string> Identities
        {
            get { return this._userIdentities; }
        }

        public virtual Guid AddSession(User user)
        {
            UserRole[] userRoles;
            switch ((UserRole)user.Role)
            {
                case UserRole.Admin:
                    userRoles = new[] { UserRole.GenericUser, UserRole.Admin };
                    break;
                case UserRole.GenericUser:
                default:
                    userRoles = new[] { UserRole.GenericUser };
                    break;
                case UserRole.System:
                    userRoles = new[] { UserRole.System, UserRole.Admin };
                    break;
            }

            var session = new Session
                              {
                                  Login = user.Login,
                                  UserId = user.Id,
                                  SessionIdentifier = Guid.NewGuid(),
                                  LastActivity = DateTime.Now,


                                  UserRoles = userRoles,
                                  Identity =
                                      new CustomGameIdentity { AuthenticationType = "Auto", IsAuthenticated = true, Name = user.Login }
                              };

            if (!this._userSessionsStorage.TryAdd(session.SessionIdentifier, session))
            {
                this._logger.Warning("Cannot add session {0}", session.SessionIdentifier);
            }

            if (!this._userIdentities.TryAdd(session.Login, session.UserId))
            {
                this._logger.Warning("Cannot add user identifier {0}", session.SessionIdentifier);
            }
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            Session session;
            if (!this._userSessionsStorage.TryGetValue(sessionId, out session))
            {
                if (session == null)
                {
                    this._logger.Warning("Session {0} not found", sessionId);
                    throw new InvalidSessionException(string.Format("Session {0} doesn't exists", sessionId), sessionId);
                }
            }
            session.LastActivity = DateTime.Now;
            return session;
        }

        public void RemoveAllSessions()
        {
            this._userSessionsStorage.Clear();
            this._userIdentities.Clear();
        }

        public void RemoveSession(Guid sessionId)
        {
            Session session;
            if (!this._userSessionsStorage.TryRemove(sessionId, out session))
            {
                this._logger.Warning("Cannot remove session {0}", sessionId);
            }
            string userId;
            if (!this._userIdentities.TryRemove(session.UserId, out userId))
            {
                this._logger.Warning("Cannot remove user identifier {0}", session.UserId);
            }
        }

        public IDictionary<Guid, Session> Sessions
        {
            get { return this._userSessionsStorage; }
        }
    }
}