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
    using Apollo.Age.Enumerations;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;       
        private LanguageManager _langManager;
       

        public BaseController()
        {
            _userService = new UserService();           
            _langManager = new LanguageManager();           

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
                _langManager.SetCulture(CurrentSession.Language);
                
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
                var userStatus = _userService.GetUserStatus(CurrentSession.AgentId, CurrentSession.UserId);

                if (userStatus >= (int)CustomerStatus.Stopped)
                {
                    if (Request.IsAjaxRequest())
                    {
                        Response.Buffer = true;
                        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        Response.Write(_langManager.GetResourceData("lblAccountWasStopped"));
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

       

        protected string GetResourceData(string key)
        {
            return _langManager.GetResourceData(key); 
        }
    }
}