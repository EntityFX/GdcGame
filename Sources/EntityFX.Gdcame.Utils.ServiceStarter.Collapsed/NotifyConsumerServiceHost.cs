using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    internal class NotifyConsumerServiceHost : NetTcpServiceHost<INotifyConsumerService>
    {
        public NotifyConsumerServiceHost(IUnityContainer container)
            : base(container)
        {
        }
    }
}