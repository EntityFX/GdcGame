namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Service.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<GameRepositoryFacade>(new InjectionFactory(
                _ => new GameRepositoryFacade
                {
                    CountersRepository = container.Resolve<ICountersRepository>(),
                    CustomRuleRepository = container.Resolve<ICustomRuleRepository>(),
                    FundsDriverRepository = container.Resolve<IItemRepository>()
                }
                )
                );

            container.RegisterType<IRatingDataAccess, LocalNodeRatingDataAccess>();
            container.RegisterType<ILocalNodeRatingDataAccess, LocalNodeRatingDataAccess>();
            return container;
        }
    }
}