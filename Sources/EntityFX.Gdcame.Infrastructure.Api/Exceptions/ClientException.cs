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
}