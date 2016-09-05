using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public interface IAuthContext<TContext>
        where TContext : class
    {
        TContext Context { get; set; }

        Uri BaseUri { get; set; }
    }
}