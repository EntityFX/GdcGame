using EntityFX.EconomicsArcade.Application.NotifyConsumerService;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.Practices.Unity;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));
            container.RegisterType<INotifyConsumerService, NotifyConsumerService>();
            //container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient>(
            //  new InjectionConstructor(
            //      ConfigurationManager.AppSettings["NotifyConsumerEndpoint_AddressServiceUrl"]
            //      ));
            container.RegisterType<IMapper<FundsCounters, FundsCounterModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<FundsDriver, FundsDriverModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
            return container;
        }
    }
}