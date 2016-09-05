using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public interface IAuthProvider<TAuthRequest, TAuthContext>
        where TAuthRequest : class
        where TAuthContext : class
    {
        Task<PasswordOAuthContext> Login(IAuthRequestData<TAuthRequest> authContext);
    }
}