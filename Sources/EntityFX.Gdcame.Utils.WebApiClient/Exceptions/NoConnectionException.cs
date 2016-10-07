using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Exceptions
{
    public class NoConnectionException : ClientException
    {
        public NoConnectionException(ErrorData errorData, string message, Exception inner) : base(errorData, message, inner)
        {
        }
    }
}