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
                StaticContentConventionBuilder.AddDirectory("app", "website/app", "js", "ts", "map", "html")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("content", "website/content", "css", "map")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts", "website/Scripts", "js", "map")
            );
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("images", "website/images", "png", "jpg")
            );
        }
    }
}