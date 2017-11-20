using System;
using EntityFX.Gdcame.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.


    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManagerFacotory
    {
        private readonly IIocContainer _container;

        public ApplicationUserManagerFacotory(IIocContainer container)
        {
            this._container = container;
        }

        public UserManager<UserIdentity> Create()
        {
            return this._container.Resolve<UserManager<UserIdentity>>();
        }
    }
}