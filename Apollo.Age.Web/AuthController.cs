// --------------------------------------------------------------------------
// <copyright file="AuthController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Criterias;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.ViewModels;
    using Apollo.Age.Core;

    public class AuthController : Controller
    {
        #region Const

        //private const string APOLLO_CURRENT_SESSION_KEY = "Apollo.Age.Web.CurrentSession";
        private const string APOLLO_SESSION_CAPTCHA = "ApolloCaptcha";
        private const string APOLLO_SESSION_CSDADMIN = "ApolloCSDAdmin";

        private const string INVALID_REQ_MSG = "Invalid Request.";
        private const string USER_NOT_FOUND_MSG = "User not found.";
        private const string AGENT_NOT_FOUND_MSG = "Agent not found.";
        private const string INVALID_USER_PWD_MSG = "Invalid Username or Password.";

        private const string CSD_ADMIN_USERNAME = "Invalid Username or Password.";

        private const string CSD_SIGNIN_URL = "~/Auth/SignIn2";

        #endregion

        #region Services

        private readonly IUserService _userService;

        #endregion

        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.Instance.CurrentSession; }
            set { SessionManager.Instance.CurrentSession = value; }
        }

        #region Ctors

        public AuthController()
        {
            _userService = new UserService();

        }

        #endregion

        #region SignIn

        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Resources.Lang.UnauthorizedMsg);
            }

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Resources.Lang.UnauthorizedMsg);
            }

            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsername(model.Username);

                if (user != null)
                {
                    //Session[Constants.APOLLO_CURRENT_SESSION_KEY] = new CurrentSession(user);
                    this.CurrentSession = new CurrentSession(user);

                    FormsAuthentication.RedirectFromLoginPage(user.Username, model.RememberMe);
                }
                else
                {
                    ModelState.AddModelError("Username", USER_NOT_FOUND_MSG);
                }
            }

            return View();
        }

        #endregion

        #region SignIn2 (for CSD login)

        [HttpGet]
        public ActionResult SignIn2()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Resources.Lang.UnauthorizedMsg);
            }

            return View();
        }

        [HttpPost]
        public ActionResult SignIn2(SignIn2ViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Resources.Lang.UnauthorizedMsg);
            }

            if (ModelState.IsValid)
            {
                // if captcha is null or incorrect
                if (Session[APOLLO_SESSION_CAPTCHA] == null
                    || Session[APOLLO_SESSION_CAPTCHA].ToString() != model.Captcha)
                {
                    ModelState.AddModelError("Captcha", Apollo.Age.Resources.Lang.IncorrectCaptchaMsg);
                    return View(model);
                }

                bool NapUserIsValid = _userService.AuthenticateNapUser(new AuthenticateNapUserCriteria
                    {
                        Username = model.Username,
                        Password = model.Password
                    });

                if (NapUserIsValid)
                {
                    Session[APOLLO_SESSION_CSDADMIN] = model.Username;
                    return RedirectToAction("SelectAccount");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, INVALID_USER_PWD_MSG);
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult SelectAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectAccount(InternalSelectAccountViewModel model)
        {
            string csdAdminName = Session[APOLLO_SESSION_CSDADMIN] as string;

            if (csdAdminName == null)
            {
                return Redirect(CSD_SIGNIN_URL);
            }

            if (ModelState.IsValid)
            {
                Apollo.Age.Entities.User agent = _userService.GetUserByUsername(model.Agent);

                if (agent != null)
                {
                    agent.IsMaster = false; // CSD user will be treated as a shadow account
                    //Session[Constants.APOLLO_CURRENT_SESSION_KEY] =
                    //    new CurrentSession(agent, Session[APOLLO_SESSION_CSDADMIN].ToString());

                    this.CurrentSession = new CurrentSession(agent, csdAdminName);

                    FormsAuthentication.RedirectFromLoginPage(agent.Username, model.RememberMe);
                }
                else
                {
                    ModelState.AddModelError("Agent", AGENT_NOT_FOUND_MSG);
                }
            }

            return View();
        }

        #endregion

        public ActionResult SignOut()
        {
            var currentSession = Session[Constants.APOLLO_CURRENT_SESSION_KEY] as CurrentSession;
            string redirectUrl = currentSession != null && currentSession.IsCsdAdmin
                ? CSD_SIGNIN_URL
                : FormsAuthentication.LoginUrl;

            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            return Redirect(redirectUrl);
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult AccountStopped()
        {
            return View();
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

        public ActionResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            int a = rand.Next(10000, 99999);
            var captcha = string.Format("{0}", a);

            // saves answer
            Session[APOLLO_SESSION_CAPTCHA] = captcha;

            FileContentResult img = null; // image stream

            using (var mem = new MemoryStream())
            using (var bmp = GenerateNoise(60, 23))
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
                gfx.DrawString(captcha, new Font("Tahoma", 13), Brushes.White, 4, 2);

                // render as JPEG
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        #endregion
    }
}