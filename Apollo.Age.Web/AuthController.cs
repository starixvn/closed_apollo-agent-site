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

    public class AuthController : Controller
    {
        private const string APOLLO_SESSION_KEY = "Apollo.Age.Web.CurrentSession";
        private const string INVALID_REQ_MSG = "Invalid Request.";
        private const string USER_NOT_FOUND_MSG = "User not found.";

        private readonly IUserService _userService;

        public AuthController()
        {
            _userService = new UserService();
        }

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

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}