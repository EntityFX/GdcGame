
namespace EntityFX.Gdcame.Application.Api.Common
{
    public class UserIdentity : IUserIdentity<string>
    {
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}