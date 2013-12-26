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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Integration.Models;
    using Apollo.Age.Integration.Services;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Fanex.Logging;

    /// <summary>
    /// Authentication Controller Class.
    /// </summary>
    [AllowAnonymous]
    public class OAuthController : Controller
    {
        private const int ServerId = 1;     // SSO Server ID

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

        /// <summary>
        /// Index page to get some property.
        /// </summary>
        /// <param name="user_id">Input a user id.</param>
        /// <param name="lang">Input language.</param>
        /// <returns>Return view.</returns>
        [AllowAnonymous]
        public ViewResult Index(string user_id, string lang)
        {
            ViewBag.Username = user_id;
            ViewBag.Language = _authService.ConvertToApolloLanguage(lang);

            return View();
        }

        /// <summary>
        /// Get code from API.
        /// </summary>
        /// <param name="code">Input a code.</param>
        /// <param name="error">Input a default error.</param>
        /// <param name="state">Input a default state.</param>
        /// <param name="username">Input a username.</param>
        /// <param name="language">Input a language.</param>
        /// <returns>Return a view.</returns>
        [P3P]
        [AllowAnonymous]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public ActionResult GetCode(string code, string error, string state, string username, string language)
        {
            var authModel = new AuthModel();

            if (!string.IsNullOrWhiteSpace(error))
            {
                authModel.ErrorCode = error;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    // Username is empty or user is disallowed sso.
                    // Disallow shadow sc88 access to our system by sso way.
                    if (!_userService.AllowLineAccountSSO(username))
                    {
                        authModel.ErrorCode = ErrorCode.ComingSoon;
                        return View(authModel);
                    }

                    authModel = _authService.GetAuthInfo(authorizationCode: code);

                    if (authModel != null && authModel.IsValid)
                    {
                        Entities.User user = _userService.GetUserByUsername(authModel.Username);

                        // Check whether authenticated username (from sungame agentsite) is the same with.
                        // Requested sso username.
                        if (user != null)
                        {
                            if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                            {
                                authModel.ErrorCode = string.Empty;
                                SessionManager.CurrentSession = new CurrentSession(user, 0, string.Empty, 0, false, language);
                                FormsAuthentication.SetAuthCookie(user.Username, true);

                                // Writes SSO logs.
                                // ActionID: 703 -> SSO login successful.
                                string doing = string.Format("SSO logged in via Agent: [{0}] - Result: Successful.", user.Username);
                                WriteLoginLog(user.Id, 0, 703, doing);
                            }
                            else
                            {
#if DEBUG
                                LogService.Info(string.Format("SSO user {0} is not the requested user [{1}]", username, user.Username));
#endif
                                authModel.ErrorCode = ErrorCode.InvalidRequest;

                                // ActionID: 7002 -> SSO login fail.
                                string doing = string.Format(
                                                "SSO logged in via Agent: [{0}] - Result: Fail({1}).",
                                                authModel.Username,
                                                Resources.Lang.AgentNotFound);
                                WriteLoginLog(0, 0, 7002, doing);
                            }
                        }
                        else
                        {
#if DEBUG
                            LogService.Error(string.Format("SSO user {0} is not found", authModel.Username));
#endif
                            authModel.ErrorCode = ErrorCode.InvalidRequest;

                            // ActionID: 7002 -> SSO login fail.
                            string doing = string.Format(
                                            "SSO logged in via Agent: [{0}] - Result: Fail({1}).",
                                            authModel.Username,
                                            Resources.Lang.AgentNotFound);
                            WriteLoginLog(0, 0, 7002, doing);
                        }
                    }
                    else
                    {
                        authModel.ErrorCode = ErrorCode.InvalidRequest;
                    }
                }
                else
                {
                    authModel.ErrorCode = ErrorCode.InvalidRequest;
                }
            }

            return View(authModel);
        }

        /// <summary>
        /// Error action.
        /// </summary>
        /// <param name="error_code">Input a error code.</param>
        /// <returns>Return a page error.</returns>
        [AllowAnonymous]
        public ViewResult Error(string error_code)
        {
            return View((object)error_code);
        }

        /// <summary>
        /// Coming soon action.
        /// </summary>
        /// <returns>Return coming soon page view.</returns>
        [AllowAnonymous]
        public ViewResult ComingSoon()
        {
            return View();
        }

        /// <summary>
        /// Sign out action.
        /// </summary>
        [AllowAnonymous]
        public void SignOut()
        {
            if (SessionManager.CurrentSession != null)
            {
                // Write log.
                // ActionId: 704 - sso logout successful
                string doing = string.Format("SSO log out via Agent: [{0}] - Result: Successful.", SessionManager.CurrentSession.Username);
                WriteLoginLog(SessionManager.CurrentSession.UserId, 0, 704, doing);
            }

            Session.Abandon();
            Response.Cookies.Clear();
        }

        /// <summary>
        /// Write log in log.
        /// </summary>
        /// <param name="agentId">Input a agent id.</param>
        /// <param name="agentUserId">Input a agent user id.</param>
        /// <param name="actionId">Input a action id.</param>
        /// <param name="doing">Input a doing.</param>
        private void WriteLoginLog(int agentId, int agentUserId, int actionId, string doing)
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