using Microsoft.AspNet.Identity;

namespace EntityFX.EconomicsArcade.Presentation.GameApi.Models
{
    public class GameUser : IUser<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
    }
}