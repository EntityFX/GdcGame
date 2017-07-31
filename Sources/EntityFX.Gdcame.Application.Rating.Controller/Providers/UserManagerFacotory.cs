namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Practices.Unity;

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManagerFacotory
    {
        private readonly IUnityContainer _container;

        public ApplicationUserManagerFacotory(IUnityContainer container)
        {
            this._container = container;
        }

        public UserManager<UserIdentity> Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return this._container.Resolve<UserManager<UserIdentity>>(
                new ParameterOverride("userStore", this._container.Resolve<IUserStore<UserIdentity>>()),
                new ParameterOverride("options", options), new ParameterOverride("context", context));
        }
    }
}