namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class PersistenceWorker : WorkerBase, IWorker
    {
        private const int PersistTimeSlotsCount = 60;
        private const int PersistTimeSlotMilliseconds = 1000;
        private const int PersistUsersChunkSize = 100;

        private ILogger _logger;
        private IGameSessions _gameSessions;
        private readonly IGameDataPersisterFactory _gameDataPersisterFactory;
        private readonly IHashHelper _hashHelper;
        private readonly GamePerformanceInfo _performanceInfo;
        private readonly IServerDataAccessService _serverDataAccessService;
        private Task _backgroundPersisterTask;
        private readonly CancellationTokenSource _backgroundPersisterTaskToken = new CancellationTokenSource();

        private readonly ConcurrentBag<Tuple<string, string, DateTime>>[] PersistTimeSlotsUsers =
            new ConcurrentBag<Tuple<string, string, DateTime>>[PersistTimeSlotsCount];

        public PersistenceWorker(ILogger logger, IGameSessions gameSessions,
            IGameDataPersisterFactory gameDataPersisterFactory, IHashHelper hashHelper, GamePerformanceInfo performanceInfo, IServerDataAccessService serverDataAccessService)
        {
            this._logger = logger;
            this._gameSessions = gameSessions;
            this._gameDataPersisterFactory = gameDataPersisterFactory;
            this._hashHelper = hashHelper;
            this._performanceInfo = performanceInfo;
            this._serverDataAccessService = serverDataAccessService;

            this.Name = "Games persistence worker";

            this._gameDataPersisterFactory = gameDataPersisterFactory;
            for (int i = 0; i < this.PersistTimeSlotsUsers.Length; i++)
            {
                this.PersistTimeSlotsUsers[i] = new ConcurrentBag<Tuple<string, string, DateTime>>();
            }
            gameSessions.GameStarted += this.GameSessions_GameStarted;
            gameSessions.GameRemoved += this.GameSessions_GameRemoved;
            gameSessions.AllGamesRemoved += this.GameSessions_AllGamesRemoved;
        }

        private void GameSessions_AllGamesRemoved(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PersistTimeSlotsUsers.Length; i++)
            {
                this.PersistTimeSlotsUsers[i] = new ConcurrentBag<Tuple<string, string, DateTime>>();
            }
            GC.Collect();
        }

        private void GameSessions_GameRemoved(object sender, Tuple<string, string> e)
        {
            int timeSlotId = this._hashHelper.GetModuloOfUserIdHash(e.Item1, PersistTimeSlotsCount);
            var userTimeSlot = this.PersistTimeSlotsUsers[timeSlotId].FirstOrDefault(_ => _.Item2 == e.Item2);
            if (userTimeSlot != null)
            {
                this.PersistTimeSlotsUsers[timeSlotId].TryTake(out userTimeSlot);
            }
        }

        //TODO: Refactor
        private void GameSessions_GameStarted(object sender, Tuple<string, string> e)
        {
            int timeSlotId = this._hashHelper.GetModuloOfUserIdHash(e.Item1, PersistTimeSlotsCount);

            this.PersistTimeSlotsUsers[timeSlotId].Add(new Tuple<string, string, DateTime>(e.Item1, e.Item2, DateTime.Now));
        }

        public override bool IsRunning
        {
            get
            {
                return this._backgroundPersisterTask.Status == TaskStatus.Running
                       || this._backgroundPersisterTask.Status == TaskStatus.WaitingForActivation
                       || this._backgroundPersisterTask.Status == TaskStatus.RanToCompletion;
            }
        }                                                                             

        public override void Run<TData>(TData data = default(TData))
        {
            this._backgroundPersisterTask = Task.Factory.StartNew(a => this.PerformBackgroundPersisting(),
                TaskCreationOptions.LongRunning, this._backgroundPersisterTaskToken.Token).ContinueWith(c =>
            {
                if (c.IsFaulted)
                {
                    this._logger.Error(c.Exception.InnerException);
                }
            });
        }

        private void PerformBackgroundPersisting()
        {
            IGameDataPersister gameDataPersister = this._gameDataPersisterFactory.BuildGameDataPersister();
            int currentTimeSlotId = 0;
            var stopwatch = new Stopwatch();
            // Infinite loop that is persisting User Games that are spread among TimeSlots
            while (true)
            {
                this.IncrementTick();
                try
                {
                    if (this._logger != null)
                        this._logger.Info("Perform persist cycle");


                    stopwatch.Restart();

                    var gamesWithUserIdsChunk = new List<GameWithUserId>();
                    ConcurrentBag<Tuple<string, string, DateTime>> timeSlot = this.PersistTimeSlotsUsers[currentTimeSlotId];
                    foreach (Tuple<string, string, DateTime> timeSlotUser in timeSlot)
                    {
                        if (this._gameSessions.Games.ContainsKey(timeSlotUser.Item1))
                        {
                            gamesWithUserIdsChunk.Add(new GameWithUserId()
                            {
                                Game = this._gameSessions.Games[timeSlotUser.Item1],
                                UserId = timeSlotUser.Item2,
                                CreateDateTime = timeSlotUser.Item3,
                                UpdateDateTime = DateTime.Now
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
                    this._performanceInfo.PersistencePerCycle = stopwatch.Elapsed;
                    if (this._logger != null)
                        this._logger.Debug("PerformBackgroundPersisting: Delay {0} ms", millisecondsUntilTimeSlotEnd);
                    var res = Task.Delay(millisecondsUntilTimeSlotEnd).Wait(millisecondsUntilTimeSlotEnd * 2);
                    currentTimeSlotId = (currentTimeSlotId + 1) % PersistTimeSlotsCount;

                    PerfomanceCounters["tick"] = stopwatch.ElapsedMilliseconds;
                    PerfomanceCounters["perf.before-timeslot-end"] = millisecondsUntilTimeSlotEnd;
                    PerfomanceCounters["games"] = this._gameSessions.Games.Count;
                }
                catch (Exception exception)
                {
                        this._logger.Error(exception);
                    throw;
                }


            }
        }
    }
}