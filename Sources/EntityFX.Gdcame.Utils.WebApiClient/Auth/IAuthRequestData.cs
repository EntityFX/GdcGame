namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public interface IAuthRequestData<T>
        where T : class
    {
        T RequestData { get; set; }
    }
}