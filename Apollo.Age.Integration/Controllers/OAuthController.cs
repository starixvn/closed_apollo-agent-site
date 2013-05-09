// --------------------------------------------------------------------------
// <copyright file="OAuthController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
//   <change who="Anders.Le" date="2013.5.7">Refactor GetCode action</change>
//   <change who="Anders.Le" date="2013.5.9">Add log</change>
// </history>


namespace Apollo.Age.Integration.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Fanex.Logging;
    using Apollo.Age.Integration.Services;
    using Apollo.Age.Integration.Models;
    using Apollo.Age.Core;

    [AllowAnonymous]
    public class OAuthController : Controller
    {
        private readonly AuthenticationService _authService;
        private readonly UserService _userService;        

        public OAuthController()
            : this(null)
        {

        }

        public OAuthController(AuthenticationService authService = null, UserService userService = null)
        {
            _authService = authService ?? new AuthenticationService();
            _userService = userService ?? new UserService();
        }
             

        [AllowAnonymous]
        public ViewResult Index(string user_id, string lang)
        {
            ViewBag.Username = user_id;
            ViewBag.Language = _authService.ConvertToApolloLanguage(lang);
            return View();
        }

        [AllowAnonymous]        
        public ActionResult GetCode(string code, string error, string state, string username, string language)
        {
            AuthModel authModel = new AuthModel();

            if (!string.IsNullOrWhiteSpace(error))
            {
                authModel.ErrorCode = error;
            }
            else
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        authModel = _authService.GetAuthInfo(authorizationCode: code);

                        if (!ReferenceEquals(authModel, null) && authModel.IsValid)
                        {
                            Entities.User user = _userService.GetUserByUsername(authModel.Username);

                            // check whether authenticated username (from sungame agentsite) is the same with
                            // request sso username
                            if (user != null && user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                            {
                                authModel.ErrorCode = string.Empty;
                                ICurrentSession currentSession = new CurrentSession(user, language);
                                SessionManager.Instance.CurrentSession = currentSession;
                                FormsAuthentication.SetAuthCookie(user.Username, true);
                            }
                            else
                            {
                                LogService.Error(string.Format("User {0} not found or authenticated user {0} and sso user {1} are not the same", authModel.Username, user.Username));
                                authModel.ErrorCode = ErrorCode.InvalidRequest;
                            }
                        }
                        else
                        {
                            authModel.ErrorCode = ErrorCode.InvalidRequest;
                        }
                    }
                    else
                    {
                        LogService.Error("Authorization code is empty");
                        authModel.ErrorCode = ErrorCode.InvalidRequest;
                    }
                }
                catch (AuthenticationException ex)
                {                    
                    authModel.ErrorCode = ErrorCode.ServerError;
                    LogService.Error(ex.Message);
                }
            }

            return View(authModel);
        }

        [AllowAnonymous]
        public ViewResult Error(string errorCode)
        {
            return View((object)errorCode);
        }

        [AllowAnonymous]
        public ActionResult SignOut()
        {
            var currentSession = Session[Constants.APOLLO_CURRENT_SESSION_KEY] as CurrentSession;
            string redirectUrl = currentSession != null && currentSession.IsCsdAdmin
                ? "~/Internal/SignIn"
                : FormsAuthentication.LoginUrl;

            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            return Redirect(redirectUrl);
        }
    }
}
