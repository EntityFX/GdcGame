namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Incrementors;
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Mappers;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Mappers;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.Counetrs;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.CustomRule;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.FundsDriver;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.UserGameSnapshot;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

    using Microsoft.Practices.Unity;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<System.Data.Entity.DbContext, DbContext>(
                new InjectionConstructor("name=EconomicsArcadeDbContext"));
            container.RegisterType<IQueryBuilder, QueryBuilder>();
            container.RegisterType<IUnitOfWork, EfUnitOfWork>();
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>();


            container
                .RegisterType
                <IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>, GetAllFundsDriversQuery>();
            container.RegisterType<IQuery<GetAllCountersCriterion, IEnumerable<CounterEntity>>, GetAllCountersQuery>();

            container
                .RegisterType<IQuery<GetAllCustomRulesCriterion, IEnumerable<CustomRuleEntity>>, GetAllCustomRuleQuery>();
            container
                .RegisterType
                <IQuery<GetUserGameSnapshotByIdCriterion, UserGameDataSnapshotEntity>, GetUserGameSnapshotByIdQuery>();

            container.RegisterType<IMapper<User, UserEntity>, UserEntityMapper>();
            container.RegisterType<IMapper<UserEntity, User>, UserContractMapper>();
            container.RegisterType<IMapper<FundsDriverEntity, Item>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IncrementorEntity, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterEntity, CounterBase>, CountersContractMapper>();
            container.RegisterType<IMapper<CustomRuleEntity, CustomRule>, CustomRuleContractMapper>();

            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            return container;
        }
    }
}