using EntityFX.Gdcame.Engine.Worker.RatingServer;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.RatingServerNetCore
{
    internal class HostService : HostServiceBase<CoreStartup>
    {
        private readonly IRuntimeHelper _runtimeHelper;

        public HostService(IRuntimeHelper runtimeHelper, AppConfiguration appConfiguration, IIocContainer container) : base(runtimeHelper, appConfiguration, container)
        {
            _runtimeHelper = runtimeHelper;
        }

        protected override IContainerBootstrapper GetContainerBootstrapper(AppConfiguration appConfiguration, IIocContainer iocContainer)
        {
            return new ContainerBootstrapper<IAuthenticator>(appConfiguration, _runtimeHelper);
        }

        protected override void ConfigureWorkers(IWorkerManager workerManager, IIocContainer container)
        {
            workerManager.Add(container.Resolve<RatingSyncWorker>());
        }

        protected override IIocContainer ConfigureAdvanced(IIocContainer container)
        {
            container.RegisterType<RatingSyncWorker>();

            container.RegisterType<IRuntimeHelper>((scope) => _runtimeHelper);
            container.RegisterType<ILogger>((scope) => new Logger(new NLoggerAdapter(NLog.LogManager.GetLogger("logger"))));
            container.RegisterType<ITaskTimer, GenericTaskTimer>();
            container.RegisterType<ICache, Cache>((scope) => new Cache(), ContainerScope.Singleton);

            return container;
        }
    }
}