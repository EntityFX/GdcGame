using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Contract.Common.WorkerManager
{
    public interface IWorkerManager
    {
        void Add(IWorker worker);
        //TODO: add AddAndStart(IWorker worker) that will start NodeDataUpdateJob. Implement new NodeDataUpdateJob:IWorker. Worker will have GameSessions reference. Also it will send usersData using TransferUsersData(IList<GameWithUserId> gamesWithUserIds) method on remote node.

        void StartAll();

        bool Start<TData>(string workerName, TData workerData) where TData : class;

        WorkerStatus[] GetWorkersStatus();

        WorkerStatus GetWorkerStatus(string workerName);
    }
}