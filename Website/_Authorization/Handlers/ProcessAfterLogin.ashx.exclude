<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Authorization.ProcessAfterLogin_Handler" %>

/*
 * Created 20100223@Lee - Process after login (convert from code of Don).
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._Authorization
{
    using System;
    using System.Web;
    using System.Collections;
    using System.Web.Security;
    using RSACryptoPad;
    
    using Fanex.Age.UI;
    
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Services;       
    
    public class ProcessAfterLogin_Handler : Fanex.Age.UI.TplHandler 
    {
        const int dwKeySize = 1024;
        private void RedirectToBeforeLoginPage()
        {
            Session.Abandon();

            // Redirect to before login page
            string loginUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl;

            Response.Redirect(loginUrl, true);
        }

        private void RedirectToDefaultPage()
        {
            Request.Cookies.Remove(CookieCurrentURLKey);
            FormsAuthentication.RedirectFromLoginPage(CurrentUserSession.Username, false);
        }

        public string CookieCurrentURLKey
        {
            get
            {
                string name = string.Join("_", new string[] { 
                   CurrentUserSession.CustId.ToString(), 
                   CurrentUserSession.SubAccId.ToString(),
                   "URL"
                });

                return Sunplus.Agent.Utilities.Encryption.PearsonEncode(name);
            }
        }        
        
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
			Response.Cache.SetNoStore();
			
            string keyPath = Server.MapPath("../../IbcKeys/ibckey.snk");

            // Get encrypted parameters: custid, subAccId, login time, password, nick name, isNickName, __utms, language, no cross cookie
            string parameter = Request["params"] + string.Empty;
            parameter = parameter.Replace(' ', '+');
            EncryptionThread encryption = new EncryptionThread();

            try
            {
                parameter = encryption.DecryptString(parameter, dwKeySize, keyPath);
            }
            catch
            {
                parameter = null;
                //Sunplus.Agent.Utilities.Logs.Write("Cannot verify request key.");
            }

            if (null == parameter)
            {
                RedirectToBeforeLoginPage();
                return;
            }

            string[] ps = parameter.Split('&');
            p.Add("parameter", ps);
        }

        
        int custId, subAccId;
        DateTime loginTime;
        string language, password, nickname;
        bool isnickname;


        // Under maintenance checking
        static object _locker = new object();
        static DateTime _lastCheckUM = DateTime.Now;
        static bool _isUM = false;        
        
        
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            string startTime, endTime;

            // Double check and lock
            if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5)) // Check down time each 5 seconds
            {
                lock (_locker)
                {
                    if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5))
                    {
                        // Check under maintenance time
                        _isUM = MaintenanceService.GetDownTime(out startTime, out endTime);
                        _lastCheckUM = DateTime.Now;
                    }
                }
            }

            if (_isUM)
            {
                Sunplus.Agent.Utilities.Logs.Write("Under maintenance.");
                RedirectToBeforeLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.undermaintenance);
            }
            
            string[] ps = p["parameter"] as string[];

            long timeTicks;            

            // Verify customer information
            if (ps.Length < 9 || !int.TryParse(ps[0], out custId) || !int.TryParse(ps[1], out subAccId) || !long.TryParse(ps[2], out timeTicks))
            {
                Sunplus.Agent.Utilities.Logs.Write("Verify customer information failed.");
                RedirectToBeforeLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            // Verify random ticket
            bool isVerifiedTicket = false;
            try
            {
                HttpCookie cookie = Request.Cookies.Get("__utms");
                if ((cookie != null && cookie.Value != Sunplus.Agent.Utilities.Encryption.CfsEnCode(ps[0])) || (cookie == null && ps[8] == "False"))
                {
                    isVerifiedTicket = false;
                }
                else
                {
					isVerifiedTicket = true;
                }
            }
            catch {
				isVerifiedTicket = false;
				Sunplus.Agent.Utilities.Logs.Write("Verify random ticket failed.");
            }
            
            if(!isVerifiedTicket)
            {                
				RedirectToBeforeLoginPage();
				return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);  
            }

            language = ps[7] as string;
            nickname = ps[4];
            bool.TryParse(ps[5], out isnickname);
            password = HttpUtility.UrlDecode(ps[3]);
            if (Validation.CheckValidLanguage(language))
            {
                CurrentUserSession.Language = language;
                CurrentUserSession.LangKey = Sunplus.Agent.Model.Common.Language.LangKeyFromCulture(language);
            }
            loginTime = new DateTime(timeTicks);

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = null;

            IUserSession session = CurrentUserSession;
            //Get Browser
            Hashtable requestInfo = Sunplus.Agent.Model.Services.DeviceService.GetRequestInfo(Request);

            bool isSuccess = bo.AfterSignIn(custId, password, subAccId, nickname, isnickname, loginTime, requestInfo, out errCode, out errMsg, ref session);

            if (!isSuccess)
            {
                RedirectToBeforeLoginPage();
            }
            else
            {

                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
                {
                    string[] domains = Sunplus.Agent.Model.BL.Definitions.AppSettings.CookielessDomainCollection;
                    TplCore.TplSettings.SetCookielessDomains(domains);
                }     
                
                RedirectToDefaultPage();
            }

            return null;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {

        }
    }
}