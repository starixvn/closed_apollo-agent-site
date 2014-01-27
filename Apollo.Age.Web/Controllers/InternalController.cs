// --------------------------------------------------------------------------
// <copyright file="InternalController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis" date="2013.5.2">Created</change>
//   <change who="Anders.Le" date="2013.6.12">Remove hardcode when checking captcha</change>
//   <change who="Anders.Le" date="2013.6.27">Change the implementation of random string</change>
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Configurations;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.ViewModels;
    using Fanex.Common;
    using Fanex.Logging;
    using System.Globalization;

    /// <summary>
    /// Internal Login Controller.
    /// </summary>
    public class InternalController : Controller
    {
        #region Const

        private const string ApolloSessionCaptcha = "ApolloCaptcha";
        private const string AllowUpdate = "AllowUpdate";
        private const string Language = "Language";
        private const string AdminName = "AdminName";
        private const string AdminId = "AdminId";
        private const string AgentName = "AgentName";
        private const int ServerId = 2;
        private const string _RandomString = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly int _randomStringLength = _RandomString.Length;
        private readonly int _expirationTime = 100; // 30s for expirating.

        #endregion Const

        #region Services

        private readonly IUserService _userService;
        private DeviceDetection _deviceService;

        #region Ctors

        public InternalController()
        {
            _userService = new UserService();
            _deviceService = new DeviceDetection();
        }

        #endregion Ctors

        #endregion Services

        /// <summary>
        /// Index Action.
        /// </summary>
        /// <returns>Redirect to Sign in action.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("SignIn");
        }

        #region SignIn

        /// <summary>
        /// Get SignIn Page.
        /// </summary>
        /// <returns>Return model.</returns>
        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            return View(new SignInViewModel());
        }

        /// <summary>
        /// Post SignIn Page.
        /// </summary>
        /// <param name="model">Input model.</param>
        /// <returns>Redirect to Select Account Action.</returns>
        [HttpPost]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            if (ModelState.IsValid)
            {
                // if captcha is null or incorrect then return View with captcha error message
                var captcha = Session[ApolloSessionCaptcha];

                if (Request.IsLocal)
                {
                    if (model.Captcha != "9999")
                    {
                        if (captcha == null || captcha.ToString() != model.Captcha.ToUpper())
                        {
                            ModelState.AddModelError("Captcha", Lang.IncorrectCaptchaMsg);
                            return View(model);
                        }
                    }
                }
                else
                {
                    if (captcha == null || captcha.ToString() != model.Captcha.ToUpper())
                    {
                        ModelState.AddModelError("Captcha", Lang.IncorrectCaptchaMsg);
                        return View(model);
                    }
                }

                // check if NAP username & password are correct
                int _adminId = 0;
                bool napUserIsValid = _userService.AuthenticateNapUser(
                    new AuthenticateNapUserCriteria
                    {
                        Username = model.Username,
                        Password = model.Password
                    },
                    ref _adminId);

                // redirect to SelectAccount page.
                if (napUserIsValid)
                {
                    TempData[Language] = model.Language;
                    TempData[AdminName] = model.Username;
                    TempData[AdminId] = _adminId;

                    // Write log.
                    // ActionId: 700 - Login successful.
                    string doing = string.Format(
                        "Admin: [{0}] Logged in to Apollo Agent Site - Result: Successful.",
                        model.Username);
                    WriteLoginLog(0, _adminId, 700, doing);

                    return RedirectToAction("SelectAccount");
                }
                else
                {
                    // Write log.
                    // ActionId: 6000 - Login fail.
                    string doing = string.Format(
                        "Admin: [{0}] Logged in to Apollo Agent Site - Result: Failed({1}) .",
                                    model.Username,
                                    Lang.InvalidUsernameOrPassword);
                    WriteLoginLog(0, _adminId, 7000, doing);

                    ModelState.AddModelError(string.Empty, Lang.InvalidUsernameOrPassword);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Post ReCheckIn.
        /// </summary>
        /// <returns>Return true or false.</returns>
        [HttpPost]
        public JsonResult ReCheckIn()
        {
            var result = false;
            if (SessionManager.CurrentSession != null)
            {
                result = _userService.InternalReCheckIn(SessionManager.CurrentSession.OnUserId);
            }

            return Json(new
            {
                isKickout = result
            });
        }

        /// <summary>
        /// Get Select Account Action.
        /// </summary>
        /// <returns>Return Model.</returns>
        [HttpGet]
        public ActionResult SelectAccount()
        {
            // Admin name must exist to continue the sign-in process.
            if (TempData[AdminName] == null && SessionManager.CurrentSession == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            else
            {
                if (SessionManager.CurrentSession != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(SessionManager.CurrentSession.Username, createPersistentCookie: false);
                }
                else
                {
                    return View(new SelectAccountViewModel
                    {
                        Language = TempData[Language].ToString(),
                        AdminId = Convert.ToInt32(TempData[AdminId]),
                        AdminName = TempData[AdminName].ToString()
                    });
                }
            }

            return View();
        }

        /// <summary>
        /// Internals the login.
        /// </summary>
        /// <param name="onlineUserId">The on user identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="adminName">Name of the admin.</param>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="loginTime">The login time.</param>
        /// <returns>
        /// Action result.
        /// </returns>
        [AllowAnonymous]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [HttpGet]
        public ActionResult SSOLogin(string onlineUserId, string adminId, string adminName, string agentId, string loginTime)
        {
            if (!string.IsNullOrWhiteSpace(onlineUserId) && 
                !string.IsNullOrWhiteSpace(adminId) && 
                !string.IsNullOrWhiteSpace(adminName) && 
                !string.IsNullOrWhiteSpace(agentId) && 
                !string.IsNullOrWhiteSpace(loginTime))
            {
                onlineUserId = Server.HtmlDecode(onlineUserId).Replace(" ", "+");
                adminId = Server.HtmlDecode(adminId).Replace(" ", "+");
                adminName = Server.HtmlDecode(adminName).Replace(" ", "+");
                agentId = Server.HtmlDecode(agentId).Replace(" ", "+");
                loginTime = Server.HtmlDecode(loginTime).Replace(" ", "+");

                var beforeTime = Cryptography.CfsDeCrypt(loginTime).Trim();
                DateTime currentLoginTime = DateTime.ParseExact(beforeTime, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                int currentAgentId = Convert.ToInt32(Cryptography.CfsDeCrypt(agentId));
                int currentOnUserId = Convert.ToInt32(Cryptography.CfsDeCrypt(onlineUserId));
                int currentAdminId = Convert.ToInt32(Cryptography.CfsDeCrypt(adminId));
                string currentAdminName = Cryptography.CfsDeCrypt(adminName);
                int siteId = AppSettings.SSOSiteId;
                int onlineAdminId = 0;

                DateTime currentTime = _userService.SSOGetTime(new SSOGetTimeCriteria
                {
                    Time = DateTime.Now
                });

                var expirationTime = currentTime - currentLoginTime;
                if (expirationTime.TotalSeconds <= _expirationTime)
                {
                    // SSO Check-in again.
                    var criteria = new SSOCheckInCriteria
                    {
                        CheckOnUserId = currentOnUserId,
                        OnUserId = 0,
                        SiteId = siteId,
                        LoginDate = DateTime.Now,
                        NetSessionId = Session.SessionID,
                        UserId = currentAdminId,
                        IP = _deviceService.ClientIp
                    };

                    onlineAdminId = _userService.SSOCheckIn(criteria);

                    if (onlineAdminId > 0)
                    {
                        var user = _userService.GetUserById(currentAgentId);
                        if (user != null)
                        {
                            User agentUser = null;
                            int userId = 0;
                            var userChecking = this.UserInfoChecking(user.Username, currentAdminId, ref agentUser, ref userId);
                            if (userChecking == 0)
                            {
                                CreateLoginSession(currentAdminId, currentAdminName, "en-US", false, agentUser, userId);

                                return Redirect(FormsAuthentication.DefaultUrl);
                            }
                        }
                    }
                }
                else
                {
                    Fanex.Logging.LogService.Info("Current time server:" + currentTime + " - Current login time:" + currentLoginTime + "- Total Second:" + expirationTime.TotalSeconds);
                }
            }

            return RedirectToAction(ApoURLs.URLs.Error + "?err=404");
        }

        /// <summary>
        /// Post Select Account Action.
        /// </summary>
        /// <param name="model">Input View Model.</param>
        /// <returns>Redirect to home page or Safe code page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAccount(SelectAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAllowUpdate = false;

                // Hard code for Super account.
                if (model.Agent.ToLower() == "super")
                {
                    if (string.IsNullOrWhiteSpace(model.SafeCode) == false)
                    {
                        var encryptSafeCode = Cryptography.CfsEnCrypt(model.SafeCode);
                        if (encryptSafeCode == AppSettings.SuperSafeCode)
                        {
                            // Allow Update once Customer has Security code.
                            isAllowUpdate = true;
                        }
                        else
                        {
                            ModelState.AddModelError("SafeCode", Lang.lblwrongsecuritycode);
                            return View(model);
                        }
                    }
                }

                return UserInfo(model.Agent, model.AdminId, model.AdminName, model.Language, isAllowUpdate);
            }

            return View(model);
        }

        /// <summary>
        /// Generate Captcha Image.
        /// </summary>
        /// <param name="noisy">Checking noise or not.</param>
        /// <returns>Return image.</returns>
        public ActionResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            var captcha = this.GetRandomString(3);

            // Saves answer
            Session[ApolloSessionCaptcha] = captcha;

            FileContentResult img = null; // Image stream.

            using (var mem = new MemoryStream())
            using (var bmp = GenerateNoise(55, 23))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;

                // Add noise.
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.White);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(255, 255, 255);
                        r = rand.Next(0, 130 / 3);
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);
                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                // Add captcha.
                gfx.DrawString(captcha.ToString(), new Font("Consolas", 13), Brushes.Crimson, 5, 2);

                // Render as JPEG.
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                img = this.File(mem.GetBuffer(), "image/png");
            }

            return img;
        }

        #endregion SignIn

        /// <summary>
        /// Sign out action.
        /// </summary>
        /// <returns>Redirect to Login Page.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public ActionResult SignOut()
        {
            var onUserId = 0;
            bool isInternalSignIn = false;
            if (SessionManager.CurrentSession != null)
            {
                isInternalSignIn = SessionManager.CurrentSession.IsInternalSignIn;
                onUserId = SessionManager.CurrentSession.OnUserId;
            }

            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            if (isInternalSignIn)
            {
                var result = _userService.InternalLogout(onUserId);
                if (result == 0)
                {
                    return Redirect(ApoURLs.URLs.InternalSignIn);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #region Helpers

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

        /// <summary>
        /// Generate Noise.
        /// </summary>
        /// <param name="width">Input width image.</param>
        /// <param name="height">Input height image.</param>
        /// <returns>Return bitmap.</returns>
        private Bitmap GenerateNoise(int width, int height)
        {
            var finalBmp = new Bitmap(width, height);
            Random r = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    finalBmp.SetPixel(x, y, Color.FromArgb(175, 172, 137));
                }
            }

            return finalBmp;
        }

        /// <summary>
        /// Write log login.
        /// </summary>
        /// <param name="agentId">Input AgentId.</param>
        /// <param name="agentUserId">Input AgentUserId.</param>
        /// <param name="actionId">Input ActionId.</param>
        /// <param name="doing">Input Doing.</param>
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

        /// <summary>
        /// Get Random string for captcha.
        /// </summary>
        /// <param name="seed">Input seed.</param>
        /// <param name="length">Input length of captcha.</param>
        /// <returns>Return string captcha.</returns>
        private string GetRandomString(int seed, int length = 4)
        {
            // Use the following string to control your set of alphabetic characters to choose from
            // For example, you could include uppercase too
            // Const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            // Random is not truly random,
            // So we try to encourage better randomness by always changing the seed value
            Random rnd = new Random(seed + DateTime.Now.Millisecond);
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                result += _RandomString[rnd.Next(0, _randomStringLength)];
            }

            return result;
        }

        #endregion Helpers

        /// <summary>
        /// Users the information checking.
        /// </summary>
        /// <param name="agentName">Name of the agent.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="agentUser">The agent user.</param>
        /// <param name="onlineUserId">The online user identifier.</param>
        /// <returns>
        /// Return user status.
        /// </returns>
        private int UserInfoChecking(string agentName, int adminId, ref User agentUser, ref int onlineUserId)
        {
            if (!string.IsNullOrWhiteSpace(agentName))
            {
                User user = _userService.InternalGetAgentByUsername(agentName);
                if (user != null)
                {
                    var userStatus = _userService.GetUserStatus(user.Agent.Id, user.Id);
                    switch (userStatus)
                    {
                        case (int)CustomerStatus.Active:
                            var onlineAgentId = _userService.InternalCheckIn(
                                adminId,
                                HttpContext.Session.SessionID,
                                _deviceService.ClientIp);
                            if (onlineAgentId == 0)
                            {
                                // Kick out user.
                                return 3;
                            }

                            onlineUserId = onlineAgentId;
                            agentUser = user;

                            // Login successfully.
                            return 0;
                        case (int)CustomerStatus.Locked:

                            // Account is locked.
                            return 1;
                        default:

                            // Account is stopped.
                            return 2;
                    }
                }
            }

            // Agent is not found.
            return 4;
        }

        /// <summary>
        /// Function Get User Info Login.
        /// </summary>
        /// <param name="agentName">Input AgentUserName.</param>
        /// <param name="adminId">Input AdminId.</param>
        /// <param name="adminName">Input AdminName.</param>
        /// <param name="language">Input Language.</param>
        /// <param name="isAllowUpdate">Input AllowUpdate.</param>
        /// <returns>Return Action Result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        private ActionResult UserInfo(string agentName, int adminId, string adminName, string language, bool isAllowUpdate)
        {
            User agentUser = null;
            int onlineUserId = 0;
            var userChecking = this.UserInfoChecking(agentName, adminId, ref agentUser, ref onlineUserId);

            switch (userChecking)
            {
                case 0:
                    {
                        CreateLoginSession(adminId, adminName, language, isAllowUpdate, agentUser, onlineUserId);

                        break;
                    }

                case 1:
                    {
                        // Account is locked.
                        ModelState.AddModelError("Agent", Lang.lblAccountWasLocked);
                        break;
                    }

                case 2:
                    {
                        // Account is stopped.
                        ModelState.AddModelError("Agent", Lang.lblAccountWasStopped);
                        break;
                    }

                case 3:
                    {
                        // Kick out account.
                        return SignOut();
                    }

                default:
                    {
                        // Agent is not found.
                        ModelState.AddModelError("Agent", Lang.AgentNotFound);
                        break;
                    }
            }

            return View();
        }

        /// <summary>
        /// Creates the login session.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="adminName">Name of the admin.</param>
        /// <param name="language">The language.</param>
        /// <param name="isAllowUpdate">If set to <c>true</c> [is allow update].</param>
        /// <param name="agentUser">The agent user.</param>
        /// <param name="onlineUserId">The online user identifier.</param>
        private void CreateLoginSession(int adminId, string adminName, string language, bool isAllowUpdate, User agentUser, int onlineUserId)
        {
            // Login successfull.
            // AppSettings.AllowInternalUpdate is a workaround solution for internal testing and.
            // May be removed in future.
            agentUser.IsMaster = isAllowUpdate;

            // Saves user session
            SessionManager.CurrentSession = new CurrentSession(
                user: agentUser,
                adminId: adminId,
                adminName: adminName,
                onUserId: onlineUserId,
                isInternalSignIn: true, // Users sign in by this view should be internal users (CSD Admin or Devs, QCs, etc).
                language: language);

            // Write log internal login.
            // ActionId: 701 - Select Account successful.
            string doing = string.Format(
                "Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Successful.",
                            adminName,
                            agentUser.Agent.Username);

            WriteLoginLog(agentUser.Agent.Id, adminId, 701, doing);

            FormsAuthentication.RedirectFromLoginPage(agentUser.Agent.Username, createPersistentCookie: false);
        }
    }
}