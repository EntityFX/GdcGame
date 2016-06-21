using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class ServiceStarterBase<TBootstrapper> : IServiceStarter
        where TBootstrapper : IContainerBootstrapper, new()
    {
        private readonly IUnityContainer _container;
        private readonly IDictionary<string, IServiceHost> _serviceHosts = new Dictionary<string, IServiceHost>();

        protected ServiceStarterBase(TBootstrapper bootstrapper)
        {
            _container = bootstrapper.Configure(new UnityContainer());
        }

        protected void AddNetTcpService<T>() where T : class
        {
            var service = new NetTcpServiceHost<T>(_container);
            _serviceHosts.Add(service.Name, service);
        }
        
        protected void OpenServices(Uri baseAddress)
        {
            foreach (var service in _serviceHosts)
            {
                service.Value.Open(new Uri(baseAddress + service.Value.Name));
                OnServiceOpened(service.Value);
            }
        }

        protected void CloseServices()
        {
            foreach (var service in _serviceHosts)
            {
                service.Value.Close();
            }
        }

        public abstract void StopService();

        public abstract void StartService();

        protected virtual void OnServiceOpened(IServiceHost service) {

        }
    }
}
