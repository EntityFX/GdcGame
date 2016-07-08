using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
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
