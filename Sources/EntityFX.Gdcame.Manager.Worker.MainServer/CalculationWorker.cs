namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Kernel.Contract;

    public class CalculationWorker : WorkerBase, IWorker
    {
        private const int ChunkSize = 500;
        private readonly ILogger _logger;
        private readonly IGameSessions _gameSessions;
        private readonly GamePerformanceInfo _performanceInfo;
        private object _stdLock = new {};
        private readonly ITaskTimer _backgroundPerformAutomaticStepsTimer;
        private Task _backgroundPerformAutomaticStepsTask;
        public CalculationWorker(ILogger logger, IGameSessions gameSessions, GamePerformanceInfo performanceInfo, ITaskTimerFactory taskTimerFactory)
        {
            this._logger = logger;
            this._gameSessions = gameSessions;
            this._performanceInfo = performanceInfo;
            this._backgroundPerformAutomaticStepsTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(1), this.PerformAutomaticSteps);

            this.Name = "Games Calculation worker";
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this._backgroundPerformAutomaticStepsTask = this._backgroundPerformAutomaticStepsTimer.Start();
        }

        public override bool IsRunning
        {
            get
            {
                return this._backgroundPerformAutomaticStepsTask.Status == TaskStatus.Running
                       || this._backgroundPerformAutomaticStepsTask.Status == TaskStatus.WaitingForActivation
                       || this._backgroundPerformAutomaticStepsTask.Status == TaskStatus.RanToCompletion;
            }
        }



        private void PerformAutomaticSteps()
        {
            this.IncrementTick();
            var sw = new Stopwatch();
            sw.Start();
            lock (this._stdLock)
            {
                try
                {
                    // Parallel.ForEach(UserGamesStorage.Values, game => game.PerformAutoStep());
                    var calulateGamesChunk = new IGame[ChunkSize];
                    var count = 0;
                    foreach (var game in this._gameSessions.Games.Values)
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
                    this._logger.Error(e);
                    throw;
                }
                this._performanceInfo.CalculationsPerCycle = sw.Elapsed;

                PerfomanceCounters["tick"] = sw.Elapsed.TotalMilliseconds;
                PerfomanceCounters["perf"] = sw.Elapsed.TotalMilliseconds / this._gameSessions.Games.Count;
                PerfomanceCounters["games"] = this._gameSessions.Games.Count;
            }

            if (this._logger != null)
                this._logger.Info("Perform steps for {0} active games and {1} sessions: {2}", this._gameSessions.Games.Count, this._gameSessions.Sessions.Count, sw.Elapsed);
        }
    }
}