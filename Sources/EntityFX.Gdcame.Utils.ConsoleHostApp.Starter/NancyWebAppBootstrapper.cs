using Nancy;
using Nancy.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter
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
                StaticContentConventionBuilder.AddDirectory("app", "website/app", "js", "html")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("assets", "website/assets", "js", "html", "css", "svg", "ttf", "eot", "woff", "woff2")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("images", "website/images", "png", "jpg")
            );
        }
    }
}