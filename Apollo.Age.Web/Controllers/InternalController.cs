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
    using System.Linq;
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
    using Fanex.Logging;
    using Fanex.Common;

    public class InternalController : Controller
    {
        #region Const

        private const string ApolloSessionCaptcha = "ApolloCaptcha";
        private const string AllowUpdate = "AllowUpdate";
        private const string Language = "Language";
        private const string AdminName = "AdminName";
        private const string AdminId = "AdminId";
        private const int ServerId = 2;

        #endregion Const

        #region Services

        private readonly IUserService _userService;
        private DeviceDetection _deviceService;

        #endregion Services

        #region Ctors

        public InternalController()
        {
            _userService = new UserService();
            _deviceService = new DeviceDetection();
        }

        #endregion Ctors

        public ActionResult Index()
        {
            return RedirectToAction("SignIn");
        }

        #region SignIn

        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            return View(new SignInViewModel());
        }

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

                int _AdminId = 0;
                bool napUserIsValid = _userService.AuthenticateNapUser(new AuthenticateNapUserCriteria
                    {
                        Username = model.Username,
                        Password = model.Password
                    }, ref _AdminId);

                #endregion check if NAP username & password are correct

                if (napUserIsValid) // redirect to SelectAccount page
                {
                    TempData[Language] = model.Language;
                    TempData[AdminName] = model.Username;
                    TempData[AdminId] = _AdminId;

                    //Write log
                    //ActionId: 700 - Login successful
                    string doing = string.Format("Admin: [{0}] Logged in to Apollo Agent Site - Result: Successful.",
                                    model.Username);
                    WriteLoginLog(0, _AdminId, 700, doing);

                    return RedirectToAction("SelectAccount");
                }
                else // return View with error message
                {
                    //Write log
                    //ActionId: 6000 - Login fail
                    string doing = string.Format("Admin: [{0}] Logged in to Apollo Agent Site - Result: Failed({1}) .",
                                    model.Username,
                                    Lang.InvalidUsernameOrPassword);
                    WriteLoginLog(0, _AdminId, 7000, doing);

                    ModelState.AddModelError(string.Empty, Lang.InvalidUsernameOrPassword);
                }
            }

            return View(model);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAccount(SelectAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAllowUpdate = false;

                if (model.Agent.ToLower() == "super")
                {
                    if (string.IsNullOrWhiteSpace(model.SafeCode))
                    {

                        ModelState.AddModelError("Security Code", "The security code field is required");
                        return View(model);
                    }
                    else
                    {
                        var enCryptSafeCode = Cryptography.CfsEnCrypt(model.SafeCode);
                        if (enCryptSafeCode == AppSettings.SuperSafeCode)
                        {
                            isAllowUpdate = true;
                        }
                        else
                        {
                            ModelState.AddModelError("Security Code", "Wrong security code");
                            return View(model);
                        }
                    }
                }

                User user = _userService.InternalGetAgentByUsername(model.Agent);

                if (user != null) // continue sign-in process
                {
                    var userStatus = _userService.GetUserStatus(user.Agent.Id, user.Id);

                    switch (userStatus)
                    {
                        case (int)CustomerStatus.Active:
                            {
                                var onUserId = _userService.InternalCheckIn(model.AdminId,
                                HttpContext.Session.SessionID, _deviceService.ClientIp);
                                if (onUserId == 0)
                                    return SignOut();
                                // AppSettings.AllowInternalUpdate is a workaround solution for internal testing and
                                // may be removed in future
                                user.IsMaster = isAllowUpdate;

                                // saves user session
                                SessionManager.CurrentSession = new CurrentSession(
                                    user: user,
                                    adminId: model.AdminId,
                                    adminName: model.AdminName,
                                    onUserId: onUserId,
                                    isInternalSignIn: true, // users sign in by this view should be internal users (CSD Admin or Devs, QCs, etc)
                                    language: model.Language);

                                // Write log internal login
                                // ActionId: 701 - Select Account successful
                                string doing = string.Format("Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Successful.",
                                                model.AdminName,
                                                user.Agent.Username);

                               // LogService.Info(doing);
                                WriteLoginLog(user.Agent.Id, model.AdminId, 701, doing);

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
                else // return View with not-found error message
                {
                    //// Write log internal login
                    // ActionId: 6001 - Select Account fail
                    string doing = string.Format("Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Failed({2}).",
                                    model.AdminName,
                                    model.Agent,
                                    Lang.AgentNotFound);

                    WriteLoginLog(0, model.AdminId, 7001, doing);
                    ModelState.AddModelError("Agent", Lang.AgentNotFound);
                }
            }

            return View(model);
        }

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
                    _deviceService.CountryCode
                    );
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

        public ActionResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            var captcha = this.GetRandomString(3);
            // saves answer
            Session[ApolloSessionCaptcha] = captcha;

            FileContentResult img = null; // image stream

            using (var mem = new MemoryStream())
            using (var bmp = GenerateNoise(55, 23))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;

                // add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.White);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(255, 255, 255);

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                // add captcha
                gfx.DrawString(captcha.ToString(), new Font("Consolas", 13), Brushes.Crimson, 5, 2);

                // render as JPEG
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                img = this.File(mem.GetBuffer(), "image/png");
            }

            return img;
        }

        private const string RandomString = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly int RandomStringLength = RandomString.Length;
        private string GetRandomString(int seed, int length = 4)
        {
            //use the following string to control your set of alphabetic characters to choose from
            //for example, you could include uppercase too
            //const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            //// Random is not truly random,
            //// so we try to encourage better randomness by always changing the seed value
            Random rnd = new Random((seed + DateTime.Now.Millisecond));
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {

                result += RandomString[rnd.Next(0, RandomStringLength)];
            }

            //// basic 5 digit random number
            //string result = rnd.Next(1000, 9999).ToString();

            //// single random character in ascii range a-z
            //string alphaChar = alphabet.Substring(rnd.Next(0, alphabet.Length - 1), 1);

            //// random position to put the alpha character
            //int replacementIndex = rnd.Next(0, (result.Length - 1));
            //result = result.Remove(replacementIndex, 1).Insert(replacementIndex, alphaChar);



            return result;


        }

        #endregion SignIn

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

        private bool IsInternalSignIn
        {
            get { return SessionManager.CurrentSession != null && SessionManager.CurrentSession.IsInternalSignIn; }
        }

        #region Helpers

        public Bitmap GenerateNoise(int width, int height)
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