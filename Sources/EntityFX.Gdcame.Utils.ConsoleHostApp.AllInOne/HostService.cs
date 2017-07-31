using System.Configuration;

using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Manager.MainServer.Workers;

using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServer
{
    using EntityFX.Gdcame.Engine.Worker.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Utils.Common;
    using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer;
    internal class HostService : HostServiceBase<CoreStartup>
    {

        protected override void ConfigureWorkers(IWorkerManager workerManager, IUnityContainer container)
        {
            workerManager.Add(container.Resolve<CalculationWorker>());
            workerManager.Add(container.Resolve<PersistenceWorker>());
            workerManager.Add(container.Resolve<SessionValidationWorker>());
            workerManager.Add(container.Resolve<RatingCalculationWorker>());
            workerManager.Add(container.Resolve<NodeDataTransferWorker>());
        }

        protected override IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration)
        {
            return new ContainerBootstrapper(appConfiguration);
        }
    }
}