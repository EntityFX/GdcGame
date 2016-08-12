using System.Collections.Generic;
using System.Data.Entity;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.CustomRule;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.User;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Mappers;
using EntityFX.Gdcame.DataAccess.Repository.Queries.Counetrs;
using EntityFX.Gdcame.DataAccess.Repository.Queries.CustomRule;
using EntityFX.Gdcame.DataAccess.Repository.Queries.FundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Queries.User;
using EntityFX.Gdcame.DataAccess.Repository.Queries.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Queries.UserRating;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.Query;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<DbContext, EconomicsArcadeDbContext>(new InjectionConstructor("name=EconomicsArcadeDbContext"));
            container.RegisterType<IQueryBuilder, QueryBuilder>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>();

            container.RegisterType<IQuery<GetUserByIdCriterion, UserEntity>, GetUserByIdQuery>();
            container.RegisterType<IQuery<GetUserByNameCriterion, UserEntity>, GetUserByNameQuery>();
            container.RegisterType<IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>, GetAllUsersQuery>();
            container.RegisterType<IQuery<GetAllUsersRatingsCriterion, IEnumerable<UserRating>>, GetAllUsersRatingsQuery>();
            container.RegisterType<IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>, GetAllFundsDriversQuery>();
            container.RegisterType<IQuery<GetAllCountersCriterion, IEnumerable<CounterEntity>>, GetAllCountersQuery>();

            container.RegisterType<IQuery<GetAllCustomRulesCriterion, IEnumerable<CustomRuleEntity>>, GetAllCustomRuleQuery>();
            container.RegisterType<IQuery<GetUserGameSnapshotByIdCriterion, UserGameDataSnapshotEntity>, GetUserGameSnapshotByIdQuery>();

            container.RegisterType<IMapper<User, UserEntity>, UserEntityMapper>();
            container.RegisterType<IMapper<UserEntity, User>, UserContractMapper>();
            container.RegisterType<IMapper<FundsDriverEntity, Item>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IncrementorEntity, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterEntity, CounterBase>, CountersContractMapper>();
            container.RegisterType<IMapper<CustomRuleEntity, CustomRule>, CustomRuleContractMapper>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IFundsDriverRepository, FundsDriverRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<IUserRatingRepository, UserRatingRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            return container;
        }
    }
}