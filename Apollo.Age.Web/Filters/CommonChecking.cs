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

    public static class CommonChecking
    {
        public static void DoUnAuthorized(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.StatusDescription = string.Empty;
                response.Write(Resources.Lang.UnauthorizedMsg);
                response.End();
            }
            else
            {
                response.Redirect(ApoURLs.URLs.AccessDenied);
            }
        }

        public static void DoUnAuthorized(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.StatusDescription = string.Empty;
                response.Write(Resources.Lang.UnauthorizedMsg);
                response.End();
            }
            else
            {
                response.Redirect(ApoURLs.URLs.AccessDenied);
            }
        }
    }
}