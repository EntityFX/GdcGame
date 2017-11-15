using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class AnonymousAuthContext<TContext> : IAuthContext<TContext>
    {
        public IApiContext<TContext> Context { get; set; }

        public Uri BaseUri { get; set; }


    }
}