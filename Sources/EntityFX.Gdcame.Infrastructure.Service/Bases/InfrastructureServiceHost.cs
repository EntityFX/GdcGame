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
	public abstract class InfrastructureServiceHost<TServiceContract, TBinding> : IServiceHost, IDisposable
		where TBinding : Binding
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
				return typeof(TServiceContract).FullName;
            }
        }

        protected InfrastructureServiceHost(IUnityContainer container)
        {
            Container = container;
        }
        
		private TBinding GetBinding()
        {
			var binding = GetBindingFactory ().Build (null);
			ConfigureBinding (binding);
			return binding;
        }

		protected abstract IBindingFactory<TBinding> GetBindingFactory ();

        protected virtual ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
			var endpoint = serviceHost.AddServiceEndpoint(typeof(TServiceContract), GetBinding(), string.Empty);
            return endpoint;
        }

		protected virtual void ConfigureBinding(TBinding binding) 
		{
		}

        public void Open(Uri endpointAddress)
        {
			_serviceHost = new UnityServiceHost(Container, Container.Resolve<TServiceContract>().GetType(), endpointAddress);
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
