﻿using System;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using System.Xml;

namespace EntityFX.Gdcame.Infrastructure.Service.NetTcp
{
	public class NetTcpProxy<TServiceContract> : InfrastructureProxy<TServiceContract, NetTcpBinding>
    {
		protected override IBindingFactory<NetTcpBinding> GetBindingFactory () 
		{
			return new NetTcpBindingFactory ();
		}
    }
}
