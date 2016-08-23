using System.Web.Http;

namespace EntityFX.Gdcame.Presentation.Web.WebApp
{
    public static class WebApiConfig
    {
        public static string UrlPrefix
        {
            get { return "api"; }
        }

        public static string UrlPrefixRelative
        {
            get { return "~/api"; }
        }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", UrlPrefix + "/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
                );
        }
    }
}