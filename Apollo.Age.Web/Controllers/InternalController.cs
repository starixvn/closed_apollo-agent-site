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
    using System.Threading.Tasks;
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

    public class InternalController : Controller
    {
        #region Const

        private const string APOLLOSESSIONCAPTCHA = "ApolloCaptcha";
        private const string ALLOWUPDATE = "AllowUpdate";
        private const string LANGUAGE = "Language";
        private const string ADMINNAME = "AdminName";
        private const string ADMINID = "AdminId";
        private const int SERVERID = 1111;

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

            if (ModelState.IsValid)
            {
                #region if captcha is null or incorrect then return View with captcha error message

                var captcha = Session[APOLLOSESSIONCAPTCHA];
                if (captcha == null || captcha.ToString() != model.Captcha)
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

                    //write log
                    string doing = "Admin: " + model.Username + " Logged in to Apollo Agent Site - Result: Successful.";
                    WritelogLogin(0, _AdminId, 1307, doing);

                    return RedirectToAction("SelectAccount");
                }
                else // return View with error message
                {
                    string doing = "Admin: " + model.Username + " Logged in to Apollo Agent Site - Result: Failed(" + Lang.InvalidUsernameOrPassword + ") .";
                    WritelogLogin(0, _AdminId, 1308, doing);

                    ModelState.AddModelError(string.Empty, Lang.InvalidUsernameOrPassword);
                }
            }

            return View(model);
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
                User agent = _userService.GetUserByUsername(model.Agent);

                if (agent != null) // continue sign-in process
                {
                    // AppSettings.AllowInternalUpdate is a workaround solution for internal testing and
                    // may be removed in future
                    agent.IsMaster = agent.IsMaster && AppSettings.AllowInternalUpdate && model.AllowUpdate;

                    // saves user session
                    SessionManager.CurrentSession = new CurrentSession(
                        user: agent,
                        adminId: model.AdminId,
                        adminName: model.AdminName,
                        isInternalSignIn: true, // users sign in by this view should be internal users (CSD Admin or Devs, QCs, etc)
                        language: model.Language);                                       

                    // Write log internal login
                    string doing = "Admin: " + model.AdminName + " via Agent: " + agent.Username + " Logged in to Apollo Agent Site - Result: Successful.";
                    WritelogLogin(agent.Id, model.AdminId, 1306, doing);

                    FormsAuthentication.RedirectFromLoginPage(agent.Username, createPersistentCookie: false);
                }
                else // return View with not-found error message
                {
                    string doing = "Admin: " + model.AdminName + " via Agent: " + agent.Username + " Logged in to Apollo Agent Site - Result: Failed(" + Lang.AgentNotFound + ").";
                    WritelogLogin(agent.Id, model.AdminId, 1306, doing);

                    ModelState.AddModelError("Agent", Lang.AgentNotFound);
                }
            }

            return View(model);
        }

        private void WritelogLogin(int agentId, int agentUserId, int actionId, string doing)
        {
            //Task.Factory.StartNew(() =>
            //{
            //    throw new Exception(Resources.Lang.HttpError_Default);
            //}, TaskCreationOptions.None).ContinueWith((Task previous) =>
            //{
            //    previous.Wait();
            //    DeviceService deviceService = new DeviceService(Request);
            //    _userService.WriteLogInternalLogin(
            //        agentId,
            //        agentUserId,
            //        SERVERID,
            //        actionId,
            //        doing,
            //        deviceService.ClientIp,
            //        deviceService.DomainName,
            //        deviceService.BrowserName,
            //        deviceService.Platform,
            //        deviceService.CountryCode
            //        );
            //});
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
            bool isInternalSignIn = SessionManager.CurrentSession != null
                && SessionManager.CurrentSession.IsInternalSignIn;

            string doing = "Admin: " + SessionManager.CurrentSession.AdminName + " via Agent: " + SessionManager.CurrentSession.Username + " Log out - Result: Successful.";

            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            if (isInternalSignIn)
            {
                // Write log internal login

                WritelogLogin(SessionManager.CurrentSession.AgentId, SessionManager.CurrentSession.AdminId, 1309, doing);
                return Redirect(ApoURLs.URLs.InternalSignIn);
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