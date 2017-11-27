using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpAuthenticatorContext : IApiContext<IAuthenticator>
    {
        public IAuthenticator ApiContext { get; set; }
    }
}