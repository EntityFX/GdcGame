using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.Utils.ClientProxy.WcfNotifyConsumer
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