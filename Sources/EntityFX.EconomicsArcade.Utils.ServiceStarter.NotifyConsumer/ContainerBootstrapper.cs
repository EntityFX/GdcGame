using System;
using EntityFX.EconomicsArcade.Application.NotifyConsumerService;
using EntityFX.EconomicsArcade.Application.NotifySignalrService;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces;
using EntityFX.EconomicsArcade.Model.Common.Mappers;
using EntityFX.EconomicsArcade.Model.Common.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();
            
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);
            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            container.RegisterType<IMapper<FundsCounters, FundsCounterModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<FundsDriver, FundsDriverModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();

            container.RegisterType<GameDataHub>();

            container.RegisterType<IHubContext>(
                new InjectionFactory(_ => GlobalHost.ConnectionManager.GetHubContext<GameDataHub>()));

            container.RegisterType<INotifyConsumerService, NotifyConsumerService>(new InjectionConstructor(
                new ResolvedParameter<ILogger>(),
                new ResolvedParameter<IMapper<GameData, GameDataModel>>(),
                new ResolvedParameter<IHubContext>()
                )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                );

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