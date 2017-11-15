using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthContext<TContext>
    {
        IApiContext<TContext> Context { get; set; }

        Uri BaseUri { get; set; }
    }

    public interface IApiContext<TContext>
    {
        TContext ApiContext { get; set; }
    }
}