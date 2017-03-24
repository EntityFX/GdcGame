using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class EmptyAuthContext : IAuthContext<object>
    {
        public object Context { get; set; }
        public Uri BaseUri { get; set; }
    }
}