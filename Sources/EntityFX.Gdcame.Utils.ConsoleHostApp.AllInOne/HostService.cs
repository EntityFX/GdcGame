using System;
using System.Configuration;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Manager.MainServer.Workers;

using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServer
{
    using EntityFX.Gdcame.Engine.Worker.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Utils.Common;
    using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer;


    internal class HostService : HostServiceBase<Startup>
    {
        private readonly IRuntimeHelper _runtimeHelper;

        protected override void ConfigureWorkers(IWorkerManager workerManager, IIocContainer container)
        {
            workerManager.Add(container.Resolve<CalculationWorker>());
            workerManager.Add(container.Resolve<PersistenceWorker>());
            workerManager.Add(container.Resolve<SessionValidationWorker>());
            workerManager.Add(container.Resolve<RatingCalculationWorker>());
            workerManager.Add(container.Resolve<NodeDataTransferWorker>());
        }

        protected override IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration, IIocContainer iocContainer)
        {
            return new ContainerBootstrapper(appConfiguration, _runtimeHelper);
        }

        public HostService(IRuntimeHelper runtimeHelper, AppConfiguration appConfiguration, IIocContainer container) : base(runtimeHelper, appConfiguration, container)
        {
            _runtimeHelper = runtimeHelper;
        }

        protected override IIocContainer ConfigureAdvanced(IIocContainer container)
        {
            container.RegisterType<CalculationWorker>();
            container.RegisterType<PersistenceWorker>();
            container.RegisterType<SessionValidationWorker>();
            container.RegisterType<RatingCalculationWorker>();
            container.RegisterType<NodeDataTransferWorker>();

            container.RegisterType<IRuntimeHelper>((scope) => new RuntimeHelper());
            container.RegisterType<ILogger>((scope) => new Logger(new NLoggerAdapter(NLog.LogManager.GetLogger("logger"))));
            container.RegisterType<ITaskTimer, GenericTaskTimer>();
            container.RegisterType<ICache, Cache>((scope) => new Cache(), ContainerScope.Singleton);

            return container;
        }
    }
}