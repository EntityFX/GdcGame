using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.WebApiClient.Exceptions
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
