using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq;

namespace EntityFX.Gdcame.Utils.ClientProxy.NotifyConsumer
{
	#if __MonoCS__
	public class NotifyConsumerProxy : NetTcpProxy<INotifyConsumerService>
	{
	}
	#else
	public class NotifyConsumerProxy : NetMsmqProxy<INotifyConsumerService>
    {
        protected override void ConfigureBinding(NetMsmqBinding binding)
        {
            binding.Durable = false;
            binding.ExactlyOnce = false;
        }
    }
	#endif
}