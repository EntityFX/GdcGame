using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        readonly IUnityContainer  _container;
        public UnitOfWorkFactory(IUnityContainer container)
        {
            _container = container;
        }


        #region Implementation of IUnitOfWorkFactory

        public IUnitOfWork Create()
        {
            return _container.Resolve<IUnitOfWork>();
        }

        #endregion
    }
}
