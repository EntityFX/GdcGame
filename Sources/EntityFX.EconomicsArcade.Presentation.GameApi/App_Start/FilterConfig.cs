using System.Web;
using System.Web.Mvc;

namespace EntityFX.EconomicsArcade.Presentation.GameApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
