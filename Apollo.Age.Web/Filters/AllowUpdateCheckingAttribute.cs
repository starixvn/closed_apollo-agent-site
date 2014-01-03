// --------------------------------------------------------------------------
// <copyright file="AllowUpdateCheckingAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.8.1">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// Allow update checking attribute class.
    /// </summary>
    public class AllowUpdateCheckingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.CurrentSession != null 
                && !SessionManager.CurrentSession.IsMaster)
            {
                CommonChecking.DoUnAuthorized(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}