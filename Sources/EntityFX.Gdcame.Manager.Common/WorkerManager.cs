namespace EntityFX.Gdcame.Manager.Common
{
    using System;
    using System.Collections.Generic;

    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class WorkerManager : IWorkerManager
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, IWorker> _workers = new Dictionary<string, IWorker>();

        public WorkerManager(ILogger logger)
        {
            this._logger = logger;
        }

        public void Add(IWorker worker)
        {
            this._workers.Add(worker.Name, worker);
        }

        public void StartAll()
        {
            foreach (var worker in this._workers)
            {
                if (!worker.Value.IsRunOnStart) continue;
                try
                {
                    worker.Value.Run<object>();
                }
                catch (Exception e)
                {
                    this._logger.Error(e);
                }

                this._logger.Info("Worker {0} started", worker.Key);
            }
        }

        public WorkerStatus[] GetWorkersStatus()
        {
            var workersStatusList = new List<WorkerStatus>();
            foreach (var worker in this._workers.Values)
            {
                workersStatusList.Add(new WorkerStatus()
                {
                    IsRunning = worker.IsRunning,
                    Name = worker.Name,
                    Ticks = worker.Ticks,
                    PerfomanceCounters = worker.PerfomanceCounters
                });
            }
            return workersStatusList.ToArray();
        }

        public bool Start<TData>(string workerName, TData workerData) where TData : class
        {
            if (!this._workers.ContainsKey(workerName))
            {
                return false;
            }

            var worker = this._workers[workerName];
            if (worker.IsRunning)
            {
                return false;
            }

            worker.Run(workerData);
            return true;
        }

        public WorkerStatus GetWorkerStatus(string workerName)
        {
            if (!this._workers.ContainsKey(workerName))
            {
                return null;
            }

            var worker = this._workers[workerName];
            return new WorkerStatus() { IsRunning = worker.IsRunning, Name = worker.Name, Ticks = worker.Ticks };
        }
    }
}