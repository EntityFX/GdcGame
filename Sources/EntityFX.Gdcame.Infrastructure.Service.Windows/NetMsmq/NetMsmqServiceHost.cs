using System.ServiceModel;
using System.ServiceModel.Description;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq
{
    public class NetMsmqServiceHost<T> : InfrastructureServiceHost<T, NetMsmqBinding>
    {
        public NetMsmqServiceHost(IUnityContainer container)
            : base(container)
        {
        }

        protected override IBindingFactory<NetMsmqBinding> GetBindingFactory()
        {
            return new NetMsmqBindingFactory();
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