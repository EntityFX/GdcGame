using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordOAuth2Context<TContext> : IAuthContext<TContext>
    {
        public IApiContext<TContext> Context { get; set; }
        public Uri BaseUri { get; set; }

        public string OAuthToken { get; set; }
    }
}