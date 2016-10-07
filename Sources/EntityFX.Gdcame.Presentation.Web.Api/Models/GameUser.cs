using Microsoft.AspNet.Identity;

namespace EntityFX.Gdcame.Application.WebApi.Models
{
    public class GameUser : IUser<string>
    {
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}