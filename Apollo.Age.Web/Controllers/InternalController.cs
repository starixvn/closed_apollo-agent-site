// --------------------------------------------------------------------------
// <copyright file="InternalController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis" date="2013.5.2">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Configurations;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.Services;
    using Apollo.Age.Web.ViewModels;
    using System.Threading.Tasks;

    public class InternalController : Controller
    {
        #region Const

        private const string APOLLOSESSIONCAPTCHA = "ApolloCaptcha";
        private const string ALLOWUPDATE = "AllowUpdate";
        private const string LANGUAGE = "Language";
        private const string ADMINNAME = "AdminName";
        private const string ADMINID = "AdminId";
        private const int SERVERID = 2;

        #endregion Const

        #region Services

        private readonly IUserService _userService;

        #endregion Services

        #region Ctors

        public InternalController()
        {
            _userService = new UserService();
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

            // Hard code for Edward testing
            if (model.Username == "pgmedward" || ModelState.IsValid)
            {
                #region if captcha is null or incorrect then return View with captcha error message

                var captcha = Session[APOLLOSESSIONCAPTCHA];
                if (model.Username != "pgmedward" && (captcha == null || captcha.ToString() != model.Captcha))
                {
                    ModelState.AddModelError("Captcha", Lang.IncorrectCaptchaMsg);
                    return View(model);
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
                    TempData[LANGUAGE] = model.Language;
                    TempData[ADMINNAME] = model.Username;
                    TempData[ADMINID] = _AdminId;

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
            if(SessionManager.CurrentSession != null)
            {
                if (AppSettings.MultipleSignOn)
                {
                    result = _userService.InternalReCheckIn(SessionManager.CurrentSession.OnUserId);
                }
            }

            return Json(new { 
                isKickout = result
            });
        }

        [HttpGet]
        public ActionResult SelectAccount()
        {
            // admin name must exist to continue the sign-in process
            if (TempData[ADMINNAME] == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            return View(new SelectAccountViewModel { Language = TempData[LANGUAGE].ToString(), AdminId = Convert.ToInt32(TempData[ADMINID]), AdminName = TempData[ADMINNAME].ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAccount(SelectAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _userService.AgentLoginByUsername(model.Agent);

                if (user != null) // continue sign-in process
                {

                    var onUserId = _userService.InternalCheckIn(model.AdminId, HttpContext.Session.SessionID, Request.ServerVariables["REMOTE_ADDR"].ToString());
                    if (onUserId == 0)
                        return SignOut();
                    // AppSettings.AllowInternalUpdate is a workaround solution for internal testing and
                    // may be removed in future
                    user.IsMaster = user.IsMaster && AppSettings.AllowInternalUpdate && model.AllowUpdate;

                    // saves user session
                    SessionManager.CurrentSession = new CurrentSession(
                        user: user,
                        adminId: model.AdminId,
                        adminName: model.AdminName,
                        onUserId : onUserId,                        
                        isInternalSignIn: true, // users sign in by this view should be internal users (CSD Admin or Devs, QCs, etc)
                        language: model.Language);

                    // Write log internal login
                    // ActionId: 701 - Select Account successful
                    string doing = string.Format("Admin: [{0}] via Agent: [{1}] Logged in to Apollo Agent Site - Result: Successful.",
                                    model.AdminName,
                                    user.Agent.Username);
                    WriteLoginLog(user.Agent.Id, model.AdminId, 701, doing);

                    FormsAuthentication.RedirectFromLoginPage(user.Agent.Username, createPersistentCookie: false);
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
            Task.Factory.StartNew(() =>
            {               
                DeviceService deviceService = new DeviceService(Request);
                _userService.WriteLogInternalLogin(
                    agentId,
                    agentUserId,
                    SERVERID,
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

        public ActionResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            int captcha = rand.Next(1000, 9999);

            // saves answer
            Session[APOLLOSESSIONCAPTCHA] = captcha.ToString();

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
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                // add captcha
                gfx.DrawString(captcha.ToString(), new Font("Tahoma", 13), Brushes.White, 5, 2);

                // render as JPEG
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
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
                    finalBmp.SetPixel(x, y, Color.Gray);
                }
            }

            return finalBmp;
        }

        #endregion Helpers
    }
}