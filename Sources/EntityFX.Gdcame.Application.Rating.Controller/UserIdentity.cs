namespace EntityFX.Gdcame.Application.Api.Common
{
    using Microsoft.AspNet.Identity;

    public class UserIdentity : IUser<string>
    {
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}