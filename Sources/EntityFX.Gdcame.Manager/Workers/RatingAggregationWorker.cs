using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Server;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Workers
{
    public class RatingAggregationWorker
    {
        private const int TimeSaveInSeconds = 30;
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private GameSessions _gameSessions;
        private readonly IServerDataAccessService _serverDataAccessService;
        private readonly ITaskTimer _backgroundSaveHistoryCheckerTimer;
        private Task _backgroundSaveHistoryCheckerTask;
        private object _stdLock = new { };

        public RatingAggregationWorker(ILogger logger, GameSessions gameSessions, IServerDataAccessService serverDataAccessService, ITaskTimerFactory taskTimerFactory)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _serverDataAccessService = serverDataAccessService;
            _backgroundSaveHistoryCheckerTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(TimeSaveInSeconds), SaveHistoryCheckTask);
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
