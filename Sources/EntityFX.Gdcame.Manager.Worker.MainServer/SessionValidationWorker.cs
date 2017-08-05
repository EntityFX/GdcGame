namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class SessionValidationWorker : WorkerBase, IWorker
    {
        private const int SessionLifeInSeconds = 3600;
        private const int SessionsCheckIntervalInSeconds = 30;

        private readonly ILogger _logger;
        private readonly IGameSessions _gameSessions;
        private readonly ITaskTimer _backgroundSessionsCheckerTimer;
        private Task _backgroundSessionsCheckerTask;
        private object _stdLock = new {};

        public SessionValidationWorker(ILogger logger, IGameSessions gameSessions, ITaskTimerFactory taskTimerFactory)
        {
            this._logger = logger;
            this._gameSessions = gameSessions;
            this._backgroundSessionsCheckerTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(SessionsCheckIntervalInSeconds), this.PerformSessionsCheckTask);
            this.Name = "Session Validation Worker";
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this._backgroundSessionsCheckerTask = this._backgroundSessionsCheckerTimer.Start();
        }


        public override bool IsRunning
        {
            get
            {
                return this._backgroundSessionsCheckerTask.Status == TaskStatus.Running
                       || this._backgroundSessionsCheckerTask.Status == TaskStatus.WaitingForActivation
                       || this._backgroundSessionsCheckerTask.Status == TaskStatus.RanToCompletion;
            }
        }

        private void PerformSessionsCheckTask()
        {
            this.IncrementTick();
            var sw = new Stopwatch();
            sw.Start();
            lock (this._stdLock)
            {
                try
                {
                    foreach (var session in this._gameSessions.Sessions)
                    {
                        if ((DateTime.Now - session.Value.LastActivity) > TimeSpan.FromSeconds(SessionLifeInSeconds))
                        {
                            this._gameSessions.RemoveSession(session.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    this._logger.Error(e);
                    throw;
                }

                PerfomanceCounters["tick"] = sw.ElapsedMilliseconds;
                PerfomanceCounters["games"] = this._gameSessions.Games.Count;
            }
            if (this._logger != null)
                this._logger.Info("Perform sessions {0} check: {1}", this._gameSessions.Sessions.Count, sw.Elapsed);
        }
    }
}