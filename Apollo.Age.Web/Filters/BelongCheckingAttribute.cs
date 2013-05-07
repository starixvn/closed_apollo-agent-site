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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Common;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Enumerations;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BelongCheckingAttribute : ActionFilterAttribute
    {
        private const int UN_AUTHORIZED_CODE = 401;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var currentSession = SessionManager.Instance.CurrentSession;
                int agentId = filterContext.ActionParameters.ContainsKey("agentId") ? Converter.ToInt(filterContext.ActionParameters["agentId"]) : 0;
                int memberId = filterContext.ActionParameters.ContainsKey("memberId") ? Converter.ToInt(filterContext.ActionParameters["memberId"]) : 0;
                if (agentId > 0)
                {
                    if (!PermissionService.HasPermissionOnUpdating(currentSession.AgentId, agentId, CustomerType.Agent))
                    {
                        DoUnAuthorized(filterContext);
                    }
                }
                else if (memberId > 0)
                {
                    if (!PermissionService.HasPermissionOnUpdating(currentSession.AgentId, memberId, CustomerType.Member))
                    {
                        DoUnAuthorized(filterContext);
                    }
                }
                else
                {
                    DoUnAuthorized(filterContext);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
            // The action filter logic.
            base.OnActionExecuting(filterContext);
        }

        private void DoUnAuthorized(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = UN_AUTHORIZED_CODE;
                response.StatusDescription = string.Empty;
                response.Write(Resources.Lang.UnauthorizedMsg);
                response.End();
            }
            else
            {
                response.Redirect("~/Auth/Unauthorized");
            }
        }
    }
}
