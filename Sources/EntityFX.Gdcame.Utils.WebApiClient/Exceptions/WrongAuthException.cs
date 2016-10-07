using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Exceptions
{
    public class WrongAuthException<TAuthRequestData> : ClientException
        where TAuthRequestData : class
    {
        public TAuthRequestData RequestData { get; private set; }

        public WrongAuthException(TAuthRequestData requestData, string message, Exception inner)
             : base(null, message, inner)
        {
            RequestData = requestData;
        }
    }
}
