using System;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe
{
    public class NetNamedPipeProxy<TServiceContract> : InfrastructureProxy<TServiceContract>
    {
        protected override System.ServiceModel.Channels.Binding GetBinding()
        {
            var binding = new NetNamedPipeBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 500000000,
                MaxReceivedMessageSize = 500000000,

            };
            return binding;
        }
    }
}