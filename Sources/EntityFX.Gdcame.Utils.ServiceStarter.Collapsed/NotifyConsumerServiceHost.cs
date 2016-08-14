﻿using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    internal class NotifyConsumerServiceHost : NetNamedPipeServiceHost<INotifyConsumerService>
    {
        public NotifyConsumerServiceHost(IUnityContainer container)
            : base(container)
        {
        }

        protected override Binding GetBinding()
        {
            var binding = (NetMsmqBinding)base.GetBinding();
            binding.ExactlyOnce = false;
            binding.Durable = false;
            return binding;
        }
    }
}