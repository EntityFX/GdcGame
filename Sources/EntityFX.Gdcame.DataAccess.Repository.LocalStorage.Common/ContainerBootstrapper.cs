namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.Common
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>();
            return container;
        }
    }
}
