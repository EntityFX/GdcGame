namespace EntityFX.Gdcame.DataAccess.Service.RatingServer
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Rating;
    using EntityFX.Gdcame.DataAccess.Contract.RatingServer.Rating;
    using EntityFX.Gdcame.Infrastructure.Common;

    using Microsoft.Practices.Unity;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {

            container.RegisterType<IRatingDataAccess, GlobalRatingDataAccess>();
            container.RegisterType<IGlobalRatingDataAccess, GlobalRatingDataAccess>();
            return container;
        }
    }
}