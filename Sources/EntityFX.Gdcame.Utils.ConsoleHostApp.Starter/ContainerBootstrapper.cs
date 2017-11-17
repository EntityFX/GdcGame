using System;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Api.Common.Mappers;
using EntityFX.Gdcame.Application.Api.Controller.MainServer;
using EntityFX.Gdcame.Application.Api.MainServer.Mappers;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Application.Providers.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Contract.MainServer.Store;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using EntityFX.Gdcame.Manager.MainServer;
using EntityFX.Gdcame.Manager.MainServer.Mappers.Store;
using EntityFX.Gdcame.NotifyConsumer;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.Hashing;
using EntityFX.Gdcame.Utils.MainServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    using System.Linq;

    using EntityFX.Gdcame.Application.Api.Common.Providers;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Service.MainServer;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;
    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    using CounterBase = EntityFX.Gdcame.Kernel.Contract.Counters.CounterBase;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        private readonly AppConfiguration _appConfiguration;

        public ContainerBootstrapper(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public IIocContainer Configure(IIocContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();


            container.RegisterType<ILogger>(() => new Logger(new NLoggerAdapter(NLog.LogManager.GetLogger("logger"))));

            var childBootstrappers = GetRepositoryProviders(_appConfiguration.RepositoryProvider).Concat(
                new IContainerBootstrapper[]
                        {
                new EntityFX.Gdcame.DataAccess.Service.Common.ContainerBootstrapper(),
                new EntityFX.Gdcame.DataAccess.Service.MainServer.ContainerBootstrapper(),
                new Manager.Common.ContainerBootstrapper(),
                new Manager.MainServer.ContainerBootstrapper(),
                new NotifyConsumer.ContainerBootstrapper()
                        }
                ).ToArray();
            Array.ForEach(childBootstrappers, _ => _.Configure(container));


            container.RegisterType<IMapperFactory, MapperFactory>();

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>();
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>();

            //Store
            container.RegisterType<IMapper<IncrementorBase, StoredIncrementor>, StoreIncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, StoredCounterBase>, StoreCounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, StoredCash>, StoreFundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, StoredItem>, StoreFundsDriverContractMapper>();
            container.RegisterType<IMapper<CustomRuleInfo, StoredCustomRuleInfo>, StoreCustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, StoredGameData>, StoreGameDataMapper>(ContainerScope.Instance, "StoreGameDataMapper");
            /////

            container.RegisterType<IGameFactory, GameFactory>();
            container.RegisterType<ITaskTimerFactory, TaskTimerFactory>();
            container.RegisterType<ITaskTimer, GenericTaskTimer>();

            container.RegisterType<IGameDataPersister, GameDataPersister>();

            container.RegisterType<IGameDataPersisterFactory, GameDataPersisterFactory>();

            container.RegisterType<ICache, Cache>(() => new Cache(), ContainerScope.Singleton );

            container.RegisterType<IHashHelper, HashHelper>();
            container.RegisterType<IPerformanceHelper>(() => new PerformanceHelper(), ContainerScope.Singleton);

            container.RegisterType(() => new GamePerformanceInfo(), ContainerScope.Singleton);
            container.RegisterType(() => new SystemInfo()
            {
                CpusCount = Environment.ProcessorCount, Os = Environment.OSVersion.ToString(), Runtime = RuntimeHelper.GetRuntimeName(), MemoryTotal = RuntimeHelper.GetTotalMemoryInMb()
            }, ContainerScope.Singleton);

            container.RegisterType<IGameSessions, GameSessions>(
                () => new GameSessions(container.Resolve<ILogger>(), 
                container.Resolve<IGameFactory>(), container.Resolve<GamePerformanceInfo>()), ContainerScope.Singleton);

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>();

            container.RegisterType<IGameDataChangesNotifier, GameDataChangesNotifier>();

            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(() => new NotifyConsumerClientFactory(container));

            /* if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
             {
                 container.Configure<Interception>()
                     .AddPolicy("logging")
                     .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                     .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
             }*/

            container.RegisterType<IMapper<Cash, CashModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<Gdcame.Contract.MainServer.Counters.CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<Gdcame.Contract.MainServer.Items.Item, ItemModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
            container.RegisterType<IMapper<BuyFundDriverResult, BuyItemModel>, FundsDriverBuyInfoMapper>();
            container.RegisterType<IMapper<MainServerStatisticsInfo, MainServerStatisticsInfoModel>, StatisticsInfoMapper>();
            container.RegisterType<IGameClientFactory, NoWcfGameManagerFactory>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();



            container.RegisterType<ApplicationUserManagerFacotory>();
            container.RegisterType<IUserStore<UserIdentity>, GameUserStore>();          
            container.RegisterType<IGameDataProvider, GameDataProvider>();

            container.RegisterType<IGameApiController, GameApiController>();

            container.RegisterType<IRatingController, LocalRatingController>();
            container.RegisterType<IServerController, ServerController>();

            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();

            container.RegisterType<UserManager<UserIdentity>, ApplicationUserManager>();
            
            return container;
        }

        private IContainerBootstrapper[] GetRepositoryProviders(string providerName)
        {
            return new[]
                       {  new CommonDatabasesProvider(providerName)
                               .GetRepositoryProvider(_appConfiguration.MongoConnectionString),
                          new MainServerDatabasesProvider(providerName)
                               .GetRepositoryProvider(_appConfiguration.MongoConnectionString)
                       };
        }
        
    }
}