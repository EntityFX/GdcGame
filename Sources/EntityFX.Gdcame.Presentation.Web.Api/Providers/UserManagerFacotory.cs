using EntityFX.Gdcame.Application.Api.MainServer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Application.Api.MainServer.Providers
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManagerFacotory
    {
        private readonly IUnityContainer _container;

        public ApplicationUserManagerFacotory(IUnityContainer container)
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