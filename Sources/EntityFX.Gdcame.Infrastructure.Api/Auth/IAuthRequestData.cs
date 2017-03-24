namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public interface IAuthRequestData<T>
        where T : class
    {
        T RequestData { get; set; }
    }
}