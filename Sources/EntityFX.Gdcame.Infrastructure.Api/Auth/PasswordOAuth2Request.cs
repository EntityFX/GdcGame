namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordOAuth2Request : IAuthRequest<PasswordOAuth2RequestData>
    {
        public PasswordOAuth2RequestData RequestData { get; set; }
    }
}