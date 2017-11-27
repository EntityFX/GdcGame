namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordAuthRequest : IAuthRequest<PasswordAuthRequestData>
    {
        public PasswordAuthRequestData RequestData { get; set; }
    }
}