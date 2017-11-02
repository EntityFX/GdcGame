namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.MainServer
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            return container;
        }
    }
}
