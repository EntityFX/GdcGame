using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Unity;
using Unity.Builder;
using Unity.Builder.Selection;
using Unity.Builder.Strategy;
using Unity.Container.Registration;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Policy;
using Unity.Registration;
using Unity.Strategy;

namespace EntityFX.Gdcame.Utils.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnity(this IServiceCollection services, Action<IUnityContainer> configurationAction = null)
        {
            return services.AddSingleton<IServiceProviderFactory<IUnityContainer>>(new UnityServiceProviderFactory(configurationAction));
        }
    }

    public class UnityServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private readonly IUnityContainer _container;

        public UnityServiceProvider(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.TryResolve(serviceType);
        }

        public object GetRequiredService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }

    internal class UnityServiceProviderFactory : IServiceProviderFactory<IUnityContainer>
    {
        private readonly Action<IUnityContainer> _configurationAction;

        public UnityServiceProviderFactory(Action<IUnityContainer> configurationAction = null)
        {
            _configurationAction = configurationAction ?? (container => { });
        }

        public IUnityContainer CreateBuilder(IServiceCollection serviceCollection)
        {
            var builder = new UnityContainer();

            builder.Populate(serviceCollection);

            _configurationAction(builder);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(IUnityContainer unityContainer)
        {
            return new UnityServiceProvider(unityContainer);
        }
    }

    internal class UnityServiceScope : IServiceScope
    {
        private readonly IUnityContainer _container;

        public UnityServiceScope(IUnityContainer container)
        {
            _container = container;
            ServiceProvider = new UnityServiceProvider(container);
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            _container.Dispose();
        }
    }

    internal class UnityServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceScopeFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            return new UnityServiceScope(_container.CreateChildContainer().AddExtensions());
        }
    }

    public static class UnityContainerUserExtensions
    {
        public static void Populate(this IUnityContainer container, IEnumerable<ServiceDescriptor> descriptors)
        {
            container.AddExtensions();
            container.RegisterType<IServiceProvider, UnityServiceProvider>();
            container.RegisterType<IServiceScopeFactory, UnityServiceScopeFactory>();

            var aggregateTypes = new HashSet<Type>(
                descriptors
                    .GroupBy(serviceDescriptor => serviceDescriptor.ServiceType, serviceDescriptor => serviceDescriptor)
                    .Where(typeGrouping => typeGrouping.Count() > 1)
                    .Select(type => type.Key)
            );

            foreach (var serviceDescriptor in descriptors)
            {
                var isAggregateType = aggregateTypes.Contains(serviceDescriptor.ServiceType);

                if (serviceDescriptor.ImplementationType != null)
                {
                    container.RegisterImplementation(serviceDescriptor, isAggregateType);
                }
                else if (serviceDescriptor.ImplementationFactory != null)
                {
                    container.RegisterFactory(serviceDescriptor, isAggregateType);
                }
                else if (serviceDescriptor.ImplementationInstance != null)
                {
                    container.RegisterSingleton(serviceDescriptor, isAggregateType);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported registration type");
                }
            }
        }

        public static IUnityContainer AddExtensions(this IUnityContainer container)
        {
            container.AddExtension(new EnumerableResolutionUnityExtension());
            container.AddExtension(new ConstructorSelectionUnityExtension());
            container.AddExtension(new DisposeExtension());

            return container;
        }

        internal static bool CanResolve(this IUnityContainer container, Type type)
        {
            if (type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract)
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType)
            {
                var gerericType = type.GetGenericTypeDefinition();
                if ((gerericType == typeof(IEnumerable<>)) ||
                    gerericType.GetTypeInfo().IsClass ||
                    container.IsRegistered(gerericType))
                {
                    return true;
                }
            }

            return container.IsRegistered(type);
        }

        internal static T TryResolve<T>(this IUnityContainer container)
        {
            var result = TryResolve(container, typeof(T));

            if (result != null)
            {
                return (T)result;
            }

            return default(T);
        }

        internal static object TryResolve(this IUnityContainer container, Type typeToResolve)
        {
            try
            {
                return container.Resolve(typeToResolve);
            }
            catch
            {
                return null;
            }
        }

        internal static object TryResolve(this IUnityContainer container, Type typeToResolve, string name)
        {
            try
            {
                return container.Resolve(typeToResolve, name);
            }
            catch
            {
                return null;
            }
        }

        private static void RegisterImplementation(this IUnityContainer container, ServiceDescriptor serviceDescriptor, bool isAggregateType)
        {
            if (isAggregateType)
            {
                container.RegisterType(
                    serviceDescriptor.ServiceType,
                    serviceDescriptor.ImplementationType,
                    serviceDescriptor.ImplementationType.AssemblyQualifiedName,
                    serviceDescriptor.Lifetime.ToUnityLifetimeManager());
            }

            container.RegisterType(
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationType,
                serviceDescriptor.Lifetime.ToUnityLifetimeManager());
        }

        private static void RegisterFactory(this IUnityContainer container, ServiceDescriptor serviceDescriptor, bool isAggregateType)
        {
            if (isAggregateType)
            {
                container.RegisterType(
                    serviceDescriptor.ServiceType,
                    serviceDescriptor.ImplementationFactory.GetType().AssemblyQualifiedName,
                    serviceDescriptor.Lifetime.ToUnityLifetimeManager(),
                    new InjectionFactory(
                        unityContainer =>
                        {
                            var serviceProvider = new UnityServiceProvider(unityContainer);
                            var instance = serviceDescriptor.ImplementationFactory(serviceProvider);
                            return instance;
                        }));
            }

            container.RegisterType(
                serviceDescriptor.ServiceType,
                serviceDescriptor.Lifetime.ToUnityLifetimeManager(),
                new InjectionFactory(
                    unityContainer =>
                    {
                        var serviceProvider = new UnityServiceProvider(unityContainer);
                        var instance = serviceDescriptor.ImplementationFactory(serviceProvider);
                        return instance;
                    }));
        }

        private static void RegisterSingleton(this IUnityContainer container, ServiceDescriptor serviceDescriptor, bool isAggregateType)
        {
            if (isAggregateType)
            {
                var name = Guid.NewGuid().ToString();
                if (serviceDescriptor.ImplementationType != null)
                {
                    name = serviceDescriptor.ImplementationType.AssemblyQualifiedName;
                }
                else if (serviceDescriptor.ImplementationInstance != null)
                {
                    name = serviceDescriptor.ImplementationInstance.GetType().AssemblyQualifiedName;
                }

                container.RegisterInstance(
                    serviceDescriptor.ServiceType,
                    name,
                    serviceDescriptor.ImplementationInstance,
                    serviceDescriptor.Lifetime.ToUnityLifetimeManager());
            }

            container.RegisterInstance(
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationInstance,
                serviceDescriptor.Lifetime.ToUnityLifetimeManager());
        }
    }

    internal static class ServiceLifetimeExtensions
    {
        internal static LifetimeManager ToUnityLifetimeManager(this ServiceLifetime lifecycle)
        {
            switch (lifecycle)
            {
                case ServiceLifetime.Transient:
                    return new TransientLifetimeManager();
                case ServiceLifetime.Singleton:
                    return new ContainerControlledLifetimeManager();
                case ServiceLifetime.Scoped:
                    return new HierarchicalLifetimeManager();
            }

            return new TransientLifetimeManager();
        }
    }

    internal class DerivedTypeResolutionUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new CustomBuilderStrategy(), UnityBuildStage.PreCreation);
        }

        internal sealed class CustomBuilderStrategy : BuilderStrategy
        {
            public override void PreBuildUp(IBuilderContext context)
            {
                if (context.Existing != null)
                {
                    return;
                }

                var originalSelectorPolicy = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out IPolicyList selectorPolicyDestination);

                selectorPolicyDestination.Set<IConstructorSelectorPolicy>(
                    new DerivedTypeConstructorSelectorPolicy(GetUnityFromBuildContext(context), originalSelectorPolicy),
                    context.BuildKey);
            }

            private IUnityContainer GetUnityFromBuildContext(IBuilderContext context)
            {
                var lifetime = context.Policies.Get<ILifetimePolicy>(NamedTypeBuildKey.Make<IUnityContainer>());
                return lifetime.GetValue() as IUnityContainer;
            }

            private class DerivedTypeConstructorSelectorPolicy : IConstructorSelectorPolicy
            {
                private readonly IUnityContainer _container;
                private readonly IConstructorSelectorPolicy _originalConstructorSelectorPolicy;

                public DerivedTypeConstructorSelectorPolicy(IUnityContainer container, IConstructorSelectorPolicy originalSelectorPolicy)
                {
                    _originalConstructorSelectorPolicy = originalSelectorPolicy;
                    _container = container;
                }

                public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
                {
                    var originalConstructor = _originalConstructorSelectorPolicy.SelectConstructor(context, resolverPolicyDestination);

                    if (originalConstructor.Constructor.GetParameters().All(arg => _container.CanResolve(arg.ParameterType)))
                    {
                        return originalConstructor;
                    }

                    var newSelectedConstructor = FindNewCtor(originalConstructor);
                    if (newSelectedConstructor == null)
                    {
                        return originalConstructor;
                    }

                    foreach (var newParameterResolver in originalConstructor.GetParameterResolvers().Take(newSelectedConstructor.Constructor.GetParameters().Length))
                    {
                        newSelectedConstructor.AddParameterResolver(newParameterResolver);
                    }

                    return newSelectedConstructor;
                }

                private SelectedConstructor FindNewCtor(SelectedConstructor originalConstructor)
                {
                    var implementingType = originalConstructor.Constructor.DeclaringType;
                    var constructors = implementingType.GetTypeInfo()
                        .DeclaredConstructors
                        .Where(constructor => constructor.IsPublic && (constructor != originalConstructor.Constructor))
                        .ToArray();

                    if (constructors.Length == 0)
                    {
                        return null;
                    }

                    if (constructors.Length == 1)
                    {
                        return new SelectedConstructor(constructors[0]);
                    }

                    var newCtor = constructors
                        .OrderByDescending(c => c.GetParameters().Length)
                        .FirstOrDefault(c => c.GetParameters().All(arg => _container.CanResolve(arg.ParameterType)));

                    if (newCtor == null)
                    {
                        return null;
                    }

                    return new SelectedConstructor(newCtor);
                }
            }
        }
    }

    internal class EnumerableResolutionUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<EnumerableResolutionStrategy>(UnityBuildStage.TypeMapping);
        }

        /// <summary>
        ///     This strategy implements the logic that will return all instances
        ///     when an <see cref="IEnumerable{T}" /> parameter is detected.
        /// </summary>
        /// <remarks>
        ///     Nicked from
        ///     https://piotr-wlodek-code-gallery.googlecode.com/svn-history/r40/trunk/Unity.Extensions/Unity.Extensions/EnumerableResolutionStrategy.cs
        /// </remarks>
        internal class EnumerableResolutionStrategy : BuilderStrategy
        {
            private static readonly MethodInfo GenericResolveEnumerableMethod =
                typeof(EnumerableResolutionStrategy).GetMethod(
                    nameof(ResolveEnumerable),
                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

            private static readonly MethodInfo GenericResolveLazyEnumerableMethod =
                typeof(EnumerableResolutionStrategy).GetMethod(
                    nameof(ResolveLazyEnumerable),
                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);

            private static Type GetTypeToBuild(Type type)
            {
                return type.GetGenericArguments()[0];
            }

            private static bool IsResolvingIEnumerable(Type type)
            {
                return type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            }

            private static bool IsResolvingLazy(Type type)
            {
                return type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == typeof(Lazy<>));
            }

            private static object ResolveLazyEnumerable<T>(IBuilderContext context)
            {
                var container = context.NewBuildUp<IUnityContainer>();

                var typeToBuild = typeof(T);
                var typeWrapper = typeof(Lazy<T>);

                return ResolveAll(container, typeToBuild, typeWrapper).OfType<Lazy<T>>().ToList();
            }

            private static object ResolveEnumerable<T>(IBuilderContext context)
            {
                var container = context.NewBuildUp<IUnityContainer>();

                var typeToBuild = typeof(T);

                return ResolveAll(container, typeToBuild, typeToBuild).OfType<T>().ToList();
            }

            private static IEnumerable<object> ResolveAll(IUnityContainer container, Type type, Type typeWrapper)
            {
                var names = GetRegisteredNames(container, type);

                if (type.GetTypeInfo().IsGenericType)
                {
                    names = names.Concat(GetRegisteredNames(container, type.GetGenericTypeDefinition()));
                }

                return names
                    .GroupBy(t => t.MappedToType)
                    .Select(t => t.First())
                    .Select(t => t.Name)
                    .Select(name => container.TryResolve(typeWrapper, name))
                    .Where(x => x != null);
            }

            private static IEnumerable<IContainerRegistration> GetRegisteredNames(IUnityContainer container, Type type)
            {
                return container.Registrations.Where(t => t.RegisteredType == type);
            }

            /// <summary>
            ///     Do the PreBuildUp stage of construction. This is where the actual work is performed.
            /// </summary>
            /// <param name="context">Current build context.</param>
            public override void PreBuildUp(IBuilderContext context)
            {
                //Guard.ArgumentNotNull(context, "context");

                if (!IsResolvingIEnumerable(context.BuildKey.Type))
                {
                    return;
                }

                MethodInfo resolverMethod;
                var typeToBuild = GetTypeToBuild(context.BuildKey.Type);

                if (IsResolvingLazy(typeToBuild))
                {
                    typeToBuild = GetTypeToBuild(typeToBuild);
                    resolverMethod = GenericResolveLazyEnumerableMethod.MakeGenericMethod(typeToBuild);
                }
                else
                {
                    resolverMethod = GenericResolveEnumerableMethod.MakeGenericMethod(typeToBuild);
                }

                var resolver = (Resolver)resolverMethod.CreateDelegate(typeof(Resolver), resolverMethod);
                context.Existing = resolver(context);
                context.BuildComplete = true;
            }

            private delegate object Resolver(IBuilderContext context);
        }
    }

    public class DisposeExtension : UnityContainerExtension, IDisposable
    {
        private DisposeStrategy _strategy = new DisposeStrategy();

        public void Dispose()
        {
            _strategy.Dispose();
            _strategy = null;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(_strategy, UnityBuildStage.TypeMapping);
        }

        private class DisposeStrategy : BuilderStrategy, IDisposable
        {
            private readonly List<IDisposable> _disposables = new List<IDisposable>();

            public void Dispose()
            {
                lock (_disposables)
                {
                    foreach (var item in _disposables)
                    {
                        item.Dispose();
                    }

                    _disposables.Clear();
                }
            }

            public override void PostBuildUp(IBuilderContext context)
            {
                var activeLifetime = context.PersistentPolicies.Get<ILifetimePolicy>(context.BuildKey, out IPolicyList lifetimePolicySource);

                var instance = context.Existing as IDisposable;

                if (instance != null
                    && !IsIDisposableInLifetimeContainer()
                    && !IsControlledByParrent()
                    && !IsInheritedStrategy()
                    && !IsCurrentUnityUnityContainer())
                {
                    lock (_disposables)
                    {
                        _disposables.Add(instance);
                    }
                }

                base.PostBuildUp(context);

                bool IsIDisposableInLifetimeContainer()
                {
                    // all IDisposable in lifitimemanager dipose when unitycontainer dispose
                    return activeLifetime is IDisposable && context.Lifetime.Contains(activeLifetime);
                }

                bool IsCurrentUnityUnityContainer()
                {
                    var lifetime = context.Policies.Get<ILifetimePolicy>(NamedTypeBuildKey.Make<IUnityContainer>());
                    return ReferenceEquals(lifetime.GetValue() as IDisposable, instance);
                }

                bool IsControlledByParrent()
                {
                    return activeLifetime is ContainerControlledLifetimeManager
                           &&
                           !ReferenceEquals(lifetimePolicySource, context.PersistentPolicies);
                }

                bool IsInheritedStrategy()
                {
                    // unity container puts the parent container strategies before child strategies when it builds the chain
                    var lastStrategy = context.Strategies.LastOrDefault(s => s is DisposeStrategy);
                    return !ReferenceEquals(this, lastStrategy);
                }
            }
        }
    }

    internal class ConstructorSelectionUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new CustomBuilderStrategy(), UnityBuildStage.PreCreation);
        }

        internal sealed class CustomBuilderStrategy : BuilderStrategy
        {
            public override void PreBuildUp(IBuilderContext context)
            {
                if (context.Existing != null)
                {
                    return;
                }

                var originalSelectorPolicy = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out IPolicyList selectorPolicyDestination);

                selectorPolicyDestination.Set<IConstructorSelectorPolicy>(
                    new DerivedTypeConstructorSelectorPolicy(GetUnityFromBuildContext(context), originalSelectorPolicy),
                    context.BuildKey);
            }

            private IUnityContainer GetUnityFromBuildContext(IBuilderContext context)
            {
                var lifetime = context.Policies.Get<ILifetimePolicy>(NamedTypeBuildKey.Make<IUnityContainer>());
                return lifetime.GetValue() as IUnityContainer;
            }

            private class DerivedTypeConstructorSelectorPolicy : IConstructorSelectorPolicy
            {
                private readonly IUnityContainer _container;
                private readonly IConstructorSelectorPolicy _originalConstructorSelectorPolicy;

                public DerivedTypeConstructorSelectorPolicy(IUnityContainer container, IConstructorSelectorPolicy originalSelectorPolicy)
                {
                    _originalConstructorSelectorPolicy = originalSelectorPolicy;
                    _container = container;
                }

                public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
                {
                    var originalConstructor = _originalConstructorSelectorPolicy.SelectConstructor(context, resolverPolicyDestination);

                    if (originalConstructor.Constructor.GetParameters().All(arg => _container.CanResolve(arg.ParameterType)))
                    {
                        return originalConstructor;
                    }

                    var implementingType = originalConstructor.Constructor.DeclaringType;
                    var bestConstructor = implementingType.GetTypeInfo()
                        .DeclaredConstructors
                        .Select(ctor => new { Constructor = ctor, Parameters = ctor.GetParameters() })
                        .OrderByDescending(x => x.Parameters.Length)
                        .FirstOrDefault(
                            _ => _.Constructor.IsPublic
                                 && _.Constructor != originalConstructor.Constructor
                                 && _.Parameters.All(arg => _container.CanResolve(arg.ParameterType)));

                    if (bestConstructor == null)
                    {
                        return originalConstructor;
                    }

                    var newSelectedConstructor = new SelectedConstructor(bestConstructor.Constructor);

                    foreach (var newParameterResolver in originalConstructor.GetParameterResolvers().Take(bestConstructor.Parameters.Length))
                    {
                        newSelectedConstructor.AddParameterResolver(newParameterResolver);
                    }

                    return newSelectedConstructor;
                }
            }
        }
    }
}
    