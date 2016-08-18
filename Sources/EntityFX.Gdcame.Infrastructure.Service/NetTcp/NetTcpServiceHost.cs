using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.NetTcp
{
	public class NetTcpServiceHost<TServiceContract> : InfrastructureServiceHost<TServiceContract, NetTcpBinding>
    {
        public NetTcpServiceHost(IUnityContainer container)
            : base(container)
        {

        }

		protected override IBindingFactory<NetTcpBinding> GetBindingFactory () 
		{
			return new NetTcpBindingFactory ();
		}
			
        protected override ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
            var endpoint = base.CreateServiceEndpoint(serviceHost);
			endpoint.Contract.SessionMode = SessionMode.NotAllowed;
            var dataContractBehavior = endpoint.Behaviors.Find<DataContractSerializerOperationBehavior>();
            if (dataContractBehavior != null)
            {
                dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }

            return endpoint;
        }
    }
}
