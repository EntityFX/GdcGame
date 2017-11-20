
namespace EntityFX.Gdcame.Application.Api.Common
{
    public interface IUserIdentity<TKey>
    {
        string Id { get; set; }
        string UserName { get; set; }
    }


    public class UserIdentity : IUserIdentity<string>
    {
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}