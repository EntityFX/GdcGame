using System.Collections.Generic;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Workermanager;

namespace EntityFX.Gdcame.Manager
{
    public class WorkerManager : IWorkerManager
    {
        private readonly ILogger _logger;
        private readonly IList<IWorker> _workers = new List<IWorker>();

        public WorkerManager(ILogger logger)
        {
            _logger = logger;
        }

        public void Add(IWorker worker)
        {
            _workers.Add(worker);
        }

        public void StartAll()
        {
            foreach (var worker in _workers)
            {
                worker.Run();
                _logger.Info("Worker {0} started", worker.Name);
            }
        }

        public WorkerStatus[] GetWorkersStatus()
        {
            var workersStatusList = new List<WorkerStatus>();
            foreach (var worker in _workers)
            {
                workersStatusList.Add(new WorkerStatus()
                {
                    IsRunning = worker.IsRunning,
                    Name = worker.Name
                });
            }
            return workersStatusList.ToArray();
        }
    }
}