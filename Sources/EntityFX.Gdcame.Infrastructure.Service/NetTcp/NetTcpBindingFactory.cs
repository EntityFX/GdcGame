using System;
using System.ServiceModel;
using System.Xml;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    internal class NetTcpBindingFactory : IBindingFactory<NetTcpBinding>
    {
        public NetTcpBinding Build(object config)
        {
            var binding = new NetTcpBinding
            {
                ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
                SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
                OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
                CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
                MaxBufferPoolSize = 1024*1024*8,
                MaxReceivedMessageSize = 1024*1024*4,
                TransferMode = TransferMode.Streamed,
                MaxBufferSize = 1024*1024*4,
                PortSharingEnabled = true
            };
            //binding.ReliableSession.Enabled = false;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;

            var myReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = 1024*1024*4, //Maybe int.MaxValue?
                MaxArrayLength = 1024*1024*4,
                MaxBytesPerRead = 1024*1024*4,
                MaxDepth = 1024*1024*512,
                MaxNameTableCharCount = 1024*8
            };

            binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);

            return binding;
        }
    }
}