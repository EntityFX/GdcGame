using System.Net;
using System.Net.Http;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using RestSharp.Portable;
using Newtonsoft.Json.Linq;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    internal static class ExceptionHandlerHelper
    {
        public static void HandleHttpRequestException(HttpRequestException httpRequestException)
        {
            var webException = httpRequestException.InnerException as WebException;
            if (webException != null && webException.Status == WebExceptionStatus.ConnectFailure)
            {
                throw new NoConnectionException(null, webException.Message, webException);
            }
            throw new ClientException(null, httpRequestException.Message, httpRequestException);
        }

        public static void HandleNotSuccessRequest(IRestResponse response)
        {
            //response.Content
            JToken token = JObject.Parse(response.Content);
            ErrorData errorData = null;
            var messageToken = (string)token.SelectToken("message", false);

            var invalidState = token.SelectToken("modelState", false);
            if (invalidState != null)
            {
                errorData = new ValidationErrorData()
                {
                    Message = messageToken,
                    ModelState = (dynamic)invalidState
                };
                throw new ClientException(errorData, response.StatusDescription);
            }

            var exceptionMessage = (string)token.SelectToken("exceptionMessage", false);
            if (exceptionMessage != null)
            {
                var exceptionType = (string)token.SelectToken("exceptionType", false);
                var stackTrace = (string)token.SelectToken("stackTrace", false);
                errorData = new ServerErrorData()
                {
                    Message = exceptionMessage,
                    ExceptionType = exceptionType,
                    StackTrace = stackTrace
                };
                throw new ClientException(errorData, response.StatusDescription);
            }


            if (messageToken != null)
            {
                errorData = new ErrorData()
                {
                    Message = messageToken
                };
                throw new ClientException(errorData, response.StatusDescription);
            }

        }
    }
}