using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthProvider<TAuthRequestData, TAuthContext>
        where TAuthRequestData : class
        where TAuthContext : class
    {
        Task<IAuthContext<TAuthContext>> Login(IAuthRequest<TAuthRequestData> authContext);
    }
}