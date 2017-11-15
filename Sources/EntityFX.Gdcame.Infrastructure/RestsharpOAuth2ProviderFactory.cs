using System;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Common;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpOAuth2ProviderFactory : IAuthProviderFactory<PasswordOAuth2RequestData, IAuthenticator>
    {
        private readonly ILogger _logger;

        public RestsharpOAuth2ProviderFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IAuthProvider<PasswordOAuth2RequestData, IAuthenticator> Build(Uri address)
        {
            return new RestsharpPasswordOAuth2Provider(address, _logger);
        }
    }
}