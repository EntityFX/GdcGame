using System;
using System.Configuration;
using EntityFX.EconomicArcade.Engine.GameEngine.Mappers;
using EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;
using EntityFX.EconomicsArcade.Contract.Manager;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Manager;
using EntityFX.EconomicsArcade.Manager.Mappers;
using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;
using ManualStepResult = EntityFX.EconomicsArcade.Contract.Game.ManualStepResult;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new EconomicsArcade.Manager.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            
            container.AddNewExtension<Interception>();


            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter(new NLogLogExFactory().GetLogger("logger")))));

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

            container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClientProxyAddress_NotifyConsumerService"]
                    )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                    new ResolvedParameter<IMapper<FundsDriver, Contract.Common.Funds.FundsDriver>>(),
                    new ResolvedParameter<INotifyConsumerService>()
                    )
                );

            if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
            {
                container.Configure<Interception>()
                .AddPolicy("logging")
                .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.EconomicsArcade.*", true));
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