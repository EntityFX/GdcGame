using System;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Infrastructure.Service.Windows.NetMsmq;

namespace EntityFX.Gdcame.Infrastructure.Service.Windows
{
    public abstract class WindowsServiceStarter<TBootstrapper> : ServicesStarterBase<TBootstrapper>, IServicesStarter
        where TBootstrapper : IContainerBootstrapper
    {
        protected WindowsServiceStarter(TBootstrapper bootstrapper) : base(bootstrapper)
        {
        }

        protected void AddNetMsmqService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetMsmqServiceHost<T>(_container);
            AddServiceHost(service, endpointAddress);
        } 
    }
}