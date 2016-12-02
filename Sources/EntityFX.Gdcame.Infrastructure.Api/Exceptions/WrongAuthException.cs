using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class WrongAuthException<TAuthRequestData> : ClientException<WrongAuthData<TAuthRequestData>>
        where TAuthRequestData : class
    {
        public WrongAuthException(WrongAuthData<TAuthRequestData> wrongAuthData, string message, Exception inner)
             : base(wrongAuthData, message, inner)
        {
        }
    }
}
