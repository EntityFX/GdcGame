using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using System.Threading;
using EntityFX.Gdcame.Manager.Contract.AdminManager;

namespace EntityFX.Gdcame.Manager
{
    public class GameSessions : IDisposable
    {
        private readonly IGameFactory _gameFactory;

        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<string, IGame> _userGamesStorage =
            new ConcurrentDictionary<string, IGame>();
        private readonly ConcurrentDictionary<Guid, Session> _userSessionsStorage = new ConcurrentDictionary<Guid, Session>();

        private readonly PerformanceInfo _performanceInfo;

        public GameSessions(ILogger logger, IGameFactory gameFactory, PerformanceInfo performanceInfo)
        {
            _logger = logger;
            _gameFactory = gameFactory;
            _performanceInfo = performanceInfo;
        }

        internal IDictionary<Guid, Session> Sessions
        {
            get { return _userSessionsStorage; }
        }

        internal IDictionary<string, IGame> Games
        {
            get { return _userGamesStorage; }
        }



        private static readonly object _stdLock = new { };

        private IGame StartGame(string userId, string login)
        {
            var game = BuildGame(userId, login);
            game.Initialize();
            if (GameStarted != null)
            {
                GameStarted(this, new Tuple<string, string>(login, userId));
            }
            return game;
        }

        public IGame GetGame(Guid sessionId)
        {
            Session session;

            session = GetSession(sessionId);
            return GetGame(session.Login);
        }

        public IGame GetGame(string username)
        {
            IGame game;
            if (!_userGamesStorage.TryGetValue(username, out game))
            {
                _logger.Warning("Cannot get game for user {0}", username);
            }
            return game;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (Sessions.Values.Count(_ => _.Login == username) > 0)
            {
                return UserGameSessionStatus.Online;
            }
            if (!_userGamesStorage.ContainsKey(username))
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

            if (!_userGamesStorage.ContainsKey(user.Login))
            {
                var game = StartGame(user.Id, user.Login);
                if (!_userGamesStorage.TryAdd(user.Login, game))
                {
                    _logger.Warning("Cannot add game for user {0}", user.Login);
                }
            }

            if (!_userSessionsStorage.TryAdd(session.SessionIdentifier, session))
            {
                _logger.Warning("Cannot add session {0}", session.SessionIdentifier);
            }
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            Session session;
            if (!_userSessionsStorage.TryGetValue(sessionId, out session))
            {
                if (session == null)
                {
                    _logger.Warning("Session {0} not found", sessionId);
                    throw new InvalidSessionException(string.Format("Session {0} doesn't exists", sessionId), sessionId);
                }
            }
            session.LastActivity = DateTime.Now;
            return session;
        }

        public void RemoveSession(Guid sessionId)
        {
            Session session;
            if (!_userSessionsStorage.TryRemove(sessionId, out session))
            {
                _logger.Warning("Cannot remove session {0}", sessionId);
            }
        }

        public void RemoveGame(UserData user)
        {

            var userSessions = Sessions.Values.Where(_ => _.Login == user.Login);
            Session session;
            foreach (var userSession in userSessions)
            {
                if (!_userSessionsStorage.TryRemove(userSession.SessionIdentifier, out session))
                {
                    _logger.Warning("Cannot remove session {0}", userSession.SessionIdentifier);
                }
            }

            IGame game;
            if (!_userGamesStorage.TryRemove(user.Login, out game))
            {
                _logger.Warning("Cannot remove game for user {0}", user.Login);
            }
            if (GameStarted != null)
            {
                GameRemoved(this, new Tuple<string, string>(user.Login, user.Id));
            }
        }

        public void RemoveAllGames()
        {
            RemoveAllSessions();
            _userGamesStorage.Clear();
            if (AllGamesRemoved != null)
            {
                AllGamesRemoved(this, null);
            }
        }

        public void RemoveAllSessions()
        {
            _userSessionsStorage.Clear();
        }

        public PerformanceInfo PerformanceInfo
        {
            get { return _performanceInfo; }
        }

        public event EventHandler<Tuple<string, string>> GameStarted;

        public event EventHandler<Tuple<string, string>> GameRemoved;

        public event EventHandler AllGamesRemoved;

        private IGame BuildGame(string userId, string userName)
        {
            return _gameFactory.BuildGame(userId, userName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}