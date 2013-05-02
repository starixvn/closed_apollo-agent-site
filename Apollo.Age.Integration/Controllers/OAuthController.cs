// --------------------------------------------------------------------------
// <copyright file="OAuthController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
// </history>


namespace Apollo.Age.Integration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using System.Web.Security;

    [AllowAnonymous]
    public class OAuthController : Controller
    {
        private readonly AuthenticationService _authService;
        private readonly UserService _userService;

        public OAuthController()
            : this(null)
        {

        }

        public OAuthController(AuthenticationService authService = null, UserService userService = null)
        {
            _authService = authService ?? new AuthenticationService();
            _userService = userService ?? new UserService();
        }
        
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }
        
        public ViewResult GetCode(string code, string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                return Error(code);
            }

            var authMsg = string.Empty;
            var authModel = _authService.GetAuthInfo(authorizationCode: code);

            if (!ReferenceEquals(authModel, null))
            {
                Entities.User user = _userService.GetUserById(authModel.UserId);

                if (user != null)
                {
                    ICurrentSession currentSession = new CurrentSession(user);                    
                    FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                }                
            }

            return View(authMsg);
        }

        /// <summary>
        ///  Process error message
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public ViewResult Error(string error)
        {
            string errorMessage = string.Empty;

            switch (error)
            {
                case "invalid_request":
                    errorMessage = "invalid request";
                    break;
                case "unauthorized_client":
                    errorMessage = "invalid_request";
                    break;
                case "access_denied":
                    errorMessage = "invalid_request";
                    break;
                case "unsupported_response_type":
                    errorMessage = "unsupported_response_type";
                    break;
                case "server_error":
                    errorMessage = "server_error";
                    break;
                case "temporarily_unavailable":
                default:
                    errorMessage = "temporarily_unavailable";
                    break;
            }

            return View((object)errorMessage);
        }
    }
}
