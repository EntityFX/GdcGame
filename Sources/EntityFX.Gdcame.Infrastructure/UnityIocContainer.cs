﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using Unity;
using Unity.Resolution;
using Unity.Injection;

namespace EntityFX.Gdcame.Infrastructure
{
    public class UnityIocContainer : IocContainerBase<IUnityContainer>, IIocContainer<IUnityContainer, IUnityContainer>
    {
        private readonly IUnityContainer _unityContainer;

        public override IUnityContainer ContainerBuilder
        {
            get
            {
                return _unityContainer;
            }
        }

        public UnityIocContainer(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            _unityContainer.RegisterInstance<IIocContainer>(this);
        }

        public IUnityContainer Configure()
        {
            return ContainerBuilder;
        }

        public T Resolve<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? _unityContainer.Resolve<T>() : _unityContainer.Resolve<T>(name);
        }

        public T Resolve<T>(string name = null, params Tuple<string, object>[] constructorParameters)
        {
            var dependencyOverrides = constructorParameters.Select(i => new ParameterOverride(i.Item1, i.Item2)).ToArray();
            return string.IsNullOrEmpty(name) ? _unityContainer.Resolve<T>(dependencyOverrides) : _unityContainer.Resolve<T>(name, dependencyOverrides);
        }

        public object Resolve(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? _unityContainer.Resolve(type) : _unityContainer.Resolve(type, name);
        }

        public object Resolve(Type type, string name = null, params object[] constructorParameters)
        {
            var dependencyOverrides = constructorParameters.Select(i => new DependencyOverride(i.GetType(), i)).ToArray();
            return string.IsNullOrEmpty(name) ? _unityContainer.Resolve(type, dependencyOverrides) : _unityContainer.Resolve(type, name, dependencyOverrides);
        }

        public void RegisterType<T>(ContainerScope scope = ContainerScope.Instance, string name = null)
        {
            if (scope == ContainerScope.Instance)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterType<T>();
                }
                else
                {
                    _unityContainer.RegisterType<T>(name);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterInstance(_unityContainer.Resolve(typeof(T)));
                }
                else
                {
                    _unityContainer.RegisterInstance(name, _unityContainer.Resolve(typeof(T)));
                }
            }
        }

        public void RegisterType<T>(Func<IResolver, T> factory, ContainerScope scope = ContainerScope.Instance, string name = null)
        {
            if (scope == ContainerScope.Instance)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterType<T>(new InjectionFactory(container => factory(this)));
                }
                else
                {
                    _unityContainer.RegisterType<T>(name, new InjectionFactory(container => factory(this)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterInstance<T>(factory(this));
                }
                else
                {
                    _unityContainer.RegisterInstance<T>(factory(this));
                }
            }
        }

        public void RegisterType<T, U>(ContainerScope scope = ContainerScope.Instance, string name = null) where U : T
        {
            if (scope == ContainerScope.Instance)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterType<T, U>();
                }
                else
                {
                    _unityContainer.RegisterType<T, U>(name);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterInstance(typeof(T),typeof(U));
                }
                else
                {
                    _unityContainer.RegisterInstance(typeof(T), name, typeof(U));
                }
            }
        }

        public void RegisterType<T, U>(Func<IResolver, U> factory, ContainerScope scope = ContainerScope.Instance, string name = null) where U : T
        {
            if (scope == ContainerScope.Instance)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterType<T, U>(new InjectionFactory(container => factory(this)));
                }
                else
                {
                    _unityContainer.RegisterType<T, U>(name, new InjectionFactory(container => factory(this)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                {
                    _unityContainer.RegisterInstance(typeof(T), factory(this));
                }
                else
                {
                    _unityContainer.RegisterInstance(typeof(T), name, factory(this));
                }
            }
        }

        public void RegisterType(Type source, Type destination)
        {
            _unityContainer.RegisterType(source, destination);
        }

        public void Dispose()
        {
            _unityContainer?.Dispose();
        }
    }
}
