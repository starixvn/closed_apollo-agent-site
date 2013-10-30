// --------------------------------------------------------------------------
// <copyright file="BelongCheckingAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.5.7">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Fanex.Common;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BelongCheckingAttribute : ActionFilterAttribute
    {
        private readonly IUserService _userService;

        public BelongCheckingAttribute()
            : this(null)
        {
        }

        public BelongCheckingAttribute(IUserService userService = null)
        {
            _userService = userService ?? new UserService();
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var currentSession = SessionManager.CurrentSession;
                int agentId = filterContext.ActionParameters.ContainsKey("agentId") ? Converter.ToInt(filterContext.ActionParameters["agentId"]) : 0;
                int memberId = filterContext.ActionParameters.ContainsKey("memberId") ? Converter.ToInt(filterContext.ActionParameters["memberId"]) : 0;
                if (memberId > 0)
                {
                    if (!_userService.CheckDownlineBelongToAgent(currentSession.AgentId, memberId, CustomerType.Member))
                    {
                       CommonChecking.DoUnAuthorized(filterContext);
                    }
                }
                else if (agentId > 0)
                {
                    if (!_userService.CheckDownlineBelongToAgent(currentSession.AgentId, agentId, CustomerType.Agent))
                    {
                        CommonChecking.DoUnAuthorized(filterContext);
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
            // The action filter logic.
            base.OnActionExecuting(filterContext);
        }

        //private void DoUnAuthorized(ActionExecutingContext filterContext)
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