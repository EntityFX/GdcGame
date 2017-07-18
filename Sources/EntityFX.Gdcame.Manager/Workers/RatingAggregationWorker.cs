using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Server;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

namespace EntityFX.Gdcame.Manager.MainServer.Workers
{
    public class RatingAggregationWorker: WorkerBase, IWorker
    {
        private const int TimeSaveInSeconds = 30;
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private readonly IServerDataAccessService _serverDataAccessService;
        private readonly ITaskTimer _backgroundSaveHistoryCheckerTimer;
        private Task _backgroundSaveHistoryCheckerTask;
        private object _stdLock = new { };

        public RatingAggregationWorker(ILogger logger, IServerDataAccessService serverDataAccessService, ITaskTimerFactory taskTimerFactory)
        {
            _logger = logger;
            _serverDataAccessService = serverDataAccessService;
            _backgroundSaveHistoryCheckerTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(TimeSaveInSeconds), SaveHistoryCheckTask);
            Name = "Rating Aggregation Worker";
        }

        public override void Run()
        {
            _backgroundSaveHistoryCheckerTask = _backgroundSaveHistoryCheckerTimer.Start();
        }


        public override bool IsRunning
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
        }
    }
}
