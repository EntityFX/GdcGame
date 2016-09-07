using System;
using System.Collections.Generic;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Service.Bases
{
    public abstract class ServicesStarterBase<TBootstrapper> : IServicesStarter
        where TBootstrapper : IContainerBootstrapper
    {
        protected readonly IUnityContainer _container;

        protected ServicesStarterBase(TBootstrapper bootstrapper)
        {
            _container = bootstrapper.Configure(new UnityContainer());
            ServiceHosts = new Dictionary<string, ServiceHostInfo>();
        }

        protected IDictionary<string, ServiceHostInfo> ServiceHosts { get; private set; }

        public abstract void StopServices();

        public abstract void StartServices();

        protected void AddServiceHost(IServiceHost serviceHost, Uri endpointAddress)
        {
            ServiceHosts.Add(serviceHost.Name, new ServiceHostInfo
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

        protected void AddCustomService<TServiceHost>(Uri endpointAddress)
            where TServiceHost : IServiceHost
        {
            var service = Activator.CreateInstance(typeof (TServiceHost), _container) as IServiceHost;
            AddServiceHost(service, endpointAddress);
        }

        protected void OpenServices()
        {
            foreach (var service in ServiceHosts)
            {
                service.Value.ServiceHost.Open(new Uri(service.Value.EndpointAddress + service.Value.ServiceHost.Name));
                OnServiceOpened(service.Value.ServiceHost);
            }
        }

        protected void CloseServices()
        {
            foreach (var service in ServiceHosts)
            {
                service.Value.ServiceHost.Close();
            }
        }

        protected virtual void OnServiceOpened(IServiceHost service)
        {
        }

        protected class ServiceHostInfo
        {
            public IServiceHost ServiceHost { get; set; }

            public Uri EndpointAddress { get; set; }
        }
    }
}