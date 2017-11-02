using Owin;

namespace EntityFX.Gdcame.Utils.Common
{
    public static class OwinExceptionHandlerMiddlewareAppBuilderExtensions
    {
        public static void UseOwinExceptionHandler(this IAppBuilder app)
        {
            app.Use<OwinExceptionHandlerMiddleware>();
        }
    }
}