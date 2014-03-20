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
    using System.IO;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.Filters;
    using Fanex.Caching;
    using Fanex.Common;
    using Fanex.Logging;
    using NPOI.HSSF.UserModel;

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
        /// <value>Current Session.</value>
        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.CurrentSession; }
        }

        #region Controller Util

        /// <summary>
        /// Gets Current Action.
        /// </summary>
        /// <value>Current Action.</value>
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
        /// <value>Current Controller.</value>
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
        /// <value>Current Area.</value>
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

        /// <summary>
        /// Excels the result.
        /// </summary>
        /// <param name="workBook">The work book.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return action result.</returns>
        protected ActionResult ExcelResult(HSSFWorkbook workBook, string fileName)
        {
            var stream = new MemoryStream();
            workBook.Write(stream);
            stream.Close();

            return File(stream.ToArray(), "application/vnd.ms-excel", fileName);
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
                CheckKickOut();
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
                Response.Redirect(ApoURLs.URLs.Error + "?err=404", false);
            }
        }

        #endregion Override functions

        #region Auth

        /// <summary>
        /// Permissions the down line detail.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// True for permission down line detail is ok.
        /// </returns>
        protected bool PermissionDownlineDetail(int customerId)
        {
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            // Current user is agent
            if (this.CurrentSession.IsMaster)
            {
                if (customerId != this.CurrentSession.UserId && !hasDownlineDetailsPermission)
                {
                    return false;
                }
            }
            else
            {
                // Current user is shadow
                if (customerId != this.CurrentSession.AgentId && !hasDownlineDetailsPermission)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checking Authentication.
        /// </summary>
        private void CheckAuthentication()
        {
            if (!Request.IsAuthenticated || SessionManager.CurrentSession == null)
            {
                Response.Redirect(ApoURLs.URLs.Unauthorized, false);
            }
        }

        /// <summary>
        /// Checking Validate Down line.
        /// </summary>
        /// <param name="filterContext">Input filter Context.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
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
        private void CheckKickOut()
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
                }
            }
        }

        #endregion Auth
    }
}