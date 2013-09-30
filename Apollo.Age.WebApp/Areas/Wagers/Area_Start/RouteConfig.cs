namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Mvc;

    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Wagers/{controller}/{action}/{id}",                
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Apollo.Age.Wagers.Controllers" });            
        }
    }
}