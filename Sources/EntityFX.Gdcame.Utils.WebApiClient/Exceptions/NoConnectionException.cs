using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Exceptions
{
    public class NoConnectionException : ClientException
    {
        public NoConnectionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}