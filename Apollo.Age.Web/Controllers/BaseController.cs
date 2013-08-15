// --------------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders" date="2013.4.6">Add CurrentSession Property</change>
//   <change who="Lewis" date="2013.4.10">Added RequestIP property</change>
//   <change who="Anders.Le" date="2013.5.2">Modify Current Session, change it to readonly</change>
//   <change who="Anders.Le" date="2013.5.2">Check whether request is authenticated</change>
//   <change who="Terry.Nguyen" date="2013.5.28">Handle unknow action</change>
//   <change who="Anders.Le" date="2013.6.30">Add property ActionName, ControllerName, and AreaName</change>
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Mvc;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Fanex.Common;
    using Fanex.Logging;
    using Apollo.Age.Web.Filters;

    /// <summary>
    /// This class override OnActionExecuting to check authentication, authorization, duplicated login
    /// All non-anonymous controller MUST inherit from this base controller
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;

        public BaseController()
        {
            _userService = new UserService();
        }

        #region Override functions
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool actionIsMarkedWithAllowAnonymous = filterContext.ActionDescriptor.IsDefined(
                typeof(AllowAnonymousAttribute), inherit: true);
            bool controllerIsMarkedWithAllowAnonymous = filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                typeof(AllowAnonymousAttribute), inherit: true);
            bool skipAuth = actionIsMarkedWithAllowAnonymous || controllerIsMarkedWithAllowAnonymous;

            if (!skipAuth)
            {
                CheckAuthentication();
                CheckValidDownline(filterContext);
                CheckKickOut(filterContext);
            }

            if (CurrentSession != null)
            {
                LanguageManager.SetCulture(CurrentSession.Language);
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                this.View(actionName).ExecuteResult(this.ControllerContext);
            }
            catch
            {
                Response.Redirect(ApoURLs.URLs.Error + "?err=404");
            }
        }
        #endregion

        #region Auth

        private void CheckAuthentication()
        {
            if (!Request.IsAuthenticated || CurrentSession == null)
            {
                Response.Redirect(ApoURLs.URLs.Unauthorized);
            }
        }

        private void CheckValidDownline(ActionExecutingContext filterContext)
        {
            try
            {
                if (SessionManager.CurrentSession != null)
                {
                    var currentSession = SessionManager.CurrentSession;

                    int agentId = filterContext.ActionParameters.ContainsKey("agentId") ? Converter.ToInt(filterContext.ActionParameters["agentId"], -1) : -1;
                    int memberId = filterContext.ActionParameters.ContainsKey("memberId") ? Converter.ToInt(filterContext.ActionParameters["memberId"], -1) : -1;
                    if (memberId >= 0 && memberId != currentSession.AgentId)
                    {
                        if (!_userService.CheckDownlineBelongToAgent(currentSession.AgentId, memberId, CustomerType.Member))
                        {
                           CommonChecking.DoUnAuthorized(filterContext);
                        }
                    }
                    else if (agentId >= 0 && agentId != currentSession.AgentId)
                    {
                        if (!_userService.CheckDownlineBelongToAgent(currentSession.AgentId, agentId, CustomerType.Agent))
                        {
                            CommonChecking.DoUnAuthorized(filterContext);
                        }
                    }
                }
                else
                {
                    CommonChecking.DoUnAuthorized(filterContext);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }

        private void CheckKickOut(ActionExecutingContext filterContext)
        {
            if (!ReferenceEquals(CurrentSession, null))
            {
                var userStatus = _userService.GetUserStatus(CurrentSession.AgentId, CurrentSession.UserId);

                if (userStatus >= (int)CustomerStatus.Stopped)
                {
                    if (Request.IsAjaxRequest())
                    {
                        Response.ContentType = "text/plain";
                        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        Response.Write(Resources.Lang.lblAccountWasStopped);//LanguageManager.GetResourceData("lblAccountWasStopped"));
                        Response.BufferOutput = true;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Flush();
                        Response.Close();
                        Response.End();
                    }
                    else
                    {
                        Response.Redirect(ApoURLs.URLs.AccountStopped, false);
                    }
                }
            }
        }

        //private void DoUnAuthorized(ActionExecutingContext filterContext)
        //{
        //    var response = filterContext.HttpContext.Response;
        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        //response.Clear();
        //        //response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //        //response.StatusDescription = string.Empty;
        //        //response.Write(Resources.Lang.UnauthorizedMsg);
        //        //response.End();
        //        Response.ContentType = "text/plain";
        //        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //        Response.Write(Resources.Lang.UnauthorizedMsg);
        //        Response.BufferOutput = true;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.Flush();
        //        Response.Close();
        //        Response.End();
        //    }
        //    else
        //    {
        //        response.Redirect(ApoURLs.URLs.AccessDenied);
        //    }
        //}

        #endregion

        #region PROPERTIES
        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.CurrentSession; }
        }

        protected string GetResourceData(string key)
        {
            return LanguageManager.GetResourceData(key);
        }

        #region Controller Util
        protected string CurrentAction
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            }
        }

        protected string CurrentController
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            }
        }

        protected string CurrentArea
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["area"] as string;
            }
        }
        #endregion
        #endregion

        
    }
}