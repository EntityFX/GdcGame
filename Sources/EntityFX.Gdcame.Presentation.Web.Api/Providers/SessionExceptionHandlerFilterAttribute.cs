using EntityFX.Gdcame.Manager.Contract.SessionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using EntityFX.Gdcame.Infrastructure.Common;


namespace EntityFX.Gdcame.Application.WebApi.Providers
{
    public class SessionExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
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
            }
        }
    }

    public class GlobalExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandlerFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.Error(context.Exception);
        }
    }
}
