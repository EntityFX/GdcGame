using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Engine.Worker.RatingServer
{
    using System.Diagnostics;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating;
    using EntityFX.Gdcame.Engine.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class RatingSyncWorker : WorkerBase, IWorker
    {
        private readonly IServerDataAccessService serverDataAccessService;

        private readonly IGlobalRatingDataAccess globalRatingDataAccess;

        private readonly INodeRatingClientFactory nodeRatingClientFactory;

        private readonly ITaskTimer backgroundRatingSyncTimer;

        private Task backgroundRatingSyncTask;

        public RatingSyncWorker(IServerDataAccessService serverDataAccessService, IGlobalRatingDataAccess globalRatingDataAccess, INodeRatingClientFactory nodeRatingClientFactory, ITaskTimerFactory taskTimerFactory)
        {
            this.serverDataAccessService = serverDataAccessService;
            this.globalRatingDataAccess = globalRatingDataAccess;
            this.nodeRatingClientFactory = nodeRatingClientFactory;
            this.backgroundRatingSyncTimer = taskTimerFactory.Build(TimeSpan.FromSeconds(10), Tick, false);
            this.Name = "Rating Sync Worker";
        }

        private void Tick()
        {
            this.IncrementTick();
            var sw = new Stopwatch();
            sw.Start();
            var servers = this.serverDataAccessService.GetServers();
            var retrieveClients = servers.Select(server => this.nodeRatingClientFactory.BuildClient(new Uri(string.Format("http://{0}:{1}", server.Address, 9001)))).ToList();
            var tasks = retrieveClients.Select(c => Task.Run(() => c.GetRaiting())).ToArray();
            Task.WaitAll(tasks);
            var ratings = Task.WhenAll(tasks).Result;
            this.globalRatingDataAccess.DropStatistics();
            foreach (var topRatingStatistics in ratings)
            {
               this.globalRatingDataAccess.CreateOrUpdateUsersRatingStatistics(topRatingStatistics); 
            }
            PerfomanceCounters["tick"] = sw.Elapsed.TotalMilliseconds;
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this.backgroundRatingSyncTask = this.backgroundRatingSyncTimer.Start();
        }

        public override bool IsRunning
        {
            get
            {
                return this.backgroundRatingSyncTask.Status == TaskStatus.Running;
            }
        }
    }
}
