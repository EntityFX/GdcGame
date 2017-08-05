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
            var swCalc = new Stopwatch();
            var swSave = new Stopwatch();
            sw.Start();
            swCalc.Start();
            swSave.Start();

            try
            {
                this.SaveRatingHistory();
                this._logger.Info("Perform rating calculation - save history for {0} games, ellapsed: {1}", this._gameSessions.Games.Count, swCalc.Elapsed);
                this.RecalculateRatingStatisticsForActiveUsers();
                this._logger.Info("Perform rating calculation - recalculate statistics {0} games, ellapsed: {1}", this._gameSessions.Games.Count, swSave.Elapsed);
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
            PerfomanceCounters["tick.calc"] = swCalc.Elapsed.TotalMilliseconds;
            PerfomanceCounters["tick.save"] = swSave.Elapsed.TotalMilliseconds;
            PerfomanceCounters["perf.calc"] = swCalc.Elapsed.TotalMilliseconds / this._gameSessions.Games.Count;
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


            //RatingHistory userRatingHistory;
            //foreach (var game in _gameSessions.Games)
            //{

            //    userRatingHistory = new RatingHistory
            //    {
            //        UserId = game.Key,
            //        ManualStepsCount = game.Value.ManualStepNumber,
            //        RootCounter = (Int32)game.Value.GameCash.RootCounter.Value,
            //        Data = DateTime.Now,
            //        TotalEarned = game.Value.GameCash.TotalEarned,
            //    };
            //    _localRatingDataAccess.PersistUsersRatingHistory(ratingHistoryChunk);
            //};
        }
        private void RecalculateRatingStatisticsForActiveUsers()
        {
            int ChunkSizeUsers = 100;
            var users = new string[ChunkSizeUsers];
            var count = 0;
            var userIdentities = this._gameSessions.Identities;
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
                        this.RecalculationRatingStatisticsChunkUsers(users);
                        users = new string[ChunkSizeUsers];
                        count = 0;
                    }
                }
            }
            if (count < ChunkSizeUsers)
            {
                if (users.FirstOrDefault() != null)
                {
                    this.RecalculationRatingStatisticsChunkUsers(users);
                    users = new string[ChunkSizeUsers];
                    count = 0;
                }
            }
        }

        private void RecalculationRatingStatisticsChunkUsers(string[] users)
        {
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            //TimeSpan periodTotal = new TimeSpan(7, 0, 0, 0);
            List<RatingStatistics> usersNewRating = new List<RatingStatistics>();
            var readHistoryWithUsersIdsForWeek = this._localNodeRatingDataAccess.ReadHistoryWithUsersIds(users, periodWeek);
            //var usersId=

            var ratingHistoryGroupedByUser = readHistoryWithUsersIdsForWeek.GroupBy(g => g.UserId);
            foreach (var ratingHistoryOfUser in ratingHistoryGroupedByUser)
            {
                usersNewRating.Add(this.RecalculationRatingStatisticsOneUser(ratingHistoryOfUser.ToList()));
            }
            this._localNodeRatingDataAccess.CreateOrUpdateUsersRatingStatistics(usersNewRating.ToArray());

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
                var firstDataUserWeek = userHistoryDay.FirstOrDefault();
                userRatingNew.ManualStepsCount.Week = lastDataUser.ManualStepsCount - firstDataUserDay.ManualStepsCount;
                userRatingNew.RootCounter.Week = lastDataUser.RootCounter - firstDataUserWeek.ManualStepsCount;
                userRatingNew.TotalEarned.Total = lastDataUser.TotalEarned - firstDataUserWeek.TotalEarned;
            }
            return userRatingNew;

        }

        //private void RecalculationRatingStatistics()
        //{
        //    TimeSpan periodDay = new TimeSpan(1, 0, 0, 0);
        //    TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
        //    //TimeSpan periodTotal = new TimeSpan(7, 0, 0, 0);
        //    List<string> userIds = new List<string>();
        //    List<RatingStatistics> usersNewRating = new List<RatingStatistics>();
        //    foreach (var game in _gameSessions.Games)
        //    {
        //        userIds.Add(game.Key);
        //    }
        //    var ratingHistoryDay = _localRatingDataAccess.ReadHistoryWithUsersIds(userIds.ToArray(), periodWeek);
        //    //var usersId=
        //    ratingHistoryDay.OrderBy(t => t.Data);

        //    //one   user
        //    foreach (var oneUserHistory in ratingHistoryDay)
        //    //    var userHistory=ratingHistoryDay.Select(t=>t.UserId==oneUserHistory.UserId)
        //    //
        //    foreach (var userHistory in ratingHistoryDay)
        //    {
        //        var existingRating = usersNewRating.FirstOrDefault(t => t.UserId == userHistory.UserId);
        //        if (existingRating == null)
        //        {
        //            existingRating = new RatingStatistics
        //            {
        //                UserId = "0",
        //                MunualStepsCount = new CountValues { Day = 0, Week = 0, Total = 0 },
        //                RootCounter = new CountValues { Day = 0, Week = 0, Total = 0 },
        //                TotalEarned = new CountValues { Day = 0, Week = 0, Total = 0 },
        //            };
        //        }
        //        else
        //        {
        //            usersNewRating.Remove(existingRating);
        //        }
        //        RatingStatistics userRatingNew = new RatingStatistics
        //        {
        //            UserId = userHistory.UserId,
        //            MunualStepsCount = new CountValues { Day = existingRating.MunualStepsCount.Day + userHistory.ManualStepsCount, Week = existingRating.MunualStepsCount.Week + userHistory.ManualStepsCount, Total = userHistory.ManualStepsCount },
        //            RootCounter = new CountValues { Day = existingRating.RootCounter.Day + userHistory.RootCounter, Week = existingRating.RootCounter.Week + userHistory.RootCounter, Total = userHistory.RootCounter },
        //            TotalEarned = new CountValues { Day = existingRating.TotalEarned.Day + userHistory.TotalEarned, Week = existingRating.TotalEarned.Week + userHistory.TotalEarned, Total = userHistory.TotalEarned },
        //        };
        //        // usersNewRating.Remove(existingRating)
        //        if (userRatingNew.UserId != "0")
        //        {
        //            usersNewRating.Add(userRatingNew);
        //            PersistRatingStatistics(usersNewRating.ToArray());
        //        }

        //    }
        //    // _localRatingDataAccess.CreateOrUpdateUsersRatingStatistics(usersNewRating.ToArray());
        //    // PersistRatingStatistics(usersNewRating.ToArray());
        //}
        //private void PersistRatingStatistics(RatingStatistics[] ratingStatistics)
        //{
        //    var ratingStatisticsChunk = new RatingStatistics[ChunkSize];
        //    var count = 0;
        //    foreach (var userRating in ratingStatistics)
        //    {
        //        if (count < ChunkSize)
        //        {
        //            ratingStatisticsChunk[count] = userRating;
        //            count++;
        //        }
        //        else
        //        {
        //            if (ratingStatisticsChunk.FirstOrDefault() != null)
        //            {
        //                _localRatingDataAccess.CreateOrUpdateUsersRatingStatistics(ratingStatisticsChunk);
        //                ratingStatisticsChunk = new RatingStatistics[ChunkSize];
        //                count = 0;
        //            }
        //        }
        //    }
        //    if (count < ChunkSize)
        //    {
        //        if (ratingStatisticsChunk.FirstOrDefault() != null)
        //        {
        //            _localRatingDataAccess.CreateOrUpdateUsersRatingStatistics(ratingStatisticsChunk);
        //            ratingStatisticsChunk = new RatingStatistics[ChunkSize];
        //            count = 0;
        //        }
        //    }


        //}
        private void CleanOldHistory()
        {
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            this._localNodeRatingDataAccess.CleanOldHistory(periodWeek);
        }
    }
}
