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

namespace EntityFX.Gdcame.Manager
{
    public class GameSessions
    {
        private readonly IGameFactory _gameFactory;

        private readonly TaskTimer _gameTimer;
        private readonly ILogger _logger;
        private readonly IHashHelper _hashHelper;

        private readonly TaskTimer _persisTimertimer;

        private readonly ConcurrentDictionary<string, IGame> UserGamesStorage =
            new ConcurrentDictionary<string, IGame>();

        private readonly ConcurrentDictionary<Guid, Session> ClientSessionsStorage = new ConcurrentDictionary<Guid, Session>();

        private readonly IGameDataPersisterFactory _gameDataPersisterFactory;
        private readonly Task _backgroundPersisterTask;
        private readonly ConcurrentBag<Tuple<string, string>>[] PersistTimeSlotsUsers = new ConcurrentBag<Tuple<string, string>>[PersistTimeSlotsCount];
        private const int PersistTimeSlotsCount = 60;
        private const int PersistTimeSlotMilliseconds = 1000;
        private const int PersistUsersChunkSize = 10;

        public GameSessions(ILogger logger, IGameFactory gameFactory, IGameDataPersisterFactory gameDataPersisterFactory, IHashHelper hashHelper)
        {
            _logger = logger;
            _gameFactory = gameFactory;
            _hashHelper = hashHelper;

            _gameDataPersisterFactory = gameDataPersisterFactory;
            for (int i = 0; i < PersistTimeSlotsUsers.Length; i++)
            {
                PersistTimeSlotsUsers[i] = new ConcurrentBag<Tuple<string, string>>();
            }

            _backgroundPersisterTask = Task.Run(() => PerformBackgroundPersisting());

            _gameTimer = new TaskTimer(TimeSpan.FromSeconds(1), TimerCallback).Start();
        }

        internal IEnumerable<Session> Sessions
        {
            get { return ClientSessionsStorage.Values; }
        }

        private void PerformBackgroundPersisting()
        {
            IGameDataPersister gameDataPersister = _gameDataPersisterFactory.BuildGameDataPersister();
            int currentTimeSlotId = 0;

            // Infinite loop that is persisting User Games that are spread among TimeSlots
            while (true)
            {
                if (_logger != null)
                    _logger.Info("Perform persist cycle");

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var gamesWithUserIdsChunk = new List<GameWithUserId>();
                ConcurrentBag<Tuple<string, string>> timeSlot = PersistTimeSlotsUsers[currentTimeSlotId];
                foreach (Tuple<string, string> timeSlotUser in timeSlot)
                {
                    gamesWithUserIdsChunk.Add(new GameWithUserId()
                    {
                        Game = UserGamesStorage[timeSlotUser.Item1],
                        UserId = timeSlotUser.Item2
                    });
                    if (gamesWithUserIdsChunk.Count >= PersistUsersChunkSize)
                    {
                        gameDataPersister.PersistGamesData(gamesWithUserIdsChunk);
                        gamesWithUserIdsChunk = new List<GameWithUserId>();
                    }
                }
                if (gamesWithUserIdsChunk.Count > 0)
                {
                    gameDataPersister.PersistGamesData(gamesWithUserIdsChunk);
                }

                stopwatch.Stop();

                int millisecondsUntilTimeSlotEnd = PersistTimeSlotMilliseconds
                                                    -
                                                    (int)
                                                        Math.Min(stopwatch.ElapsedMilliseconds, PersistTimeSlotMilliseconds);
                Task.Delay(millisecondsUntilTimeSlotEnd).Wait();

                currentTimeSlotId = (currentTimeSlotId + 1) % PersistTimeSlotsCount;
            }
        }

        private void TimerCallback()
        {
            var sw = new Stopwatch();
            sw.Start();
            UserGamesStorage.AsParallel().ForAll(_ => _.Value.PerformAutoStep());
            if (_logger != null)
                _logger.Info("Perform steps for {0} active games and {1} sessions: {2}", UserGamesStorage.Count, ClientSessionsStorage.Count, sw.Elapsed);
        }

        public IGame GetGame(Guid sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null)
            {
                throw new InvalidSessionException(string.Format("Session {0} doesn't exists", sessionId), sessionId);
            }
            session.LastActivity = DateTime.Now;
            if (!UserGamesStorage.ContainsKey(session.Login))
            {
                var game = BuildGame(session.UserId, session.Login);
                game.Initialize();
                UserGamesStorage.TryAdd(session.Login, game);

                int timeSlotId = _hashHelper.GetModuloOfUserIdHash(session.UserId, PersistTimeSlotsCount);
                PersistTimeSlotsUsers[timeSlotId].Add(new Tuple<string, string>(session.Login, session.UserId));
            }
            return UserGamesStorage[session.Login];
        }

        public IGame GetGame(string username)
        {
            return UserGamesStorage.ContainsKey(username) ? UserGamesStorage[username] : null;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (Sessions.Count(_ => _.Login == username) > 0)
            {
                return UserGameSessionStatus.Online;
            }
            if (!UserGamesStorage.ContainsKey(username))
            {
                return UserGameSessionStatus.GameNotStarted;
            }
            return UserGameSessionStatus.Offline;
        }

        public Guid AddSession(User user)
        {
            var session = new Session
            {
                Login = user.Login,
                UserId = user.Id,
                SessionIdentifier = Guid.NewGuid(),
                UserRoles = user.IsAdmin ? new[] {UserRole.GenericUser, UserRole.Admin} : new[] {UserRole.GenericUser},
                Identity =
                    new CustomGameIdentity {AuthenticationType = "Auto", IsAuthenticated = true, Name = user.Login}
            };
            ClientSessionsStorage.TryAdd(session.SessionIdentifier, session);
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            return !ClientSessionsStorage.ContainsKey(sessionId) ? null : ClientSessionsStorage[sessionId];
        }

        public bool RemoveSession(Guid sessionId)
        {
            Session session;
            return ClientSessionsStorage.TryRemove(sessionId, out session);
        }

        public void RemoveAllSessions()
        {
            ClientSessionsStorage.Clear();
        }

        private IGame BuildGame(string userId, string userName)
        {
            return _gameFactory.BuildGame(userId, userName);
        }
    }
}