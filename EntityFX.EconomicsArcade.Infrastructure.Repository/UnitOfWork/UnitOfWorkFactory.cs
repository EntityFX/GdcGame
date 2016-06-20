using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        IUnityContainer  _container;
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
