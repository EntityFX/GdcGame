using System;
using System.Linq;
using Autofac;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure
{
    public class AutofacIocContainer : IocContainerBase<Autofac.ContainerBuilder>, IIocContainer<Autofac.ContainerBuilder, IContainer>
    {
        private ContainerBuilder _containerBuilder;

        public override ContainerBuilder ContainerBuilder
        {
            get { return _containerBuilder; }
        }

        public IContainer Container { get; private set; }

        public AutofacIocContainer(ContainerBuilder containerBuilderBuilder)
        {
            _containerBuilder = containerBuilderBuilder;
        }

        public void RegisterType<T>(ContainerScope scope = ContainerScope.Instance, string name = null)
        {
            var builder = _containerBuilder.RegisterType<T>();
            if (scope == ContainerScope.Singleton)
            {
                builder = builder.SingleInstance();
            }

            if (!string.IsNullOrEmpty(name))
            {
                builder.Named<T>(name);
            }
        }

        public void RegisterType<T>(Func<IResolver, T> factory, ContainerScope scope = ContainerScope.Instance, string name = null)
        {
            var builder = _containerBuilder.Register<T>((context, parameters) => factory(this));
            if (scope == ContainerScope.Singleton)
            {
                builder = builder.SingleInstance();
            }

            if (!string.IsNullOrEmpty(name))
            {
                builder.Named<T>(name);
            }
        }

        public void RegisterType<T, U>(ContainerScope scope = ContainerScope.Instance, string name = null) where U : T
        {
            var builder = _containerBuilder.RegisterType<U>().As<T>();
            if (scope == ContainerScope.Singleton)
            {
                builder = builder.SingleInstance();
            }

            if (!string.IsNullOrEmpty(name))
            {
                builder.Named<T>(name);
            }
        }

        public void RegisterType<T, U>(Func<IResolver, U> factory, ContainerScope scope = ContainerScope.Instance, string name = null) where U : T
        {
            var builder = _containerBuilder.Register<U>((context, parameters) => factory(this)).As<T>();
            if (scope == ContainerScope.Singleton)
            {
                builder = builder.SingleInstance();
            }

            if (!string.IsNullOrEmpty(name))
            {
                builder.Named<T>(name);
            }
        }

        public void RegisterType(Type source, Type destination)
        {
            var builder = _containerBuilder.RegisterType(destination).As(source);
        }

        public IContainer Configure()
        {
            Container = _containerBuilder.Build();
            return Container;
        }

        public T Resolve<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? Container.Resolve<T>() : Container.ResolveNamed<T>(name);
        }

        public T Resolve<T>(string name = null, params Tuple<string, object>[] constructorParameters)
        {
            NamedParameter[] parameters =
                constructorParameters.Select(p => new NamedParameter(p.Item1, p.Item2)).ToArray();

            return string.IsNullOrEmpty(name) ? Container.Resolve<T>(parameters) : Container.ResolveNamed<T>(name, parameters);
        }

        public object Resolve(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? Container.Resolve(type) : Container.ResolveNamed(name, type);
        }

        public object Resolve(Type type, string name = null, params object[] constructorParameters)
        {
            TypedParameter[] parameters =
                constructorParameters.Select(p => new TypedParameter(p.GetType(), p)).ToArray();

            return string.IsNullOrEmpty(name) ? Container.Resolve(type, parameters) : Container.ResolveNamed(name, type, parameters);
        }

        public void Dispose()
        {
        }
    }
}