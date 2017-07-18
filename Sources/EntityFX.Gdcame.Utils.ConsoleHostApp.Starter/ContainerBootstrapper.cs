using System;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Api.Common.Mappers;
using EntityFX.Gdcame.Application.Api.Controller.MainServer;
using EntityFX.Gdcame.Application.Api.MainServer.Mappers;
using EntityFX.Gdcame.Application.Api.MainServer.Models;
using EntityFX.Gdcame.Application.Api.MainServer.Providers;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Application.Providers.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Service;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager;
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
using PortableLog.NLog;
using CounterBase = EntityFX.Gdcame.Common.Contract.Counters.CounterBase;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        private readonly AppConfiguration _appConfiguration;

        public ContainerBootstrapper(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public IUnityContainer Configure(IUnityContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            var childBootstrappers = new IContainerBootstrapper[]
                        {
                GetRepositoryProvider(_appConfiguration.RepositoryProvider),
                new DataAccess.Service.ContainerBootstrapper(),
                new Manager.Common.ContainerBootstrapper(),
                new Manager.MainServer.ContainerBootstrapper(),
                new NotifyConsumer.ContainerBootstrapper()
                        };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);

            container.RegisterType<IMapperFactory, MapperFactory>();

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessDocumentService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IUserDataAccessService, UserDataAccessService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessDocumentService>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            //Store
            container.RegisterType<IMapper<IncrementorBase, StoredIncrementor>, StoreIncrementorContractMapper>();
            container.RegisterType<IMapper<GameEngine.Contract.Counters.CounterBase, StoredCounterBase>, StoreCounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, StoredCash>, StoreFundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, StoredItem>, StoreFundsDriverContractMapper>();
            container.RegisterType<IMapper<CustomRuleInfo, StoredCustomRuleInfo>, StoreCustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, StoredGameData>, StoreGameDataMapper>("StoreGameDataMapper");
            /////

            container.RegisterType<IGameFactory, GameFactory>();
            container.RegisterType<ITaskTimerFactory, TaskTimerFactory>();
            container.RegisterType<ITaskTimer, GenericTaskTimer>();

            container.RegisterType<IGameDataPersister, GameDataPersister>(
                new InjectionConstructor(
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapperFactory>()
                    )
                );

            container.RegisterType<IGameDataPersisterFactory, GameDataPersisterFactory>();

            container.RegisterType<IHashHelper, HashHelper>();
            container.RegisterInstance<IPerformanceHelper>(new PerformanceHelper());

            container.RegisterInstance(new PerformanceInfo());
            container.RegisterInstance(new SystemInfo()
            {
                CpusCount = Environment.ProcessorCount, Os = Environment.OSVersion.ToString(), Runtime = RuntimeHelper.GetRuntimeName(), MemoryTotal = RuntimeHelper.GetTotalMemoryInMb()
            });

            container.RegisterInstance(
                new GameSessions(container.Resolve<ILogger>(), 
                container.Resolve<IGameFactory>(), container.Resolve<PerformanceInfo>()));

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>(new InjectionConstructor(
                new ResolvedParameter<ILogger>(),
                new ResolvedParameter<IMapper<GameData, GameDataModel>>(),
                new ResolvedParameter<IHubContextAccessor>(),
                new ResolvedParameter<IConnections>()
                )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameDataChangesNotifier, GameDataChangesNotifier>();

            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(new InjectionConstructor(
                new ResolvedParameter<IUnityContainer>(),
                string.Empty));

            /* if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
             {
                 container.Configure<Interception>()
                     .AddPolicy("logging")
                     .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                     .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
             }*/

            container.RegisterType<IMapper<Cash, CashModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<Gdcame.Common.Contract.Items.Item, ItemModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
            container.RegisterType<IMapper<BuyFundDriverResult, BuyItemModel>, FundsDriverBuyInfoMapper>();
            container.RegisterType<IMapper<StatisticsInfo, ServerStatisticsInfoModel>, StatisticsInfoMapper>();
            container.RegisterType<IMapper<TopRatingStatistics, TopRatingStatisticsModel>, TopRatingStatisticsModelMapper>();
            container.RegisterType<IGameClientFactory, NoWcfGameManagerFactory>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();



            container.RegisterType<ApplicationUserManagerFacotory>();
            container.RegisterType<IUserStore<GameUser>, GameUserStore>();          
            container.RegisterType<IGameDataProvider, GameDataProvider>();

            container.RegisterType<IGameApiController, GameApiController>();
          //  container.RegisterType<IRatingController, RatingController>();
            container.RegisterType<IRatingController, LocalRatingController>();
            container.RegisterType<IServerController, ServerController>();

            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();

            container.RegisterType<UserManager<GameUser>, ApplicationUserManager>();
            
            return container;
        }

        private IContainerBootstrapper GetRepositoryProvider(string providerName)
        {
            switch (providerName)
            {
                case "EntityFramework":
                    return new DataAccess.Repository.Ef.ContainerBootstrapper();
                case "Mongo":
                    return new DataAccess.Repository.Mongo.ContainerBootstrapper(_appConfiguration.MongoConnectionString);
                case "LocalStorage":
                default:
                    return new DataAccess.Repository.LocalStorage.ContainerBootstrapper();
            }
        }
    }
}