using System;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Common;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpOAuth2ProviderFactory : IAuthProviderFactory<PasswordAuthRequestData, IAuthenticator>
    {
        private readonly ILogger _logger;

        public RestsharpOAuth2ProviderFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IAuthProvider<PasswordAuthRequestData, IAuthenticator> Build(Uri address)
        {
            return new RestsharpPasswordAuthProvider(address, _logger);
        }
    }
}