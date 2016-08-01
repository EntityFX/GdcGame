using System.Collections.Generic;
using System.Data.Entity;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.CustomRule;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;
using EntityFX.EconomicsArcade.DataAccess.Repository.Mappers;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.Counetrs;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.CustomRule;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.User;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCustomRuleInfo;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserFundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserGameCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserRating;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
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
            container.RegisterType<IQuery<GetUserGameCounterByIdCriterion, UserGameCounterEntity>, GetUserGameCounterByIdQuery>();
            container.RegisterType<IQuery<GetUserCountersByUserIdCriterion, IEnumerable<UserCounterEntity>>, GetUserCountersByUserIdQuery>();
            container.RegisterType<IQuery<GetUserFundsDriverByUserIdCriterion, IEnumerable<UserFundsDriverEntity>>, GetUserFundDriverByUserIdQuery>();
            container.RegisterType<IQuery<GetAllCustomRulesCriterion, IEnumerable<CustomRuleEntity>>, GetAllCustomRuleQuery>();
            container.RegisterType<IQuery<GetUserCustomRuleInfoByUserIdCriterion, IEnumerable<UserCustomRuleEntity>>, GetUserCustomRuleInfoByUserIdQuery>();

            container.RegisterType<IMapper<User, UserEntity>, UserEntityMapper>();
            container.RegisterType<IMapper<UserEntity, User>, UserContractMapper>();
            container.RegisterType<IMapper<FundsDriverEntity, FundsDriver>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IncrementorEntity, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterEntity, CounterBase>, CountersContractMapper>();
            container.RegisterType<IMapper<UserGameCounter, UserGameCounterEntity>, UserGameCounterEntityMapper>();
            container.RegisterType<IMapper<UserGameCounterEntity, UserGameCounter>, UserGameCounterContractMapper>();
            container.RegisterType<IMapper<CounterBase, UserCounterEntity>, UserCounterEntityMapper>();
            container.RegisterType<IMapper<UserCounterEntity, CounterBase>, UserCounterContractMapper>();
            container.RegisterType<IMapper<FundsDriver, UserFundsDriverEntity>, UserFundsDriverEntityMapper>();
            container.RegisterType<IMapper<UserFundsDriverEntity, FundsDriver>, UserFundsDriverContractMapper>();
            container.RegisterType<IMapper<CustomRuleEntity, CustomRule>, CustomRuleContractMapper>();
            container.RegisterType<IMapper<UserCustomRuleEntity, CustomRuleInfo>, UserCustomRuleContractMapper>();
            container.RegisterType<IMapper<CustomRuleInfo, UserCustomRuleEntity>, UserCustomRuleEntityMapper>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IFundsDriverRepository, FundsDriverRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<IUserGameCounterRepository, UserGameCounterRepository>();
            container.RegisterType<IUserCounterRepository, UserCounterRepository>();
            container.RegisterType<IUserFundsDriverRepository, UserFundsDriverRepository>();
            container.RegisterType<IUserRatingRepository, UserRatingRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserCustomRuleRepository, UserCustomRuleRepository>();
            return container;
        }
    }
}