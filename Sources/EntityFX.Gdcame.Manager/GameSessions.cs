using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using Microsoft.Practices.ObjectBuilder2;

namespace EntityFX.Gdcame.Manager
{
    public class GameSessions
    {
        private readonly ILogger _logger;
        private readonly IGameFactory _gameFactory;
        private readonly ConcurrentDictionary<string, IGame> GameSessionsStorage = new ConcurrentDictionary<string, IGame>();

        private readonly ConcurrentDictionary<Guid, Session> SessionsStorage = new ConcurrentDictionary<Guid, Session>();

        private readonly Timer _gameTimer;

        private readonly Timer _persisTimertimer;

        private void GamePersistCallback(object state)
        {
            var sw = new Stopwatch();
            sw.Start();
            //GameSessionsStorage.Ta
            GameSessionsStorage.AsParallel().ForAll(_ => ((NetworkGame)_.Value).PerformGameDataChanged());
            //GameSessionsStorage.ForEach(_ => Task.Run(() => ((NetworkGame)_.Value).PerformGameDataChanged()));
            if (_logger != null)
                _logger.Info("Perform persist for {0} game sessions: {1}", GameSessionsStorage.ToList().Count, sw.Elapsed);
        }

        public GameSessions(ILogger logger, IGameFactory gameFactory)
        {
            _logger = logger;
            _gameFactory = gameFactory;
            _gameTimer = new Timer(TimerCallback, null, 0, 1000);

            _persisTimertimer = new Timer(GamePersistCallback, null, 0, 30000);
        }

        private void TimerCallback(object state)
        {
            var sw = new Stopwatch();
            sw.Start();
            GameSessionsStorage.AsParallel().ForAll(_ => _.Value.PerformAutoStep());
            if (_logger != null)
                _logger.Info("Perform steps for {0} game sessions: {1}", GameSessionsStorage.Count, sw.Elapsed);
        }

        public IGame GetGame(Guid sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null)
            {
                throw new InvalidSessionException(string.Format("Session {0} doesn't exists", sessionId), sessionId);
            }
            session.LastActivity = DateTime.Now;
            if (!GameSessionsStorage.ContainsKey(session.Login))
            {
                var game = BuildGame(session.UserId, session.Login);
                game.Initialize();
                GameSessionsStorage.TryAdd(session.Login, game);
            }
            return GameSessionsStorage[session.Login];
        }

        public IGame GetGame(string username)
        {
            return GameSessionsStorage.ContainsKey(username) ? GameSessionsStorage[username] : null;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (Sessions.Count(_ => _.Login == username) > 0)
            {
                return UserGameSessionStatus.Online;
            }
            if (!GameSessionsStorage.ContainsKey(username))
            {
                return UserGameSessionStatus.GameNotStarted;
            }
            return UserGameSessionStatus.Offline;
        }

        public Guid AddSession(User user)
        {
            var session = new Session()
            {
                Login = user.Email,
                UserId = user.Id,
                SessionIdentifier = Guid.NewGuid(),
                UserRoles = user.IsAdmin ? new[] { UserRole.GenericUser, UserRole.Admin } : new[] { UserRole.GenericUser },
                Identity = new CustomGameIdentity() { AuthenticationType = "Auto", IsAuthenticated = true, Name = user.Email }
            };
            SessionsStorage.TryAdd(session.SessionIdentifier, session);
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            return !SessionsStorage.ContainsKey(sessionId) ? null : SessionsStorage[sessionId];
        }

        public bool RemoveSession(Guid sessionId)
        {
            Session session;
            return SessionsStorage.TryRemove(sessionId, out session);
        }

        public void RemoveAllSessions()
        {
            SessionsStorage.Clear();
        }

        private IGame BuildGame(int userId, string userName)
        {
            return _gameFactory.BuildGame(userId, userName);
        }

        internal IEnumerable<Session> Sessions
        {
            get { return SessionsStorage.Values; }
        }
    }
}
