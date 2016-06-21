﻿using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public class NetTcpProxyFactory<TServiceContract> : InfrastructureProxyFactory<TServiceContract>
    {
        protected override System.ServiceModel.Channels.Binding GetBinding()
        {
            return new NetTcpBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 500000000,
                MaxReceivedMessageSize = 500000000
            };

        }
    }
}
