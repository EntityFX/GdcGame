namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordAuthRequest<T> : IAuthRequestData<PasswordAuthData>
    {
        public PasswordAuthData RequestData { get; set; }
    }
}