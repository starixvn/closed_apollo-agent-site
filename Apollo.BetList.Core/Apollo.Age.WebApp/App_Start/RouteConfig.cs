using System.Web.Mvc;
using System.Web.Routing;

namespace Apollo.Age.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("AccessDenied", "AccessDenied", new { controller = "Error", action = "AccessDenied" });
            routes.MapRoute("AccountStopped", "AccountStopped", new { controller = "Error", action = "AccountStopped" });
            routes.MapRoute("Error", "Error", new { controller = "Error", action = "Index" });
            routes.MapRoute("Unauthorized", "Unauthorized", new { controller = "Error", action = "Unauthorized" });
            routes.MapRoute("UnderMaintenance", "UnderMaintenance", new { controller = "Error", action = "UnderMaintenance" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}