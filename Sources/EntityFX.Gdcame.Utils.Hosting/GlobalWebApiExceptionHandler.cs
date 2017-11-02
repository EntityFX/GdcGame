using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class GlobalWebApiExceptionHandler : ExceptionHandler
    {
        private readonly ILogger _logger;

        public GlobalWebApiExceptionHandler(Infrastructure.Common.ILogger logger)
        {
            _logger = logger;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            base.Handle(context);
            var innerExceptionMessage = context.Exception.InnerException != null ?
                string.Format(", Inner exception: {0}", context.Exception.InnerException.Message) :
                string.Empty;
            var errorMessage = string.Format("Critical error. Unhandled Exception: {0}{1}", context.Exception.Message, innerExceptionMessage);
            _logger.Error(context.Exception);
            context.Result = new ResponseMessageResult(context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError, errorMessage));
        }

    }
}