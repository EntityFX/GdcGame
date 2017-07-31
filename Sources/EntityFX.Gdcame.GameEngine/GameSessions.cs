namespace EntityFX.Gdcame.Engine.GameEngine
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    public class GameSessions : IDisposable, IGameSessions
    {
        private readonly IGameFactory _gameFactory;

        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<string, IGame> _userGamesStorage =
            new ConcurrentDictionary<string, IGame>();
        private readonly ConcurrentDictionary<Guid, Session> _userSessionsStorage = new ConcurrentDictionary<Guid, Session>();
        private readonly ConcurrentDictionary<string, string> _userIdentities =
            new ConcurrentDictionary<string, string>();

        private readonly GamePerformanceInfo _performanceInfo;

        public GameSessions(ILogger logger, IGameFactory gameFactory, GamePerformanceInfo performanceInfo)
        {
            this._logger = logger;
            this._gameFactory = gameFactory;
            this._performanceInfo = performanceInfo;
        }

        public IDictionary<Guid, Session> Sessions
        {
            get { return this._userSessionsStorage; }
        }

        public IDictionary<string, IGame> Games
        {
            get { return this._userGamesStorage; }
        }

        public IDictionary<string, string> Identities
        {
            get { return this._userIdentities; }
        }

        private static readonly object _stdLock = new { };

        //TODO: make public
        public IGame StartGame(string userId, string login)
        {
            var game = this.BuildGame(userId, login);
            game.Initialize();
            if (this.GameStarted != null)
            {
                this.GameStarted(this, new Tuple<string, string>(login, userId));
            }
            return game;
        }
        //TODO: add FreezeUserGame(userId, login) that will set IsFreezed property of Game that will be checked by Game methods.
        public IGame GetGame(Guid sessionId)
        {
            Session session;

            session = this.GetSession(sessionId);
            return GetGame(session.Login);
        }

        public IGame GetGame(string username)
        {
            IGame game;
            if (!this._userGamesStorage.TryGetValue(username, out game))
            {
                this._logger.Warning("Cannot get game for user {0}", username);
            }
            return game;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (this.Sessions.Values.Count(_ => _.Login == username) > 0)
            {
                return UserGameSessionStatus.Online;
            }
            if (!this._userGamesStorage.ContainsKey(username))
            {
                return UserGameSessionStatus.GameNotStarted;
            }
            return UserGameSessionStatus.Offline;
        }

        public Guid AddSession(User user)
        {
            UserRole[] userRoles;
            switch ((UserRole)user.Role)
            {
                case UserRole.Admin:
                    userRoles = new[] { UserRole.GenericUser, UserRole.Admin };
                    break;
                case UserRole.GenericUser:
                default:
                    userRoles = new[] { UserRole.GenericUser};
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

            if (!this._userGamesStorage.ContainsKey(user.Login))
            {
                var game = this.StartGame(user.Id, user.Login);
                if (!this._userGamesStorage.TryAdd(user.Login, game))
                {
                    this._logger.Warning("Cannot add game for user {0}", user.Login);
                }
            }

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

        public void RemoveGame(UserData user)
        {

            var userSessions = this.Sessions.Values.Where(_ => _.Login == user.Login);
            foreach (var userSession in userSessions)
            {
                this.RemoveSession(userSession.SessionIdentifier);
            }

            IGame game;
            if (!this._userGamesStorage.TryRemove(user.Login, out game))
            {
                this._logger.Warning("Cannot remove game for user {0}", user.Login);
            }
            if (this.GameStarted != null)
            {
                this.GameRemoved(this, new Tuple<string, string>(user.Login, user.Id));
            }
        }

        public void RemoveAllGames()
        {
            this.RemoveAllSessions();
            this._userGamesStorage.Clear();
            if (this.AllGamesRemoved != null)
            {
                this.AllGamesRemoved(this, null);
            }
        }

        public void RemoveAllSessions()
        {
            this._userSessionsStorage.Clear();
            this._userIdentities.Clear();
        }

        public GamePerformanceInfo PerformanceInfo
        {
            get { return this._performanceInfo; }
        }

        public event EventHandler<Tuple<string, string>> GameStarted;

        public event EventHandler<Tuple<string, string>> GameRemoved;

        public event EventHandler AllGamesRemoved;

        private IGame BuildGame(string userId, string userName)
        {
            return this._gameFactory.BuildGame(userId, userName);
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
    }
}