using EntityFX.EconomicsArcade.Presentation.GameApi.Models;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Presentation.GameApi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    namespace EntityFX.EconomicsArcade.Presentation.GameApi
    {
        // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

        public class ApplicationUserManagerFacrtory
        {
            private readonly IUnityContainer _container;

            public ApplicationUserManagerFacrtory(IUnityContainer container)
            {
                _container = container;
            }

            public UserManager<GameUser> Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            {
                return _container.Resolve<UserManager<GameUser>>(
                    new ParameterOverride("userStore", _container.Resolve<IUserStore<GameUser>>()),
                        new ParameterOverride("options", options), new ParameterOverride("context", context));
            }
        }
    }
}


       

