using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Platform;
using Unity;

namespace EntityFX.Gdcame.Utils.Common
{
    public class UnityDependencyScope : IDependencyScope, IDisposable
    {
        private bool _disposed;
        protected IUnityContainer Container { get; private set; }

        public UnityDependencyScope(IUnityContainer container)
        {
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IHttpController).IsAssignableFrom(serviceType))
                return this.Container.Resolve(serviceType);
            try
            {
                return this.Container.Resolve(serviceType);
            }
            catch
            {
                return (object) null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.ResolveAll(serviceType);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Container.Dispose();
        }
    }

    public class UnityDependencyResolver : UnityDependencyScope, IDependencyResolver, IDependencyScope, IDisposable
    {
        public UnityDependencyResolver(IUnityContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            return (IDependencyScope) new UnityDependencyScope(this.Container.CreateChildContainer());
        }
    }

}