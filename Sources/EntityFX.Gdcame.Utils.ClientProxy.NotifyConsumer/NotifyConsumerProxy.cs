using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Service.NetMsmq;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer
{
    public class NotifyConsumerProxy : NetMsmqProxy<INotifyConsumerService>
    {
        protected override Binding GetBinding()
        {
            var binding = (NetMsmqBinding)base.GetBinding();
            binding.Durable = false;
            binding.ExactlyOnce = false;
            return binding;
        }
    }
}