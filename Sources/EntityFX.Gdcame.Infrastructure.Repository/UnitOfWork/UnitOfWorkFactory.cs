using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IIocContainer _container;

        public UnitOfWorkFactory(IIocContainer container)
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