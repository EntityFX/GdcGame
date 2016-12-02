using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthProvider<TAuthRequest, TAuthContext>
        where TAuthRequest : class
        where TAuthContext : class
    {
        Task<PasswordOAuthContext> Login(IAuthRequestData<TAuthRequest> authContext);
    }
}