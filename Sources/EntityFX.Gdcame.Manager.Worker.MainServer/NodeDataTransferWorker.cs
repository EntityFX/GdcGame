namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    public class NodeDataTransferWorker : WorkerBase
    {
        private readonly IServerDataAccessService serverDataAccessService;

        private readonly IUserDataAccessService userDataAccessService;

        /// <summary>
        /// The _data transfer task.
        /// </summary>
        private Task _dataTransferTask;
        private readonly CancellationTokenSource cancellationTaskToken = new CancellationTokenSource();

        public NodeDataTransferWorker(IServerDataAccessService serverDataAccessService, IUserDataAccessService userDataAccessService)
        {
            this.serverDataAccessService = serverDataAccessService;
            this.userDataAccessService = userDataAccessService;
            this.Name = "NodeDataTransferWorker";
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this._dataTransferTask =
                Task.Factory.StartNew(
                    a => this.PerformBackgroundPersisting(data as string[]),
                    TaskCreationOptions.LongRunning,
                    this.cancellationTaskToken.Token).ContinueWith(
                    c =>
                        {
                            if (c.IsFaulted)
                            {
                                // Logger.Error(c.Exception.InnerException);
                            }
                        });
        }

        private void PerformBackgroundPersisting(string[] servers)
        {
            this.IncrementTick();
            //_serverDataAccessService.UpdateServers(newServersList);
        }

        public override bool IsRunOnStart
        {
            get
            {
                return false;
            }
        }

        public override bool IsRunning
        {
            get
            {
                return this._dataTransferTask != null && (this._dataTransferTask.Status == TaskStatus.Running
                                                          || this._dataTransferTask.Status == TaskStatus.WaitingForActivation
                                                          || this._dataTransferTask.Status == TaskStatus.RanToCompletion);
            }
        }
    }
}
