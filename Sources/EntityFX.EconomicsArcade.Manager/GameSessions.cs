using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager;


namespace EntityFX.EconomicsArcade.Manager
{
    public class GameSessions
    {
        private readonly IGameFactory _gameFactory;
        private static readonly IDictionary<string, IGame> GameSessionsStorage = new ConcurrentDictionary<string, IGame>();

        private static readonly IDictionary<Guid, Session> SessionsStorage = new ConcurrentDictionary<Guid, Session>();

        private static Timer _timer = new Timer(TimerCallback, null, 0, 1000);

        public GameSessions(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private static void TimerCallback(object state)
        {
            foreach (var gameSession in GameSessionsStorage)
            {
                gameSession.Value.PerformAutoStep();
            }
        }

        public IGame GetGame(Guid sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null)
            {
                throw new InvalidSessionException(string.Format("Session {0} doesn't exists",sessionId), sessionId);
            }

            if (!GameSessionsStorage.ContainsKey(session.Login))
            {
                var game = BuildGame(session.UserId, session.Login);
                game.Initialize();
                GameSessionsStorage.Add(session.Login, game);
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
                SessionIdentifier = Guid.NewGuid()
            };
            SessionsStorage.Add(session.SessionIdentifier, session);
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            return !SessionsStorage.ContainsKey(sessionId) ? null : SessionsStorage[sessionId];
        }

        public bool RemoveSession(Guid sessionId)
        {
            return SessionsStorage.Remove(sessionId);
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
