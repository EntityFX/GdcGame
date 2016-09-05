using System.Net;
using System.Net.Http;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    internal static class ExceptionHandlerHelper
    {
        public static void HandleHttpRequestException(HttpRequestException httpRequestException)
        {
            var webException = httpRequestException.InnerException as WebException;
            if (webException != null && webException.Status == WebExceptionStatus.ConnectFailure)
            {
                throw new NoConnectionException(webException.Message, webException);
            }
            throw new ClientException(httpRequestException.Message, httpRequestException);
        }
    }
}