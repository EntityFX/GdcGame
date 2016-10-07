using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Exceptions
{
    public class ClientException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ClientException(ErrorData errorData)
        {
            ErrorData = errorData;
        }

        public ClientException(ErrorData errorData, string message) : base(message)
        {
            ErrorData = errorData;
        }

        public ClientException(ErrorData errorData, string message, Exception inner) : base(message, inner)
        {
            ErrorData = errorData;
        }

        public ErrorData ErrorData { get; private set; }
    }


    public class ErrorData
    {
        public  string Message { get; set; }
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
}