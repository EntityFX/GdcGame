using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using EntityFX.EconomicsArcade.Manager;
using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Collapsed
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new EconomicsArcade.DataAccess.Repository.ContainerBootstrapper(),
                new EconomicsArcade.DataAccess.Service.ContainerBootstrapper(),
                new Manager.ContainerBootstrapper(),
            };
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();

            container.RegisterType<ILogger>(new InjectionFactory(
               _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient<NetTcpProxy<IGameDataRetrieveDataAccessService>>>("GameDataRetrieveDataAccessClient",
               new InjectionConstructor(
                   "net.pipe://localhost/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataRetrieveDataAccessService"
                   )
               ,
               new InterceptionBehavior<PolicyInjectionBehavior>()
               , new Interceptor<InterfaceInterceptor>()
               );
            container.RegisterType<IUserDataAccessService, UserDataAccessClient<NetTcpProxy<IUserDataAccessService>>>("UserDataAccessClient",
                new InjectionConstructor(
                    "net.pipe://localhost/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService"
                    )
                ,
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessClient<NetMsmqProxy<IGameDataStoreDataAccessService>>>("GameDataStoreDataAccessClient",
                new InjectionConstructor(
                    "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataStoreDataAccessService"
                    ),
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
                );

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient>("NotifyConsumerServiceClient",
                new InjectionConstructor(
                    "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.NotifyConsumerService.INotifyConsumerService"
                    )
                , new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>()
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