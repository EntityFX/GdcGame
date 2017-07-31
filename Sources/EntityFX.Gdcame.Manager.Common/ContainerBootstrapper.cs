using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;


namespace EntityFX.Gdcame.Manager.Common
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            /////

            container.RegisterType<IMapperFactory, MapperFactory>();


            container.RegisterType<IRatingManager, RatingManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            container.RegisterType<IServerManager, ServerManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());


            container.RegisterInstance<IWorkerManager>(container.Resolve<WorkerManager>());

            return container;
        }
    }
}