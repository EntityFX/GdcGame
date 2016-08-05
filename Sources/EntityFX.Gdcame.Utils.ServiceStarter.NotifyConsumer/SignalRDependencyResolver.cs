using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.NotifyConsumer
{
    class SignalRDependencyResolver : DefaultDependencyResolver
    {
        private IUnityContainer _container;

        public SignalRDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.Resolve(serviceType);
            else return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.ResolveAll(serviceType);
            else return base.GetServices(serviceType);
        }
    }
}