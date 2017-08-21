namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class WrongAuthData<TAuthRequestData> : ErrorData
    {
        public TAuthRequestData RequestData { get; set; }
    }
}