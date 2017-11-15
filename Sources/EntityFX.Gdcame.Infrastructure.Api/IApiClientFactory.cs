using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Infrastructure.Api
{
    public interface IApiClientFactory<TContext>
    {
        IApiClient Build(IAuthContext<TContext> context);
    }
}