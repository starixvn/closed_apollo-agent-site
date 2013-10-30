// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace Apollo.Age.WebApp
{
    using System.Web.Mvc;
    using System.Web.Routing;

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
            routes.MapRoute("WhoAmI", "WhoAmI", new { controller = "Error", action = "WhoAmI" });
            routes.MapRoute("RedirectToUM", "RedirectToUM", new { controller = "Error", action = "RedirectToUM" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}