using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;


namespace EntityFX.Gdcame.Manager.Common
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            /////

            container.RegisterType<IMapperFactory, MapperFactory>();


            container.RegisterType<IRatingManager, RatingManager>();

            container.RegisterType<IServerManager, ServerManager>();

            container.RegisterType<IWorkerManager, WorkerManager>((scope) => new WorkerManager(scope.Resolve<ILogger>()), ContainerScope.Singleton);

            return container;
        }
    }
}