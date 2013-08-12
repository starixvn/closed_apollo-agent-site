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
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Integration.Models;
    using Apollo.Age.Integration.Services;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Apollo.Age.Web.Services;
    using Fanex.Logging;

    [AllowAnonymous]
    public class OAuthController : Controller
    {
        private const int ServerId = 1;

        private readonly AuthenticationService _authService;
        private readonly UserService _userService;
        private DeviceDetection _deviceService;

        public OAuthController()
            : this(null)
        {
        }

        public OAuthController(AuthenticationService authService = null, UserService userService = null)
        {
            _authService = authService ?? new AuthenticationService();
            _userService = userService ?? new UserService();
            _deviceService = new DeviceDetection();
        }

        [AllowAnonymous]
        public ViewResult Index(string user_id, string lang)
        {
            ViewBag.Username = user_id;
            ViewBag.Language = _authService.ConvertToApolloLanguage(lang);
            return View();
        }

        [P3P]
        [AllowAnonymous]
        public ActionResult GetCode(string code, string error, string state, string username, string language)
        {
          

            var authModel = new AuthModel();


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

                        if (authModel != null && authModel.IsValid)
                        {
                            
                            // username is empty or user is disallowed sso
                            if (!_userService.AllowLineAccountSSO(username))
                            {
                                authModel.ErrorCode = ErrorCode.ComingSoon;
                                return View(authModel);
                            }

                            Entities.User user = _userService.GetUserByUsername(authModel.Username);

                            // check whether authenticated username (from sungame agentsite) is the same with
                            // requested sso username
                            if (user != null)
                            {
                                if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                                {
                                    authModel.ErrorCode = string.Empty;
                                    SessionManager.CurrentSession = new CurrentSession(user, 0, string.Empty, 0, false, language);
                                    FormsAuthentication.SetAuthCookie(user.Username, true);

                                    // writes SSO logs
                                    // ActionID: 703 -> SSO login successful.
                                    string doing = string.Format("SSO logged in via Agent: [{0}] - Result: Successful.", user.Username);
                                    WriteLoginLog(user.Id, 0, 703, 0, doing);
                                }
                                else
                                {
#if DEBUG
                                    LogService.Error(string.Format("User {0} is not the requested user [{1}]", authModel.Username, user.Username));
#endif
                                    authModel.ErrorCode = ErrorCode.InvalidRequest;

                                    // ActionID: 7002 -> SSO login fail.
                                    string doing = string.Format("SSO logged in via Agent: [{0}] - Result: Fail({1}).",
                                                    authModel.Username,
                                                    Resources.Lang.AgentNotFound);
                                    WriteLoginLog(0, 0, 7002, 0, doing);
                                }
                            }
                            else
                            {
#if DEBUG
                                LogService.Error(string.Format("User {0} is not found", authModel.Username));
#endif
                                authModel.ErrorCode = ErrorCode.InvalidRequest;

                                // ActionID: 7002 -> SSO login fail.
                                string doing = string.Format("SSO logged in via Agent: [{0}] - Result: Fail({1}).",
                                                authModel.Username,
                                                Resources.Lang.AgentNotFound);
                                WriteLoginLog(0, 0, 7002, 0, doing);
                            }
                        }
                        else
                        {
                            LogService.Error("Authorization fail.");

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
        public ViewResult Error(string error_code)
        {
            return View((object)error_code);
        }

        [AllowAnonymous]
        public ViewResult ComingSoon()
        {
            return View();
        }

        [AllowAnonymous]
        public void SignOut()
        {
            if (SessionManager.CurrentSession != null)
            {
                //write log
                // ActionId: 704 - sso logout successful
                string doing = string.Format("SSO log out via Agent: [{0}] - Result: Successful.", SessionManager.CurrentSession.Username);
                WriteLoginLog(SessionManager.CurrentSession.UserId, 0, 704, 0, doing);
            }
            Session.Abandon();
            Response.Cookies.Clear();
            // FormsAuthentication.SignOut();
        }

        private void WriteLoginLog(int agentId, int agentUserId, int actionId, int serverId, string doing)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                _userService.WriteLogInternalLogin(
                    agentId,
                    agentUserId,
                    ServerId,
                    actionId,
                    doing,
                    _deviceService.ClientIp,
                    _deviceService.DomainName,
                    _deviceService.BrowserName,
                    _deviceService.Platform,
                    _deviceService.CountryCode);
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                LogService.Error(ex.Message);
                throw ex.Flatten();
            }
        }
    }
}