using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System;

    using EntityFX.Gdcame.Application.Api.Common;


    public class ApplicationUserManager : UserManager<UserIdentity>, IDisposable
    {

        private static readonly PasswordValidator<UserIdentity> _passwordValidator =
            new PasswordValidator<UserIdentity>();

    /*public ApplicationUserManager(IUserStore<UserIdentity> userStore,
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            : base(userStore)
        {

            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<UserIdentity>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            this.PasswordValidator = _passwordValidator;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider =
                    new DataProtectorTokenProvider<UserIdentity>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }*/
        public ApplicationUserManager(IUserStore<UserIdentity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<UserIdentity> passwordHasher, IEnumerable<IUserValidator<UserIdentity>> userValidators, IEnumerable<IPasswordValidator<UserIdentity>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserIdentity>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
    }
}