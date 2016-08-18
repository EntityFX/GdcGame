using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;

namespace EntityFX.Gdcame.Utils.ClientProxy.NotifyConsumer
{
	#if __MonoCS__
	public class NotifyConsumerProxy : NetTcpProxy<INotifyConsumerService>
	{
	}
	#else
	public class NotifyConsumerProxy : EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq.NetMsmqProxy<INotifyConsumerService>
    {
        protected override void ConfigureBinding(NetMsmqBinding binding)
        {
            binding.Durable = false;
            binding.ExactlyOnce = false;
        }
    }
	#endif
}