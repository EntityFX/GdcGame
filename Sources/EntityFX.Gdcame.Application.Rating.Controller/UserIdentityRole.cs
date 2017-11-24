using System.Security.Claims;

namespace EntityFX.Gdcame.Application.Api.Common
{
    public class UserIdentityRole
    {
        public string Id { get; internal set; }
        public string Name { get; set; }

        public Claim[] Claims { get; set; }
    }
}