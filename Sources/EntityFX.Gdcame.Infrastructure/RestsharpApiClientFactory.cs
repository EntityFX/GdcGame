using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpApiClientFactory : IApiClientFactory<IAuthenticator>
    {
        public IApiClient Build(IAuthContext<IAuthenticator> context)
        {
            return new RestsharpApiAdapter(context);
        }
    }
}