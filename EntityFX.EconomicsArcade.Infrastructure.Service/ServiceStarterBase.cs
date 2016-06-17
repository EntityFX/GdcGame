using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class ServiceStarterBase<TBootstrapper> : IServiceStarter
        where TBootstrapper : IContainerBootstrapper, new()
    {
        private readonly IUnityContainer _container;
        private IDictionary<string, IServiceHost> _serviceHosts = new Dictionary<string, IServiceHost>();

        public ServiceStarterBase(TBootstrapper bootstrapper)
        {
            _container = bootstrapper.Configure(new UnityContainer());
        }

        protected void AddNetTcpService<T>() where T : class
        {
            var _service = new NetTcpServiceHost<T>(_container);
            _serviceHosts.Add(_service.Name, _service);
        }
        
        protected void OpenServices(Uri baseAddress)
        {
            foreach (var service in _serviceHosts)
            {
                service.Value.Open(baseAddress);
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

        public void StopService()
        {
            throw new NotImplementedException();
        }

        public void StartService()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnServiceOpened(IServiceHost service) {

        }
    }
}
