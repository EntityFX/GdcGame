using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.AdminManager;

namespace EntityFX.Gdcame.Manager.Workers
{
    public class PersistenceWorker : IWorker
    {
        private const int PersistTimeSlotsCount = 60;
        private const int PersistTimeSlotMilliseconds = 1000;
        private const int PersistUsersChunkSize = 100;

        private ILogger _logger;
        private GameSessions _gameSessions;
        private readonly IGameDataPersisterFactory _gameDataPersisterFactory;
        private readonly IHashHelper _hashHelper;
        private readonly PerformanceInfo _performanceInfo;
        private readonly TaskTimer _backgroundPersistenceTimer;
        private Task _backgroundPersisterTask;
        private readonly CancellationTokenSource _backgroundPersisterTaskToken = new CancellationTokenSource();
        private readonly ConcurrentBag<Tuple<string, string>>[] PersistTimeSlotsUsers = new ConcurrentBag<Tuple<string, string>>[PersistTimeSlotsCount];

        public PersistenceWorker(ILogger logger, GameSessions gameSessions, IGameDataPersisterFactory gameDataPersisterFactory, IHashHelper hashHelper, PerformanceInfo performanceInfo)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _gameDataPersisterFactory = gameDataPersisterFactory;
            _hashHelper = hashHelper;
            _performanceInfo = performanceInfo;

            _gameDataPersisterFactory = gameDataPersisterFactory;
            for (int i = 0; i < PersistTimeSlotsUsers.Length; i++)
            {
                PersistTimeSlotsUsers[i] = new ConcurrentBag<Tuple<string, string>>();
            }
            gameSessions.GameStarted += GameSessions_GameStarted;
            gameSessions.GameRemoved += GameSessions_GameRemoved;
        }

        private void GameSessions_GameRemoved(object sender, Tuple<string, string> e)
        {
            int timeSlotId = _hashHelper.GetModuloOfUserIdHash(e.Item1, PersistTimeSlotsCount);
            var userTimeSlot = PersistTimeSlotsUsers[timeSlotId].FirstOrDefault(_ => _.Item2 == e.Item2);
            if (userTimeSlot != null)
            {
                PersistTimeSlotsUsers[timeSlotId].TryTake(out userTimeSlot);
            }
        }

        private void GameSessions_GameStarted(object sender, Tuple<string, string> e)
        {
            int timeSlotId = _hashHelper.GetModuloOfUserIdHash(e.Item1, PersistTimeSlotsCount);
            PersistTimeSlotsUsers[timeSlotId].Add(new Tuple<string, string>(e.Item1, e.Item2));
        }

        public void Run()
        {
            _backgroundPersisterTask = Task.Factory.StartNew(a => PerformBackgroundPersisting(), TaskCreationOptions.LongRunning, _backgroundPersisterTaskToken.Token);
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
                    if (_gameSessions.Games.ContainsKey(timeSlotUser.Item1))
                    {
                        gamesWithUserIdsChunk.Add(new GameWithUserId()
                        {
                            Game = _gameSessions.Games[timeSlotUser.Item1],
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
                _performanceInfo.PersistencePerCycle = stopwatch.Elapsed;
                if (_logger != null)
                    _logger.Debug("PerformBackgroundPersisting: Delay {0} ms", millisecondsUntilTimeSlotEnd);
                var res = Task.Delay(millisecondsUntilTimeSlotEnd).Wait(millisecondsUntilTimeSlotEnd * 2);
                currentTimeSlotId = (currentTimeSlotId + 1) % PersistTimeSlotsCount;
            }
        }
    }
}