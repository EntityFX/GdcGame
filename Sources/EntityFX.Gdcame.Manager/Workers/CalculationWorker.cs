using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;

namespace EntityFX.Gdcame.Manager.MainServer.Workers
{
    public class CalculationWorker : IWorker
    {
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private readonly GameSessions _gameSessions;
        private readonly PerformanceInfo _performanceInfo;
        private object _stdLock = new {};
        private readonly ITaskTimer _backgroundPerformAutomaticStepsTimer;
        private Task _backgroundPerformAutomaticStepsTask;
        public CalculationWorker(ILogger logger, GameSessions gameSessions, PerformanceInfo performanceInfo, ITaskTimerFactory taskTimerFactory)
        {
            _logger = logger;
            _gameSessions = gameSessions;
            _performanceInfo = performanceInfo;
            _backgroundPerformAutomaticStepsTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(1), PerformAutomaticSteps);

            Name = "Games Calculation worker";
        }

        public void Run()
        {
            _backgroundPerformAutomaticStepsTask = _backgroundPerformAutomaticStepsTimer.Start();
        }

        public bool IsRunning
        {
            get
            {
                return _backgroundPerformAutomaticStepsTask.Status == TaskStatus.Running
                       || _backgroundPerformAutomaticStepsTask.Status == TaskStatus.WaitingForActivation
                       || _backgroundPerformAutomaticStepsTask.Status == TaskStatus.RanToCompletion;
            }
        }

        public string Name { get; private set; }

        private void PerformAutomaticSteps()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (_stdLock)
            {
                try
                {
                    // Parallel.ForEach(UserGamesStorage.Values, game => game.PerformAutoStep());
                    var calulateGamesChunk = new IGame[ChunkSize];
                    var count = 0;
                    foreach (var game in _gameSessions.Games.Values)
                    {
                        if (count < ChunkSize)
                        {
                            calulateGamesChunk[count] = game;
                            count++;
                        }
                        else
                        {
                            Task.Factory.StartNew(_ =>
                            {
                                var games = (IGame[])_;
                                for (int i = 0; i < games.Length; i++)
                                {
                                    games[i].PerformAutoStep();
                                }
                            }, calulateGamesChunk);
                            calulateGamesChunk = new IGame[ChunkSize];
                            count = 0;
                        }
                    }
                    if (count < ChunkSize)
                    {
                        Task.Factory.StartNew(_ =>
                        {
                            var games = (IGame[])_;
                            for (int i = 0; i < games.Length; i++)
                            {
                                if (games[i] != null)
                                {
                                    games[i].PerformAutoStep();
                                }
                            }
                        }, calulateGamesChunk);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    throw;
                }
                _performanceInfo.CalculationsPerCycle = sw.Elapsed;
            }

            if (_logger != null)
                _logger.Info("Perform steps for {0} active games and {1} sessions: {2}", _gameSessions.Games.Count, _gameSessions.Sessions.Count, sw.Elapsed);
        }
    }
}