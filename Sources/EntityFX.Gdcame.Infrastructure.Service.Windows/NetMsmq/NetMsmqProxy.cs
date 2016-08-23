using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq
{
    public class NetMsmqProxy<TServiceContract> : InfrastructureProxy<TServiceContract, NetMsmqBinding>
    {
        protected override IBindingFactory<NetMsmqBinding> GetBindingFactory()
        {
            return new NetMsmqBindingFactory();
        }
    }
}