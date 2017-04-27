using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Server;

namespace EntityFX.Gdcame.Manager.Workers
{
    public class RatingAggregationWorker
    {
        private const int TimeSaveInSeconds = 30;
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private GameSessions _gameSessions;
        private readonly IServerDataAccessService _serverDataAccessService;
        private readonly TaskTimer _backgroundSaveHistoryCheckerTimer;
        private Task _backgroundSaveHistoryCheckerTask;
        private object _stdLock = new { };

        public RatingAggregationWorker(ILogger logger, GameSessions gameSessions, IServerDataAccessService serverDataAccessService)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _serverDataAccessService = serverDataAccessService;
            _backgroundSaveHistoryCheckerTimer = new TaskTimer(TimeSpan.FromSeconds(TimeSaveInSeconds), SaveHistoryCheckTask);
            Name = "Rating Aggregation Worker";
        }

        public void Run()
        {
            _backgroundSaveHistoryCheckerTask = _backgroundSaveHistoryCheckerTimer.Start();
        }

        public string Name { get; private set; }

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
                   //
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
    }
}
