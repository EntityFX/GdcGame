using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common
{
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Queries.User;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            container.RegisterType<IQuery<GetUserByIdCriterion, UserEntity>, GetUserByIdQuery>();
            container.RegisterType<IQuery<GetUserByNameCriterion, UserEntity>, GetUserByNameQuery>();
            container.RegisterType<IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>, GetAllUsersQuery>();
            container.RegisterType<IQuery<GetUsersBySearchStringCriterion, IEnumerable<UserEntity>>, GetUsersBySearchStringQuery>();
            container.RegisterType<IQuery<GetUsersByOffsetCriterion, IEnumerable<UserEntity>>, GetUsersByOffsetQuery>();

            container.RegisterType<IUserRepository, UserRepository>();
            return container;
        }
    }
}
