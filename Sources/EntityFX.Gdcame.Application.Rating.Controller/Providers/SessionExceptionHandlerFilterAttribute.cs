using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{

    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class SessionExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;


        public SessionExceptionHandlerFilterAttribute(ILogger logger)
        {
            this._logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var invalidSessionException = context.Exception as InvalidSessionException;
            if (invalidSessionException != null)
            {
                context.Result = new ForbidResult(
                    new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"message", context.Exception.Message}
                    }));
                return;
            }

            var gameFrozenException = context.Exception as GameFrozenException;
            if (gameFrozenException != null)
            {
                context.Result = new RedirectResult(gameFrozenException.Server, true);
                return;
            }

            if (context.Exception != null)
            {
                context.Result = new BadRequestObjectResult(context.Exception.GetType().ToString());
                this._logger.Error(context.Exception);
            }
        }
    }

    public class CustomHttpException : Exception
    {
        public int Code { get; }

        public CustomHttpException(int code)
        {
            Code = code;
        }
    }
}
