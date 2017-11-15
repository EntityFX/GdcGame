namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class ValidationErrorData : ErrorData
    {
        public object ModelState { get; set; }
    }
}