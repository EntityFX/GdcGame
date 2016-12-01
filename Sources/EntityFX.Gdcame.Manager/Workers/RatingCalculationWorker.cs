﻿using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Manager.Workers
{
    public class RatingCalculationWorker : IWorker
    {
        private const int TimeSaveInSeconds = 30;       
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private GameSessions _gameSessions;
        private readonly ILocalRatingDataAccess _localRatingDataAccess;
        private readonly TaskTimer _backgroundSaveHistoryCheckerTimer;
        private Task _backgroundSaveHistoryCheckerTask;
        private object _stdLock = new { };

        public RatingCalculationWorker(ILogger logger, GameSessions gameSessions, ILocalRatingDataAccess localRatingDataAccess)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _localRatingDataAccess = localRatingDataAccess;
            _backgroundSaveHistoryCheckerTimer = new TaskTimer(TimeSpan.FromSeconds(TimeSaveInSeconds), SaveHistoryCheckTask);
            Name = "Rating Calculation Worker";
        }

        public void Run()
        {
            _backgroundSaveHistoryCheckerTask = _backgroundSaveHistoryCheckerTimer.Start();
        }

        public string Name { get; }

        public bool IsRunning
        {
            get
            {
                return _backgroundSaveHistoryCheckerTask.Status == TaskStatus.Running
                       || _backgroundSaveHistoryCheckerTask.Status == TaskStatus.WaitingForActivation
                       || _backgroundSaveHistoryCheckerTask.Status == TaskStatus.RanToCompletion;
            }
        }

        private void SaveHistoryCheckTask()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {
                    SaveHistory();
                    RecalculationRatingStatisticsAllUsers();
                    CleanOldHistory();
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
            }
            if (_logger != null)
                _logger.Info("Perform sessions {0} check: {1}", _gameSessions.Sessions.Count, sw.Elapsed);
        }

        private void SaveHistory()
        {           
            var ratingHistoryChunk = new RatingHistory[ChunkSize];
            var count = 0;
            RatingHistory userRatingHistory;
            foreach (var game in _gameSessions.Games)
            {
                if (count < ChunkSize)
                {
                    userRatingHistory = new RatingHistory
                    {
                        UserId = game.Key,
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
                        _localRatingDataAccess.PersistUsersRatingHistory(ratingHistoryChunk);
                        ratingHistoryChunk = new RatingHistory[ChunkSize];
                        count = 0;
                    }
                }
            }
            if (count < ChunkSize)
            {
                if (ratingHistoryChunk.FirstOrDefault() != null)
                {
                    _localRatingDataAccess.PersistUsersRatingHistory(ratingHistoryChunk);
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
        private void RecalculationRatingStatisticsAllUsers()
        {
            int ChunkSizeUsers = 100;
            List<string> userId = new List<string>();
            var count = 0;
            foreach (var game in _gameSessions.Games)
            {
                if (count < ChunkSizeUsers)
                {
                    userId.Add(game.Key);
                    count++;
                }
                else
                {
                    if (userId.FirstOrDefault() != null)
                    {
                        RecalculationRatingStatisticsChunkUsers(userId);
                        userId = new List<string>();
                        count = 0;
                    }
                }
            }
            if (count < ChunkSizeUsers)
            {
                if (userId.FirstOrDefault() != null)
                {
                    RecalculationRatingStatisticsChunkUsers(userId);
                    userId = new List<string>();
                    count = 0;
                }
            }
        }

        private void RecalculationRatingStatisticsChunkUsers(List<string> userId)
        {
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            //TimeSpan periodTotal = new TimeSpan(7, 0, 0, 0);
            List<RatingStatistics> usersNewRating = new List<RatingStatistics>();
            var ratingHistoryDay = _localRatingDataAccess.ReadHistoryWithUsersIds(userId.ToArray(), periodWeek);
            //var usersId=
            ratingHistoryDay.OrderBy(t => t.Data);
            List<RatingHistory> userHistoreTotal = new List<RatingHistory>();

            foreach (var user in userId)
            {
                foreach (var oneUserHistory in ratingHistoryDay)
                    if (oneUserHistory.UserId == user)
                    {
                        userHistoreTotal.Add(oneUserHistory);
                    }
                usersNewRating.Add(RecalculationRatingStatisticsOneUser(userHistoreTotal));
            }
            _localRatingDataAccess.CreateOrUpdateUsersRatingStatistics(usersNewRating.ToArray());

        }

        private RatingStatistics RecalculationRatingStatisticsOneUser(List<RatingHistory> userHistoreTotal)
        {

            var lastDataUser = userHistoreTotal.OrderByDescending(t => t.Data).FirstOrDefault();
            //add the values for the Total
            RatingStatistics userRatingNew = new RatingStatistics
            {
                UserId = lastDataUser.UserId,
                MunualStepsCount = new CountValues { Day = lastDataUser.ManualStepsCount, Week = lastDataUser.ManualStepsCount, Total = lastDataUser.ManualStepsCount },
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
                userRatingNew.MunualStepsCount.Day = lastDataUser.ManualStepsCount - firstDataUserDay.ManualStepsCount;
                userRatingNew.RootCounter.Day = lastDataUser.RootCounter - firstDataUserDay.RootCounter;
                userRatingNew.TotalEarned.Day = lastDataUser.TotalEarned - firstDataUserDay.TotalEarned;

                //add the values for the 7 day
                TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
                var userHistoryWeek = userHistoreTotal.FindAll(t => t.Data >= lastDataUser.Data.Subtract(periodWeek)).OrderBy(t => t.Data);
                var firstDataUserWeek = userHistoryDay.FirstOrDefault();
                userRatingNew.MunualStepsCount.Week = lastDataUser.ManualStepsCount - firstDataUserDay.ManualStepsCount;
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
            _localRatingDataAccess.CleanOldHistory(periodWeek);
        }
    }
}
