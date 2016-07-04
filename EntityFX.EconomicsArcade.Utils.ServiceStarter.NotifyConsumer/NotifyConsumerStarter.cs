using System;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer
{
    public class NotifyConsumerStarter : ServiceStarterBase<ContainerBootstrapper>, IServiceStarter
    {
        private readonly Uri _baseUrl;

        public NotifyConsumerStarter(ContainerBootstrapper containerBootstrapper, string baseUrl)
            : base(containerBootstrapper)
        {
            _baseUrl = new Uri(baseUrl);
        }

        public override void StartService()
        {
            AddNetMsmqService<INotifyConsumerService>(_baseUrl);
            OpenServices();
        }

        public override void StopService()
        {
            CloseServices();
        }

        protected override void OnServiceOpened(IServiceHost service)
        {
            ServiceInfoHelper.PrintServiceHostInfo(service.ServiceHost);
        }
    }

}
