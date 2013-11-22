// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Mvc;

    /// <summary>
    /// Route config.
    /// </summary>
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