namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public class PasswordAuthRequest<T> : IAuthRequestData<PasswordAuthData>
    {
        public PasswordAuthData RequestData { get; set; }
    }
}