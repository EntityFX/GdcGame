using System;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordOAuthContext : IAuthContext<IAuthenticator>
    {
        public IAuthenticator Context { get; set; }
        public Uri BaseUri { get; set; }

        public string OAuthToken { get; set; }
    }
}