using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using Microsoft.Owin;

namespace EntityFX.Gdcame.Utils.Common
{
    public class OwinExceptionHandlerMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public OwinExceptionHandlerMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            _next = next;
        }

        public ILogger Logger { get; set; }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try
            {
                await _next(environment);
            }
            catch (Exception ex)
            {
                try
                {

                    var owinContext = new OwinContext(environment);

                   // Logger.Error(ex);

                    HandleException(ex, owinContext);

                    return;
                }
                catch (Exception)
                {
                    // If there's a Exception while generating the error page, re-throw the original exception.
                }
                throw;
            }
        }
        private void HandleException(Exception ex, IOwinContext context)
        {
            var request = context.Request;

            //Build a model to represet the error for the client
          //  Logger.Error(ex);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ReasonPhrase = "Internal Server Error";
            context.Response.ContentType = "application/json";
            context.Response.Write(string.Format("Critical error: {0}", ex.Message));

        }

    }
}