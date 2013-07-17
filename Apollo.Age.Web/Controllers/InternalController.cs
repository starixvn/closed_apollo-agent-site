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

    /// <summary>
    /// Internal Login Controller
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
        private readonly int _RandomStringLength = _RandomString.Length;

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

        /// <summary>
        /// Get infor logged in by internal or not.
        /// </summary>
        private bool IsInternalSignIn
        {
            get { return SessionManager.CurrentSession != null && SessionManager.CurrentSession.IsInternalSignIn; }
        }

        #endregion Services

        /// <summary>
        /// Index Action.
        /// </summary>
        /// <returns>Redirect to Sigin action.</returns>
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
        public ActionResult SignIn(SignInViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            if (ModelState.IsValid)
            {
                #region if captcha is null or incorrect then return View with captcha error message

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

                #endregion if captcha is null or incorrect then return View with captcha error message

                #region check if NAP username & password are correct

                int _adminId = 0;
                bool napUserIsValid = _userService.AuthenticateNapUser(
                    new AuthenticateNapUserCriteria
                    {
                        Username = model.Username,
                        Password = model.Password
                    },
                    ref _adminId);

                #endregion check if NAP username & password are correct

                // redirect to SelectAccount page.
                if (napUserIsValid)
                {
                    TempData[Language] = model.Language;
                    TempData[AdminName] = model.Username;
                    TempData[AdminId] = _adminId;

                    //Write log
                    //ActionId: 700 - Login successful
                    string doing = string.Format(
                        "Admin: [{0}] Logged in to Apollo Agent Site - Result: Successful.",
                        model.Username);
                    WriteLoginLog(0, _adminId, 700, doing);

                    return RedirectToAction("SelectAccount");
                }
                else
                {
                    //Write log
                    //ActionId: 6000 - Login fail
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
        /// <returns>Return Json.</returns>
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
            // admin name must exist to continue the sign-in process
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
        /// Post Select Account Action.
        /// </summary>
        /// <param name="model">Input View Model</param>
        /// <returns>Redirect to home page or Safe code page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAccount(SelectAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAllowUpdate = false;

                if (model.Agent.ToLower() == "super")
                {
                    TempData[AgentName] = model.Agent;
                    TempData[Language] = model.Language;
                    TempData[AdminName] = model.AdminName;
                    TempData[AdminId] = model.AdminId;

                    return RedirectToAction("SafeCode");
                }
                else
                {
                    return UserInfo(model.Agent, model.AdminId, model.AdminName, model.Language, isAllowUpdate);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Get Safe Code Action.
        /// </summary>
        /// <returns>Return View Model.</returns>
        [HttpGet]
        public ActionResult SafeCode()
        {
            if (TempData[AgentName] == null && SessionManager.CurrentSession == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            return View(new SafeCodeViewModel
            {
                AdminId = Convert.ToInt32(TempData[AdminId]),
                AdminName = TempData[AdminName].ToString(),
                Agent = TempData[AgentName].ToString(),
                Language = TempData[Language].ToString()
            });
        }

        /// <summary>
        /// Post Action SafeCode.
        /// </summary>
        /// <param name="model">Input View Model.</param>
        /// <returns>Redirect to Home Page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SafeCode(SafeCodeViewModel model)
        {
            bool isAllowUpdate = false;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.SecurityCode))
                {
                    ModelState.AddModelError("Security Code", Lang.lblsecuritycodeempty);
                    return View(model);
                }
                else
                {
                    var encryptSafeCode = Cryptography.CfsEnCrypt(model.SecurityCode);
                    if (encryptSafeCode == AppSettings.SuperSafeCode)
                    {
                        isAllowUpdate = true;
                    }
                    else
                    {
                        ModelState.AddModelError("Security Code", Lang.lblwrongsecuritycode);
                        return View(model);
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

            /// saves answer
          
            Session[ApolloSessionCaptcha] = captcha;

            FileContentResult img = null; /// image stream

            using (var mem = new MemoryStream())
            using (var bmp = GenerateNoise(55, 23))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;

                /// add noise
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

                /// add captcha
                gfx.DrawString(captcha.ToString(), new Font("Consolas", 13), Brushes.Crimson, 5, 2);

                /// render as JPEG
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

        /// <summary>
        /// Function Get User Info Login.
        /// </summary>
        /// <param name="agentName">Agent UserName.</param>
        /// <param name="adminId">Admin Id.</param>
        /// <param name="adminName">Admin Name.</param>
        /// <param name="language">Language.</param>
        /// <param name="isAllowUpdate">Allow Update.</param>
        /// <returns></returns>
        private ActionResult UserInfo(string agentName, int adminId, string adminName, string language, bool isAllowUpdate)
        {
            User user = _userService.InternalGetAgentByUsername(agentName);
            /// continue sign-in process.
            if (user != null)
            {
                var userStatus = _userService.GetUserStatus(user.Agent.Id, user.Id);

                switch (userStatus)
                {
                    case (int)CustomerStatus.Active:
                        {
                            var onUserId = _userService.InternalCheckIn(
                                adminId,
                            HttpContext.Session.SessionID,
                            _deviceService.ClientIp);
                            if (onUserId == 0)
                            {
                                return SignOut();
                            }
                            /// AppSettings.AllowInternalUpdate is a workaround solution for internal testing and.
                            /// may be removed in future.
                            user.IsMaster = isAllowUpdate;

                            /// saves user session
                            SessionManager.CurrentSession = new CurrentSession(
                                user: user,
                                adminId: adminId,
                                adminName: adminName,
                                onUserId: onUserId,
                                isInternalSignIn: true, // users sign in by this view should be internal users (CSD Admin or Devs, QCs, etc).
                                language: language);

                            /// Write log internal login.
                            /// ActionId: 701 - Select Account successful.
                            string doing = string.Format(
                                "Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Successful.",
                                            adminName,
                                            user.Agent.Username);

                            WriteLoginLog(user.Agent.Id, adminId, 701, doing);

                            FormsAuthentication.RedirectFromLoginPage(user.Agent.Username, createPersistentCookie: false);

                            break;
                        }

                    case (int)CustomerStatus.Locked:
                        {
                            ModelState.AddModelError("Agent", Lang.lblAccountWasLocked);

                            break;
                        }

                    default:
                        {
                            ModelState.AddModelError("Agent", Lang.lblAccountWasStopped);

                            break;
                        }
                }
            }
            else
            {
                /// Write log internal login.
                /// ActionId: 6001 - Select Account fail.
                string doing = string.Format(
                    "Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Failed({2}).",
                                adminName,
                                agentName,
                                Lang.AgentNotFound);

                WriteLoginLog(0, adminId, 7001, doing);
                ModelState.AddModelError("Agent", Lang.AgentNotFound);
            }

            return View();
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="agentId">Agent Id</param>
        /// <param name="agentUserId">Agent User Id</param>
        /// <param name="actionId">Action Id</param>
        /// <param name="doing">Doing</param>
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
        /// <param name="seed">input seed.</param>
        /// <param name="length">input length of captcha.</param>
        /// <returns>return string captcha.</returns>
        private string GetRandomString(int seed, int length = 4)
        {
            //use the following string to control your set of alphabetic characters to choose from
            //for example, you could include uppercase too
            //const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            //// Random is not truly random,
            //// so we try to encourage better randomness by always changing the seed value
            Random rnd = new Random(seed + DateTime.Now.Millisecond);
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                result += _RandomString[rnd.Next(0, _RandomStringLength)];
            }                       

            return result;
        }

        #region Helpers

        /// <summary>
        /// Generate Noise.
        /// </summary>
        /// <param name="width">input width image</param>
        /// <param name="height">input height image</param>
        /// <returns>return bitmap</returns>
        private Bitmap GenerateNoise(int width, int height)
        {
            var finalBmp = new Bitmap(width, height);
            Random r = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int num = r.Next(0, 256);
                    finalBmp.SetPixel(x, y, Color.FromArgb(175, 172, 137));
                }
            }

            return finalBmp;
        }

        #endregion Helpers
    }
}