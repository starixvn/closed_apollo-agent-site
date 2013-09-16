// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Web.Filters;
    using Fanex.Caching;
    using Fanex.Common;
    using Fanex.Logging;
using Apollo.Age.Web.Services;

    [SuppressMessage("CSharp.Nexcel.NexcelCustom", "SP2101:CSharp.Nexcel.NexcelCustom", Justification = "Reviewed.")]

    /// <summary>
    /// This class override OnActionExecuting to check authentication, authorization, duplicated login.
    /// All non-anonymous controller MUST inherit from this base controller.
    /// </summary>
    public class BaseController : Controller
    {
        private const string KickOutStatusCacheKey = "Fanex.Cache.Apollo.KickoutStatus";
        private const string CheckDownlineAgentCacheKey = "Fanex.Cache.Apollo.DownlineAgent";
        private const string CheckDownlineMemberCacheKey = "Fanex.Cache.Apollo.DownlineMember";
        private readonly IUserService _userService;
        private readonly ICache _cacheManager;
                

        public BaseController()
        {
            _userService = new UserService();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        
        }

        #region PROPERTIES

        /// <summary>
        /// Gets Current Session.
        /// </summary>
        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.CurrentSession; }
        }

        #region Controller Util

        /// <summary>
        /// Gets Current Action.
        /// </summary>
        protected string CurrentAction
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            }
        }

        /// <summary>
        /// Gets Current Controller.
        /// </summary>
        protected string CurrentController
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            }
        }

        /// <summary>
        /// Gets Current Area.
        /// </summary>
        protected string CurrentArea
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["area"] as string;
            }
        }

        #endregion Controller Util

        #endregion PROPERTIES

        /// <summary>
        /// Gets Resource Data.
        /// </summary>
        /// <param name="key">Input a Key.</param>
        /// <returns>Return resource value.</returns>
        protected string GetResourceData(string key)
        {
            return LanguageManager.GetResourceData(key);
        }

        #region Override functions

        /// <summary>
        /// Override Action Executing for checking Authentication, Validate down line and Kick out user.
        /// </summary>
        /// <param name="filterContext">Input filter Context.</param>
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

        /// <summary>
        /// Handle Unknown Action.
        /// </summary>
        /// <param name="actionName">Input Action Name.</param>
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

        #endregion Override functions

        #region Auth

        /// <summary>
        /// Checking Authentication.
        /// </summary>
        private void CheckAuthentication()
        {
            if (!Request.IsAuthenticated || CurrentSession == null)
            {
                Response.Redirect(ApoURLs.URLs.Unauthorized);
            }
        }               

        /// <summary>
        /// Checking Validate Down line.
        /// </summary>
        /// <param name="filterContext">Input filter Context.</param>
        private void CheckValidDownline(ActionExecutingContext filterContext)
        {
            try
            {
                if (SessionManager.CurrentSession != null)
                {
                    var currentSession = SessionManager.CurrentSession;
                    int agentId = filterContext.ActionParameters.ContainsKey("agentId") ?
                        Converter.ToInt(filterContext.ActionParameters["agentId"], -1) : -1;
                    int memberId = filterContext.ActionParameters.ContainsKey("memberId") ?
                        Converter.ToInt(filterContext.ActionParameters["memberId"], -1) : -1;
                    bool isDownline = false;

                    if (memberId >= 0 && memberId != currentSession.AgentId)
                    {
                        string cacheKey = CheckDownlineAgentCacheKey + CurrentSession.AgentId.ToString() + memberId.ToString();

                        if (_cacheManager.Contains(cacheKey))
                        {
                            isDownline = _cacheManager.Get<bool>(cacheKey);
                        }
                        else
                        {
                            isDownline = _userService.CheckDownlineBelongToAgent(currentSession.AgentId, memberId, CustomerType.Member);
                            _cacheManager.Set<bool>(cacheKey, isDownline, TimeSpan.FromSeconds(5));
                        }

                        if (!isDownline)
                        {
                            CommonChecking.DoUnAuthorized(filterContext);
                        }
                    }
                    else if (agentId >= 0 && agentId != currentSession.AgentId)
                    {
                        string cacheKey = CheckDownlineAgentCacheKey + CurrentSession.AgentId.ToString() + agentId.ToString();

                        if (_cacheManager.Contains(cacheKey))
                        {
                            isDownline = _cacheManager.Get<bool>(cacheKey);
                        }
                        else
                        {
                            isDownline = _userService.CheckDownlineBelongToAgent(currentSession.AgentId, agentId, CustomerType.Agent);
                            _cacheManager.Set<bool>(cacheKey, isDownline, TimeSpan.FromSeconds(30));
                        }

                        if (!isDownline)
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

        /// <summary>
        /// Checking Kick Out.
        /// </summary>
        /// <param name="filterContext">Input filter context.</param>
        private void CheckKickOut(ActionExecutingContext filterContext)
        {
            if (!ReferenceEquals(CurrentSession, null))
            {
                int userStatus = -1;
                string cacheKey = KickOutStatusCacheKey + CurrentSession.AgentId.ToString() + CurrentSession.UserId.ToString();
                if (_cacheManager.Contains(cacheKey))
                {
                    userStatus = _cacheManager.Get<int>(cacheKey);
                }
                else
                {
                    userStatus = _userService.GetUserStatus(CurrentSession.AgentId, CurrentSession.UserId);
                    _cacheManager.Set<int>(cacheKey, userStatus, TimeSpan.FromSeconds(5));
                }

                if (userStatus >= (int)CustomerStatus.Stopped)
                {                   
                    Response.Redirect(ApoURLs.URLs.AccountStopped, false);

                    //if (Request.IsAjaxRequest())
                    //{
                    //    Response.ContentType = "text/plain";
                    //    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    //    Response.Write(Resources.Lang.lblAccountWasStopped);
                    //    Response.BufferOutput = true;
                    //    Response.ClearContent();
                    //    Response.ClearHeaders();
                    //    Response.Flush();
                    //    Response.Close();
                    //    Response.End();
                    //}
                    //else
                    //{
                        
                    //    Response.Redirect(ApoURLs.URLs.AccountStopped, false);
                    //}
                }
            }
        }

        #endregion Auth
    }
}