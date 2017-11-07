using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    internal class SignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IIocContainer _container;

        public SignalRDependencyResolver(IIocContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
            //return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            //return _container.ResolveAll(serviceType);
            return base.GetServices(serviceType);
        }
    }
}