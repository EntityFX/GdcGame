﻿using System;
using System.Configuration;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.DataAccess.Service;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.NotifyConsumer;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;
using ContainerBootstrapper = EntityFX.Gdcame.DataAccess.Repository.Ef.ContainerBootstrapper;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Manager.Mappers.Store;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    public class FullCollapsedContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new ContainerBootstrapper(),
                new DataAccess.Service.ContainerBootstrapper(),
                new Manager.ContainerBootstrapper(),
                new NotifyConsumer.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);
            container.RegisterType<IOperationContextHelper, WcfOperationContextHelper>();

            container.RegisterType<ILogger>(new InjectionFactory(
               _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

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
            container.RegisterType<IMapper<CounterBase, StoredCounterBase>, StoreCounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, StoredCash>, StoreFundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, StoredItem>, StoreFundsDriverContractMapper>();
            container.RegisterType<IMapper<CustomRuleInfo, StoredCustomRuleInfo>, StoreCustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, StoredGameData>, StoreGameDataMapper>("StoreGameDataMapper");
            /////

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterInstance<GameSessions>(new GameSessions(container.Resolve<ILogger>(), container.Resolve<IGameFactory>()));

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>(new InjectionConstructor(
                new ResolvedParameter<ILogger>(),
                new ResolvedParameter<IMapper<GameData, GameDataModel>>(),
                new ResolvedParameter<IHubContextAccessor>(),
                new ResolvedParameter<IConnections>()
                )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapper<IGame, StoredGameData>>("StoreGameDataMapper"),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                                        new ResolvedParameter<IMapper<Item, StoredItem>>(),
                    new ResolvedParameter<IMapper<Item, Gdcame.Common.Contract.Items.Item>>(),
                    new ResolvedParameter<INotifyConsumerClientFactory>()
                    )
                );

            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(new InjectionConstructor(
                    new ResolvedParameter<IUnityContainer>(),
                    string.Empty));

            if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
            {
                container.Configure<Interception>()
                .AddPolicy("logging")
                .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
            }

            if (!Environment.UserInteractive)
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperLogger>();
            }
            else
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperConsole>();
            }

            return container;
        }
    }
}