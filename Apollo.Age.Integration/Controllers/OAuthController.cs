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
        public ViewResult Index(string user_id, string lang)
        {
            ViewBag.Username = user_id;
            ViewBag.Language = ConvertToApolloLanguage(lang);
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetCode(string code, string error, string state, string username, string language)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                return Error(code);
            }

            var authMsg = string.Empty;
            var authModel = _authService.GetAuthInfo(authorizationCode: code);

            if (!ReferenceEquals(authModel, null) && authModel.IsValid)
            {
                Entities.User user = _userService.GetUserByUsername(authModel.Username);

                if (user != null && user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {                    
                    ICurrentSession currentSession = new CurrentSession(user, language);
                    FormsAuthentication.SetAuthCookie(user.Username, true);                    

                    RedirectToAction("RedirectFromSsoPage", currentSession);
                }
            }

            return Error(authMsg);
        }


        /// <summary>
        /// this action requires authentication, param from current session
        /// </summary>
        /// <param name="currentSession"></param>
        /// <returns></returns>
        public ViewResult RedirectFromSsoPage(ICurrentSession currentSession)
        {
            if (currentSession != null)
            {
                return View();
            }

            return Error();
        }

        /// <summary>
        ///  Process error message
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public ViewResult Error(string error="")
        {
            string errorMessage = string.Empty;

            switch (error)
            {
                case "invalid_request":
                    errorMessage = "invalid request";
                    break;
                case "unauthorized_client":
                    errorMessage = "unauthorized_client";
                    break;
                case "access_denied":
                    errorMessage = "access_denied";
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

        /// <summary>
        /// Convert language of sungame to apo language
        /// </summary>
        /// <returns></returns>
        [NonAction]        
        private string ConvertToApolloLanguage(string langOfSungame)
        {
            switch (langOfSungame)
            {                 
                case "zh_TW" :
                    return "zh-TW";
                case "zh_GB" :
                    return "zh-CN";
                case "ja_JP" :
                    return "ja-JP";
                case "th_TH":
                    return "th-TH";
                case "vm_VM":
                    return "vi-VN";
                case "en_US":
                default:
                    return "en-US";
            }
        }
    }
}
