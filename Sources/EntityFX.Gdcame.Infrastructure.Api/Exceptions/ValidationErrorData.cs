namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class ValidationErrorData : ErrorData
    {
        public dynamic ModelState { get; set; }
    }
}