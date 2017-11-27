using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class TokenAuthContext<TContext> : IAuthContext<TContext>
    {
        public IApiContext<TContext> Context { get; set; }
        public Uri BaseUri { get; set; }

        public string Token { get; set; }
    }
}