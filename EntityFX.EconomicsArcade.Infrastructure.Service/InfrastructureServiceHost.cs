using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Unity.Wcf;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class InfrastructureServiceHost<T> : IServiceHost, IDisposable
    {
        private ServiceHost _serviceHost;

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

        public InfrastructureServiceHost(IUnityContainer container)
        {
            Container = container;
        }
        
        protected virtual Binding GetBinding()
        {
            return new NetTcpBinding();
        }

        protected virtual ServiceEndpoint CreateServiceEndpoint(ServiceHost serviceHost)
        {
            return serviceHost.AddServiceEndpoint(typeof(T), GetBinding(), string.Empty);
        }

        public void Open(Uri endpointAddress)
        {
            _serviceHost = new UnityServiceHost(Container, Container.Resolve<T>().GetType(), new Uri[] { endpointAddress });
            CreateServiceEndpoint(_serviceHost);
            _serviceHost.Open();
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
