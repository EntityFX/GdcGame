using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class NoConnectionException : ClientException<ErrorData>
    {
        public NoConnectionException(ErrorData errorData, string message, Exception inner) : base(errorData, message, inner)
        {
        }
    }
}