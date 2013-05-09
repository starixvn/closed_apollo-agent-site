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
    using Apollo.Age.Core;
    using Apollo.Age.Criterias;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.ViewModels;

    public class InternalController : Controller
    {
        #region Const

        //private const string APOLLO_CURRENT_SESSION_KEY = "Apollo.Age.Web.CurrentSession";
        private const string APOLLO_SESSION_CAPTCHA = "ApolloCaptcha";
        private const string APOLLO_SESSION_CSDADMIN = "ApolloCSDAdmin";
        private const string APOLLO_SESSION_CSDADMIN_ALLOWEDUPDATE = "ApolloCSDAdminAllowedUpdate";
        private const string APOLLO_SESSION_CSDADMIN_LANGUAGUE = "ApolloCSDAdminLangague";

        private const string INVALID_REQ_MSG = "Invalid Request.";
        private const string USER_NOT_FOUND_MSG = "User not found.";
        private const string AGENT_NOT_FOUND_MSG = "Agent not found.";
        private const string INVALID_USER_PWD_MSG = "Invalid Username or Password.";

        private const string CSD_ADMIN_USERNAME = "Invalid Username or Password.";

        private const string CSD_SIGNIN_URL = "~/Internal/SignIn";

        #endregion Const

        #region Services

        private readonly IUserService _userService;

        #endregion Services

        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.Instance.CurrentSession; }
            set { SessionManager.Instance.CurrentSession = value; }
        }

        #region Ctors

        public InternalController()
        {
            _userService = new UserService();
        }

        #endregion Ctors               

        #region SignIn (for CSD login)

        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Resources.Lang.UnauthorizedMsg);
            }
            if (Session[APOLLO_SESSION_CSDADMIN] != null)
            {
                return Redirect("~/Internal/SelectAccount");
            }

            return View(new InternalSignInViewModel());
        }

        [HttpPost]
        public ActionResult SignIn(InternalSignInViewModel model)
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
                    Session[APOLLO_SESSION_CSDADMIN_ALLOWEDUPDATE] = model.AllowUpdates;
                    Session[APOLLO_SESSION_CSDADMIN_LANGUAGUE] = model.Language;

                    return RedirectToAction("SelectAccount");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, INVALID_USER_PWD_MSG);
                }
            }

            return View(new InternalSignInViewModel());
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
                    agent.IsMaster = Configurations.AppSettings.AllowInternalUpdate && Convert.ToBoolean(Session[APOLLO_SESSION_CSDADMIN_ALLOWEDUPDATE]);

                    this.CurrentSession = new CurrentSession(agent, csdAdminName, Session[APOLLO_SESSION_CSDADMIN_LANGUAGUE].ToString());

                    FormsAuthentication.RedirectFromLoginPage(agent.Username, model.RememberMe);
                }
                else
                {
                    ModelState.AddModelError("Agent", AGENT_NOT_FOUND_MSG);
                }
            }

            return View();
        }

        #endregion SignIn (for CSD login)

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
            int captcha = rand.Next(1000, 9999);

            // saves answer
            Session[APOLLO_SESSION_CAPTCHA] = captcha.ToString();

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

        #endregion Helpers
    }
}