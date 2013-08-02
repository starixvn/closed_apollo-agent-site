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
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Fanex.Logging;

    public class InternalCheckingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (SessionManager.CurrentSession != null)
                {
                    var currentSession = SessionManager.CurrentSession;
                    if (!currentSession.IsInternalSignIn)
                    {
                       CommonChecking.DoUnAuthorized(filterContext);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
            base.OnActionExecuted(filterContext);
        }

        //private void DoUnAuthorized(ActionExecutedContext filterContext)
        //{
        //    var response = filterContext.HttpContext.Response;
        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        response.Clear();
        //        response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //        response.StatusDescription = string.Empty;
        //        response.Write(Resources.Lang.UnauthorizedMsg);
        //        response.End();
        //    }
        //    else
        //    {
        //        response.Redirect(ApoURLs.URLs.AccessDenied);
        //    }
        //}
    }
}