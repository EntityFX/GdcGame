using System;

namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>();
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>();
            container.RegisterType<GameRepositoryFacade>((resolver) => new GameRepositoryFacade
            {
                CountersRepository = resolver.Resolve<ICountersRepository>(),
                CustomRuleRepository = resolver.Resolve<ICustomRuleRepository>(),
                FundsDriverRepository = resolver.Resolve<IItemRepository>()
            }, ContainerScope.Instance, String.Empty);

            container.RegisterType<IRatingDataAccess, LocalNodeRatingDataAccess>();
            container.RegisterType<ILocalNodeRatingDataAccess, LocalNodeRatingDataAccess>();
            return container;
        }
    }
}