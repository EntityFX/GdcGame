using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class ClientException<T> : Exception, IClientException<T>
        where T : ErrorData
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ClientException(T errorData)
        {
            ErrorData = errorData;
        }

        public ClientException(T errorData, string message) : base(message)
        {
            ErrorData = errorData;
        }

        public ClientException(T errorData, string message, Exception inner) : base(message, inner)
        {
            ErrorData = errorData;
        }

        public T ErrorData { get; private set; }
    }

    public interface IClientException<out T>
        where T : ErrorData
    {
        T ErrorData { get; }
    }

    public class ErrorData
    {
        public  string Message { get; set; }
    }

    public class InvalidSessionErrorData : ErrorData
    {
        public Guid SessionGuid { get; set; }
    }

    public class ServerErrorData : ErrorData
    {
        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }
    }

    public class ValidationErrorData : ErrorData
    {
        public dynamic ModelState { get; set; }
    }

    public class WrongAuthData<TAuthRequestData> : ErrorData
    {
        public TAuthRequestData RequestData { get; set; }
    }
}