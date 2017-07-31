﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

namespace EntityFX.Gdcame.Manager.MainServer.Workers
{
    public class SessionValidationWorker : WorkerBase, IWorker
    {
        private const int SessionLifeInSeconds = 3600;
        private const int SessionsCheckIntervalInSeconds = 30;

        private readonly ILogger _logger;
        private readonly GameSessions _gameSessions;
        private readonly ITaskTimer _backgroundSessionsCheckerTimer;
        private Task _backgroundSessionsCheckerTask;
        private object _stdLock = new {};

        public SessionValidationWorker(ILogger logger, GameSessions gameSessions, ITaskTimerFactory taskTimerFactory)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _backgroundSessionsCheckerTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(SessionsCheckIntervalInSeconds), PerformSessionsCheckTask);
            Name = "Session Validation Worker";
        }

        public override void Run<TData>(TData data = default(TData))
        {
            _backgroundSessionsCheckerTask = _backgroundSessionsCheckerTimer.Start();
        }


        public override bool IsRunning
        {
            get
            {
                return _backgroundSessionsCheckerTask.Status == TaskStatus.Running
                       || _backgroundSessionsCheckerTask.Status == TaskStatus.WaitingForActivation
                       || _backgroundSessionsCheckerTask.Status == TaskStatus.RanToCompletion;
            }
        }

        private void PerformSessionsCheckTask()
        {
            IncrementTick();
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {
                    foreach (var session in _gameSessions.Sessions)
                    {
                        if ((DateTime.Now - session.Value.LastActivity) > TimeSpan.FromSeconds(SessionLifeInSeconds))
                        {
                            _gameSessions.RemoveSession(session.Key);
                        }
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
    }
}