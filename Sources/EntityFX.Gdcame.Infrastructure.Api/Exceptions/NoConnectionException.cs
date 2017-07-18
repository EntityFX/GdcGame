using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class NoConnectionException : ClientException<NoServerConnectionData>
    {
        public NoConnectionException(NoServerConnectionData errorData, string message, Exception inner) : base(errorData, message, inner)
        {
        }
    }
}