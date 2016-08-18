using System;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using System.ServiceModel;
using System.Xml;

namespace EntityFX.Gdcame.Infrastructure.Service
{
	internal class NetNamedPipeBindingFactory : IBindingFactory<NetNamedPipeBinding>
	{
		public NetNamedPipeBinding Build (object config) 
		{
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
		}
	}
}

