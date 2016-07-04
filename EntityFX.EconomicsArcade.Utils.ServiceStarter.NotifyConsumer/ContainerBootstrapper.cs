using System.Configuration;
using EntityFX.EconomicsArcade.Application.NotifyConsumerService;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
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
            return container;
        }
    }
}