using EntityFX.Gdcame.Application.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EntityFX.Gdcame.Application.WebApi.Providers
{
    public class ApplicationUserManager : UserManager<GameUser>
    {

        private static readonly PasswordValidator _passwordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false
            };

    public ApplicationUserManager(IUserStore<GameUser> userStore,
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            : base(userStore)
        {

            // Configure validation logic for usernames
            UserValidator = new UserValidator<GameUser>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = _passwordValidator;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<GameUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}