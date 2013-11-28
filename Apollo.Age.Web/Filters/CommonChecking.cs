// --------------------------------------------------------------------------
// <copyright file="CommonChecking.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.8.1">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Common checking class.
    /// </summary>
    public static class CommonChecking
    {
        /// <summary>
        /// Return page access denied once user dose not permission.
        /// </summary>
        /// <param name="filterContext">Input Action Executed Context.</param>
        public static void DoUnAuthorized(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Write(Resources.Lang.UnauthorizedMsg);
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                response.Redirect(ApoURLs.URLs.AccessDenied, false);
            }
        }

        /// <summary>
        /// Return page access denied once user dose not permission.
        /// </summary>
        /// <param name="filterContext">Input Action Executing Context.</param>
        public static void DoUnAuthorized(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Write(Resources.Lang.UnauthorizedMsg);
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                response.Redirect(ApoURLs.URLs.AccessDenied, false);
            }
        }
    }
}