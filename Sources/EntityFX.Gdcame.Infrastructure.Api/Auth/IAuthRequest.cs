namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthRequest<T>
        where T : class
    {
        T RequestData { get; set; }
    }
}