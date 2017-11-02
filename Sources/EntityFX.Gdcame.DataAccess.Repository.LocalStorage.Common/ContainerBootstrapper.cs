namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.Common
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>();
            return container;
        }
    }
}
