// --------------------------------------------------------------------------
// <copyright file="InternalCheckingAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.17">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// Internal checking attribute class.
    /// </summary>
    public class InternalCheckingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (SessionManager.CurrentSession != null
                && !SessionManager.CurrentSession.IsInternalSignIn)
            {
                CommonChecking.DoUnAuthorized(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}