using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe
{
    public class NetNamedPipeServiceHost<T> : InfrastructureServiceHost<T>
    {
        public NetNamedPipeServiceHost(IUnityContainer container)
            : base(container)
        {

        }
        
        protected override Binding GetBinding()
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
           /* binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;*/

            var myReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = 2147483647, //Maybe int.MaxValue?
                MaxArrayLength = 2147483647,
                MaxBytesPerRead = 2147483647,
                MaxDepth = 2147483647,
                MaxNameTableCharCount = 2147483647
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