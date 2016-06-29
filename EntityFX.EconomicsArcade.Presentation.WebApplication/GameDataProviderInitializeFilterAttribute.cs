using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication
{
    public class GameDataProviderInitializeFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}