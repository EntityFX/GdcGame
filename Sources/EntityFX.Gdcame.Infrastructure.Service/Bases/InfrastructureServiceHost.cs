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
        protected InfrastructureServiceHost(IUnityContainer container)
        {
            Container = container;
        }

        protected virtual IUnityContainer Container { get; private set; }

        public void Dispose()
        {
            ((IDisposable) ServiceHost).Dispose();
        }

        public ServiceHost ServiceHost { get; private set; }

        public Uri Endpoint
        {
            get { return ServiceHost.BaseAddresses.First(); }
        }

        public string Name
        {
            get { return typeof (TServiceContract).FullName; }
        }

        public void Open(Uri endpointAddress)
        {
            ServiceHost = new UnityServiceHost(Container, Container.Resolve<TServiceContract>().GetType(),
                endpointAddress);
            CreateServiceEndpoint(ServiceHost);
            var serviceDebugBehavior = ServiceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (serviceDebugBehavior == null)
            {
                ServiceHost.Description.Behaviors
                    .Add(new ServiceDebugBehavior {IncludeExceptionDetailInFaults = true});
            }
            else
            {
                serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            }
            BeforeServiceOpen(ServiceHost);
            ServiceHost.Open();
        }

        public void Close()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
            }
        }

        private TBinding GetBinding()
        {
            var binding = GetBindingFactory().Build(null);
            ConfigureBinding(binding);
            return binding;
        }

        protected abstract IBindingFactory<TBinding> GetBindingFactory();

        protected virtual ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
            var endpoint = serviceHost.AddServiceEndpoint(typeof (TServiceContract), GetBinding(), string.Empty);
            return endpoint;
        }

        protected virtual void ConfigureBinding(TBinding binding)
        {
        }

        protected virtual void BeforeServiceOpen(ServiceHost serviceHost)
        {
        }
    }
}