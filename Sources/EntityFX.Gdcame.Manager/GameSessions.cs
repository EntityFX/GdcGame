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

namespace EntityFX.Gdcame.Manager
{
    public class GameSessions
    {
        private readonly IGameFactory _gameFactory;

        private readonly TaskTimer _gameTimer;
        private readonly ILogger _logger;
        private readonly IHashHelper _hashHelper;

        private readonly IDictionary<string, IGame> UserGamesStorage =
            new Dictionary<string, IGame>();

        private readonly IDictionary<Guid, Session> ClientSessionsStorage = new Dictionary<Guid, Session>();

        private readonly IGameDataPersisterFactory _gameDataPersisterFactory;
        private readonly Task _backgroundPersisterTask;
        private readonly CancellationTokenSource _backgroundPersisterTaskToken = new CancellationTokenSource();
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

            _backgroundPersisterTask = Task.Factory.StartNew(a => PerformBackgroundPersisting(), TaskCreationOptions.LongRunning, _backgroundPersisterTaskToken.Token);

            _gameTimer = new TaskTimer(TimeSpan.FromSeconds(1), TimerCallback).Start();
        }

        internal IDictionary<Guid, Session> Sessions
        {
            get { return ClientSessionsStorage; }
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
                    if (UserGamesStorage.ContainsKey(timeSlotUser.Item1))
                    {
                        gamesWithUserIdsChunk.Add(new GameWithUserId()
                        {
                            Game = UserGamesStorage[timeSlotUser.Item1],
                            UserId = timeSlotUser.Item2
                        });
                    }

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
                if (_logger != null)
                    _logger.Debug("PerformBackgroundPersisting: Delay {0} ms", millisecondsUntilTimeSlotEnd);
                var res = Task.Delay(millisecondsUntilTimeSlotEnd).Wait(millisecondsUntilTimeSlotEnd * 2);

                currentTimeSlotId = (currentTimeSlotId + 1) % PersistTimeSlotsCount;
            }
        }

        private void TimerCallback()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {

                    Parallel.ForEach(UserGamesStorage.Values, game =>
                    {
                        game.PerformAutoStep();
                    });


                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }

            if (_logger != null)
                _logger.Info("Perform steps for {0} active games and {1} sessions: {2}", UserGamesStorage.Count, ClientSessionsStorage.Count, sw.Elapsed);
        }

        private static readonly object _stdLock = new { };

        private IGame StartGame(string userId, string login)
        {
            var game = BuildGame(userId, login);
            game.Initialize();
            int timeSlotId = _hashHelper.GetModuloOfUserIdHash(userId, PersistTimeSlotsCount);
            PersistTimeSlotsUsers[timeSlotId].Add(new Tuple<string, string>(login, userId));
            return game;
        }

        public IGame GetGame(Guid sessionId)
        {
            Session session;

            session = GetSession(sessionId);
            if (session == null)
            {
                _logger.Warning("Session {0} doesn't exists", sessionId);
                throw new InvalidSessionException(string.Format("Session {0} doesn't exists", sessionId), sessionId);
            }

            lock (_stdLock)
            {

                return UserGamesStorage[session.Login];
            }
        }

        public IGame GetGame(string username)
        {
            return UserGamesStorage.ContainsKey(username) ? UserGamesStorage[username] : null;
        }

        public UserGameSessionStatus GetGameSessionStatus(string username)
        {
            if (Sessions.Values.Count(_ => _.Login == username) > 0)
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
                UserRoles = user.IsAdmin ? new[] { UserRole.GenericUser, UserRole.Admin } : new[] { UserRole.GenericUser },
                Identity =
                    new CustomGameIdentity { AuthenticationType = "Auto", IsAuthenticated = true, Name = user.Login }
            };

            lock (_stdLock)
            {
                if (!UserGamesStorage.ContainsKey(user.Login))
                {
                    var game = StartGame(user.Id, user.Login);
                    UserGamesStorage[user.Login] = game;
                }
                ClientSessionsStorage[session.SessionIdentifier] = session;
            }



            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            lock (_stdLock)
            {
                return ClientSessionsStorage[sessionId];
            }
        }

        public void RemoveSession(Guid sessionId)
        {
            lock (_stdLock)
            {
                ClientSessionsStorage.Remove(sessionId);
            }

        }

        public void RemoveGame(UserData user)
        {

            var userSessions = Sessions.Values.Where(_ => _.Login == user.Login);
            lock (this)
            {
                foreach (var userSession in userSessions)
                {
                    ClientSessionsStorage.Remove(userSession.SessionIdentifier);
                }

                UserGamesStorage[user.Login] = null;
                UserGamesStorage.Remove(user.Login);
            }
            int timeSlotId = _hashHelper.GetModuloOfUserIdHash(user.Login, PersistTimeSlotsCount);
            var userTimeSlot = PersistTimeSlotsUsers[timeSlotId].FirstOrDefault(_ => _.Item2 == user.Id);
            if (userTimeSlot != null)
            {
                PersistTimeSlotsUsers[timeSlotId].TryTake(out userTimeSlot);
            }

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