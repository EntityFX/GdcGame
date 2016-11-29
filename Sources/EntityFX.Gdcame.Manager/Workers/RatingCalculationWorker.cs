using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Manager.Workers
{
    public class RatingCalculationWorker
    {
        private const int TimeSaveInSeconds = 3600;
        private const int SessionsCheckIntervalInSeconds = 30;

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
                    foreach (var session in _gameSessions.Games)
                    {
                        
                        //if ((DateTime.Now - session.Value.LastActivity) > TimeSpan.FromSeconds(SessionLifeInSeconds))
                        //{
                        //    _gameSessions.RemoveSession(session.Key);
                        //}
                    }
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
            RatingHistory userRatingHistory;
            foreach (var game in _gameSessions.Games)
            {

                userRatingHistory = new RatingHistory
                {
                    UserId = game.Key,
                    ManualStepsCount = game.Value.ManualStepNumber,
                    RootCounter = (Int32)game.Value.GameCash.RootCounter.Value,
                    Data = DateTime.Now,
                    TotalEarned = game.Value.GameCash.TotalEarned,
                };
                _localRatingDataAccess.PersistRatingHistory(userRatingHistory);
            };
        }
        private void RecalculationRatingStatistics()
        {
            TimeSpan periodDay = new TimeSpan(1, 0, 0, 0);
            TimeSpan periodWeek = new TimeSpan(7, 0, 0, 0);
            //TimeSpan periodTotal = new TimeSpan(7, 0, 0, 0);
            List<string> userIds = new List<string>();
            Dictionary<string, RatingStatistics> usersNewRating = new Dictionary<string, RatingStatistics>();
            foreach (var game in _gameSessions.Games)
            {
                userIds.Add(game.Key);
            }
            var ratingHistoryDay= _localRatingDataAccess.ReadHistoryWithUsersIds(userIds.ToArray(),periodDay);
            //var usersId=
            ratingHistoryDay.OrderBy(t => t.Data);

            RatingStatistics userRating = new RatingStatistics
            {
                UserId = "0",
                MunualStepsCount = new CountValues { Day = 0, Week = 0, Total = 0 },
                RootCounter = new CountValues { Day = 0, Week = 0, Total = 0 },
                TotalEarned = new CountValues { Day = 0, Week = 0, Total = 0 },
            };

            foreach (var userHistory in ratingHistoryDay)
            {
                RatingStatistics userRatingNew = new RatingStatistics
                {
                    UserId = userHistory.UserId,
                    MunualStepsCount = new CountValues { Day = userRating.MunualStepsCount.Day + userHistory.ManualStepsCount, Week = userRating.MunualStepsCount.Week + userHistory.ManualStepsCount, Total = userRating.MunualStepsCount.Total + userHistory.ManualStepsCount },
                    RootCounter = new CountValues { Day = userRating.RootCounter.Day + userHistory.RootCounter, Week = userRating.RootCounter.Week + userHistory.RootCounter, Total = userRating.RootCounter.Total + userHistory.RootCounter },
                    TotalEarned = new CountValues { Day = userRating.TotalEarned.Day + userHistory.TotalEarned, Week = userRating.TotalEarned.Week + userHistory.TotalEarned, Total = userRating.TotalEarned.Total + userHistory.TotalEarned },
                };
                userRating = userRatingNew;
                usersNewRating.First(t => t.Key == userRatingNew.UserId);
              //  usersNewRating.
                usersNewRating.Add(userRatingNew.UserId, userRatingNew);
            }
           // var ratingHistoryWeek = _localRatingDataAccess.ReadHistoryWithUsersIds(userIds.ToArray(), periodDay);
           // var ratingHistoryTotal = _localRatingDataAccess.ReadHistoryWithUsersIds(userIds.ToArray(), periodDay);



          ///  _localRatingDataAccess.CreateOrUpdateUsersRatingStatistics();
        }
        private void CleanOldHistory()
        {
        }
    }
}
