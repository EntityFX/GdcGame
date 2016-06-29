using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IGameManager, GameManagerClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], typeof(Guid)));
            container.RegisterType<ISimpleUserManager, SimpleUserManagerClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]));
            container.RegisterType<SessionManagerClient, SessionManagerClient>(
                new InjectionConstructor(
                ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"]));
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
        }
    }
}
