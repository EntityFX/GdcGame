using System;
using System.Collections.Generic;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Infrastructure.Service.NetMsmq;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.Bases
{
    public abstract class ServicesStarterBase<TBootstrapper> : IServicesStarter
        where TBootstrapper : IContainerBootstrapper
    {
        protected readonly IUnityContainer _container;
        private readonly IDictionary<string, ServiceHostInfo> _serviceHosts = new Dictionary<string, ServiceHostInfo>();

        protected IDictionary<string, ServiceHostInfo> ServiceHosts
        {
            get { return _serviceHosts; }
        }

        protected ServicesStarterBase(TBootstrapper bootstrapper)
        {
            _container = bootstrapper.Configure(new UnityContainer());
        }

        protected void AddServiceHost(IServiceHost serviceHost, Uri endpointAddress)
        {
            _serviceHosts.Add(serviceHost.Name, new ServiceHostInfo()
            {
                EndpointAddress = endpointAddress,
                ServiceHost = serviceHost
            });
        }

        protected void AddNetTcpService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetTcpServiceHost<T>(_container);
            AddServiceHost(service, endpointAddress);
        }

        protected void AddNetNamedPipeService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetNamedPipeServiceHost<T>(_container);
            AddServiceHost(service, endpointAddress);
        }

        protected void AddNetMsmqService<T>(Uri endpointAddress) where T : class
        {
            var service = new NetMsmqServiceHost<T>(_container);
            AddServiceHost(service, endpointAddress);
        }

        protected void AddCustomService<TServiceHost>(Uri endpointAddress)
            where TServiceHost : IServiceHost
        {
            var service = Activator.CreateInstance(typeof(TServiceHost), _container) as IServiceHost;
            AddServiceHost(service, endpointAddress);
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

        public abstract void StopServices();

        public abstract void StartServices();

        protected virtual void OnServiceOpened(IServiceHost service) {

        }

        protected class ServiceHostInfo
        {
            public IServiceHost ServiceHost { get; set; }

            public Uri EndpointAddress { get; set; }
        }
    }
}
