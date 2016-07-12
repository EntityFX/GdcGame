﻿using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public class NetMsmqProxy<TServiceContract> : InfrastructureProxy<TServiceContract>
    {
        protected override System.ServiceModel.Channels.Binding GetBinding()
        {
            return new System.ServiceModel.NetMsmqBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 500000000,
                MaxReceivedMessageSize = 500000000,
                UseActiveDirectory = false,
                ExactlyOnce = true,
                Security = new NetMsmqSecurity()
                {
                    Transport = new MsmqTransportSecurity()
                    {
                        MsmqAuthenticationMode = MsmqAuthenticationMode.None,
                        MsmqProtectionLevel = ProtectionLevel.None
                    },
                    Message = new MessageSecurityOverMsmq()
                    {
                        AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256,
                        ClientCredentialType = MessageCredentialType.None
                    }
                }
            };
        }
    }
}
