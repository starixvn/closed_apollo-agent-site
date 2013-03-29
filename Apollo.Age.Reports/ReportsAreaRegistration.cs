// --------------------------------------------------------------------------
// <copyright file="ReportsAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Reports"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}