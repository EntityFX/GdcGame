﻿using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Security;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    internal class NetMsmqBindingFactory : IBindingFactory<NetMsmqBinding>
    {
        public NetMsmqBinding Build(object config)
        {
            {
                return new NetMsmqBinding
                {
                    ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                    SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                    OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                    CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                    MaxBufferPoolSize = 500000000,
                    MaxReceivedMessageSize = 500000000,
                    UseActiveDirectory = false,
                    ExactlyOnce = true,
                    //QueueTransferProtocol = System.ServiceModel.QueueTransferProtocol.Srmp,
                    Security = new NetMsmqSecurity
                    {
                        Transport = new MsmqTransportSecurity
                        {
                            MsmqAuthenticationMode = MsmqAuthenticationMode.None,
                            MsmqProtectionLevel = ProtectionLevel.None
                        },
                        Message = new MessageSecurityOverMsmq
                        {
                            AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256,
                            ClientCredentialType = MessageCredentialType.None
                        }
                    }
                };
            }
        }
    }
}