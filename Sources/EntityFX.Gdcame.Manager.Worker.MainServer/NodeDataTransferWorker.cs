namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
    using DataAccess.Contract.MainServer.GameData;

    public class NodeDataTransferWorker : WorkerBase
    {
        private readonly IServerDataAccessService serverDataAccessService;

        private readonly IUserDataAccessService userDataAccessService;

        private readonly IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService;

        /// <summary>
        /// The _data transfer task.
        /// </summary>
        private Task _dataTransferTask;
        private readonly CancellationTokenSource cancellationTaskToken = new CancellationTokenSource();

        public NodeDataTransferWorker(IServerDataAccessService serverDataAccessService, IUserDataAccessService userDataAccessService, IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService)
        {
            this.serverDataAccessService = serverDataAccessService;
            this.userDataAccessService = userDataAccessService;
            this.gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            this.Name = "NodeDataTransferWorker";
        }

        public void TransferDataOnServer(string[] servers, string[] userIds)
        {
            var userSavedData = this.gameDataRetrieveDataAccessService.GetStoredGameData(userIds);
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
                                //Log.Error(c.Exception.InnerException);
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
                return this._dataTransferTask != null && (this._dataTransferTask.Status == TaskStatus.Running);
            }
        }
    }
}
