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
        private static readonly CultureInfo usCulture = CultureInfo.CreateSpecificCulture("en-US");
        private ResourceManager _resourceManagerLang;

        public BaseController()
        {
            _userService = new UserService();
            _resourceManagerLang = new ResourceManager(typeof(Lang)) { IgnoreCase = true };
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
                SetCulture(CurrentSession.Language);
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



        private void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = usCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = usCulture.NumberFormat;
        }

        #region Auth

        private void CheckAuthentication()
        {
            if (!Request.IsAuthenticated || CurrentSession == null)
            {
                Response.Redirect(ApoURLs.URLs.Unauthorized);
            }
        }

        private void CheckValidDownline()
        {

        }

        private void CheckKickOut(ActionExecutingContext filterContext)
        {
            if (!ReferenceEquals(CurrentSession, null))
            {
                var isKickOut = _userService.IsKickOut(CurrentSession.AgentId, CurrentSession.UserId);

                if (isKickOut)
                {
                    if (Request.IsAjaxRequest())
                    {
                        Response.Buffer = true;
                        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        Response.Write(GetResourceData("lblAccountWasStopped"));
                        Response.Flush();
                        Response.Close();
                        Response.End();
                    }
                    else
                    {
                        Response.Redirect(ApoURLs.URLs.AccountStopped);
                    }
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
            if (!ReferenceEquals(_resourceManagerLang, null))
            {
                return _resourceManagerLang.GetString(key);
            }
            return string.Empty;
        }
    }
}