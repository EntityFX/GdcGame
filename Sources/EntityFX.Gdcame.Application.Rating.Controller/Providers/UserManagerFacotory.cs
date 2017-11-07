using System;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManagerFacotory
    {
        private readonly IIocContainer _container;

        public ApplicationUserManagerFacotory(IIocContainer container)
        {
            this._container = container;
        }

        public UserManager<UserIdentity> Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return this._container.Resolve<UserManager<UserIdentity>>(null,
                new Tuple<string, object>("userStore", this._container.Resolve<IUserStore<UserIdentity>>()),
                new Tuple<string, object>("options", options), new Tuple<string, object>("context", context));
        }
    }
}