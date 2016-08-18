using System;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe
{
	public class NetNamedPipeProxy<TServiceContract> : InfrastructureProxy<TServiceContract, NetNamedPipeBinding>
    {
		protected override IBindingFactory<NetNamedPipeBinding> GetBindingFactory () 
		{
			return new NetNamedPipeBindingFactory ();
		}
    }
}