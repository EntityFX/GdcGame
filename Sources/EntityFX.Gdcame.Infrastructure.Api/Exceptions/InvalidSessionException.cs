using System;

namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    public class InvalidSessionException : ClientException<InvalidSessionErrorData>
    {
        public InvalidSessionException(InvalidSessionErrorData invalidSessionErrorData, string message)
             : base(invalidSessionErrorData, message)
        {
        }

        public InvalidSessionException(InvalidSessionErrorData invalidSessionErrorData, string message, Exception inner)
             : base(invalidSessionErrorData, message, inner)
        {
        }
    }
}
