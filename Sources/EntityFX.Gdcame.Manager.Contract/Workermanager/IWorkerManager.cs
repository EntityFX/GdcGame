using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Contract.Workermanager
{
    public interface IWorkerManager
    {
        void Add(IWorker worker);

        void StartAll();

        WorkerStatus[] GetWorkersStatus();
    }
}