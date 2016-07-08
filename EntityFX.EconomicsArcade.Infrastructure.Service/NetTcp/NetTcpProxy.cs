using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public class NetTcpProxy<TServiceContract> : InfrastructureProxy<TServiceContract>
    {
        protected override System.ServiceModel.Channels.Binding GetBinding()
        {
            var binding = new NetTcpBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 500000000,
                MaxReceivedMessageSize = 500000000,

            };
            binding.Security.Mode = SecurityMode.None;
            return binding;
        }
    }
}
