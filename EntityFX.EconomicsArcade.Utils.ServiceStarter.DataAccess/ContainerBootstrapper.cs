using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using System.Data.Entity;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Mappers;
using EntityFX.EconomicsArcade.DataAccess.Service;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.Counetrs;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Service.Mappers;

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

            container.RegisterType<IQuery<GetUserByIdCriterion, UserEntity>, GetUserByIdQuery>();
            container.RegisterType<IQuery<GetUserByNameCriterion, UserEntity>, GetUserByNameQuery>();
            container.RegisterType<IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>, GetAllUsersQuery>();
            container.RegisterType<IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>, GetAllFundsDriversQuery>();
            container.RegisterType<IQuery<GetAllCountersCriterion, IEnumerable<CounterEntity>>, GetAllCountersQuery>();
            container.RegisterType<IQuery<GetUserGameCounterByIdCriterion, UserGameCounterEntity>, GetUserGameCounterByIdQuery>();
            container.RegisterType < IQuery<GetUserCountersByUserIdCriterion, IEnumerable<UserCounterEntity>>, GetUserCountersByUserIdQuery>();

            container.RegisterType<IMapper<User, UserEntity>, UserEntityMapper>();
            container.RegisterType<IMapper<UserEntity, User>, UserContractMapper>();
            container.RegisterType<IMapper<FundsDriverEntity, FundsDriver>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IncrementorEntity, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterEntity, CounterBase>, CountersContractMapper>();
            container.RegisterType<IMapper<UserGameCounter, UserGameCounterEntity>, UserGameCounterEntityMapper>();
            container.RegisterType<IMapper<UserGameCounterEntity, UserGameCounter>, UserGameCounterContractMapper>();
            container.RegisterType<IMapper<CounterBase, UserCounterEntity>, UserCounterEntityMapper>();
            container.RegisterType<IMapper<UserCounterEntity, CounterBase>, UserCounterContractMapper>();

            container.RegisterType<IMapper<GameData, UserGameCounter>, UserGameCounterMapper>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IFundsDriverRepository, FundsDriverRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<IUserGameCounterRepository, UserGameCounterRepository>();
            container.RegisterType<IUserCounterRepository, UserCounterRepository>();

            container.RegisterType<IUserDataAccessService, UserDataAccessService>();
            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessService>();
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessService>();
            return container;
        }
    }
}
