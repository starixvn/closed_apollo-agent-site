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
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using System.Resources;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Interface;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;

        public BaseController()
        {
            _userService = new UserService();
        }

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
                CheckValidDownline();
                CheckKickOut(filterContext);
            }

            if (CurrentSession != null)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CurrentSession.Language);
            }

            base.OnActionExecuting(filterContext);
        }

        #region Auth

        private void CheckAuthentication()
        {
            if (!Request.IsAuthenticated || CurrentSession == null)
            {
                Response.Redirect(WebConstants.UnauthorizedUrl);
            }
        }

        private void CheckValidDownline()
        {

        }

        private void CheckKickOut(ActionExecutingContext filterContext)
        {
            var isKickOut = _userService.IsKickOut(CurrentSession.AgentId, CurrentSession.UserId);

            if (isKickOut)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.Buffer = true;
                    Response.StatusCode = WebConstants.UN_AUTHORIZED_CODE;
                    Response.Write(GetResourceData("lblAccountWasStopped"));
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }
                else
                {
                    Response.Redirect(WebConstants.AccountStoppedUrl);
                }
            }
        }

        #endregion

        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.CurrentSession; }
        }

        protected string RequestIP
        {
            get
            {
                string IP = string.Empty;
                string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(userHostAddress))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        IP = itemIPA.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(IP))
                {
                    return IP;
                }

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.OrdinalIgnoreCase))
                    {
                        IP = itemIPA.ToString();
                        break;
                    }
                }

                return IP.Trim();
            }
        }

        protected string GetResourceData(string key)
        {
            return (new ResourceManager(typeof(Lang)) { IgnoreCase = true }).GetString(key);
        }
    }
}