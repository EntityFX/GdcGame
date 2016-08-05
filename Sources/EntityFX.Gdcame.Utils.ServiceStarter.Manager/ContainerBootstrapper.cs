using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
using EntityFX.Gdcame.Common.Contract;
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
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Utils.ClientProxy.DataAccess;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Manager
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new Gdcame.Manager.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            
            container.AddNewExtension<Interception>();


            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter(new NLogLogExFactory().GetLogger("logger")))));

            container.RegisterType<IOperationContextHelper, WcfOperationContextHelper>();

            //container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient>();
            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient<NetTcpProxy<IGameDataRetrieveDataAccessService>>>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataRetrieveDataAccessService"]
                    )
                ,
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IUserDataAccessService, UserDataAccessClient<NetTcpProxy<IUserDataAccessService>>>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_UserDataAccessService"]
                    )
                ,
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessClient<NetMsmqProxy<IGameDataStoreDataAccessService>>>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataStoreDataAccessService"]
                    ),
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient<NotifyConsumerProxy>>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClientProxyAddress_NotifyConsumerService"]
                    )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterType<INotifyConsumerClientFactory, NotifyConsumerClientFactory>(new InjectionConstructor(
                    new ResolvedParameter<IUnityContainer>(),
                    string.Empty));

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                    new ResolvedParameter<IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver>>(),
                    new ResolvedParameter<INotifyConsumerClientFactory>()
                    )
                );

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