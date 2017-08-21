namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public interface IClientException<out T>
        where T : ErrorData
    {
        T ErrorData { get; }
    }
}