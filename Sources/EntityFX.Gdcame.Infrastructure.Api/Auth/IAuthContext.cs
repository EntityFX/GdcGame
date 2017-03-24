using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthContext<TContext>
        where TContext : class
    {
        TContext Context { get; set; }

        Uri BaseUri { get; set; }
    }
}