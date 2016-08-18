using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.Practices.Unity;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;

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