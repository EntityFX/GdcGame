using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe
{
	public class NetNamedPipeServiceHost<T> : InfrastructureServiceHost<T, NetNamedPipeBinding>
    {
        public NetNamedPipeServiceHost(IUnityContainer container)
            : base(container)
        {

        }
        
		protected override IBindingFactory<NetNamedPipeBinding> GetBindingFactory () 
		{
			return new NetNamedPipeBindingFactory ();
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