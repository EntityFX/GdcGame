using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Security;
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
