using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class ServiceStarterBase<TBootstrapper> : IServiceStarter
        where TBootstrapper : IContainerBootstrapper, new()
    {
        private readonly IUnityContainer _container;
        private readonly IDictionary<string, ServiceHostInfo> _serviceHosts = new Dictionary<string, ServiceHostInfo>();

        protected ServiceStarterBase(TBootstrapper bootstrapper)
        {
            _container = bootstrapper.Configure(new UnityContainer());
        }

        protected void AddNetTcpService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetTcpServiceHost<T>(_container);
            _serviceHosts.Add(service.Name, new ServiceHostInfo()
            {
                EndpointAddress = endpointAddress,
                ServiceHost = service
            });
        }

        protected void AddNetMsmqService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetMsmqServiceHost<T>(_container);
            _serviceHosts.Add(service.Name, new ServiceHostInfo()
            {
                EndpointAddress = endpointAddress,
                ServiceHost = service
            });
        }
        
        protected void OpenServices()
        {
            foreach (var service in _serviceHosts)
            {
                service.Value.ServiceHost.Open(new Uri(service.Value.EndpointAddress.ToString() + service.Value.ServiceHost.Name));
                OnServiceOpened(service.Value.ServiceHost);
            }
        }

        protected void CloseServices()
        {
            foreach (var service in _serviceHosts)
            {
                service.Value.ServiceHost.Close();
            }
        }

        public abstract void StopService();

        public abstract void StartService();

        protected virtual void OnServiceOpened(IServiceHost service) {

        }

        private class ServiceHostInfo
        {
            public IServiceHost ServiceHost { get; set; }

            public Uri EndpointAddress { get; set; }
        }
    }
}
