using System;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using System.ServiceModel;
using System.Xml;

namespace EntityFX.Gdcame.Infrastructure.Service
{
	internal class NetTcpBindingFactory : IBindingFactory<NetTcpBinding>
	{
		NetTcpBinding Build (object config) 
		{
			var binding = new NetTcpBinding()
			{
				ReceiveTimeout = new TimeSpan(0, 0, 10, 0, 0),
				SendTimeout = new TimeSpan(0, 0, 10, 0, 0),
				OpenTimeout = new TimeSpan(0, 0, 10, 0, 0),
				CloseTimeout = new TimeSpan(0, 0, 10, 0, 0),
				MaxBufferPoolSize = 1024*1024*512,
				MaxReceivedMessageSize = 1024*1024*512,
				TransferMode = TransferMode.Streamed,
				MaxBufferSize = 1024*1024*512,
				PortSharingEnabled = true
			};
			binding.ReliableSession.Enabled = false;
			binding.Security.Mode = SecurityMode.None;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
			binding.Security.Message.ClientCredentialType = MessageCredentialType.None;

			var myReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxStringContentLength = 1024*1024*512, //Maybe int.MaxValue?
				MaxArrayLength = 1024*1024*512,
				MaxBytesPerRead = 1024*1024*512,
				MaxDepth = 1024*1024*512,
				MaxNameTableCharCount = 1024*1024*512
			};

			binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);

			return binding;
		}
	}
}

