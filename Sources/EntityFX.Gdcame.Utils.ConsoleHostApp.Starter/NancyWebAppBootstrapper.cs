using Nancy;
using Nancy.Conventions;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => Response.AsFile("website/index.html");
        }
    }

    public class NancyWebAppBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("app", "website/app", "js", "ts", "map", "html")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("styles", "website/styles", "css", "map")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("scripts", "website/Scripts", "js")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("images", "website/images", "png", "jpg")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("fonts", "website/fonts", "eot", "ttf", "svg", "woff", "woff2")
            );
        }
    }
}