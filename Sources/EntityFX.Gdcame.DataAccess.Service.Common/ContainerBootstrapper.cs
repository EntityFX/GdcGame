namespace EntityFX.Gdcame.DataAccess.Service.Common
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            container.RegisterType<IUserDataAccessService, UserDataAccessService>();

            container.RegisterType<IServerDataAccessService, ServerDataAccessService>();

            return container;
        }
    }
}