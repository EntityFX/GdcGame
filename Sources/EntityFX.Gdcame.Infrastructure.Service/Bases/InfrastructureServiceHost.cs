using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace EntityFX.Gdcame.Infrastructure.Service.Bases
{
    public abstract class InfrastructureServiceHost<T> : IServiceHost, IDisposable
    {
        private ServiceHost _serviceHost;

        public ServiceHost ServiceHost
        {
            get
            {
                return _serviceHost;
            }
        }

        public Uri Endpoint
        {
            get
            {
                return _serviceHost.BaseAddresses.First();
            }
        }

        protected virtual IUnityContainer Container
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return typeof(T).FullName;
            }
        }

        protected InfrastructureServiceHost(IUnityContainer container)
        {
            Container = container;
        }
        
        protected virtual Binding GetBinding()
        {
            return new NetTcpBinding();
        }

        protected virtual ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
            var endpoint = serviceHost.AddServiceEndpoint(typeof(T), GetBinding(), string.Empty);
            return endpoint;
        }

        public void Open(Uri endpointAddress)
        {
            _serviceHost = new UnityServiceHost(Container, Container.Resolve<T>().GetType(), endpointAddress);
            CreateServiceEndpoint(_serviceHost);
            var serviceDebugBehavior = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (serviceDebugBehavior == null)
            {
                _serviceHost.Description.Behaviors
                    .Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            }
            BeforeServiceOpen(_serviceHost);
            _serviceHost.Open();
        }

        protected virtual void BeforeServiceOpen(ServiceHost serviceHost)
        {
            
        }

        public void Close()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }
        }

        public void Dispose()
        {
            ((IDisposable)_serviceHost).Dispose();
        }
    }
}
