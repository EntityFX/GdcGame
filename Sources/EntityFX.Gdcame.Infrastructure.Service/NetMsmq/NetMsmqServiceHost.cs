using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.NetMsmq
{
    public class NetMsmqServiceHost<T> : InfrastructureServiceHost<T>
    {
        public NetMsmqServiceHost(IUnityContainer container)
            :base(container)
        {

        }

        protected override Binding GetBinding()
        {
            var binding = new NetMsmqBinding()
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 500000000,
                MaxReceivedMessageSize = 500000000,
                UseActiveDirectory = false,
                ExactlyOnce = true,
                QueueTransferProtocol = System.ServiceModel.QueueTransferProtocol.Srmp,
                Security = new NetMsmqSecurity()
                {
                    Transport = new MsmqTransportSecurity()
                    {
                        MsmqAuthenticationMode = MsmqAuthenticationMode.None, MsmqProtectionLevel = ProtectionLevel.None
                    },
                    Message = new MessageSecurityOverMsmq()
                    {
                        AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256, ClientCredentialType = MessageCredentialType.None
                    }
                }
            };

            var myReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = int.MaxValue,
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxDepth = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue
            };

            binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);

            return binding;
        }

        protected override ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
            var endpoint = base.CreateServiceEndpoint(serviceHost);
            var dataContractBehavior = endpoint.Behaviors.Find<DataContractSerializerOperationBehavior>();
            if (dataContractBehavior != null)
            {
                dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }

            return endpoint;
        }
    }
}
