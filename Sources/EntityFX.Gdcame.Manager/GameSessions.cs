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
    public class GameSessions : IDisposable
    {
        private readonly IGameFactory _gameFactory;


        private readonly ILogger _logger;
        private readonly IHashHelper _hashHelper;

        private readonly ConcurrentDictionary<string, IGame> _userGamesStorage =
            new ConcurrentDictionary<string, IGame>();

        private readonly ConcurrentDictionary<Guid, Session> _userSessionsStorage = new ConcurrentDictionary<Guid, Session>();

        private readonly IGameDataPersisterFactory _gameDataPersisterFactory;


        private readonly ConcurrentBag<Tuple<string, string>>[] PersistTimeSlotsUsers = new ConcurrentBag<Tuple<string, string>>[PersistTimeSlotsCount];

        private const int PersistTimeSlotsCount = 60;
        private const int PersistTimeSlotMilliseconds = 1000;
        private const int PersistUsersChunkSize = 10;
        private const int SessionLifeInSeconds = 3600;
        private const int SessionsCheckIntervalInSeconds = 30;

        private readonly CancellationTokenSource _backgroundPersisterTaskToken = new CancellationTokenSource();

        private readonly Task _backgroundPerformAutomaticStepsTask;
        private readonly Task _backgroundPersisterTask;
        private readonly Task _backgroundSessionsCheckerTask;

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
            _backgroundPerformAutomaticStepsTask = new TaskTimer(TimeSpan.FromSeconds(1), PerformAutomaticSteps).Start();
            _backgroundSessionsCheckerTask = new TaskTimer(TimeSpan.FromSeconds(SessionsCheckIntervalInSeconds), PerformSessionsCheckTask).Start();
        }

        internal IDictionary<Guid, Session> Sessions
        {
            get { return _userSessionsStorage; }
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
                    if (_userGamesStorage.ContainsKey(timeSlotUser.Item1))
                    {
                        gamesWithUserIdsChunk.Add(new GameWithUserId()
                        {
                            Game = _userGamesStorage[timeSlotUser.Item1],
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

        private void PerformAutomaticSteps()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {
                    // Parallel.ForEach(UserGamesStorage.Values, game => game.PerformAutoStep());
                    var calulateGamesChunk = new IGame[100];
                    var count = 0;
                    foreach (var game in _userGamesStorage.Values)
                    {
                        if (count < 100)
                        {
                            calulateGamesChunk[count] = game;
                            count++;
                        } else
                        {
                            Task.Factory.StartNew(_ => {
                                var games = (IGame[])_;
                                for (int i = 0; i < games.Length; i++)
                                {
                                    games[i].PerformAutoStep();
                                }
                            }, calulateGamesChunk);
                            calulateGamesChunk = new IGame[100];
                            count = 0;
                        }
                    }
                    if (count < 100)
                    {
                        Task.Factory.StartNew(_ => {
                            var games = (IGame[])_;
                            for (int i = 0; i < games.Length; i++)
                            {
                                games[i].PerformAutoStep();
                            }
                        }, calulateGamesChunk);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }

            if (_logger != null)
                _logger.Info("Perform steps for {0} active games and {1} sessions: {2}", _userGamesStorage.Count, _userSessionsStorage.Count, sw.Elapsed);
        }

        private void PerformSessionsCheckTask()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {
                    foreach (var session in _userSessionsStorage)
                    {
                        if ((DateTime.Now - session.Value.LastActivity) > TimeSpan.FromSeconds(SessionLifeInSeconds))
                        {
                            RemoveSession(session.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }
            if (_logger != null)
                _logger.Info("Perform sessions {0} check: {1}",  _userSessionsStorage.Count, sw.Elapsed);
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
            var session = new Session
            {
                Login = user.Login,
                UserId = user.Id,
                SessionIdentifier = Guid.NewGuid(),
                UserRoles = user.IsAdmin ? new[] { UserRole.GenericUser, UserRole.Admin } : new[] { UserRole.GenericUser },
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
            int timeSlotId = _hashHelper.GetModuloOfUserIdHash(user.Login, PersistTimeSlotsCount);
            var userTimeSlot = PersistTimeSlotsUsers[timeSlotId].FirstOrDefault(_ => _.Item2 == user.Id);
            if (userTimeSlot != null)
            {
                PersistTimeSlotsUsers[timeSlotId].TryTake(out userTimeSlot);
            }

        }

        public void RemoveAllSessions()
        {
            _userSessionsStorage.Clear();
        }

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
                    _backgroundPersisterTaskToken.Dispose();
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