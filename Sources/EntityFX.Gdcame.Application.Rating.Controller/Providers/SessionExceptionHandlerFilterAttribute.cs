namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Filters;

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class SessionExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        private class FrozenGameNewServer
        {
            public string Server { get; set; }
        }


        public SessionExceptionHandlerFilterAttribute(ILogger logger)
        {
            this._logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var invalidSessionException = context.Exception as InvalidSessionException;
            if (invalidSessionException != null)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Forbidden
                    , new HttpError(context.Exception.Message)
                    {
                        ExceptionType = context.Exception.GetType().ToString(),
                        MessageDetail = invalidSessionException.SessionGuid.ToString()
                    });
                return;
            }

            var gameFrozenException = context.Exception as GameFrozenException;
            if (gameFrozenException != null)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.MovedPermanently
                    , new FrozenGameNewServer { Server = gameFrozenException.Server});
                return;
            }

            if (context.Exception != null)
            {
                this._logger.Error(context.Exception);
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    new HttpError(context.Exception.Message)
                    {
                        ExceptionType = context.Exception.GetType().ToString(),
                        MessageDetail = context.Exception.Message
                    });
            }
        }
    }
}
