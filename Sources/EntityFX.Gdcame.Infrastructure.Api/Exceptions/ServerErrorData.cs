namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class ServerErrorData : ErrorData
    {
        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }
    }
}