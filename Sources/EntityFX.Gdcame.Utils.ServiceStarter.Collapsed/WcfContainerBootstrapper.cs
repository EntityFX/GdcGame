using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Infrastructure.Service.NetMsmq;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Manager;
using EntityFX.Gdcame.NotifyConsumer;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.ClientProxy.DataAccess;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;
using ContainerBootstrapper = EntityFX.Gdcame.DataAccess.Repository.ContainerBootstrapper;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    public class WcfContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new ContainerBootstrapper(),
                new Gdcame.DataAccess.Service.ContainerBootstrapper(),
                new Gdcame.Manager.ContainerBootstrapper(),
                new Gdcame.NotifyConsumer.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);
            container.RegisterType<IOperationContextHelper, WcfOperationContextHelper>();

            container.RegisterType<ILogger>(new InjectionFactory(
               _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));



            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient<NetTcpProxy<IGameDataRetrieveDataAccessService>>>("GameDataRetrieveDataAccessClient",
               new InjectionConstructor(
                   "net.pipe://localhost/EntityFX.Gdcame.DataAccess/EntityFX.Gdcame.DataAccess.Contract.GameData.IGameDataRetrieveDataAccessService"
                   )
               ,
               new InterceptionBehavior<PolicyInjectionBehavior>()
               , new Interceptor<InterfaceInterceptor>()
               );
            container.RegisterType<IUserDataAccessService, UserDataAccessClient<NetTcpProxy<IUserDataAccessService>>>("UserDataAccessClient",
                new InjectionConstructor(
                    "net.pipe://localhost/EntityFX.Gdcame.DataAccess/EntityFX.Gdcame.DataAccess.Contract.User.IUserDataAccessService"
                    )
                ,
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessClient<NetMsmqProxy<IGameDataStoreDataAccessService>>>("GameDataStoreDataAccessClient",
                new InjectionConstructor(
                    "net.msmq://localhost/private/EntityFX.Gdcame.DataAccess.Contract.GameData.IGameDataStoreDataAccessService"
                    ),
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterInstance<GameSessions>(new GameSessions(container.Resolve<ILogger>(), container.Resolve<IGameFactory>()));

            container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient<NotifyConsumerProxy>>("NotifyConsumerServiceClient",
                new InjectionConstructor(
                    "net.msmq://localhost/private/EntityFX.Gdcame.NotifyConsumer.Contract.INotifyConsumerService"
                    )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>(new InjectionConstructor(
                new ResolvedParameter<ILogger>(),
                new ResolvedParameter<IMapper<GameData, GameDataModel>>(),
                new ResolvedParameter<IHubContextAccessor>(),
                                new ResolvedParameter<IConnections>()
                )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                );

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>("GameDataStoreDataAccessClient"),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                    new ResolvedParameter<IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver>>(),
                    new ResolvedParameter<INotifyConsumerClientFactory>()
                    )
                );


            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(new InjectionConstructor(
                    new ResolvedParameter<IUnityContainer>(),
                    "NotifyConsumerServiceClient"));

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