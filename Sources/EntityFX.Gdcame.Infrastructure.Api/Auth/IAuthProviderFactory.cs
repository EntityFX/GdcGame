using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthProviderFactory<TAuthRequestData, TAuthContext>
        where TAuthRequestData : class
        where TAuthContext : class
    {
        IAuthProvider<TAuthRequestData, TAuthContext> Build(Uri address);
    }
}