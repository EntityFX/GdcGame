namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System;

    using EntityFX.Gdcame.Application.Api.Common;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationUserManager : UserManager<UserIdentity>, IDisposable
    {

        private static readonly PasswordValidator _passwordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false
            };

    public ApplicationUserManager(IUserStore<UserIdentity> userStore,
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
        }
    }
}