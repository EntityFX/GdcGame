using System;
using System.Net;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace EntityFX.Gdcame.Infrastructure
{
    internal static class ExceptionHandlerHelper
    {
        public static void HandleHttpRequestException(Exception httpRequestException)
        {
            var webException = (httpRequestException != null) ?  httpRequestException.InnerException as WebException : null;
            if (webException != null && (webException.Status == WebExceptionStatus.ConnectFailure || webException.Status  == WebExceptionStatus.UnknownError))
            {
                throw new NoConnectionException(new NoServerConnectionData()
                {
                    Message = webException.Message,
                }, webException.Message, webException);
            }
            throw new ClientException<ServerErrorData>(new ServerErrorData
            {
                Message = httpRequestException.Message,
                ExceptionType = httpRequestException.GetType().ToString()
            }, 
            httpRequestException.Message, httpRequestException);
        }

        public static void HandleNotSuccessRequest(IRestResponse response)
        {
            JToken token = null;
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new NoConnectionException(new NoServerConnectionData() {Message = response.ErrorMessage, Uri = response.ResponseUri}, response.ErrorMessage,
                    response.ErrorException);
            }



            try
            {
                token = JToken.Parse(response.Content);
            }
            catch (System.Exception)
            {
                throw new ClientException<ErrorData>(new ServerErrorData() { Message = response.StatusDescription, ExceptionType = "Critical Error"}, response.StatusDescription);
            }

            if (token != null && token.Type == JTokenType.String)
            {
                throw new ClientException<ServerErrorData>(new ServerErrorData() { Message = token.Value<string>()}, response.StatusDescription);
            }

            var serverToken = token.SelectToken("server", false);
            if (serverToken != null && response.StatusCode == HttpStatusCode.MovedPermanently)
            {
                throw new ClientException<GameFrozenErrorData>(new GameFrozenErrorData() { Server = serverToken.Value<string>() });
            }

            var messageToken = (string)token.SelectToken("message", false);

            var exceptionType = (string)token.SelectToken("exceptionType", false);
            if (exceptionType != null)
            {
                var messageDetail = (string)token.SelectToken("messageDetail", false);
                if (exceptionType.Contains("InvalidSessionException"))
                {
                    throw new InvalidSessionException(new InvalidSessionErrorData
                    {
                        Message = messageToken, SessionGuid = Guid.Parse(messageDetail)
                    }, messageToken);
                }
            }

            var invalidState = token.SelectToken("errorDetails", false);
            if (invalidState != null && invalidState.Type == JTokenType.Array)
            {
                var validationErrorData = new ValidationErrorData()
                {
                    Message = messageToken,
                    ModelState = (dynamic)invalidState
                };
                throw new ClientException<ValidationErrorData>(validationErrorData, response.StatusDescription);
            }

            var exceptionMessage = (string)token.SelectToken("exceptionMessage", false);
            if (exceptionMessage != null)
            {
                var stackTrace = (string)token.SelectToken("stackTrace", false);
                var serverErrorData = new ServerErrorData()
                {
                    Message = exceptionMessage,
                    ExceptionType = exceptionType,
                    StackTrace = stackTrace
                };
                throw new ClientException<ServerErrorData>(serverErrorData, response.StatusDescription);
            }


            if (messageToken != null)
            {
                var errorData = new ErrorData()
                {
                    Message = messageToken
                };
                throw new ClientException<ErrorData>(errorData, response.StatusDescription);
            }

        }
    }
}