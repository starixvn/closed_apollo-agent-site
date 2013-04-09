/*
 * Created 20100113@Lee - Sign-in page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._Authorization
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using Fanex.Agent.UI._Authorization;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Services.Impl;
    using Sunplus.Agent.Model.BL;
    using Apollo.Age.Web;

    public partial class SignIn_Page : Fanex.Age.UI.TplPage2
    {
        /// <summary>
        /// This variable indicate selenium testting - automation testing
        /// Do not check validate captcha code
        /// This Appsetting variable has to set to False on live enviroment
        /// </summary>
        private readonly bool IsAllowAutomationCaptchaCode = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["AllowAutomationCaptchaCode"]));
        private readonly IUserService _userService;
        string _userName = string.Empty;

        public SignIn_Page()
        {
            _userService = new UserService();
        }

        #region Define Properties

        private int LoginFailedCount
        {
            get
            {
                if (HttpContext.Current.Session["LoginFailedCount"] == null) return 0;
                return (int)(HttpContext.Current.Session["LoginFailedCount"]);
            }
            set
            {
                HttpContext.Current.Session["LoginFailedCount"] = value;
            }
        }

        private DateTime PreviousLoginFailedTime
        {
            get
            {
                return Cast.AsDateTime(HttpContext.Current.Session["PreviousLoginFailedTime"], DateTime.MinValue);
            }
            set
            {
                HttpContext.Current.Session["PreviousLoginFailedTime"] = value;
            }
        }

        #endregion Define Properties

        public override void OnPostBack()
        {
            if (LoginFailedCount > 5) // Login Fail > 5
            {
                if (PreviousLoginFailedTime.AddMinutes(5).CompareTo(DateTime.Now) > 0) // After 5'
                {
                    PreviousLoginFailedTime = DateTime.Now;
                    RegisterJSONError(1, Resources.Lang.loginWrong5Times);
                    return;
                }
                else
                    LoginFailedCount = 0;
            }

            this.CheckLogin();
        }

        #region Event check login

        private void CheckLogin()
        {
            // Checking username/password/code here
            string userName = Request["txtUserName"].Trim();
            string passWord = Request["txtPassWord"];
            string language = Request["hidLanguage"];
            IUserSession session = CurrentUserSession;

            //Get Browser
            Hashtable requestInfo = Sunplus.Agent.Model.Services.DeviceService.GetRequestInfo(Request);

            if (Validation.CheckValidLanguage(language))
            {
                session.Language = language;
                session.LangKey = Sunplus.Agent.Model.Common.Language.LangKeyFromCulture(language);
                SetLanguage(language);
            }
            else
            {
                return;
            }

            //  string code = Request["txtCaptcha"];
            _userName = userName;

            Apollo.Age.Entities.User user = _userService.GetUserByUsername(userName);

            if (user != null)
            {
                ICurrentSession currentSession = new CurrentSession(user);
                Session["Apollo.Age.Web.CurrentSession"] = currentSession;
                UserSessionService.SetUserSessionValues(session, user);

                

                //session.CurrentUser = user;
                FormsAuthentication.RedirectFromLoginPage(user.Username, false);
            }
            else
            {
                PreviousLoginFailedTime = DateTime.Now;
                LoginFailedCount++;
                if (LoginFailedCount > 5)
                {
                    RegisterJSONError(1, Resources.Lang.loginWrong5Times);
                    return;
                }
                else
                {
                    // Return error
                    RegisterJSONError(16, "Login failed");
                }
            }

            //AuthorizationBO bo = new AuthorizationBO(Session.SessionID);

            //int errCode;
            //string errMsg = null;
            //string nickName = string.Empty;

            //// Do sign-in
            //bool isAuthenticated = bo.SignIn(
            //    userName,
            //    passWord,
            //    requestInfo,
            //    out errCode,
            //    out errMsg,
            //    ref session);

            //if (isAuthenticated)
            //{
            //    LoginFailedCount = 0;

            //    // FPS.Client.Service fps = new FPS.Client.Service();
            //    // fps.add_event(userName, Request["__di"], "Login", "Agency", Request.UserHostAddress);

            //    FormsAuthentication.RedirectFromLoginPage(session.UserName, false);
            //}
            //else
            //{
            //    PreviousLoginFailedTime = DateTime.Now;
            //    LoginFailedCount++;
            //    if (LoginFailedCount > 5)
            //    {
            //        RegisterJSONError(1, Resources.Lang.loginWrong5Times);
            //        return;
            //    }
            //    else
            //    {
            //        // Return error
            //        RegisterJSONError(errCode, errMsg);
            //    }
            //}
        }

        #endregion Event check login

        public override void ProcessRequest(ref Hashtable p)
        {
            // No param need when render page
        }

        public override object ValidateRequest(Hashtable p)
        {
            // Before/After Login
            // Redirect to before login
            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
            {
                string beforeLoginUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl;
                string baseUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BaseUrl;

                if (beforeLoginUrl != baseUrl)
                {
                    Session.Abandon();

                    if (Request.Cookies["SSL"] == null || Request.Cookies["SSL"].Value != "true")
                    {
                        beforeLoginUrl = beforeLoginUrl.Replace("https://", "http://");
                    }

                    if (beforeLoginUrl.ToLower().EndsWith("/accounting/"))
                    {
                        beforeLoginUrl = beforeLoginUrl + "Index.aspx";
                    }

                    Response.Redirect(beforeLoginUrl, true);
                }

                string[] domains = Sunplus.Agent.Model.BL.Definitions.AppSettings.CookielessDomainCollection;
                TplCore.TplSettings.SetCookielessDomains(domains);
            }

            // Check under maintenance time (no need, before login did it)

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            return null;
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
            {
                return;
            }

            RegisterJavascriptObject("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());

            // Default Korean language for ISPO site
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsISPOSite)
            {
                CurrentUserSession.Language = "ko-KR";
            }
            else
            {
                RegisterJavascriptObject("language", 0);
            }

            SignIn_Template tpl = new SignIn_Template();
            tpl.username_value = _userName;

            // if allow automation test
            // pass a dummy value to by pass check javascript validation
            //if (IsAllowAutomationCaptchaCode)
            //{
            //    tpl.captcha_value = "8799";
            //}
            //else
            //{
            //    tpl.captcha_value = string.Empty;
            //}

            //Mobile Device Browser
            if (this.isMobileBrowser())
            {
                tpl.VersionMobile = "<a id='mobileId'>" + Resources.Lang.mobile + "</a> - " + Resources.Lang.fullSite;
            }
            else
            {
                tpl.VersionMobile = string.Empty;
            }

            writer.Write(tpl);
        }

        #region Detecting a mobile browser

        public bool isMobileBrowser()
        {
            //First try built in asp.net check
            if (Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //Then try checking for the HTTP_X_WAP_PROFILE hear
            if (Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //Then try checking that HTTP_ACCEPT exists and WAP
            if (Request.ServerVariables["HTTP_ACCEPT"] != null &&
                Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //And finally check the HTTP_USER_AGENT
            //Hear variable for any one of the following
            if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                    new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone"
                };

                //Loop through each item in the list created above
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Detecting a mobile browser
    }
}