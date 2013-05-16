// --------------------------------------------------------------------------
// <copyright file="OAuthController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
//   <change who="Anders.Le" date="2013.5.7">Refactor GetCode action</change>
//   <change who="Anders.Le" date="2013.5.9">Add log</change>
//   <change who="Anders.Le" date="2013.5.15">Change error message log</change>
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
    using System.Threading.Tasks;
    using Apollo.Age.Web.Services;

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
                                ICurrentSession currentSession = new CurrentSession(user, 0, string.Empty ,false, language);
                                SessionManager.CurrentSession = currentSession;
                                FormsAuthentication.SetAuthCookie(user.Username, true);

                                //Write log SSO
                                string doing = "SSO logged in via Agent: " + user.Username + " - Result: Successful.";
                                WritelogLogin(user.Id, 0, 1400, 0, doing);
                            }
                            else
                            {
                                LogService.Error(string.Format("User {0} not found or not the requested user ({1})", authModel.Username, user.Username));
                                authModel.ErrorCode = ErrorCode.InvalidRequest;

                                string doing = "SSO logged in via Agent: " + user.Username + " - Result: Fail(User not found).";
                                WritelogLogin(0, 0, 1401, 0, doing);
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
        public void SignOut()
        {
            string doing = "SSO log out via Agent: " + SessionManager.CurrentSession.Username + " - Result: Successful.";
            WritelogLogin(SessionManager.CurrentSession.UserId, 0, 1402, 0, doing);

            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }

        private void WritelogLogin(int agentId, int agentUserId, int actionId, int serverId, string doing)
        {
            Task.Factory.StartNew(() =>
            {
                DeviceService deviceService = new DeviceService(Request);
                _userService.WriteLogInternalLogin(
                    agentId,
                    agentUserId,
                    serverId,
                    actionId,
                    doing,
                    deviceService.ClientIp,
                    deviceService.DomainName,
                    deviceService.BrowserName,
                    deviceService.Platform,
                    deviceService.CountryCode
                    );
            });
        }
    }
}