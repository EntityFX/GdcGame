namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.MainServer
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            return container;
        }
    }
}
