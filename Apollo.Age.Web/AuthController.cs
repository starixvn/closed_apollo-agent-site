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
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web.ViewModels;
    using Apollo.Age.Criterias;

    public class AuthController : Controller
    {
        private const string APOLLO_SESSION_KEY = "Apollo.Age.Web.CurrentSession";
        private const string INVALID_REQ_MSG = "Invalid Request.";
        private const string USER_NOT_FOUND_MSG = "User not found.";
        private const string AGENT_NOT_FOUND_MSG = "Agent not found.";
        private const string INVALID_USER_PWD_MSG = "Invalid Username or Password.";        

        private readonly IUserService _userService;

        public AuthController()
        {
            _userService = new UserService();
        }

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
                    Session[APOLLO_SESSION_KEY] = new CurrentSession(user);

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
                bool NapUserIsValid = _userService.AuthenticateNapUser(new AuthenticateNapUserCriteria
                    {
                        Username = model.Username,
                        Password = model.Password
                    });

                if (NapUserIsValid)
                {
                    var agent = _userService.GetUserByUsername(model.Agent);

                    if (agent != null)
                    {
                        agent.IsMaster = false; // CSD user will be treated as a shadow account
                        Session[APOLLO_SESSION_KEY] = new CurrentSession(agent);

                        FormsAuthentication.RedirectFromLoginPage(agent.Username, model.RememberMe);
                    }
                    else
                    {
                        ModelState.AddModelError("Agent", AGENT_NOT_FOUND_MSG);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, INVALID_USER_PWD_MSG);
                }
            }

            return View();
        }

        #endregion

        public ActionResult SignOut()
        {
            Session.Abandon();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}