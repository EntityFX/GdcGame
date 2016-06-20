using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Service;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Service.Mappers;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using System.Data.Entity;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<DbContext, EconomicsArcadeDbContext>(new InjectionConstructor("name=EconomicsArcadeDbContext"));
            container.RegisterType<IQueryBuilder, QueryBuilder>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IMapper<User, UserEntity>, UserEntityMapper>();
            return container;
        }
    }
}
