namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class RatingCalculationWorker : WorkerBase, IWorker
    {
        private const int TimeSaveInSeconds = 30;
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private readonly IGameSessions _gameSessions;
        private readonly ILocalNodeRatingDataAccess _localNodeRatingDataAccess;
        private readonly ITaskTimer _backgroundSaveHistoryCheckerTimer;
        private Task _backgroundSaveHistoryCheckerTask;
        private object _stdLock = new { };

        private bool isCalculating = false;

        public RatingCalculationWorker(ILogger logger, IGameSessions gameSessions, ILocalNodeRatingDataAccess localNodeRatingDataAccess, ITaskTimerFactory taskTimerFactory)
        {
            this._logger = logger;
            this._gameSessions = gameSessions;
            this._localNodeRatingDataAccess = localNodeRatingDataAccess;
            this._backgroundSaveHistoryCheckerTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(TimeSaveInSeconds), this.PerformRatingCalculation);
            this.Name = "Rating Calculation Worker";
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this._backgroundSaveHistoryCheckerTask = this._backgroundSaveHistoryCheckerTimer.Start();
        }

        public override bool IsRunning
        {
            get
            {
                return this._backgroundSaveHistoryCheckerTask.Status == TaskStatus.Running
                       || this._backgroundSaveHistoryCheckerTask.Status == TaskStatus.WaitingForActivation
                       || this._backgroundSaveHistoryCheckerTask.Status == TaskStatus.RanToCompletion;
            }
        }

        private void PerformRatingCalculation()
        {
            this.IncrementTick();
            lock (_stdLock)
            {
                if (this.isCalculating) return;
                this.isCalculating = true;
            }



            var sw = new Stopwatch();
            sw.Start();
            var swCalc = new Stopwatch();
            var swSave = new Stopwatch();

            swCalc.Start();
            CalculateStatisticsPerformanceCounter[] calculateStatisticsPerformanceCounters;
            try
            {

                swSave.Start();
                this.SaveRatingHistory();
                this._logger.Info("Perform rating calculation - save history for {0} games, ellapsed: {1}", this._gameSessions.Games.Count, swSave.Elapsed);
                swSave.Stop();
                swCalc.Start();
                calculateStatisticsPerformanceCounters = this.RecalculateRatingStatisticsForActiveUsers();
                this._logger.Info("Perform rating calculation - recalculate statistics {0} games, ellapsed: {1}", this._gameSessions.Games.Count, swCalc.Elapsed);
                swCalc.Stop();
                this.CleanOldHistory();
            }
            catch (Exception e)
            {
                this._logger.Error(e);
                throw;
            }


            lock (_stdLock)
            {
                this.isCalculating = false;
            }

            PerfomanceCounters["tick"] = sw.Elapsed.TotalMilliseconds;
            PerfomanceCounters["perf"] = sw.Elapsed.TotalMilliseconds / this._gameSessions.Games.Count;
            PerfomanceCounters["chunk"] = ChunkSize;
            PerfomanceCounters["tick.calc"] = swCalc.Elapsed.TotalMilliseconds;
            PerfomanceCounters["tick.save"] = swSave.Elapsed.TotalMilliseconds;
            PerfomanceCounters["perf.calc"] = swCalc.Elapsed.TotalMilliseconds / this._gameSessions.Games.Count;
            PerfomanceCounters["perf.calc.read-avg"] = calculateStatisticsPerformanceCounters.Length > 0 ? calculateStatisticsPerformanceCounters.Average(c => c.ReadHistoryForUsersMillisecs) : 0;
            PerfomanceCounters["tick.save"] = swSave.Elapsed.TotalMilliseconds / this._gameSessions.Games.Count;
            PerfomanceCounters["games"] = this._gameSessions.Games.Count;

            if (this._logger != null)
                this._logger.Info("Perform rating calculation for {0} games, ellapsed: {1}", this._gameSessions.Games.Count, sw.Elapsed);
        }

        private void SaveRatingHistory()
        {
            var ratingHistoryChunk = new RatingHistory[ChunkSize];
            var count = 0;
            var userIdentities = this._gameSessions.Identities;
            foreach (var game in this._gameSessions.Games)
            {
                if (count < ChunkSize)
                {
                    var userRatingHistory = new RatingHistory
                    {
                        UserId = userIdentities[game.Key],
                        ManualStepsCount = game.Value.ManualStepNumber,
                        RootCounter = (int)game.Value.GameCash.RootCounter.Value,
                        Data = DateTime.Now,
                        TotalEarned = game.Value.GameCash.TotalEarned,
                    };
                    ratingHistoryChunk[count] = userRatingHistory;
                    count++;
                }
                else
                {
                    if (ratingHistoryChunk.FirstOrDefault() != null)
                    {
                        this._localNodeRatingDataAccess.PersistUsersRatingHistory(ratingHistoryChunk);
                        ratingHistoryChunk = new RatingHistory[ChunkSize];
                        count = 0;
                    }
                }
            }
            if (count < ChunkSize)
            {
                if (ratingHistoryChunk.FirstOrDefault() != null)
                {
                    this._localNodeRatingDataAccess.PersistUsersRatingHistory(ratingHistoryChunk);
                    ratingHistoryChunk = new RatingHistory[ChunkSize];
                    count = 0;
                }
            }

        }
        private CalculateStatisticsPerformanceCounter[] RecalculateRatingStatisticsForActiveUsers()
        {
            int ChunkSizeUsers = ChunkSize;
            var users = new string[ChunkSizeUsers];
            var count = 0;
            var userIdentities = this._gameSessions.Identities;
            var tasksList = new List<Task<CalculateStatisticsPerformanceCounter>>();
            foreach (var game in this._gameSessions.Games)
            {
                if (count < ChunkSizeUsers)
                {
                    users[count] = userIdentities[game.Key];
                    count++;
                }
                else
                {
                    if (users.FirstOrDefault() != null)
                    {
                        string[] chunk = new string[ChunkSizeUsers];
                        Array.Copy(users, chunk, ChunkSizeUsers);
                        var task = new Task<CalculateStatisticsPerformanceCounter>(() => this.RecalculationRatingStatisticsChunkUsers(chunk));
                        tasksList.Add(task);
                        users = new string[ChunkSizeUsers];
                        count = 0;
                    }
                }
            }
            if (count < ChunkSizeUsers)
            {
                if (users.FirstOrDefault() != null)
                {
                    string[] chunk = new string[count];
                    Array.Copy(users, chunk, count);
                    var task = new Task<CalculateStatisticsPerformanceCounter>(() => this.RecalculationRatingStatisticsChunkUsers(chunk));
                    tasksList.Add(task);
                    users = new string[ChunkSizeUsers];
                    count = 0;
                }
            }
            foreach (var task in tasksList)
            {
                task.Start();
            }
            Task.WaitAll(tasksList.ToArray());
            return Task.WhenAll(tasksList).Result;
        }

        private class CalculateStatisticsPerformanceCounter
        {
            public double ReadHistoryForUsersMillisecs { get; set; }
        }

        private CalculateStatisticsPerformanceCounter RecalculationRatingStatisticsChunkUsers(string[] users)
        {
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            var sw = new Stopwatch();
            sw.Start();
            var result = new CalculateStatisticsPerformanceCounter();
            //TimeSpan periodTotal = new TimeSpan(7, 0, 0, 0);
            List<RatingStatistics> usersNewRating = new List<RatingStatistics>();
            var readHistoryWithUsersIdsForWeek = this._localNodeRatingDataAccess.ReadHistoryWithUsersIds(users, periodWeek);
            result.ReadHistoryForUsersMillisecs = sw.Elapsed.TotalMilliseconds;
            sw.Stop();
            //var usersId=

            var ratingHistoryGroupedByUser = readHistoryWithUsersIdsForWeek.GroupBy(g => g.UserId);
            foreach (var ratingHistoryOfUser in ratingHistoryGroupedByUser)
            {
                usersNewRating.Add(this.RecalculationRatingStatisticsOneUser(ratingHistoryOfUser.ToList()));
            }
            this._localNodeRatingDataAccess.CreateOrUpdateUsersRatingStatistics(usersNewRating.ToArray());
            return result;
        }

        private RatingStatistics RecalculationRatingStatisticsOneUser(List<RatingHistory> userHistoreTotal)
        {

            var lastDataUser = userHistoreTotal.OrderByDescending(t => t.Data).FirstOrDefault();
            //add the values for the Total
            RatingStatistics userRatingNew = new RatingStatistics
            {
                UserId = lastDataUser.UserId,
                ManualStepsCount = new CountValues { Day = lastDataUser.ManualStepsCount, Week = lastDataUser.ManualStepsCount, Total = lastDataUser.ManualStepsCount },
                RootCounter = new CountValues { Day = lastDataUser.RootCounter, Week = lastDataUser.RootCounter, Total = lastDataUser.RootCounter },
                TotalEarned = new CountValues { Day = lastDataUser.TotalEarned, Week = lastDataUser.TotalEarned, Total = lastDataUser.TotalEarned },
            };
            if (userHistoreTotal.Count >= 2)
            {
                // add the values for the day
                // var dataExtreme= lastRecordHistory.Data.Subtract(period);
                TimeSpan periodDay = new TimeSpan(1, 0, 0, 0);
                var userHistoryDay = userHistoreTotal.FindAll(t => t.Data >= lastDataUser.Data.Subtract(periodDay)).OrderBy(t => t.Data);
                var firstDataUserDay = userHistoryDay.FirstOrDefault();
                userRatingNew.ManualStepsCount.Day = lastDataUser.ManualStepsCount - firstDataUserDay.ManualStepsCount;
                userRatingNew.RootCounter.Day = lastDataUser.RootCounter - firstDataUserDay.RootCounter;
                userRatingNew.TotalEarned.Day = lastDataUser.TotalEarned - firstDataUserDay.TotalEarned;

                //add the values for the 7 day
                TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
                var userHistoryWeek = userHistoreTotal.FindAll(t => t.Data >= lastDataUser.Data.Subtract(periodWeek)).OrderBy(t => t.Data);
                var firstDataUserWeek = userHistoryWeek.FirstOrDefault();
                userRatingNew.ManualStepsCount.Week = lastDataUser.ManualStepsCount - firstDataUserWeek.ManualStepsCount;
                userRatingNew.RootCounter.Week = lastDataUser.RootCounter - firstDataUserWeek.RootCounter;
                userRatingNew.TotalEarned.Total = lastDataUser.TotalEarned - firstDataUserWeek.TotalEarned;
            }
            return userRatingNew;

        }

        private void CleanOldHistory()
        {
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            this._localNodeRatingDataAccess.CleanOldHistory(periodWeek);
        }
    }
}
