using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Service.Mappers;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IMapper<GameData, UserGameCounter>, UserGameCounterMapper>();

            container.RegisterType<IUserDataAccessService, UserDataAccessService>(
               new InterceptionBehavior<PolicyInjectionBehavior>()
               , new Interceptor<InterfaceInterceptor>()
               );
            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<GameDataCachingInterceptionBehavior>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            return container;
        }
    }
}