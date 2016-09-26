using System;
using System.Configuration;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Common.Presentation.Model.Mappers;
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
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Manager.Mappers.Store;
using EntityFX.Gdcame.NotifyConsumer;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Presentation.Web.Api.Models;
using EntityFX.Gdcame.Presentation.Web.Api.Providers;
using EntityFX.Gdcame.Presentation.Web.Controller;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Presentation.Web.Model.Mappers;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;
using CounterBase = EntityFX.Gdcame.Common.Contract.Counters.CounterBase;
using EntityFX.Gdcame.Presentation.Contract.Controller;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            var childBootstrappers = new IContainerBootstrapper[]
                        {
                new DataAccess.Repository.Ef.ContainerBootstrapper(), 
                //new DataAccess.Repository.Mongo.ContainerBootstrapper(),
                new DataAccess.Service.ContainerBootstrapper(),
                new Manager.ContainerBootstrapper(),
                new NotifyConsumer.ContainerBootstrapper()
                        };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

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

            container.RegisterType<IGameDataPersister, GameDataPersister>(
                new InjectionConstructor(
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapperFactory>()
                    )
                );

            container.RegisterType<IGameDataPersisterFactory, GameDataPersisterFactory>();

            container.RegisterType<IHashHelper, HashHelper>();

            container.RegisterInstance(new GameSessions(container.Resolve<ILogger>(), container.Resolve<IGameFactory>(), container.Resolve<IGameDataPersisterFactory>(), container.Resolve<IHashHelper>()));

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>(new InjectionConstructor(
                new ResolvedParameter<ILogger>(),
                new ResolvedParameter<IMapper<GameData, GameDataModel>>(),
                new ResolvedParameter<IHubContextAccessor>(),
                new ResolvedParameter<IConnections>()
                )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameDataChangesNotifier, GameDataChangesNotifier>(
                new InjectionConstructor(
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapperFactory>(),
                    new ResolvedParameter<INotifyConsumerClientFactory>()
                    )
                );

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
            container.RegisterType<IGameClientFactory, NoWcfGameManagerFactory>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();

            container.RegisterType<ISessionManager, SessionManager>(
    new InterceptionBehavior<PolicyInjectionBehavior>(),
    new Interceptor<InterfaceInterceptor>());

            container.RegisterType<IRatingManager, RatingManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            container.RegisterType<IGameManager, GameManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());
            container.RegisterType<ISimpleUserManager, SimpleUserManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IAdminManager, AdminManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());

            container.RegisterType<ApplicationUserManagerFacotory>();
            container.RegisterType<UserManager<GameUser>, ApplicationUserManager>();
            container.RegisterType<IUserStore<GameUser>, GameUserStore>();
            //container.RegisterType<IAccountController,AuthController>();
            container.RegisterType<IGameDataProvider, GameDataProvider>();

            container.RegisterType<IGameApiController, GameApiController>();
            container.RegisterType<IRatingApiController, RatingApiController>();
            container.RegisterType<IAccountController, AccountController>();
            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();

            return container;
        }
    }
}