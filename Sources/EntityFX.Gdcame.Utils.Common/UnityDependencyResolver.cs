using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;

namespace EntityFX.Gdcame.Utils.Common
{
    public class UnityDependencyProvider : IServiceProvider
    {
        private bool _disposed;
        protected IUnityContainer Container { get; private set; }

        public UnityDependencyProvider(IUnityContainer container)
        {
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
                return this.Container.Resolve(serviceType);
        }
    }


    public class UnityControllerActivator : IControllerActivator
    {
        private IUnityContainer _unityContainer;

        public UnityControllerActivator(IUnityContainer container)
        {
            _unityContainer = container;
        }

        #region Implementation of IControllerActivator

        public object Create(ControllerContext context)
        {
            return _unityContainer.Resolve(context.ActionDescriptor.ControllerTypeInfo.AsType());
        }


        public void Release(ControllerContext context, object controller)
        {
            //ignored
        }

        #endregion
    }

    public class UnityFallbackProviderExtension : UnityContainerExtension
    {
        #region Const

        ///Used for Resolving the Default Container inside the UnityFallbackProviderStrategy class
        public const string FALLBACK_PROVIDER_NAME = "UnityFallbackProvider";

        #endregion

        #region Vars

        // The default Service Provider so I can Register it to the IUnityContainer
        private IServiceProvider _defaultServiceProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the UnityFallbackProviderExtension class
        /// </summary>
        /// <param name="defaultServiceProvider">The default Provider used to fall back to</param>
        public UnityFallbackProviderExtension(IServiceProvider defaultServiceProvider)
        {
            _defaultServiceProvider = defaultServiceProvider;
        }

        #endregion

        #region Overrides of UnityContainerExtension

        /// <summary>
        /// Initializes the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        /// <see cref="T:Microsoft.Practices.Unity.ExtensionContext" /> by adding strategies, policies, etc. to
        /// install it's functions into the container.</remarks>
        protected override void Initialize()
        {
            // Register the default IServiceProvider with a name.
            // Now the UnityFallbackProviderStrategy can Resolve the default Provider if needed
            Context.Container.RegisterInstance(FALLBACK_PROVIDER_NAME, _defaultServiceProvider);

            // Create the UnityFallbackProviderStrategy with our UnityContainer
            var strategy = new UnityFallbackProviderStrategy(Context.Container);

            // Adding the UnityFallbackProviderStrategy to be executed with the PreCreation LifeCycleHook
            // PreCreation because if it isnt registerd with the IUnityContainer there will be an Exception
            // Now if the IUnityContainer "magically" gets a Instance of a Type it will accept it and move on
            Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }

        #endregion
    }

    public class UnityFallbackProviderStrategy : BuilderStrategy
    {
        private IUnityContainer _container;

        public UnityFallbackProviderStrategy(IUnityContainer container)
        {
            _container = container;
        }

        #region Overrides of BuilderStrategy

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            NamedTypeBuildKey key = context.OriginalBuildKey;

            // Checking if the Type we are resolving is registered with the Container
            if (!_container.IsRegistered(key.Type))
            {
                // If not we first get our default IServiceProvider and then try to resolve the type with it
                // Then we save the Type in the Existing Property of IBuilderContext to tell Unity
                // that it doesnt need to resolve the Type
                context.Existing = _container.Resolve<IServiceProvider>(UnityFallbackProviderExtension.FALLBACK_PROVIDER_NAME).GetService(key.Type);
            }

            // Otherwise we do the default stuff
            base.PreBuildUp(context);
        }

        #endregion
    }
}