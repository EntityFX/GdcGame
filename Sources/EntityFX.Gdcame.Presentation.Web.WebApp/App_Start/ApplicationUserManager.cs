using EntityFX.Gdcame.Presentation.Web.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EntityFX.Gdcame.Presentation.Web.WebApp
{
    public class ApplicationUserManager : UserManager<GameUser>
    {
        private readonly IUserStore<GameUser> _userStore;

        public ApplicationUserManager(IUserStore<GameUser> userStore,
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            : base(userStore)
        {
            _userStore = userStore;


            // Configure validation logic for usernames
            UserValidator = new UserValidator<GameUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<GameUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

    }
}