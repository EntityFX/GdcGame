using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Workers
{
    public class SessionValidationWorker : IWorker
    {
        private const int SessionLifeInSeconds = 3600;
        private const int SessionsCheckIntervalInSeconds = 30;

        private readonly ILogger _logger;
        private GameSessions _gameSessions;
        private readonly TaskTimer _backgroundSessionsCheckerTimer;
        private Task _backgroundSessionsCheckerTask;
        private object _stdLock = new {};

        public SessionValidationWorker(ILogger logger, GameSessions gameSessions)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _backgroundSessionsCheckerTimer = new TaskTimer(TimeSpan.FromSeconds(SessionsCheckIntervalInSeconds), PerformSessionsCheckTask);
        }

        public void Run()
        {
            _backgroundSessionsCheckerTask = _backgroundSessionsCheckerTimer.Start();
        }

        private void PerformSessionsCheckTask()
        {
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