// --------------------------------------------------------------------------
// <copyright file="WagersAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Wagers area registration.
    /// </summary>
    public class WagersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Wagers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}